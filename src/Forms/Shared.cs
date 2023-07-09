using sMkTaskManager.Classes;
using sMkTaskManager.Forms;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static partial class Shared {

    internal static frmMain MainForm;
    internal static bool InitComplete = false;
    internal static bool LoadComplete = false;
    internal static bool RealExit = true;
    internal static int CurrentSessionID = Process.GetCurrentProcess().SessionId;
    internal static int PrivateMsgID = 0;

    internal static void SetStatusText(string value = "", ToolStripLabel? obj = null) {
        if (MainForm == null) FindOwnerForm();
        MainForm?.SetStatusText(value, obj);
    }
    internal static void FindOwnerForm() {
        if (MainForm == null) {
            foreach (Form f in Application.OpenForms) {
                if (f.Name == "frmMain") { MainForm = (frmMain)f; break; }
            }
        }
    }
    internal static void OpenProcessForm(int PID) {
        if (MainForm == null) FindOwnerForm();
        if (MainForm == null) return;
        bool xFound = false;
        MainForm.Cursor = Cursors.WaitCursor;
        foreach (Form child in MainForm!.OwnedForms) {
            if (child == null || child.Name == null) continue;
            if (child.Name.Equals("frmProc-" + PID)) {
                xFound = true;
                child.Focus(); child.Show();
                if (child.WindowState == FormWindowState.Minimized) child.WindowState = FormWindowState.Normal;
                break;
            }
        }
        if (!xFound) {
            Forms.frmProcess_Details frm = new() {
                Owner = MainForm,
                Name = "frmProc-" + PID,
                Tag = PID,
                PID = PID
            };
            frm.Show();
        }
        MainForm.Cursor = Cursors.Default;
    }
    internal static bool BusyCursor {
        get {
            return !(MainForm?.Cursor == Cursors.Default);
        }
        set {
            if (MainForm == null) FindOwnerForm();
            if (MainForm == null) return;
            MainForm.Cursor = value ? Cursors.AppStarting : Cursors.Default;
        }
    }

    internal static Dictionary<string, Stopwatch> Timmings = new();
    // internal static TaskManagerSystem System = new();

    internal static List<Color> TimmingsColors = new() {
        Color.FromArgb(255, 192, 192), Color.FromArgb(255, 224, 192), Color.FromArgb(255, 255, 192),
        Color.FromArgb(192, 255, 192), Color.FromArgb(192, 255, 255), Color.FromArgb(192, 192, 255),
        Color.FromArgb(255, 192, 255), Color.FromArgb(192, 192, 192)
    };


}

[SupportedOSPlatform("windows")]
internal static class Tabs {

    internal static Dictionary<string, ITaskManagerTab> Tab = new();
    internal static T? GetTab<T>() {
        foreach (KeyValuePair<string, ITaskManagerTab> t in Tab) {
            if (t.Value.GetType() == typeof(T)) { return (T)t.Value; }
        }
        return default;
    }
    internal static ITaskManagerTab? GetTab(string tabKey) {
        return Tab.FirstOrDefault(t => t.Key == tabKey).Value;
    }
    internal static ITaskManagerTab? ActiveTab {
        get {
            foreach (ITaskManagerTab t in Tab.Values) {
                if (((Control)t).Parent == null) continue;
                if (((Control)t).Parent!.Visible) return t;
            }
            return default;
        }
    }
}