using sMkTaskManager.Classes;
using sMkTaskManager.Controls;

namespace sMkTaskManager.Forms;

public interface ITaskManagerTab {
    public abstract event EventHandler? ForceRefreshClicked;
    public abstract event EventHandler? RefreshStarts;
    public abstract event EventHandler? RefreshComplete;

    public sMkListView? ListView { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }

    public string TimmingKey { get; }
    public long TimmingValue { get; }
    public bool CanSelectColumns { get; }
    public TaskManagerColumnTypes ColumnType { get; }

    public void Refresher(bool firstTime = false);
    public void ForceRefresh() { }
    public void LoadSettings() { }
    public bool SaveSettings() { return true; }
    public void ApplySettings() { }
    public void SetColumns(in ListView.ListViewItemCollection colItems) { }
    public ListView.ColumnHeaderCollection? GetColumns() { return null; }

}
