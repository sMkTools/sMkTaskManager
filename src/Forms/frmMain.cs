using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Timers;
using sMkTaskManager.Classes;
using sMkTaskManager.Forms;

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

        //Tester tt = new();
        //tt.Handles.SetValue(1024);
        //tt.Handles.SetValue(4096);
        //tt.Handles.SetFormat(MetricFormats.Kb);
        //tt.Handles.FormatString = "{0:#,0.0} Kb.";
        //Debug.WriteLine($"{tt} Value: {tt.Handles.Value} - Delta:{tt.Handles.Delta}");
        //Debug.WriteLine($"{tt} Value: {tt.Handles.ValueFmt} - Delta:{tt.Handles.DeltaFmt}");

    }

    // Local variables...
    private List<Task> _MonitorTasks = new();
    private System.Timers.Timer _ParallelTimer, _MonitorTriggerTimer;
    private System.Windows.Forms.Timer StatusTimer = new();
    private bool _InitComplete = false, _LoadComplete = false;
    private readonly Stopwatch _StopWatch1 = new(), _StopWatch2 = new(), _StopWatch3 = new();
    private Size? _prevSize, _prevMinSize; Point? _prevLocation;
    private Size? _fullScreenSize; Point? _fullScreenLocation;


    private void OnLoadEventHandler(object sender, EventArgs e) {
        Extensions.StartMeasure(_StopWatch1);

        _MonitorTriggerTimer = new() { Enabled = false };
        _MonitorTriggerTimer.Elapsed += MonitorTriggerExecutor;
        _ParallelTimer = new() { Interval = 1, AutoReset = false };
        _ParallelTimer.Elapsed += OnLoadParallelInit;

        Settings.LoadAll();
        OnLoadSetFixedValues();
        OnLoadLoadSettings();
        OnLoadAddHandlers();
        Settings_Apply();

        // At this point, we can follow with Parallel Init;
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
        // Acurately set Update Timer
        switch (Settings.UpdateSpeed) {
            case <= 500: mnuView_SpeedHigh.PerformClick(); break;
            case > 2000: mnuView_SpeedLow.PerformClick(); break;
            default: mnuView_SpeedNormal.PerformClick(); break;
        }


        // Load Column definitions for all the listviews
        Settings.LoadColsInformation(TaskManagerColumnTypes.Process, tabProcs.lv, ref Tables.ColsProcesses);
        tabProcs.btnAllUsers.Checked = Settings.ShowAllProcess;

    }
    private void OnLoadAddHandlers() {
        Tables.System.MetricValueChanged += evPerf_MetricValueChanged;
        Tables.System.RefreshCompleted += evPerf_RefreshCompleted;
        tabPerf.MouseDoubleClick += evPerf_MouseDoubleClick;
        tabProcs.ForceRefreshClicked += evProc_ForceRefresh;
        ssBtnState.DropDownOpening += evStatusBarStateOpening;
        ssBtnState.DropDownClosed += evStatusBarStateClosed;
        ssBtnState.ButtonDoubleClick += evStatusBarStateDoubleClick;
    }
    private void OnLoadSetFixedValues() {
        // TODO: We should get rid of this function and move these somewherre else.
        // Processes ListView Settings
        tabProcs.lv.ContentType = typeof(TaskManagerProcess);
        tabProcs.lv.DataSource = Tables.Processes.DataExporter;
        tabProcs.lv.SpaceFirstValue = Settings.IconsInProcess;
        //proc_ImageList.Images.Clear();
        //proc_ImageList.Images.Add(Properties.Process_xEmpty);
        //proc_ImageList.Images.Add(Properties.Process_xInfo);


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
        SetStatusText();
        StatusTimer.Stop();
    }

    private void evStatusBarStateOpening(object? sender, EventArgs e) {
        while (mnuView.DropDownItems.Count > 0) ssBtnState.DropDownItems.Add(mnuView.DropDownItems[0]);
    }
    private void evStatusBarStateClosed(object? sender, EventArgs e) {
        while (ssBtnState.DropDownItems.Count > 0) mnuView.DropDownItems.Add(ssBtnState.DropDownItems[0]);
    }
    private void evStatusBarStateDoubleClick(object? sender, EventArgs e) {
        MonitorRunning = !MonitorRunning;
    }

    private void evPerf_MouseDoubleClick(object? sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && e.Clicks > 1) { FullScreen = !FullScreen; }
    }
    private void evPerf_MetricValueChanged(object sender, Metric metric, MetricChangedEventArgs e) {
        switch (e.MetricName) {
            case "PhysicalTotal": tabPerf.gbMemory_Total.Text = Tables.System.PhysicalTotal.ValueFmt; break;
            case "PhysicalAvail": tabPerf.gbMemory_Avail.Text = Tables.System.PhysicalAvail.ValueFmt; break;
            case "SystemCached": tabPerf.gbMemory_Cached.Text = Tables.System.SystemCached.ValueFmt; break;

            case "CommitTotal": tabPerf.gbCommit_Current.Text = Tables.System.CommitTotal.ValueFmt; break;
            case "CommitPeak": tabPerf.gbCommit_Peak.Text = Tables.System.CommitPeak.ValueFmt; break;
            case "CommitLimit": tabPerf.gbCommit_Limit.Text = Tables.System.CommitLimit.ValueFmt; break;
            case "KernelTotal": tabPerf.gbKernel_Total.Text = Tables.System.KernelTotal.ValueFmt; break;
            case "KernelPaged": tabPerf.gbKernel_Paged.Text = Tables.System.KernelPaged.ValueFmt; break;
            case "KernelNonPaged": tabPerf.gbKernel_NonPaged.Text = Tables.System.KernelNonPaged.ValueFmt; break;
            case "PageFileTotal": tabPerf.gbPagefile_Limit.Text = Tables.System.PageFileTotal.ValueFmt; break;
            case "PageFileUsed": tabPerf.gbPagefile_Current.Text = Tables.System.PageFileUsed.ValueFmt; break;
            case "PageFilePeak": tabPerf.gbPagefile_Peak.Text = Tables.System.PageFilePeak.ValueFmt; break;

            case "ioReadCount": tabPerf.gbIOops_Reads.Text = Tables.System.ioReadCount.ValueFmt; break;
            case "ioReadBytes": tabPerf.gbIOtranf_Reads.Text = Tables.System.ioReadBytes.ValueFmt; break;
            case "ioWriteCount": tabPerf.gbIOops_Writes.Text = Tables.System.ioWriteCount.ValueFmt; break;
            case "ioWriteBytes": tabPerf.gbIOtranf_Writes.Text = Tables.System.ioWriteBytes.ValueFmt; break;
            case "ioOtherCount": tabPerf.gbIOops_Others.Text = Tables.System.ioOtherCount.ValueFmt; break;
            case "ioOtherBytes": tabPerf.gbIOtranf_Others.Text = Tables.System.ioOtherBytes.ValueFmt; break;

            case "HandleCount": tabPerf.gbSystem_Handles.Text = Tables.System.HandleCount.ValueFmt; break;
            case "ThreadCount": tabPerf.gbSystem_Threads.Text = Tables.System.ThreadCount.ValueFmt; break;
            case "ProcessCount": tabPerf.gbSystem_Processes.Text = Tables.System.ProcessCount.ValueFmt; break;
            case "DevicesCount": tabPerf.gbSystem_Devices.Text = Tables.System.DevicesCount.ValueFmt; break;
            case "ServicesCount": tabPerf.gbSystem_Services.Text = Tables.System.ServicesCount.ValueFmt; break;


        }
    }
    private void evPerf_RefreshCompleted(object? sender, EventArgs e) {
        // Always set these values. regardless we are visible or not.
        tabPerf.gbSystem_UpTime.Text = Tables.System.UpTime;
        tabPerf.meterCpu.SetValue(Tables.System.CpuUsage.Value);
        tabPerf.chartCpu.AddValue(Tables.System.CpuUsage.Value, Tables.System.CpuUsageKernel.Value);
        tabPerf.meterMem.SetValue(Tables.System.MemoryUsage, Tables.System.MemoryUsageString);
        tabPerf.chartMem.AddValue((double)Tables.System.MemoryUsage, Tables.System.SwapUsage);
        tabPerf.meterIO.SetValue(Tables.System.ioDataUsage, Tables.System.ioDataUsageString);
        tabPerf.chartIO.AddValue((double)Tables.System.ioOtherBytes.Delta / 1024, (double)Tables.System.ioWriteBytes.Delta / 1024, (double)Tables.System.ioReadBytes.Delta / 1024);
        tabPerf.meterDisk.SetValue(Tables.System.DiskUsage, Tables.System.DiskUsageString);
        tabPerf.chartDisk.AddValue((double)Tables.System.DiskRead.Delta / 1024, (double)Tables.System.DiskWrite.Delta / 1024);
        tabPerf.meterNet.SetValue(Tables.System.NetworkUsage, Tables.System.NetworkUsageString);
        tabPerf.chartNet.AddValue((double)Tables.System.NetReceived.Delta / 1024, (double)Tables.System.NetSent.Delta / 1024);
        // Always set the status bar labels as well
        ssProcesses.Text = "Processes: " + Tables.System.ProcessCount.Value;
        ssServices.Text = "Services: " + Tables.System.ServicesCount.Value;
        ssCpuLoad.Text = "CPU Load: " + Tables.System.CpuUsage.Value + "%";
        // Always flush the ETW, if its active.
        ETW.Flush();
    }

    private void evProc_ForceRefresh(object? sender, EventArgs e) {
        tabProcs.lv.SuspendLayout();
        Tables.Processes.Clear();
        tabProcs.lv.Items.Clear();
        Refresh_Processes(true);
        tabProcs.lv.ResumeLayout();
    }

    private void Refresh_Applications(bool firstTime = false) {
        TimmingStart();
        Thread.Sleep(Extensions.RandomGenerator.Next(20, 50));
        TimmingStop();
    }
    private void Refresh_Performance(bool firstTime = false) {
        if (InvokeRequired) { BeginInvoke(() => Refresh_Performance(firstTime)); return; }
        TimmingStart();
        // Call the Refresher, and cancel events cascading if we are not visible. 
        Tables.System.Refresh();
        TimmingStop();
    }
    private void Refresh_Processes(bool firstTime = false) {
        if (InvokeRequired) { BeginInvoke(() => Refresh_Processes(firstTime)); return; }
        TimmingStart();

        // Check if we are the active tab or its firstTime
        if (tc.SelectedTab != tpProcesses && !firstTime) return;
        if (tabProcs.lv.Items.Count == 0) firstTime = true;
        // Store last round items and initialize new ones
        HashSet<int> LastRun = new();
        LastRun.UnionWith(Tables.HashProcesses);
        Tables.HashProcesses.Clear();


        // Allocate Main Pointer and offset
        long lastOffset = 0;
        IntPtr hmain = IntPtr.Zero;
        if (TaskManagerProcess.GetProcessesPointer(ref hmain)) {
            API.SYSTEM_PROCESS_INFORMATION spi;
            spi = (API.SYSTEM_PROCESS_INFORMATION)Marshal.PtrToStructure(hmain, typeof(API.SYSTEM_PROCESS_INFORMATION))!;
            Array.Resize(ref spi.Threads, (int)spi.NumberOfThreads);
            lastOffset = hmain;
            while (spi.NextEntryOffset >= 0) {
                if (tabProcs.AllUsers || spi.SessionId == Shared.CurrentSessionID || spi.UniqueProcessId == 0) {
                    TaskManagerProcess? thisProcess;
                    Tables.HashProcesses.Add(spi.UniqueProcessId.ToInt32());
                    if (Tables.Processes.Contains(spi.UniqueProcessId)) {
                        thisProcess = Tables.Processes.GetProcess(spi.UniqueProcessId)!;
                        if (thisProcess.BackColor == Settings.Highlights.NewColor) thisProcess.BackColor = Color.Empty;
                        try {
                            thisProcess.Update(spi, Tables.ColsProcesses);
                        } catch (Exception ex) { Shared.DebugTrap(ex, 021); }
                    } else {
                        thisProcess = new TaskManagerProcess(spi.UniqueProcessId);
                        try {
                            thisProcess.Load(spi, Tables.ColsProcesses);
                        } catch (Exception ex) { Shared.DebugTrap(ex, 022); }
                        if (Settings.Highlights.NewItems && !firstTime) thisProcess.BackColor = Settings.Highlights.NewColor;
                        thisProcess.ImageIndex = (spi.UniqueProcessId == 0) ? 0 : 1;
                        //if (spi.UniqueProcessId.ToInt32() > Shared.bpi && tabProcs.lv.SmallImageList != null && GetProcessIcon(thisProcess.ID, thisProcess.Name, thisProcess.ImagePath)) {
                        //    thisProcess.ImageKey = thisProcess.ID + "-" + thisProcess.Name;
                        //    thisProcess.ImageIndex = -1;
                        //}
                        Tables.Processes.Add(thisProcess);
                    }
                }
                if (spi.NextEntryOffset > 0) {
                    lastOffset += spi.NextEntryOffset;
                    spi = (API.SYSTEM_PROCESS_INFORMATION)Marshal.PtrToStructure((IntPtr)lastOffset, typeof(API.SYSTEM_PROCESS_INFORMATION))!;
                    Array.Resize(ref spi.Threads, (int)spi.NumberOfThreads);
                } else {
                    break;
                }
            }
            Marshal.FreeHGlobal(hmain);
        }
        // Clean old Items
        LastRun.ExceptWith(Tables.HashProcesses);
        foreach (var pid in LastRun) {
            TaskManagerProcess? thisProcess = Tables.Processes.GetProcess(pid);
            if (thisProcess == null) continue;
            if (thisProcess.BackColor == Settings.Highlights.RemovedColor || Settings.Highlights.RemovedItems == false) {
                tabProcs.lv.RemoveItemByKey(thisProcess.ID);
                Tables.Processes.Remove(thisProcess);
            } else {
                thisProcess.BackColor = Settings.Highlights.RemovedColor;
                Tables.HashProcesses.Add(thisProcess.PID);
            }
            thisProcess = null;

        }
        // Set LV First Column Width, only if firstTime
        if (firstTime) tabProcs.lv.Columns[0].Width = -1;

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

    internal bool FullScreen {
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
    internal void Settings_Apply() {
        // General Settings 
        ((ToolStripMenuItem)mnuOptions.DropDownItems["mnuOptions_OnTop"]).Checked = Settings.AlwaysOnTop;
        ((ToolStripMenuItem)mnuOptions.DropDownItems["mnuOptions_HideMinimize"]).Checked = Settings.ToTrayWhenMinimized;
        ((ToolStripMenuItem)mnuOptions.DropDownItems["mnuOptions_MinimizeClose"]).Checked = Settings.MinimizeWhenClosing;
        TopMost = Settings.AlwaysOnTop;
        ssBusyTime.Visible = Settings.TimmingInStatus;
        ssServices.Visible = Settings.ServicesInStatus;
        tabProcs.lv.AlternateRowColors = Settings.AlternateRowColors;
        tabProcs.lv.SpaceFirstValue = Settings.IconsInProcess;

        // Performance Graphs Settings
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
        tabPerf.chartCpu.PenGridVertical.DashStyle = (DashStyle)Settings.Performance.VerticalGridStyle;
        tabPerf.chartCpu.PenGridVertical.Color = Settings.Performance.VerticalGridColor;
        tabPerf.chartCpu.PenGridHorizontal.DashStyle = (DashStyle)Settings.Performance.HorizontalGridStyle;
        tabPerf.chartCpu.PenGridHorizontal.Color = Settings.Performance.HorizontalGridColor;
        tabPerf.chartCpu.PenAverage.DashStyle = (DashStyle)Settings.Performance.AverageLineStyle;
        tabPerf.chartCpu.PenAverage.Color = Settings.Performance.AverageLineColor;
        tabPerf.chartCpu.LightColors = Settings.Performance.LightColors;
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

    internal bool MonitorBusy;
    internal bool MonitorRunning {
        get { return _MonitorTriggerTimer.Enabled; }
        set { MonitorToggle(); }
    }
    internal double MonitorSpeed {
        get { return _MonitorTriggerTimer.Interval; }
        set {
            _MonitorTriggerTimer.Interval = value;
            if (MonitorRunning) MonitorUpdateBtnStatus();
        }
    }
    internal void MonitorStart(bool firstTime = false) {
        if (MonitorRunning) return;
        MonitorRefreshParallel(firstTime);
        _MonitorTriggerTimer.Start();
        SetStatusText();
        MonitorUpdateBtnStatus();
    }
    internal void MonitorStop() {
        if (!MonitorRunning) return;
        _MonitorTriggerTimer.Stop();
        SetStatusText();
        MonitorUpdateBtnStatus();
    }
    internal void MonitorToggle() {
        if (_MonitorTriggerTimer.Enabled) { MonitorStop(); } else { MonitorStart(); }
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
        SetStatusText(_StopWatch1.ElapsedMilliseconds + "ms.", ssBusyTime);
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
        SetStatusText(_StopWatch1.ElapsedMilliseconds + "ms.", ssBusyTime);
        TimmingDisplay();
    }
    private void MonitorTriggerExecutor(object? sender, ElapsedEventArgs e) {
        if (MonitorRunning) { MonitorRefreshParallel(); }
    }
    private void MonitorUpdateBtnStatus() {
        if (ss.InvokeRequired) { ss.Invoke(MonitorUpdateBtnStatus); return; }
        if (MonitorRunning) {
            if (_MonitorTriggerTimer.Interval is <= 500) ssBtnState.Text = "High";
            if (_MonitorTriggerTimer.Interval is > 500 and <= 2000) ssBtnState.Text = "Normal";
            if (_MonitorTriggerTimer.Interval is > 2000) ssBtnState.Text = "Low";
        } else { ssBtnState.Text = "Paused"; }
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
    private void TimmingStart([CallerMemberName] string methodName = "") {
        if (!Tables.Timmings.ContainsKey(methodName)) {
            Tables.Timmings.Add(methodName, new());
        } else {
            Tables.Timmings[methodName].Restart();
        }
    }
    private void TimmingStop([CallerMemberName] string methodName = "") {
        if (Tables.Timmings.ContainsKey(methodName)) Tables.Timmings[methodName].Stop();
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
        foreach (var t in Tables.Timmings) {
            if (!timmingStrip.Items.ContainsKey(t.Key)) continue;
            var itm = timmingStrip.Items[t.Key];
            itm.Visible = true;
            itm.Tag ??= t.Value.ElapsedMilliseconds;
            itm.Tag = Math.Round((t.Value.ElapsedMilliseconds + Convert.ToDouble(itm.Tag)) / 2, 2);
            itm.Text = itm.AccessibleName + string.Format("{0:N0}", t.Value.ElapsedMilliseconds) + "ms.";
            itm.ToolTipText = $"Average: {string.Format("{0:N2}", itm.Tag)}ms ";
        };

    }

    public void SetStatusText(string value = "", ToolStripLabel? obj = null) {
        obj ??= ssText;
        if (ss.InvokeRequired) { ss.Invoke(SetStatusText, new object[] { value, obj }); return; }
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
