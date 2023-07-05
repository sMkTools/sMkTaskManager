using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.ServiceProcess;
using sMkTaskManager.Classes;
using sMkTaskManager.Controls;
namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
internal class tabServices : UserControl, ITaskManagerTab {
    private readonly Stopwatch _stopWatch = new();
    internal HashSet<string> ColsServices = new();
    internal HashSet<string> HashServices = new();
    internal TaskManagerServicesCollection Services = new();

    internal sMkListView lv;
    private ImageList il;
    private ContextMenuStrip cms;
    private ToolStripMenuItem cmsProperties;
    private ToolStripMenuItem cmsOpenLocation;
    private ToolStripSeparator cmsSeparator1;
    private ToolStripMenuItem cmsStart;
    private ToolStripMenuItem cmsStop;
    private ToolStripMenuItem cmsPause;
    private ToolStripMenuItem cmsResume;
    private ToolStripSeparator cmsSeparator2;
    private ToolStripMenuItem cmsGoToProcess;
    private ToolStripMenuItem cmsOnline;
    private ToolStripMenuItem cmsFileProperties;
    private Button btnStart;
    private Button btnStop;
    private CheckBox btnDescriptions;
    private Button btnRestart;
    private Button btnServices;
    private Button btnForceRefresh;
    private ColumnHeader lvName;
    private TextBox txtDescriptions;

    public event EventHandler? ForceRefreshClicked;
    public event EventHandler? RefreshStarts;
    public event EventHandler? RefreshComplete;

    public tabServices() {
        InitializeComponent();
        Extensions.CascadingDoubleBuffer(this);
        il!.Images.Clear();
        il!.Images.Add(Resources.Resources.Service_Running);
        il!.Images.Add(Resources.Resources.Service_Stoped);
        il!.Images.Add(Resources.Resources.Service_Disabled);
        lv!.ContentType = typeof(TaskManagerService);
        lv!.DataSource = Services.DataExporter;
    }

