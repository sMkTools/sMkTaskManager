using System.Diagnostics;
using System.Runtime.Versioning;
using System.Timers;
using sMkTaskManager.Classes;
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

    private void frmMain_Load(object sender, EventArgs e) {
        Extensions.StartMeasure(_StopWatch1);
        timer1.Interval = 1000;
        timer1.Start();
        Settings.LoadAll();
        frmMain_LoadSettings();
        // Parallel Init by using a Timer
        _ParallelTimer = new() { Interval = 1, AutoReset = false };
        _ParallelTimer.Elapsed += ParallelInit;
        _ParallelTimer.Start();

        ETW.Start();

        _LoadComplete = true;
        Extensions.StopMeasure(_StopWatch1);

    }
    private void frmMain_LoadSettings() {
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

    private void frmAddHandlers() {
        _System.MetricValueChanged += perf_MetricValueChangedEventHandler;
        tabPerformance.MouseUp += perf_MouseUpEventHandler;
        tabPerformance.MouseDown += perf_MouseDownEventHandler;
        tabPerformance.MouseMove += perf_MouseMoveEventHandler;
    }
    private void ParallelInit(object? sender, ElapsedEventArgs e) {
        _ParallelTimer.Stop();
        Stopwatch sw = new();
        Extensions.StartMeasure(sw);

        frmAddHandlers();
        if (InvokeRequired) { BeginInvoke(Settings_Apply); } else { Settings_Apply(); }

        _InitComplete = true;
        Extensions.StopMeasure(sw);
    }

    private void perf_MouseMoveEventHandler(object? sender, MouseEventArgs e) { }
    private void perf_MouseDownEventHandler(object? sender, MouseEventArgs e) { }
    private void perf_MouseUpEventHandler(object? sender, MouseEventArgs e) { }
    private void perf_MetricValueChangedEventHandler(object sender, Metric metric, MetricChangedEventArgs e) { }

    private void timer1_Tick(object sender, EventArgs e) {
        _System.Refresh();
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
