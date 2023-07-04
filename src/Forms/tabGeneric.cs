using sMkTaskManager.Controls;

namespace sMkTaskManager.Forms;

public interface ITaskManagerTab {
    public event EventHandler? ForceRefreshClicked;
    public event EventHandler? RefreshStarts;
    public event EventHandler? RefreshComplete;

    internal sMkListView ListView { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public bool Active { get; set; }

    public string TimmingKey { get; }
    public long TimmingValue { get; }

    public void Refresher(bool firstTime = false);
    public void LoadSettings();
    public bool SaveSettings();
}
