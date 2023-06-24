using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Timers;
using Microsoft.VisualBasic;
using sMkTaskManager.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace sMkTaskManager;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmMain : Form {

    public frmMain() {
        InitializeComponent();
        Initialize_MainMenu();
        // Flicker Free Controls by DoubleBuffer
        Extensions.CascadingDoubleBuffer(this);
        // Extensions.UseImmersiveDarkMode(Handle, true);
        // Extensions.UseImmersiveRoundCorner(Handle, 3);
    }


    // Local variables...
    private List<Task> _MonitorTasks = new();
    private System.Timers.Timer _ParallelTimer, _MonitorTriggerTimer;
    private System.Windows.Forms.Timer StatusTimer = new();
    private bool _InitComplete = false, _LoadComplete = false;
    private readonly Stopwatch _StopWatch1 = new(), _StopWatch2 = new(), _StopWatch3 = new();
    private readonly TaskManagerSystem _System = new();
    private Size? _prevSize, _prevMinSize; Point? _prevLocation;
    private Size? _fullScreenSize; Point? _fullScreenLocation;
    private Dictionary<string, Stopwatch> _Timmings = new();

    private HashSet<string> _ProcsColumns = new();
    private HashSet<string> _HashProcs = new();

    private void OnLoadEventHandler(object sender, EventArgs e) {
        Extensions.StartMeasure(_StopWatch1);
        Settings.LoadAll();
        OnLoadLoadSettings();
        OnLoadAddHandlers();
        Settings_Apply();
        // This Timer is used to trigger the Monitor Refresh
        _MonitorTriggerTimer = new() { Interval = Settings.UpdateSpeed, Enabled = false };
        _MonitorTriggerTimer.Elapsed += MonitorTriggerExecutor;
        // Parallel Init by using a Timer
        _ParallelTimer = new() { Interval = 1, AutoReset = false };
        _ParallelTimer.Elapsed += OnLoadParallelInit;
        _ParallelTimer.Start();

        _LoadComplete = true;
        Extensions.StopMeasure(_StopWatch1);
    }
    private void OnLoadParallelInit(object? sender, ElapsedEventArgs e) {
        _ParallelTimer.Stop();
        Stopwatch sw = new();
        Extensions.StartMeasure(sw);
        // We should use this to initialize something that is not really critical?
        ETW.Start();
        _InitComplete = true;
        Extensions.StopMeasure(sw);
        MonitorStart(true);
    }
    private void OnLoadLoadSettings() {
        // Load Window Size & Position
        Settings.LoadMainWindow();
        if (Settings.RememberPositions) {
            Width = Math.Max(MinimumSize.Width, Settings.MainWindow.Size.Width);
            Height = Math.Max(MinimumSize.Height, Settings.MainWindow.Size.Height);
            if (Settings.MainWindow.Location.X != 0 || Settings.MainWindow.Location.Y != 0) {
                Location = Settings.MainWindow.Location;
            }
            if (Settings.MainWindow.Maximized) {
                WindowState = FormWindowState.Maximized;
            }
        } else { Width = 760; Height = 750; }
        // Load Last Tab
        if (Settings.RememberActiveTab) { if (Settings.ActiveTab < tc.TabCount) tc.SelectTab(Settings.ActiveTab); }
        // Properly Handle Start Minimized
        if (Settings.StartMinimized) {
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = !Settings.ToTrayWhenMinimized;
            if (Settings.ToTrayWhenMinimized) Hide();
        }
        // Load Column definitions for all the listviews
        Settings.LoadColsInformation("colsProcess", tabProcs.lv, ref _ProcsColumns);
        tabProcs.btnAllUsers.Checked = Settings.ShowAllProcess;

    }
    private void OnLoadAddHandlers() {
        _System.MetricValueChanged += perf_MetricValueChangedEventHandler;
        _System.RefreshCompleted += perf_RefreshCompletedEventHandler;
        tabPerf.MouseDoubleClick += perf_MouseDoubleClickEventHandler;
    }
    private void OnSizeChangedEventHandler(object sender, EventArgs e) {
        if (!_LoadComplete) return;
        ToolStripMenuItem mnuItm = (ToolStripMenuItem)mnuOptions.DropDownItems["mnuOptions_HideMinimize"];

        if (WindowState == FormWindowState.Minimized && mnuItm.Checked) {
            WindowState = FormWindowState.Minimized;
            Hide();
        }
        if (WindowState != FormWindowState.Minimized) {
            Settings.MainWindow.Maximized = (WindowState == FormWindowState.Maximized);
        }
        if ((WindowState == FormWindowState.Normal | WindowState == FormWindowState.Maximized) && !Visible) {
            Visible = true;
        }
    }
    private void OnStatusTimerEventHandler(object sender, EventArgs e) {
        SetStatusText(null);
        StatusTimer.Stop();
    }

    private void perf_MouseDoubleClickEventHandler(object? sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && e.Clicks > 1) { FullScreen = !FullScreen; }
    }
    private void perf_MetricValueChangedEventHandler(object sender, Metric metric, MetricChangedEventArgs e) {
        switch (e.MetricName) {
            case "PhysicalTotal": tabPerf.gbMemory_Total.Text = _System.PhysicalTotal.ValueFmt; break;
            case "PhysicalAvail": tabPerf.gbMemory_Avail.Text = _System.PhysicalAvail.ValueFmt; break;
            case "SystemCached": tabPerf.gbMemory_Cached.Text = _System.SystemCached.ValueFmt; break;

            case "CommitTotal": tabPerf.gbCommit_Current.Text = _System.CommitTotal.ValueFmt; break;
            case "CommitPeak": tabPerf.gbCommit_Peak.Text = _System.CommitPeak.ValueFmt; break;
            case "CommitLimit": tabPerf.gbCommit_Limit.Text = _System.CommitLimit.ValueFmt; break;
            case "KernelTotal": tabPerf.gbKernel_Total.Text = _System.KernelTotal.ValueFmt; break;
            case "KernelPaged": tabPerf.gbKernel_Paged.Text = _System.KernelPaged.ValueFmt; break;
            case "KernelNonPaged": tabPerf.gbKernel_NonPaged.Text = _System.KernelNonPaged.ValueFmt; break;
            case "PageFileTotal": tabPerf.gbPagefile_Limit.Text = _System.PageFileTotal.ValueFmt; break;
            case "PageFileUsed": tabPerf.gbPagefile_Current.Text = _System.PageFileUsed.ValueFmt; break;
            case "PageFilePeak": tabPerf.gbPagefile_Peak.Text = _System.PageFilePeak.ValueFmt; break;

            case "ioReadCount": tabPerf.gbIOops_Reads.Text = _System.ioReadCount.ValueFmt; break;
            case "ioReadBytes": tabPerf.gbIOtranf_Reads.Text = _System.ioReadBytes.ValueFmt; break;
            case "ioWriteCount": tabPerf.gbIOops_Writes.Text = _System.ioWriteCount.ValueFmt; break;
            case "ioWriteBytes": tabPerf.gbIOtranf_Writes.Text = _System.ioWriteBytes.ValueFmt; break;
            case "ioOtherCount": tabPerf.gbIOops_Others.Text = _System.ioOtherCount.ValueFmt; break;
            case "ioOtherBytes": tabPerf.gbIOtranf_Others.Text = _System.ioOtherBytes.ValueFmt; break;

            case "HandleCount": tabPerf.gbSystem_Handles.Text = _System.HandleCount.ValueFmt; break;
            case "ThreadCount": tabPerf.gbSystem_Threads.Text = _System.ThreadCount.ValueFmt; break;
            case "ProcessCount": tabPerf.gbSystem_Processes.Text = _System.ProcessCount.ValueFmt; break;
            case "DevicesCount": tabPerf.gbSystem_Devices.Text = _System.DevicesCount.ValueFmt; break;
            case "ServicesCount": tabPerf.gbSystem_Services.Text = _System.ServicesCount.ValueFmt; break;


        }
    }
    private void perf_RefreshCompletedEventHandler(object? sender, EventArgs e) {
        // Always set these values. regardless we are visible or not.
        tabPerf.gbSystem_UpTime.Text = _System.UpTime;
        tabPerf.meterCpu.SetValue(_System.CpuUsage.Value);
        tabPerf.chartCpu.AddValue(_System.CpuUsage.Value, _System.CpuUsageKernel.Value);
        tabPerf.meterMem.SetValue(_System.MemoryUsage, _System.MemoryUsageString);
        tabPerf.chartMem.AddValue((double)_System.MemoryUsage, _System.SwapUsage);
        tabPerf.meterIO.SetValue(_System.ioDataUsage, _System.ioDataUsageString);
        tabPerf.chartIO.AddValue((double)_System.ioOtherBytes.Delta / 1024, (double)_System.ioWriteBytes.Delta / 1024, (double)_System.ioReadBytes.Delta / 1024);
        tabPerf.meterDisk.SetValue(_System.DiskUsage, _System.DiskUsageString);
        tabPerf.chartDisk.AddValue((double)_System.DiskRead.Delta / 1024, (double)_System.DiskWrite.Delta / 1024);
        tabPerf.meterNet.SetValue(_System.NetworkUsage, _System.NetworkUsageString);
        tabPerf.chartNet.AddValue((double)_System.NetReceived.Delta / 1024, (double)_System.NetSent.Delta / 1024);
        // Always set the status bar labels as well
        ssProcesses.Text = "Processes: " + _System.ProcessCount.Value;
        ssServices.Text = "Services: " + _System.ServicesCount.Value;
        ssCpuLoad.Text = "CPU Load: " + _System.CpuUsage.Value + "%";
        // Always flush the ETW, if its active.
        ETW.Flush();
    }

    private void Refresh_Applications(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(20, 50));
        TimmingStop();
    }
    private void Refresh_Performance(bool firstTime = false) {
        if (InvokeRequired) {
            BeginInvoke(() => Refresh_Performance(firstTime));
            Debug.WriteLine("Invoked");
            return;
        }
        TimmingStart();
        // Call the Refresher, and cancel events cascading if we are not visible. 
        _System.Refresh();
        TimmingStop();
    }
    private void Refresh_Processes(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(10, 30));
        TimmingStop();
    }
    private void Refresh_Services(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(10, 30));
        TimmingStop();
    }
    private void Refresh_Connections(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(1, 30));
        TimmingStop();
    }
    private void Refresh_Ports(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(1, 20));
        TimmingStop();
    }
    private void Refresh_Nics(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(1, 10));
        TimmingStop();
    }
    private void Refresh_TrayIcons(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(1, 5));
        TimmingStop();
    }
    private void Refresh_Users(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(1, 5));
        TimmingStop();
    }

    public bool FullScreen {
        get { return Settings.inFullScreen; }
        set {
            if (value) { tc.SelectTab(tpPerformance); }
            Settings.inFullScreen = value;
            mnu.Visible = !value;
            ss.Visible = !value;
            tc.Visible = !value;
            if (value) {
                Controls.Add(tabPerf);
                if (WindowState == FormWindowState.Normal) {
                    _prevLocation = Location;
                    _prevMinSize = MinimumSize;
                    _prevSize = Size;
                }
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
                MinimumSize = new(100, 300);
                ControlBox = false;
                TopMost = true;
                if (WindowState == FormWindowState.Normal) {
                    _fullScreenSize ??= Size;
                    _fullScreenLocation ??= Location;
                    Size = (Size)_fullScreenSize;
                    Location = (Point)_fullScreenLocation;
                }
            } else {
                tpPerformance.Controls.Add(tabPerf);
                if (WindowState == FormWindowState.Normal) {
                    _fullScreenSize = Size;
                    _fullScreenLocation = Location;
                }
                FormBorderStyle = FormBorderStyle.Sizable;
                ControlBox = true;
                TopMost = Settings.AlwaysOnTop;
                if (WindowState == FormWindowState.Normal) {
                    Size = (Size)_prevSize!;
                    MinimumSize = (Size)_prevMinSize!;
                    Location = (Point)_prevLocation!;
                }
            }
            OnSizeChangedEventHandler(this, new EventArgs());
        }
    }
    private void Settings_Apply() {
        //Performance Graphs Settings
        tabPerf.chartCpu.SetIndexes("Total", Settings.Performance.ShowKernelTime ? "Kernel" : null);
        tabPerf.chartCpu.BackSolid = Settings.Performance.Solid;
        tabPerf.chartCpu.AntiAliasing = Settings.Performance.AntiAlias;
        tabPerf.chartCpu.ShadeBackground = Settings.Performance.ShadeBackground;
        tabPerf.chartCpu.DisplayAverage = Settings.Performance.DisplayAverages;
        tabPerf.chartCpu.DisplayLegends = Settings.Performance.DisplayLegends;
        tabPerf.chartCpu.DisplayIndexes = Settings.Performance.DisplayIndexes;
        tabPerf.chartCpu.DetailsOnHover = Settings.Performance.DisplayOnHover;
        tabPerf.chartCpu.ValueSpacing = Settings.Performance.ValueSpacing;
        tabPerf.chartCpu.GridSpacing = Settings.Performance.GridSize;
        tabPerf.chartCpu.PenGridVertical.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.VerticalGridStyle;
        tabPerf.chartCpu.PenGridVertical.Color = Settings.Performance.VerticalGridColor;
        tabPerf.chartCpu.PenGridHorizontal.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.HorizontalGridStyle;
        tabPerf.chartCpu.PenGridHorizontal.Color = Settings.Performance.HorizontalGridColor;
        tabPerf.chartCpu.PenAverage.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.AverageLineStyle;
        tabPerf.chartCpu.PenAverage.Color = Settings.Performance.AverageLineColor;
        tabPerf.chartCpu.LightColors = Settings.Performance.LightColors;
        // TODO: Force for now, remove later
        tabPerf.chartCpu.DisplayAverage = true;
        tabPerf.chartCpu.DisplayLegends = true;
        tabPerf.chartCpu.DisplayIndexes = true;

        tabPerf.chartMem.CopySettings(tabPerf.chartCpu);
        tabPerf.chartIO.CopySettings(tabPerf.chartCpu);
        tabPerf.chartDisk.CopySettings(tabPerf.chartCpu);
        tabPerf.chartNet.CopySettings(tabPerf.chartCpu);
        tabPerf.meterCpu.LightColors = tabPerf.chartCpu.LightColors;
        tabPerf.meterMem.LightColors = tabPerf.chartCpu.LightColors;
        tabPerf.meterIO.LightColors = tabPerf.chartCpu.LightColors;
        tabPerf.meterDisk.LightColors = tabPerf.chartCpu.LightColors;
        tabPerf.meterNet.LightColors = tabPerf.chartCpu.LightColors;

    }

    internal bool MonitorRunning;
    internal bool MonitorBusy;
    internal double MonitorSpeed {
        get { return _MonitorTriggerTimer.Interval; }
        set { _MonitorTriggerTimer.Interval = value; }
    }
    internal void MonitorStart(bool firstTime = false) {
        MonitorRunning = true;
        MonitorRefreshParallel(firstTime);
        _MonitorTriggerTimer.Start();
        SetStatusText();
    }
    internal void MonitorStop() {
        _MonitorTriggerTimer.Stop();
        MonitorRunning = false;
        SetStatusText();
    }
    internal void MonitorToggle() {
        if (MonitorRunning) { MonitorStop(); } else { MonitorStart(); }
    }
    internal void MonitorRefresh(bool firstTime = false) {
        if (MonitorBusy) return;
        _StopWatch1.Restart();
        MonitorBusy = true;
        Refresh_Applications(firstTime);
        Refresh_Performance(firstTime);
        Refresh_Processes(firstTime);
        Refresh_Services(firstTime);
        Refresh_Connections(firstTime);
        Refresh_Ports(firstTime);
        Refresh_Nics(firstTime);
        Refresh_TrayIcons(firstTime);
        Refresh_Users(firstTime);
        MonitorBusy = false;
        _StopWatch1.Stop();
        SetStatusText(ssBusyTime, _StopWatch1.ElapsedMilliseconds + "ms.");
        TimmingDisplay();
    }
    internal async void MonitorRefreshParallel(bool firstTime = false) {
        if (MonitorBusy) return;
        _StopWatch1.Restart();
        MonitorBusy = true;
        _MonitorTasks.Clear();
        _MonitorTasks.Add(Task.Run(() => Refresh_Applications(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Performance(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Processes(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Services(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Connections(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Ports(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Nics(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_TrayIcons(firstTime)));
        _MonitorTasks.Add(Task.Run(() => Refresh_Users(firstTime)));
        await Task.WhenAll(_MonitorTasks);
        MonitorBusy = false;
        _StopWatch1.Stop();
        SetStatusText(ssBusyTime, _StopWatch1.ElapsedMilliseconds + "ms.");
        TimmingDisplay();
    }
    private void MonitorTriggerExecutor(object? sender, ElapsedEventArgs e) {
        if (MonitorRunning) { MonitorRefreshParallel(); }
    }

    private bool TimmingVisible {
        get { return timmingStrip.Visible; }
        set {
            if (timmingStrip.Visible == value) return;
            if (timmingStrip.Items.Count == 0 && value) TimmingInitStrip();
            timmingStrip.Visible = value;
            tc.Height += value ? -20 : +20;
            if (timmingStrip.Dock == DockStyle.Top) tc.Top += value ? +20 : -20; ;
        }
    }
    private void TimmingStart([System.Runtime.CompilerServices.CallerMemberName] string methodName = "") {
        if (!_Timmings.ContainsKey(methodName)) {
            _Timmings.Add(methodName, new());
        } else {
            _Timmings[methodName].Restart();
        }
    }
    private void TimmingStop([System.Runtime.CompilerServices.CallerMemberName] string methodName = "") {
        if (_Timmings.ContainsKey(methodName)) _Timmings[methodName].Stop();
    }
    private void TimmingInitStrip() {
        timmingStrip.Items.Clear();
        timmingStrip.Items.AddRange(new ToolStripStatusLabel[] {
            new ToolStripStatusLabel("  ") { Name = "Space", Size=new(17,17), Spring=false, Visible=true, AutoSize=false },
            new ToolStripStatusLabel("Apps") { Name = "Refresh_Applications", BackColor = Color.FromArgb(255, 192, 192) },
            new ToolStripStatusLabel("Procs") { Name = "Refresh_Processes", BackColor = Color.FromArgb(255, 224, 192) },
            new ToolStripStatusLabel("Servs") { Name = "Refresh_Services", BackColor = Color.FromArgb(255, 255, 192) },
            new ToolStripStatusLabel("Perf") { Name = "Refresh_Performance", BackColor = Color.FromArgb(192, 255, 192) },
            new ToolStripStatusLabel("Net") { Name = "Refresh_Nics", BackColor = Color.FromArgb(192, 255, 255) },
            new ToolStripStatusLabel("Conns") { Name = "Refresh_Connections", BackColor = Color.FromArgb(192, 255, 255) },
            new ToolStripStatusLabel("Port") { Name = "Refresh_Ports", BackColor = Color.FromArgb(192, 192, 255) },
            new ToolStripStatusLabel("Users") { Name = "Refresh_Users", BackColor = Color.FromArgb(255, 192, 255) },
            new ToolStripStatusLabel("Trays") { Name = "Refresh_SystemTray", BackColor = Color.FromArgb(192, 192, 192) }
        });
        foreach (ToolStripStatusLabel c in timmingStrip.Items) {
            if (c.Name == "Space") continue;
            c.BorderSides = ToolStripStatusLabelBorderSides.All;
            c.BorderStyle = Border3DStyle.Flat;
            c.AutoToolTip = false;
            c.Spring = true;
            c.Visible = false;
            c.AccessibleName = c.Text + ": ";
        }
    }
    private void TimmingDisplay() {
        if (!TimmingVisible) return;
        if (InvokeRequired) { Invoke(TimmingDisplay); return; }
        foreach (var t in _Timmings) {
            if (!timmingStrip.Items.ContainsKey(t.Key)) continue;
            var itm = timmingStrip.Items[t.Key];
            itm.Visible = true;
            itm.Tag ??= t.Value.ElapsedMilliseconds;
            itm.Tag = Math.Round((t.Value.ElapsedMilliseconds + Convert.ToDouble(itm.Tag)) / 2, 2);
            itm.Text = itm.AccessibleName + string.Format("{0:N0}", t.Value.ElapsedMilliseconds) + "ms.";
            itm.ToolTipText = $"Average: {string.Format("{0:N2}", itm.Tag)}ms ";
        };

    }

    public void SetStatusText(ToolStripLabel? obj = null, string value = "") {
        obj ??= ssText;
        if (ss.InvokeRequired) { ss.Invoke(SetStatusText, new object[] { obj, value }); return; }
        if (value == "") { value = MonitorRunning ? "Running ..." : "Paused ..."; }
        if (value != obj.Text) {
            obj.Text = value;
            StatusTimer.Stop();
            StatusTimer.Interval = 10000;
            StatusTimer.Start();
        }
    }
    public string GetStatusText(ToolStripLabel? obj) {
        obj ??= ssText;
        return obj.Text;
    }

}
