using sMkTaskManager.Classes;
using sMkTaskManager.Forms;

namespace sMkTaskManager;

partial class frmMain {
    internal void Feature_SelectColumns(TaskManagerColumnTypes? Type = null) {
        if (tc.SelectedTab == tpProcesses) Type = TaskManagerColumnTypes.Process;
        if (tc.SelectedTab == tpServices) Type = TaskManagerColumnTypes.Services;
        if (tc.SelectedTab == tpConnections) Type = TaskManagerColumnTypes.Connections;
        if (Type == null) return;

        if (Type == TaskManagerColumnTypes.Process) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Process);
            frmcols.LoadCheckedColumns(tabProcs.lv.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(tabProcs.lv, frmcols.ColItems);
                //RebuildActiveColumns(tabProcs.lv, Tables.ColsProcesses);
                //tabProcs.btnForceRefresh.PerformClick();
            }
        } else if (Type == TaskManagerColumnTypes.Services) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Services);
            // frmcols.LoadCheckedColumns(serv_ListView.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(serv_ListView, frmcols.ColItems);
                //RebuildActiveColumns(serv_ListView, m_ServsColumns);
                //serv_btnForceRefresh.PerformClick();
            }
        } else if (Type == TaskManagerColumnTypes.Connections) {
            using frmColumns frmcols = new(TaskManagerColumnTypes.Connections);
            // frmcols.LoadCheckedColumns(conn_ListView.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(conn_ListView, frmcols.ColItems);
                //RebuildActiveColumns(conn_ListView, m_ConnsColumns);
                //conn_btnForceRefresh.PerformClick();
            }
        }


    }
}