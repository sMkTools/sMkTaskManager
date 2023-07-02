using System.Reflection.Metadata.Ecma335;
using sMkTaskManager.Classes;
using sMkTaskManager.Forms;

namespace sMkTaskManager;

partial class frmMain {

    internal void Feature_RealExit() {
        if (InvokeRequired) { BeginInvoke(Feature_RealExit); return; }
        _RealExit = true;
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
    internal void Feature_SelectColumns(TaskManagerColumnTypes? Type = null) {
        if (tc.SelectedTab == tpProcesses) Type = TaskManagerColumnTypes.Process;
        if (tc.SelectedTab == tpServices) Type = TaskManagerColumnTypes.Services;
        if (tc.SelectedTab == tpConnections) Type = TaskManagerColumnTypes.Connections;
        if (Type == null) return;

        if (Type == TaskManagerColumnTypes.Process) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Process);
            frmcols.LoadCheckedColumns(tabProcs.lv.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                tabProcs.lv.SetColumns(frmcols.ColItems);
                Tables.ColsProcesses = tabProcs.lv.Columns.Cast<ColumnHeader>().Select(x => x.Name).ToHashSet()!;
                tabProcs.btnForceRefresh.PerformClick();
            }
        } else if (Type == TaskManagerColumnTypes.Services) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Services);
            // frmcols.LoadCheckedColumns(tabServices.lv.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //tabServices.lv.SetColumns(frmcols.ColItems);
                //RebuildActiveColumns(serv_ListView, m_ServsColumns);
                //tabServices.btnForceRefresh.PerformClick();
            }
        } else if (Type == TaskManagerColumnTypes.Connections) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Connections);
            // frmcols.LoadCheckedColumns(tabConns.lv.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //tabConns.lv.SetColumns(frmcols.ColItems);
                //RebuildActiveColumns(conn_ListView, m_ConnsColumns);
                //tabConns.btnForceRefresh.PerformClick();
            }
        }
    }

}