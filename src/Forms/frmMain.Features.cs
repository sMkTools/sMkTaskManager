using sMkTaskManager.Forms;

namespace sMkTaskManager;

partial class frmMain {
    internal void Feature_SelectColumns(frmColumns.ColumnListType? Type = null) {
        if (tc.SelectedTab == tpProcesses) Type = frmColumns.ColumnListType.Process;
        if (tc.SelectedTab == tpServices) Type = frmColumns.ColumnListType.Services;
        if (tc.SelectedTab == tpConnections) Type = frmColumns.ColumnListType.Connections;
        if (Type == null) return;

        if (Type == frmColumns.ColumnListType.Process) {
            using frmColumns frmcols = new(frmColumns.ColumnListType.Process);
            frmcols.LoadCheckedColumns(tabProcs.lv.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(tabProcs.lv, frmcols.ColItems);
                //RebuildActiveColumns(tabProcs.lv, Tables.ColsProcesses);
                //tabProcs.btnForceRefresh.PerformClick();
            }
        } else if (Type == frmColumns.ColumnListType.Services) {
            using frmColumns frmcols = new(frmColumns.ColumnListType.Services);
            // frmcols.LoadCheckedColumns(serv_ListView.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(serv_ListView, frmcols.ColItems);
                //RebuildActiveColumns(serv_ListView, m_ServsColumns);
                //serv_btnForceRefresh.PerformClick();
            }
        } else if (Type == frmColumns.ColumnListType.Connections) {
            using frmColumns frmcols = new(frmColumns.ColumnListType.Connections);
            // frmcols.LoadCheckedColumns(conn_ListView.Columns);
            if (frmcols.ShowDialog(this) == DialogResult.OK) {
                //SelectColumnsHelper(conn_ListView, frmcols.ColItems);
                //RebuildActiveColumns(conn_ListView, m_ConnsColumns);
                //conn_btnForceRefresh.PerformClick();
            }
        }


    }
}