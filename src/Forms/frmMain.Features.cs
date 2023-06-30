using System.Diagnostics;
using sMkTaskManager.Classes;
using sMkTaskManager.Controls;
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