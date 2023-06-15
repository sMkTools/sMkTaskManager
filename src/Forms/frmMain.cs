using sMkTaskManager.Classes;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
public partial class frmMain : Form {
    public frmMain() {
        InitializeComponent();
        Initialize_MainMenu();

    }
    Random rnd = new();
    TaskManagerSystem t = new();

    private void frmMain_Load(object sender, EventArgs e) {
        // timer1.Stop();
        timer1.Interval = 1000;
        t.PageFileTotal.SetFormat(MetricFormats.Gb);
        t.MetricValueChanged += T_MetricValueChanged;

        //Debug.WriteLine($"{t.UpTime}");

    }

    private void T_MetricValueChanged(object sender, Metric metric, MetricChangedEventArgs e) {
        //if (e.MetricName == "ServicesCount") {
        //    Debug.WriteLine($"{e.MetricName} Value: {metric.ValueFmt} - Delta: {metric.DeltaFmt} ");
        //}

        //        if (e.MetricName == "Test" && e.ValueType == "Delta") { Debug.WriteLine($"Test Delta Changed: {t.Test.Value} - Delta: {t.Test.Delta} "); }
    }

    private void T_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {
        // if (e.PropertyName == "ioReadBytes") { Debug.WriteLine($"Read Bytes: { t.ioReadBytesFmt} - Delta: {t.Deltas.ioReadBytesFmt} "); }
        // if (e.PropertyName == "KernelTotal") { Debug.WriteLine($"Kernel Total: {t.KernelTotalFmt} - Delta: {t.Deltas.KernelTotalFmt} "); }
        // if (e.PropertyName == "ThreadCount") { Debug.WriteLine($"Thread Count: {t.ThreadCount} - Delta: {t.Deltas.ThreadCount} "); }
        // if (e.PropertyName == "Test") { Debug.WriteLine($"Test: {t2.TestFmt} - Delta: {t2.TestDeltaFmt} "); }
        // Debug.WriteLine($"{e.PropertyName}");
        // if (e.PropertyName == "ioReadCount2") { Debug.WriteLine($"ioReadCount2: {t.ioReadCount2.ValueFmt} - Delta: {t.ioReadCount2.DeltaFmt} "); }
    }

    private void timer1_Tick(object sender, EventArgs e) {
        t.Refresh();

        tabPerformance.chartCpu.AddValue(t.CpuUsage.Value, t.CpuUsageKernel.Value);
        tabPerformance.chartMem.AddValue(t.MemoryUsage);
        tabPerformance.chartIO.AddValue(t.ioReadBytes.Delta / 1024, t.ioWriteBytes.Delta / 1024);

        // tabPerformance.chartDisk.AddValue(t.DiskUsageR.Value / 1024, t.DiskUsageW.Value / 1024);
        // tabPerformance.chartNet.AddValue(t.NetworkUsageR.Value / 1024, t.NetworkUsageS.Value / 1024);

        tabPerformance.meterCpu.SetValue(t.CpuUsage.Value);
        tabPerformance.meterMem.SetValue(t.MemoryUsage, t.MemoryUsageString);
        tabPerformance.meterIO.SetValue(t.ioUsageBytes.Delta, t.ioUsageBytes.DeltaFmt);

        // tabPerformance.meterDisk.SetValue(t.DiskUsage.Value, t.DiskUsage.ValueFmt);
        // tabPerformance.meterNet.SetValue(t.NetworkUsage.Value, t.NetworkUsage.ValueFmt);


    }
}
