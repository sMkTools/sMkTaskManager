using System.ComponentModel;
using System.Runtime.Versioning;
using sMkTaskManager.Classes;
namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmPreferences : Form {

    public frmPreferences() {
        InitializeComponent();
    }

    private void onLoad(object sender, EventArgs e) {
        if (Owner == null) { CenterToScreen(); }
        LoadSettings();
        Settings.LoadCustomColors();
        cd.CustomColors = Settings.CustomColors.ToArray();
    }
    private void onFormClosing(object sender, FormClosingEventArgs e) {
        Settings.CustomColors = cd.CustomColors.ToList();
        Settings.SaveCustomColors();
    }
    private void btnOk_Click(object sender, EventArgs e) {
        SaveSettings();
        if (Owner != null && Owner.GetType() == typeof(frmMain)) ((frmMain)Owner).Settings_Apply();
        DialogResult = DialogResult.OK;
    }
    private void btnApply_Click(object sender, EventArgs e) {
        SaveSettings();
        if (Owner != null && Owner.GetType() == typeof(frmMain)) ((frmMain)Owner).Settings_Apply();
    }
    private void btnCancel_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();
    }
    private void btnColors_Click(object sender, EventArgs e) {
        cd.Color = ((Button)sender).BackColor;
        if (cd.ShowDialog(this) == DialogResult.OK) ((Button)sender).BackColor = cd.Color;
    }
    private void tbValues_Scroll(object sender, EventArgs e) {
        if (sender == null) return;
        if (sender == p_tbGridSize) p_lblGridSize.Text = p_tbGridSize.Value.ToString();
        if (sender == n_tbGridSize) n_lblGridSize.Text = n_tbGridSize.Value.ToString();
        if (sender == p_tbValueSpacing) p_lblValueSpacing.Text = p_tbValueSpacing.Value.ToString();
        if (sender == n_tbValueSpacing) n_lblValueSpacing.Text = n_tbValueSpacing.Value.ToString();
    }

    public void LoadSettings() {
        try { // General Settings
            g_chkStartMinimized.Checked = Settings.StartMinimized;
            g_chkWindowOnTop.Checked = Settings.AlwaysOnTop;
            g_chkMinimizeClose.Checked = Settings.MinimizeWhenClosing;
            g_chkRememberWindow.Checked = Settings.RememberPositions;
            g_chkShowIcons.Checked = Settings.IconsInProcess;
            g_chkServicesStatus.Checked = Settings.ServicesInStatus;
            g_chkTimmingStatus.Checked = Settings.TimmingInStatus;
            g_chkAlternateRowColors.Checked = Settings.AlternateRowColors;
            t_chkHideToTray.Checked = Settings.ToTrayWhenMinimized;
            t_chkCloseToTray.Checked = Settings.ToTrayWhenClosed;
            t_chkRequireDobleClick.Checked = Settings.DblClickToRestore;
            t_chkShowCPU.Checked = Settings.ShowCPUOnTray;
        } catch { }
        try { // Highlight Items Checks & Colors
            g_chkHighlightNew.Checked = Settings.Highlights.NewItems;
            g_chkHighlightChanges.Checked = Settings.Highlights.ChangingItems;
            g_chkHighlightRemoved.Checked = Settings.Highlights.RemovedItems;
            g_chkHighlightFrozen.Checked = Settings.Highlights.FrozenItems;
            g_btnHighlightNew.BackColor = Settings.Highlights.NewColor;
            g_btnHighlightChanges.BackColor = Settings.Highlights.ChangingColor;
            g_btnHighlightRemoved.BackColor = Settings.Highlights.RemovedColor;
            g_btnHighlightFrozen.BackColor = Settings.Highlights.FrozenColor;
        } catch { }
        try { // Performance Graphs Settings
            p_chkSolidGraphs.Checked = Settings.Performance.Solid;
            p_chkAntiAlias.Checked = Settings.Performance.AntiAlias;
            p_chkShadeBackground.Checked = Settings.Performance.ShadeBackground;
            p_chkAvgValue.Checked = Settings.Performance.DisplayAverages;
            p_chkShowLegends.Checked = Settings.Performance.DisplayLegends;
            p_chkShowIndexes.Checked = Settings.Performance.DisplayIndexes;
            p_chkOnlyOnHover.Checked = Settings.Performance.DisplayOnHover;
            p_tbValueSpacing.Value = Settings.Performance.ValueSpacing;
            p_tbGridSize.Value = Settings.Performance.GridSize;
            p_chkLightBackground.Checked = Settings.Performance.LightColors;
            p_chkKernelTime.Checked = Settings.Performance.ShowKernelTime;
            p_chkSeparateCpu.Checked = Settings.Performance.SeparateCPUs;
            p_GridVertical.SelectedIndex = Settings.Performance.VerticalGridStyle;
            p_GridVerticalColor.BackColor = Settings.Performance.VerticalGridColor;
            p_GridHorizontal.SelectedIndex = Settings.Performance.HorizontalGridStyle;
            p_GridHorizontalColor.BackColor = Settings.Performance.HorizontalGridColor;
            p_AverageLine.SelectedIndex = Settings.Performance.AverageLineStyle;
            p_AverageLineColor.BackColor = Settings.Performance.AverageLineColor;
            p_lblGridSize.Text = p_tbGridSize.Value.ToString();
            p_lblValueSpacing.Text = p_tbValueSpacing.Value.ToString();
        } catch { }
        try { // Networking Graphs Settings
            n_chkKeepDrawing.Checked = Settings.Networking.KeepUpdating;
            n_chkSolidGraphs.Checked = Settings.Networking.Solid;
            n_chkAntiAlias.Checked = Settings.Networking.AntiAlias;
            n_chkShadeBackground.Checked = Settings.Networking.ShadeBackground;
            n_chkAvgValue.Checked = Settings.Networking.DisplayAverages;
            n_chkShowIndexes.Checked = Settings.Networking.DisplayIndexes;
            n_chkShowLegends.Checked = Settings.Networking.DisplayLegends;
            n_tbValueSpacing.Value = Settings.Networking.ValueSpacing;
            n_tbGridSize.Value = Settings.Networking.GridSize;
            n_chkLightBackground.Checked = Settings.Networking.LightColors;
            n_GridVertical.SelectedIndex = Settings.Networking.VerticalGridStyle;
            n_GridVerticalColor.BackColor = Settings.Networking.VerticalGridColor;
            n_GridHorizontal.SelectedIndex = Settings.Networking.HorizontalGridStyle;
            n_GridHorizontalColor.BackColor = Settings.Networking.HorizontalGridColor;
            n_AverageLine.SelectedIndex = Settings.Networking.AverageLineStyle;
            n_AverageLineColor.BackColor = Settings.Networking.AverageLineColor;
            n_btnUpColor.BackColor = Settings.Networking.UploadColor;
            n_btnDnColor.BackColor = Settings.Networking.DownloadColor;
            n_lblGridSize.Text = n_tbGridSize.Value.ToString();
            n_lblValueSpacing.Text = n_tbValueSpacing.Value.ToString();
        } catch { }

    }
    public void SaveSettings() {
        // General Settings
        Settings.RememberActiveTab = true;
        Settings.RememberPositions = g_chkRememberWindow.Checked;
        Settings.StartMinimized = g_chkStartMinimized.Checked;
        Settings.AlwaysOnTop = g_chkWindowOnTop.Checked;
        Settings.MinimizeWhenClosing = g_chkMinimizeClose.Checked;
        Settings.IconsInProcess = g_chkShowIcons.Checked;
        Settings.ServicesInStatus = g_chkServicesStatus.Checked;
        Settings.TimmingInStatus = g_chkTimmingStatus.Checked;
        Settings.AlternateRowColors = g_chkAlternateRowColors.Checked;
        Settings.ToTrayWhenMinimized = t_chkHideToTray.Checked;
        Settings.ToTrayWhenClosed = t_chkCloseToTray.Checked;
        Settings.ShowCPUOnTray = t_chkShowCPU.Checked;
        Settings.DblClickToRestore = t_chkRequireDobleClick.Checked;
        // Highlight Items Checks & Colors
        Settings.Highlights.NewItems = g_chkHighlightNew.Checked;
        Settings.Highlights.ChangingItems = g_chkHighlightChanges.Checked;
        Settings.Highlights.RemovedItems = g_chkHighlightRemoved.Checked;
        Settings.Highlights.FrozenItems = g_chkHighlightFrozen.Checked;
        Settings.Highlights.NewColor = g_btnHighlightNew.BackColor;
        Settings.Highlights.ChangingColor = g_btnHighlightChanges.BackColor;
        Settings.Highlights.RemovedColor = g_btnHighlightRemoved.BackColor;
        Settings.Highlights.FrozenColor = g_btnHighlightFrozen.BackColor;
        // Performance Graphs Settings
        Settings.Performance.Solid = p_chkSolidGraphs.Checked;
        Settings.Performance.AntiAlias = p_chkAntiAlias.Checked;
        Settings.Performance.ShadeBackground = p_chkShadeBackground.Checked;
        Settings.Performance.DisplayAverages = p_chkAvgValue.Checked;
        Settings.Performance.DisplayLegends = p_chkShowLegends.Checked;
        Settings.Performance.DisplayIndexes = p_chkShowIndexes.Checked;
        Settings.Performance.DisplayOnHover = p_chkOnlyOnHover.Checked;
        Settings.Performance.ValueSpacing = p_tbValueSpacing.Value;
        Settings.Performance.GridSize = p_tbGridSize.Value;
        Settings.Performance.LightColors = p_chkLightBackground.Checked;
        Settings.Performance.ShowKernelTime = p_chkKernelTime.Checked;
        Settings.Performance.SeparateCPUs = p_chkSeparateCpu.Checked;
        Settings.Performance.VerticalGridStyle = p_GridVertical.SelectedIndex;
        Settings.Performance.VerticalGridColor = p_GridVerticalColor.BackColor;
        Settings.Performance.HorizontalGridStyle = p_GridHorizontal.SelectedIndex;
        Settings.Performance.HorizontalGridColor = p_GridHorizontalColor.BackColor;
        Settings.Performance.AverageLineStyle = p_AverageLine.SelectedIndex;
        Settings.Performance.AverageLineColor = p_AverageLineColor.BackColor;
        // Networking Graphs Settings
        Settings.Networking.KeepUpdating = n_chkKeepDrawing.Checked;
        Settings.Networking.Solid = n_chkSolidGraphs.Checked;
        Settings.Networking.AntiAlias = n_chkAntiAlias.Checked;
        Settings.Networking.ShadeBackground = n_chkShadeBackground.Checked;
        Settings.Networking.DisplayAverages = n_chkAvgValue.Checked;
        Settings.Networking.DisplayIndexes = n_chkShowIndexes.Checked;
        Settings.Networking.DisplayLegends = n_chkShowLegends.Checked;
        Settings.Networking.ValueSpacing = n_tbValueSpacing.Value;
        Settings.Networking.GridSize = n_tbGridSize.Value;
        Settings.Networking.LightColors = n_chkLightBackground.Checked;
        Settings.Networking.VerticalGridStyle = n_GridVertical.SelectedIndex;
        Settings.Networking.VerticalGridColor = n_GridVerticalColor.BackColor;
        Settings.Networking.HorizontalGridStyle = n_GridHorizontal.SelectedIndex;
        Settings.Networking.HorizontalGridColor = n_GridHorizontalColor.BackColor;
        Settings.Networking.AverageLineStyle = n_AverageLine.SelectedIndex;
        Settings.Networking.AverageLineColor = n_AverageLineColor.BackColor;
        Settings.Networking.UploadColor = n_btnUpColor.BackColor;
        Settings.Networking.DownloadColor = n_btnDnColor.BackColor;

        Settings.SaveAll();
    }

}