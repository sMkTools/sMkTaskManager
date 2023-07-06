namespace sMkTaskManager;

partial class frmMain {
    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }
    private void InitializeComponent() {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
        mnu = new MenuStrip();
        mnuFile = new ToolStripMenuItem();
        mnuOptions = new ToolStripMenuItem();
        mnuView = new ToolStripMenuItem();
        mnuHelp = new ToolStripMenuItem();
        ss = new StatusStrip();
        ssText = new ToolStripStatusLabel();
        ssProcesses = new ToolStripStatusLabel();
        ssServices = new ToolStripStatusLabel();
        ssCpuLoad = new ToolStripStatusLabel();
        ssBtnState = new ToolStripSplitButton();
        ssBusyTime = new ToolStripStatusLabel();
        timer1 = new System.Windows.Forms.Timer(components);
        tc = new TabControl();
        tpApplications = new TabPage();
        tpProcesses = new TabPage();
        tpServices = new TabPage();
        tpPerformance = new TabPage();
        tabPerf = new Forms.tabPerformance();
        tpNetworking = new TabPage();
        tpConnections = new TabPage();
        tpPorts = new TabPage();
        tpUsers = new TabPage();
        timmingStrip = new StatusStrip();
        tabProcs = new Forms.tabProcesses();
        tabServs = new Forms.tabServices();
        tabConns = new Forms.tabConnections();
        mnu.SuspendLayout();
        ss.SuspendLayout();
        tc.SuspendLayout();
        tpProcesses.SuspendLayout();
        tpPerformance.SuspendLayout();
        SuspendLayout();
        // 
        // mnu
        // 
        mnu.Items.AddRange(new ToolStripItem[] { mnuFile, mnuOptions, mnuView, mnuHelp });
        mnu.Location = new Point(0, 0);
        mnu.Name = "mnu";
        mnu.Size = new Size(584, 24);
        mnu.TabIndex = 0;
        mnu.Text = "menuStrip1";
        // 
        // mnuFile
        // 
        mnuFile.Name = "mnuFile";
        mnuFile.Size = new Size(37, 20);
        mnuFile.Text = "&File";
        // 
        // mnuOptions
        // 
        mnuOptions.Name = "mnuOptions";
        mnuOptions.Size = new Size(61, 20);
        mnuOptions.Text = "&Options";
        // 
        // mnuView
        // 
        mnuView.Name = "mnuView";
        mnuView.Size = new Size(44, 20);
        mnuView.Text = "&View";
        // 
        // mnuHelp
        // 
        mnuHelp.Alignment = ToolStripItemAlignment.Right;
        mnuHelp.Name = "mnuHelp";
        mnuHelp.Size = new Size(44, 20);
        mnuHelp.Text = "&Help";
        // 
        // ss
        // 
        ss.Items.AddRange(new ToolStripItem[] { ssText, ssProcesses, ssServices, ssCpuLoad, ssBtnState, ssBusyTime });
        ss.Location = new Point(0, 537);
        ss.Name = "ss";
        ss.Size = new Size(584, 24);
        ss.TabIndex = 1;
        ss.Text = "statusStrip1";
        // 
        // ssText
        // 
        ssText.BorderSides = ToolStripStatusLabelBorderSides.Right;
        ssText.BorderStyle = Border3DStyle.Etched;
        ssText.Name = "ssText";
        ssText.Size = new Size(154, 19);
        ssText.Spring = true;
        ssText.Text = "sMk Task Manager";
        ssText.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ssProcesses
        // 
        ssProcesses.AutoSize = false;
        ssProcesses.BorderSides = ToolStripStatusLabelBorderSides.Right;
        ssProcesses.BorderStyle = Border3DStyle.Etched;
        ssProcesses.Name = "ssProcesses";
        ssProcesses.Size = new Size(95, 19);
        ssProcesses.Text = "Processes: 0";
        ssProcesses.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ssServices
        // 
        ssServices.AutoSize = false;
        ssServices.BorderSides = ToolStripStatusLabelBorderSides.Right;
        ssServices.BorderStyle = Border3DStyle.Etched;
        ssServices.Name = "ssServices";
        ssServices.Size = new Size(85, 19);
        ssServices.Text = "Services: 0";
        ssServices.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ssCpuLoad
        // 
        ssCpuLoad.AutoSize = false;
        ssCpuLoad.BorderSides = ToolStripStatusLabelBorderSides.Right;
        ssCpuLoad.BorderStyle = Border3DStyle.Etched;
        ssCpuLoad.Name = "ssCpuLoad";
        ssCpuLoad.Size = new Size(100, 19);
        ssCpuLoad.Text = "CPU Load: 0%";
        ssCpuLoad.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ssBtnState
        // 
        ssBtnState.AutoSize = false;
        ssBtnState.DoubleClickEnabled = true;
        ssBtnState.ImageTransparentColor = Color.Magenta;
        ssBtnState.Name = "ssBtnState";
        ssBtnState.Size = new Size(80, 22);
        ssBtnState.Text = "High";
        ssBtnState.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // ssBusyTime
        // 
        ssBusyTime.AutoSize = false;
        ssBusyTime.BorderSides = ToolStripStatusLabelBorderSides.Left;
        ssBusyTime.BorderStyle = Border3DStyle.Etched;
        ssBusyTime.Margin = new Padding(5, 3, 0, 2);
        ssBusyTime.Name = "ssBusyTime";
        ssBusyTime.Size = new Size(50, 19);
        ssBusyTime.Text = "20ms";
        // 
        // tc
        // 
        tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tc.Controls.Add(tpApplications);
        tc.Controls.Add(tpProcesses);
        tc.Controls.Add(tpServices);
        tc.Controls.Add(tpPerformance);
        tc.Controls.Add(tpNetworking);
        tc.Controls.Add(tpConnections);
        tc.Controls.Add(tpPorts);
        tc.Controls.Add(tpUsers);
        tc.ItemSize = new Size(70, 20);
        tc.Location = new Point(3, 27);
        tc.Margin = new Padding(0, 3, 0, 3);
        tc.Name = "tc";
        tc.Padding = new Point(6, 4);
        tc.SelectedIndex = 0;
        tc.Size = new Size(577, 508);
        tc.TabIndex = 1;
        // 
        // tpApplications
        // 
        tpApplications.BackColor = SystemColors.Control;
        tpApplications.Location = new Point(4, 24);
        tpApplications.Name = "tpApplications";
        tpApplications.Size = new Size(569, 480);
        tpApplications.TabIndex = 0;
        tpApplications.Text = "Applications";
        // 
        // tpProcesses
        // 
        tpProcesses.BackColor = SystemColors.Control;
        tpProcesses.Controls.Add(tabProcs);
        tpProcesses.Location = new Point(4, 24);
        tpProcesses.Name = "tpProcesses";
        tpProcesses.Size = new Size(569, 480);
        tpProcesses.TabIndex = 2;
        tpProcesses.Text = "Processes";
        // 
        // tabProcs
        // 
        tabProcs.Active = true;
        tabProcs.Description = "Processes";
        tabProcs.Dock = DockStyle.Fill;
        tabProcs.InfoText = "";
        tabProcs.Location = new Point(0, 0);
        tabProcs.Name = "tabProcs";
        tabProcs.Size = new Size(569, 480);
        tabProcs.TabIndex = 0;
        tabProcs.Title = "Processes";
        // 
        // tpServices
        // 
        tpServices.BackColor = SystemColors.Control;
        tpServices.Controls.Add(tabServs);
        tpServices.Location = new Point(4, 24);
        tpServices.Name = "tpServices";
        tpServices.Size = new Size(569, 480);
        tpServices.TabIndex = 3;
        tpServices.Text = "Services";
        // 
        // tabServs
        // 
        tabServs.Active = true;
        tabServs.Description = "Services";
        tabServs.Dock = DockStyle.Fill;
        tabServs.Location = new Point(0, 0);
        tabServs.Name = "tabServs";
        tabServs.Size = new Size(569, 480);
        tabServs.TabIndex = 0;
        tabServs.Title = "Services";
        // 
        // tpPerformance
        // 
        tpPerformance.BackColor = SystemColors.Control;
        tpPerformance.Controls.Add(tabPerf);
        tpPerformance.Location = new Point(4, 24);
        tpPerformance.Name = "tpPerformance";
        tpPerformance.Size = new Size(569, 480);
        tpPerformance.TabIndex = 1;
        tpPerformance.Text = "Performance";
        // 
        // tabPerf
        // 
        tabPerf.Dock = DockStyle.Fill;
        tabPerf.Location = new Point(0, 0);
        tabPerf.Name = "tabPerf";
        tabPerf.Size = new Size(569, 480);
        tabPerf.TabIndex = 0;
        // 
        // tpNetworking
        // 
        tpNetworking.BackColor = SystemColors.Control;
        tpNetworking.Location = new Point(4, 24);
        tpNetworking.Name = "tpNetworking";
        tpNetworking.Size = new Size(569, 480);
        tpNetworking.TabIndex = 4;
        tpNetworking.Text = "Networking";
        // 
        // tpConnections
        // 
        tpConnections.BackColor = SystemColors.Control;
        tpConnections.Controls.Add(tabConns);
        tpConnections.Location = new Point(4, 24);
        tpConnections.Name = "tpConnections";
        tpConnections.Size = new Size(569, 480);
        tpConnections.TabIndex = 5;
        tpConnections.Text = "Connections";
        // 
        // tabConns
        // 
        tabConns.Active = true;
        tabConns.Description = "Connections";
        tabConns.Dock = DockStyle.Fill;
        tabConns.Location = new Point(0, 0);
        tabConns.Name = "tabConns";
        tabConns.Size = new Size(569, 480);
        tabConns.TabIndex = 0;
        tabConns.Title = "Connections";
        // 
        // tpPorts
        // 
        tpPorts.BackColor = SystemColors.Control;
        tpPorts.Location = new Point(4, 24);
        tpPorts.Name = "tpPorts";
        tpPorts.Size = new Size(569, 480);
        tpPorts.TabIndex = 6;
        tpPorts.Text = "Ports";
        // 
        // tpUsers
        // 
        tpUsers.BackColor = SystemColors.Control;
        tpUsers.Location = new Point(4, 24);
        tpUsers.Name = "tpUsers";
        tpUsers.Size = new Size(569, 480);
        tpUsers.TabIndex = 7;
        tpUsers.Text = "Users";
        // 
        // timmingStrip
        // 
        timmingStrip.AllowMerge = false;
        timmingStrip.Dock = DockStyle.Top;
        timmingStrip.GripMargin = new Padding(0);
        timmingStrip.Location = new Point(0, 515);
        timmingStrip.Name = "timmingStrip";
        timmingStrip.ShowItemToolTips = true;
        timmingStrip.Size = new Size(584, 22);
        timmingStrip.SizingGrip = false;
        timmingStrip.TabIndex = 2;
        timmingStrip.Text = "statusStrip1";
        timmingStrip.Visible = false;
        // 
        // frmMain
        // 
        ClientSize = new Size(584, 561);
        Controls.Add(timmingStrip);
        Controls.Add(tc);
        Controls.Add(ss);
        Controls.Add(mnu);
        KeyPreview = true;
        MainMenuStrip = mnu;
        Margin = new Padding(6);
        MinimumSize = new Size(480, 400);
        Icon = Resources.Resources.frmMain;
        Name = "frmMain";
        Text = "sMk Task Manager - Next Gen";
        FormClosing += OnClosingEventHandler;
        Load += OnLoadEventHandler;
        SizeChanged += OnSizeChangedEventHandler;
        KeyDown += OnKeyDownEventHandler;
        mnu.ResumeLayout(false);
        mnu.PerformLayout();
        ss.ResumeLayout(false);
        ss.PerformLayout();
        tc.ResumeLayout(false);
        tpProcesses.ResumeLayout(false);
        tpPerformance.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private MenuStrip mnu;
    private ToolStripMenuItem mnuFile;
    private ToolStripMenuItem mnuOptions;
    private ToolStripMenuItem mnuView;
    private ToolStripMenuItem mnuHelp;
    private StatusStrip ss;
    private ToolStripStatusLabel ssText;
    private ToolStripStatusLabel ssProcesses;
    private ToolStripStatusLabel ssServices;
    private ToolStripStatusLabel ssCpuLoad;
    private ToolStripSplitButton ssBtnState;
    private ToolStripStatusLabel ssBusyTime;
    private System.Windows.Forms.Timer timer1;
    private TabControl tc;
    private TabPage tpApplications;
    private TabPage tpPerformance;
    private Forms.tabPerformance tabPerf;
    private TabPage tpProcesses;
    private TabPage tpServices;
    private TabPage tpNetworking;
    private TabPage tpConnections;
    private TabPage tpPorts;
    private TabPage tpUsers;
    private StatusStrip timmingStrip;
    private Forms.tabProcesses tabProcs;
    private Forms.tabServices tabServs;
    private Forms.tabConnections tabConns;
}