    private IContainer? components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }
    private void InitializeComponent() {
        components = new Container();
        btnStart = new Button();
        btnStop = new Button();
        btnDescriptions = new CheckBox();
        btnRestart = new Button();
        btnServices = new Button();
        btnForceRefresh = new Button();
        lv = new sMkListView();
        lvName = new ColumnHeader();
        txtDescriptions = new TextBox();
        il = new ImageList(components);
        cms = new ContextMenuStrip(components);
        cmsProperties = new ToolStripMenuItem();
        cmsFileProperties = new ToolStripMenuItem();
        cmsOpenLocation = new ToolStripMenuItem();
        cmsSeparator1 = new ToolStripSeparator();
        cmsStart = new ToolStripMenuItem();
        cmsStop = new ToolStripMenuItem();
        cmsPause = new ToolStripMenuItem();
        cmsResume = new ToolStripMenuItem();
        cmsSeparator2 = new ToolStripSeparator();
        cmsGoToProcess = new ToolStripMenuItem();
        cmsOnline = new ToolStripMenuItem();
        cms.SuspendLayout();
        SuspendLayout();
        // 
        // btnStart
        // 
        btnStart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnStart.Enabled = false;
        btnStart.Location = new Point(6, 571);
        btnStart.Margin = new Padding(0, 3, 3, 3);
        btnStart.Name = "btnStart";
        btnStart.Size = new Size(60, 23);
        btnStart.TabIndex = 1;
        btnStart.Text = "Start";
        // 
        // btnStop
        // 
        btnStop.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnStop.Enabled = false;
        btnStop.Location = new Point(69, 571);
        btnStop.Margin = new Padding(0, 3, 3, 3);
        btnStop.Name = "btnStop";
        btnStop.Size = new Size(60, 23);
        btnStop.TabIndex = 2;
        btnStop.Text = "Stop";
        // 
        // btnDescriptions
        // 
        btnDescriptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDescriptions.Appearance = Appearance.Button;
        btnDescriptions.Location = new Point(195, 571);
        btnDescriptions.Margin = new Padding(0, 3, 3, 3);
        btnDescriptions.Name = "btnDescriptions";
        btnDescriptions.Size = new Size(80, 23);
        btnDescriptions.TabIndex = 4;
        btnDescriptions.Text = "Descriptions";
        btnDescriptions.TextAlign = ContentAlignment.MiddleCenter;
        btnDescriptions.UseVisualStyleBackColor = true;
        // 
        // btnRestart
        // 
        btnRestart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnRestart.Enabled = false;
        btnRestart.Location = new Point(132, 571);
        btnRestart.Margin = new Padding(0, 3, 3, 3);
        btnRestart.Name = "btnRestart";
        btnRestart.Size = new Size(60, 23);
        btnRestart.TabIndex = 3;
        btnRestart.Text = "Restart";
        // 
        // btnServices
        // 
        btnServices.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnServices.Location = new Point(405, 571);
        btnServices.Margin = new Padding(3, 3, 0, 3);
        btnServices.Name = "btnServices";
        btnServices.Size = new Size(97, 23);
        btnServices.TabIndex = 5;
        btnServices.Text = "Services MMC";
        // 
        // btnForceRefresh
        // 
        btnForceRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnForceRefresh.Location = new Point(505, 571);
        btnForceRefresh.Margin = new Padding(3, 3, 0, 3);
        btnForceRefresh.Name = "btnForceRefresh";
        btnForceRefresh.Size = new Size(89, 23);
        btnForceRefresh.TabIndex = 6;
        btnForceRefresh.Text = "Force Refresh";
        // 
        // lv
        // 
        lv.AlternateRowColors = false;
        lv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lv.Columns.AddRange(new ColumnHeader[] { lvName });
        lv.FullRowSelect = true;
        lv.GridLines = true;
        lv.SmallImageList = il;
        lv.LabelWrap = false;
        lv.Location = new Point(6, 6);
        lv.Margin = new Padding(6, 6, 6, 0);
        lv.Name = "lv";
        lv.ShowGroups = false;
        lv.ShowItemToolTips = true;
        lv.Size = new Size(588, 559);
        lv.Sortable = true;
        lv.SortColumn = 0;
        lv.Sorting = SortOrder.Ascending;
        lv.TabIndex = 0;
        lv.UseCompatibleStateImageBehavior = false;
        lv.View = View.Details;
        // 
        // lvName
        // 
        lvName.Tag = "Name";
        lvName.Text = "Name";
        lvName.Width = 150;
        // 
        // txtDescriptions
        // 
        txtDescriptions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtDescriptions.BorderStyle = BorderStyle.FixedSingle;
        txtDescriptions.HideSelection = false;
        txtDescriptions.Location = new Point(6, 505);
        txtDescriptions.Multiline = true;
        txtDescriptions.Name = "txtDescriptions";
        txtDescriptions.ReadOnly = true;
        txtDescriptions.ScrollBars = ScrollBars.Vertical;
        txtDescriptions.Size = new Size(588, 60);
        txtDescriptions.TabIndex = 7;
        txtDescriptions.TabStop = false;
        txtDescriptions.Visible = false;
        // 
        // il
        // 
        il.ColorDepth = ColorDepth.Depth32Bit;
        il.ImageSize = new Size(16, 16);
        il.TransparentColor = Color.Transparent;
        // 
        // cms
        // 
        cms.Items.AddRange(new ToolStripItem[] { cmsProperties, cmsFileProperties, cmsOpenLocation, cmsSeparator1, cmsStart, cmsStop, cmsPause, cmsResume, cmsSeparator2, cmsGoToProcess, cmsOnline });
        cms.Name = "cms";
        cms.Size = new Size(157, 214);
        // 
        // cmsProperties
        // 
        cmsProperties.Name = "cmsProperties";
        cmsProperties.Size = new Size(156, 22);
        cmsProperties.Text = "P&roperties";
        // 
        // cmsFileProperties
        // 
        cmsFileProperties.Name = "cmsFileProperties";
        cmsFileProperties.Size = new Size(156, 22);
        cmsFileProperties.Text = "File Prope&rties";
        // 
        // cmsOpenLocation
        // 
        cmsOpenLocation.Name = "cmsOpenLocation";
        cmsOpenLocation.Size = new Size(156, 22);
        cmsOpenLocation.Text = "&Open Location";
        // 
        // cmsSeparator1
        // 
        cmsSeparator1.Name = "cmsSeparator1";
        cmsSeparator1.Size = new Size(153, 6);
        // 
        // cmsStart
        // 
        cmsStart.Name = "cmsStart";
        cmsStart.Size = new Size(156, 22);
        cmsStart.Text = "&Start Service";
        // 
        // cmsStop
        // 
        cmsStop.Name = "cmsStop";
        cmsStop.Size = new Size(156, 22);
        cmsStop.Text = "Sto&p Service";
        // 
        // cmsPause
        // 
        cmsPause.Name = "cmsPause";
        cmsPause.Size = new Size(156, 22);
        cmsPause.Text = "Pa&use Service";
        // 
        // cmsResume
        // 
        cmsResume.Name = "cmsResume";
        cmsResume.Size = new Size(156, 22);
        cmsResume.Text = "Resu&me Service";
        // 
        // cmsSeparator2
        // 
        cmsSeparator2.Name = "cmsSeparator2";
        cmsSeparator2.Size = new Size(153, 6);
        // 
        // cmsGoToProcess
        // 
        cmsGoToProcess.Name = "cmsGoToProcess";
        cmsGoToProcess.Size = new Size(156, 22);
        cmsGoToProcess.Text = "&Go To Process...";
        // 
        // cmsOnline
        // 
        cmsOnline.Name = "cmsOnline";
        cmsOnline.Size = new Size(156, 22);
        cmsOnline.Text = "Search &Online...";
        // 
        // tabServices
        // 
        Controls.Add(txtDescriptions);
        Controls.Add(lv);
        Controls.Add(btnForceRefresh);
        Controls.Add(btnServices);
        Controls.Add(btnRestart);
        Controls.Add(btnDescriptions);
        Controls.Add(btnStop);
        Controls.Add(btnStart);
        Name = "tabServices";
        Size = new Size(600, 600);
        cms.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    public sMkListView ListView => lv;
    public string Title { get; set; } = "Processes";
    public string Description { get; set; } = "Processes";
    public string TimmingKey { get; } = "Procs";
    public long TimmingValue => _stopWatch.ElapsedMilliseconds;
    public bool Active { get; set; } = true;

    public void Refresher(bool firstTime = false) {
        if (InvokeRequired) { BeginInvoke(() => Refresher(firstTime)); return; }
        // Check if we are the active tab or its firstTime
        if (!Active && !firstTime) return;
        if (lv.Items.Count == 0) firstTime = true;
        _stopWatch.Restart();
        RefreshStarts?.Invoke(this, EventArgs.Empty);
        // Store last round items and initialize new ones
        HashSet<string> LastRun = new();
        LastRun.UnionWith(HashServices);
        HashServices.Clear();
        // Iterate through all the services
        foreach (ServiceController s in ServiceController.GetServices()) {
            if (Shared.skipServices.Contains(s.DisplayName) || Shared.skipServices.Contains(s.ServiceName)) continue;
            TaskManagerService thisService = new(s.ServiceName);
            HashServices.Add(s.ServiceName);
            if (Services.Contains(s.ServiceName)) {
                thisService = Services.GetService(s.ServiceName)!; ;
                if (thisService.BackColor == Settings.Highlights.NewColor) thisService.BackColor = Color.Empty;
                try {
                    thisService.Update();
                } catch (Exception ex) { Shared.DebugTrap(ex, 031); }
            } else {
                try {
                    thisService.Load(s);
                } catch (Exception ex) { Shared.DebugTrap(ex, 032); }

                if (Settings.Highlights.NewItems && !firstTime)  thisService.BackColor = Settings.Highlights.NewColor;
                Services.Add(thisService);
            }
        }
        // Clean out old Items
        LastRun.ExceptWith(HashServices);
        for (int i = 0; i < LastRun.Count; i++) {
            TaskManagerService? thisService = Services.GetService(LastRun.ElementAtOrDefault(i)!);
            if (thisService == null) continue;
            if (!Settings.Highlights.RemovedItems || thisService.BackColor == Settings.Highlights.RemovedColor) {
                lv.RemoveItemByKey(thisService.Ident);
                Services.Remove(thisService);
            } else {
                thisService.BackColor = Settings.Highlights.RemovedColor;
                HashServices.Add(thisService.Ident);
            }
        }
        _stopWatch.Stop();
        RefreshComplete?.Invoke(this, EventArgs.Empty);
    }
    public void LoadSettings() {
        Settings.LoadColsInformation(TaskManagerColumnTypes.Services, lv, ref ColsServices);

    }
    public bool SaveSettings() {
        return Settings.SaveColsInformation("colsServices", lv);
    }

}
