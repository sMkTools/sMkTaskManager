using sMkTaskManager.Forms;
using sMkTaskManager.Classes;
namespace sMkTaskManager;

partial class frmMain {

    internal void Feature_RealExit() {
        if (InvokeRequired) { BeginInvoke(Feature_RealExit); return; }
        Shared.RealExit = true;
        Close();
    }
    internal void Feature_ActivateMainWindow() {
        Show();
        ShowInTaskbar = true;
        if (WindowState == FormWindowState.Minimized) {
            WindowState = Settings.MainWindow.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        Activate();
    }
    internal void Feature_AboutDialog() {
        using frmAbout d = new();
        d.ShowDialog(this);
    }
    internal void Feature_Preferences(int section = 0) {
        using frmPreferences frm = new();
        frm.StartPosition = FormStartPosition.CenterParent;
        frm.tc.SelectedIndex = section;
        frm.ShowDialog(this);
    }
    internal void Feature_ToggleAlwaysOnTop() {
        mnuOptions_OnTop.Checked = !mnuOptions_OnTop.Checked;
        TopMost = mnuOptions_OnTop.Checked;
        Settings.AlwaysOnTop = mnuOptions_OnTop.Checked;
    }
    internal void Feature_ToggleHighlightChanges() {
        mnuOptions_Highlight.Checked = !mnuOptions_Highlight.Checked;
        Settings.Highlights.ChangingItems = mnuOptions_Highlight.Checked;
    }
    internal void Feature_ToggleHideOnMinimize() {
        mnuOptions_HideMinimize.Checked = !mnuOptions_HideMinimize.Checked;
        Settings.ToTrayWhenMinimized = mnuOptions_HideMinimize.Checked;
    }
    internal void Feature_ToggleMinimizeOnClose() {
        mnuOptions_MinimizeClose.Checked = !mnuOptions_MinimizeClose.Checked;
        Settings.MinimizeWhenClosing = mnuOptions_MinimizeClose.Checked;
    }
    internal void Feature_SelectColumns() {
        if (Tabs.ActiveTab == null) return;
        ITaskManagerTab tab = Tabs.ActiveTab;
        if (!tab.CanSelectColumns) return;
        if (tab.ColumnType == TaskManagerColumnTypes.None) return;

        using frmColumns frmcols = new(tab.ColumnType);
        frmcols.LoadCheckedColumns(tab.GetColumns()!);
        if (frmcols.ShowDialog(this) == DialogResult.OK) {
            tab.SetColumns(frmcols.ColItems);
            tab.ForceRefresh();
        }

    }
    internal void Feature_NewTask() {
        API.RunFileDlg(Handle, IntPtr.Zero, null, null, null, 0);
    }
    internal void Feature_NewTaskAsUser() { }
    internal void Feature_MonitorPowerOff() {
        // const int SC_MONITORPOWER = 0xF170;
        // const int WM_SYSCOMMAND = 0x112;
        API.SendMessage(Handle, 0x112, 0xF170, 2);

    }
    internal void Feature_MonitorScreenSaver() {
        // const uint SC_SCREENSAVE = 0xF140;
        // const int WM_SYSCOMMAND = 0x112;
        API.SendMessage(Handle, 0x112, 0xF140, IntPtr.Zero);
    }
    internal void Feature_Computer(string action = "") {
        switch (action.ToUpper()) {
            case "LOCK": API.LockWorkStation(); break;
            case "LOG OFF": API.ExitWindowsEx(API.ExitWindows.LogOff, 0); break;
            case "SLEEP": Application.SetSuspendState(PowerState.Suspend, false, false); break;
            case "HIBERNATE": Application.SetSuspendState(PowerState.Hibernate, false, false); break;
            case "RESTART": API.ExitWindowsEx(API.ExitWindows.Reboot, 0); break;
            case "SHUTDOWN": API.ExitWindowsEx(API.ExitWindows.ShutDown, 0); break;
            case "FORCE SHUTDOWN": API.ExitWindowsEx(API.ExitWindows.ShutDown | API.ExitWindows.Force, 0); break;
        }
    }

}