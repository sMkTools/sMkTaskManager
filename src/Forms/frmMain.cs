using System.Diagnostics;
using System.Runtime.Versioning;
using System.Timers;
using sMkTaskManager.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
public partial class frmMain : Form {

    public frmMain() {
        InitializeComponent();
        Initialize_MainMenu();
        // Flicker Free Controls by DoubleBuffer
        Extensions.CascadingDoubleBuffer(this);
    }

    // Local variables...
    private System.Timers.Timer _ParallelTimer;
    private bool _InitComplete = false, _LoadComplete = false;
    private readonly Stopwatch _StopWatch1 = new(), _StopWatch2 = new(), _StopWatch3 = new();
    private readonly TaskManagerSystem _System = new();
    private Size? _prevSize, _prevMinSize; Point? _prevLocation;
    private Size? _fullScreenSize; Point? _fullScreenLocation;

    private void OnLoadEventHandler(object sender, EventArgs e) {
        Extensions.StartMeasure(_StopWatch1);
        Settings.LoadAll();
        OnLoadLoadSettings();
        OnLoadAddHandlers();
        Settings_Apply();
        // Parallel Init by using a Timer
        _ParallelTimer = new() { Interval = 1, AutoReset = false };
        _ParallelTimer.Elapsed += ParallelInit;
        _ParallelTimer.Start();

        ETW.Start();

        _LoadComplete = true;
        Extensions.StopMeasure(_StopWatch1);
        timer1.Interval = 1000;
        timer1.Start();

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
    }
    private void OnLoadAddHandlers() {
        _System.MetricValueChanged += perf_MetricValueChangedEventHandler;
        tabPerformance.MouseDoubleClick += perf_MouseDoubleClickEventHandler;
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

    private void ParallelInit(object? sender, ElapsedEventArgs e) {
        _ParallelTimer.Stop();
        Stopwatch sw = new();
        Extensions.StartMeasure(sw);

        // We should use this to initialize something that is not really critical?

        _InitComplete = true;
        Extensions.StopMeasure(sw);
    }

    private void perf_MouseDoubleClickEventHandler(object? sender, MouseEventArgs e) {
        if (e.Button == MouseButtons.Left && e.Clicks > 1) { FullScreen = !FullScreen; }
    }

    private void perf_MetricValueChangedEventHandler(object sender, Metric metric, MetricChangedEventArgs e) {
        switch (e.MetricName) {
            case "PhysicalTotal":
                Debug.WriteLine("Entre");
                tabPerformance.gbMemory_Total.Text = _System.PhysicalTotal.ValueFmt;
                break;
            case "PhysicalAvail": tabPerformance.gbMemory_Avail.Text = _System.PhysicalAvail.ValueFmt; break;
            case "SystemCached": tabPerformance.gbMemory_Cached.Text = _System.SystemCached.ValueFmt; break;

            case "CommitTotal": tabPerformance.gbCommit_Current.Text = _System.CommitTotal.ValueFmt; break;
            case "CommitPeak": tabPerformance.gbCommit_Peak.Text = _System.CommitPeak.ValueFmt; break;
            case "CommitLimit": tabPerformance.gbCommit_Limit.Text = _System.CommitLimit.ValueFmt; break;
            case "KernelTotal": tabPerformance.gbKernel_Total.Text = _System.KernelTotal.ValueFmt; break;
            case "KernelPaged": tabPerformance.gbKernel_Paged.Text = _System.KernelPaged.ValueFmt; break;
            case "KernelNonPaged": tabPerformance.gbKernel_NonPaged.Text = _System.KernelNonPaged.ValueFmt; break;
            case "PageFileTotal": tabPerformance.gbPagefile_Limit.Text = _System.PageFileTotal.ValueFmt; break;
            case "PageFileUsed": tabPerformance.gbPagefile_Current.Text = _System.PageFileUsed.ValueFmt; break;
            case "PageFilePeak": tabPerformance.gbPagefile_Peak.Text = _System.PageFilePeak.ValueFmt; break;

            case "ioReadCount": tabPerformance.gbIOops_Reads.Text = _System.ioReadCount.ValueFmt; break;
            case "ioReadBytes": tabPerformance.gbIOtranf_Reads.Text = _System.ioReadBytes.ValueFmt; break;
            case "ioWriteCount": tabPerformance.gbIOops_Writes.Text = _System.ioWriteCount.ValueFmt; break;
            case "ioWriteBytes": tabPerformance.gbIOtranf_Writes.Text = _System.ioWriteBytes.ValueFmt; break;
            case "ioOtherCount": tabPerformance.gbIOops_Others.Text = _System.ioOtherCount.ValueFmt; break;
            case "ioOtherBytes": tabPerformance.gbIOtranf_Others.Text = _System.ioOtherBytes.ValueFmt; break;

            case "HandleCount": tabPerformance.gbSystem_Handles.Text = _System.HandleCount.ValueFmt; break;
            case "ThreadCount": tabPerformance.gbSystem_Threads.Text = _System.ThreadCount.ValueFmt; break;
            case "ProcessCount": tabPerformance.gbSystem_Processes.Text = _System.ProcessCount.ValueFmt; break;
            case "DevicesCount": tabPerformance.gbSystem_Devices.Text = _System.DevicesCount.ValueFmt; break;
            case "ServicesCount": tabPerformance.gbSystem_Services.Text = _System.ServicesCount.ValueFmt; break;


        }
    }

    private void Refresh_Performance(bool firstTime = false) {
        // Call the Refresher, and cancel events cascading if we are not visible. 
        _System.Refresh(tc.SelectedTab != tpPerformance & !firstTime);
        // Always set these values. regardless we are visible or not.
        tabPerformance.gbSystem_UpTime.Text = _System.UpTime;
        tabPerformance.meterCpu.SetValue(_System.CpuUsage.Value);
        tabPerformance.chartCpu.AddValue(_System.CpuUsage.Value, _System.CpuUsageKernel.Value);
        tabPerformance.meterMem.SetValue(_System.MemoryUsage, _System.MemoryUsageString);
        tabPerformance.chartMem.AddValue((double)_System.MemoryUsage, _System.SwapUsage);
        tabPerformance.meterIO.SetValue(_System.ioDataUsage, _System.ioDataUsageString);
        tabPerformance.chartIO.AddValue((double)_System.ioOtherBytes.Delta / 1024, (double)_System.ioWriteBytes.Delta / 1024, (double)_System.ioReadBytes.Delta / 1024);
        tabPerformance.meterDisk.SetValue(_System.DiskUsage, _System.DiskUsageString);
        tabPerformance.chartDisk.AddValue((double)_System.DiskRead.Delta / 1024, (double)_System.DiskWrite.Delta / 1024);
        tabPerformance.meterNet.SetValue(_System.NetworkUsage, _System.NetworkUsageString);
        tabPerformance.chartNet.AddValue((double)_System.NetReceived.Delta / 1024, (double)_System.NetSent.Delta / 1024);
        // Always set the status bar labels as well
        ssProcesses.Text = "Processes: " + _System.ProcessCount.Value;
        ssServices.Text = "Services: " + _System.ServicesCount.Value;
        ssCpuLoad.Text = "CPU Load: " + _System.CpuUsage.Value + "%";
        // Always flush the ETW, if its active.
        ETW.Flush();
    }

    private void timer1_Tick(object sender, EventArgs e) {
        Refresh_Performance();
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
                Controls.Add(tabPerformance);
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
                tpPerformance.Controls.Add(tabPerformance);
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
        tabPerformance.chartCpu.SetIndexes("Total", Settings.Performance.ShowKernelTime ? "Kernel" : null);
        tabPerformance.chartCpu.BackSolid = Settings.Performance.Solid;
        tabPerformance.chartCpu.AntiAliasing = Settings.Performance.AntiAlias;
        tabPerformance.chartCpu.ShadeBackground = Settings.Performance.ShadeBackground;
        tabPerformance.chartCpu.DisplayAverage = Settings.Performance.DisplayAverages;
        tabPerformance.chartCpu.DisplayLegends = Settings.Performance.DisplayLegends;
        tabPerformance.chartCpu.DisplayIndexes = Settings.Performance.DisplayIndexes;
        tabPerformance.chartCpu.DetailsOnHover = Settings.Performance.DisplayOnHover;
        tabPerformance.chartCpu.ValueSpacing = Settings.Performance.ValueSpacing;
        tabPerformance.chartCpu.GridSpacing = Settings.Performance.GridSize;
        tabPerformance.chartCpu.PenGridVertical.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.VerticalGridStyle;
        tabPerformance.chartCpu.PenGridVertical.Color = Settings.Performance.VerticalGridColor;
        tabPerformance.chartCpu.PenGridHorizontal.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.HorizontalGridStyle;
        tabPerformance.chartCpu.PenGridHorizontal.Color = Settings.Performance.HorizontalGridColor;
        tabPerformance.chartCpu.PenAverage.DashStyle = (System.Drawing.Drawing2D.DashStyle)Settings.Performance.AverageLineStyle;
        tabPerformance.chartCpu.PenAverage.Color = Settings.Performance.AverageLineColor;
        tabPerformance.chartCpu.LightColors = Settings.Performance.LightColors;
        // TODO: Force for now, remove later
        tabPerformance.chartCpu.DisplayAverage = true;
        tabPerformance.chartCpu.DisplayLegends = true;
        tabPerformance.chartCpu.DisplayIndexes = true;

        tabPerformance.chartMem.CopySettings(tabPerformance.chartCpu);
        tabPerformance.chartIO.CopySettings(tabPerformance.chartCpu);
        tabPerformance.chartDisk.CopySettings(tabPerformance.chartCpu);
        tabPerformance.chartNet.CopySettings(tabPerformance.chartCpu);
        tabPerformance.meterCpu.LightColors = tabPerformance.chartCpu.LightColors;
        tabPerformance.meterMem.LightColors = tabPerformance.chartCpu.LightColors;
        tabPerformance.meterIO.LightColors = tabPerformance.chartCpu.LightColors;
        tabPerformance.meterDisk.LightColors = tabPerformance.chartCpu.LightColors;
        tabPerformance.meterNet.LightColors = tabPerformance.chartCpu.LightColors;

    }
}
