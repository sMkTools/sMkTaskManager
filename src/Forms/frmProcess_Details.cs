using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Versioning;
using sMkTaskManager.Classes;

namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmProcess_Details : Form {
    private Process p1;
    private TaskManagerProcess p2;
    private System.Timers.Timer UpdateTimer = new();
    private readonly HashSet<string> VisibleValues = new();

    public frmProcess_Details() {
        InitializeComponent();
        InitializeVisibleValues();
        Extensions.CascadingDoubleBuffer(this);
    }

    public void InitializeVisibleValues() {
        // Set all the values that we use so we get notification of them.
        VisibleValues.Add("Priority");
        VisibleValues.Add("CreationTime");
        VisibleValues.Add("RunTime");
        VisibleValues.Add("CpuTime");
        VisibleValues.Add("UserTime");
        VisibleValues.Add("KernelTime");
        VisibleValues.Add("WorkingSet");
        VisibleValues.Add("WorkingSetPeak");
        VisibleValues.Add("WorkingSetPrivate");
        VisibleValues.Add("WorkingSetShareable");
        VisibleValues.Add("PagedMemory");
        VisibleValues.Add("PagedMemoryPeak");
        VisibleValues.Add("VirtualMemory");
        VisibleValues.Add("VirtualMemoryPeak");
        VisibleValues.Add("CpuUsage");
        VisibleValues.Add("PrivateBytes");
        VisibleValues.Add("Handles");
        VisibleValues.Add("Threads");
        VisibleValues.Add("PageFaults");
        VisibleValues.Add("GDIObjects");
        VisibleValues.Add("UserObjects");
        VisibleValues.Add("GDIObjectsPeak");
        VisibleValues.Add("UserObjectsPeak");
        VisibleValues.Add("ReadOperations");
        VisibleValues.Add("ReadTransfer");
        VisibleValues.Add("WriteOperations");
        VisibleValues.Add("WriteTransfer");
        VisibleValues.Add("OtherOperations");
        VisibleValues.Add("OtherTransfer");
        VisibleValues.Add("DiskRead");
        VisibleValues.Add("DiskReadDelta");
        VisibleValues.Add("DiskReadRate");
        VisibleValues.Add("DiskWrite");
        VisibleValues.Add("DiskWriteDelta");
        VisibleValues.Add("DiskWriteRate");
        VisibleValues.Add("NetSent");
        VisibleValues.Add("NetSentDelta");
        VisibleValues.Add("NetSentRate");
        VisibleValues.Add("NetRcvd");
        VisibleValues.Add("NetRcvdDelta");
        VisibleValues.Add("NetRcvdRate");
    }

    private void OnLoad(object sender, EventArgs e) {
        Settings.LoadProcDetails();
        try {
            if (Settings.RememberPositions) {
                //Width = Math.Max(MinimumSize.Width, Settings.ProcessDetails.Size.Width);
                //Height = Math.Max(MinimumSize.Height, Settings.ProcessDetails.Size.Height);
                //if (Settings.ProcessDetails.Location.X != 0 || Settings.ProcessDetails.Location.Y != 0) Location = Settings.ProcessDetails.Location;
            }
        } catch { }
        try { tbSpeed.Value = Settings.ProcessDetails.UpdateSpeed; } catch { tbSpeed.Value = 1; }
        try { tc.SelectTab(Settings.ProcessDetails.LastTab); } catch { }
        tbSpeed_ValueChanged(tbSpeed, EventArgs.Empty);
    }
    private void OnFormClosing(object sender, FormClosingEventArgs e) {
        UpdateTimer.Stop();
        VisibleValues.Clear();
        if (Settings.RememberPositions && WindowState == FormWindowState.Normal) {
            Settings.ProcessDetails.Size = Size;
            Settings.ProcessDetails.Location = Location;
        }
        Settings.ProcessDetails.LastTab = tc.SelectedIndex;
        Settings.ProcessDetails.UpdateSpeed = tbSpeed.Value;
        Settings.SaveProcDetails();
        try { Dispose(); } catch { }
    }

    private void btnOK_Click(object sender, EventArgs e) { Close(); }
    private void btnCancel_Click(object sender, EventArgs e) { Close(); }
    private void tbSpeed_ValueChanged(object sender, EventArgs e) {
        if (tbSpeed.Value > 0) {
            UpdateTimer.Interval = tbSpeed.Value * 1000;
            lblSpeedValue.Text = tbSpeed.Value + "s";
            if (!UpdateTimer.Enabled) UpdateTimer.Start();
        } else {
            lblSpeedValue.Text = "Halt";
            UpdateTimer.Stop();
        }
    }


    public int PID { get; set; }
}

