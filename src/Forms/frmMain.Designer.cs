namespace sMkTaskManager {
    partial class frmMain {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
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
            tcApplications = new TabPage();
            tcProcesses = new TabPage();
            tabProcs = new Forms.tabProcesses();
            tcServices = new TabPage();
            tpPerformance = new TabPage();
            tabPerf = new Forms.tabPerformance();
            tcNetworking = new TabPage();
            tcConnections = new TabPage();
            tcPorts = new TabPage();
            tcUsers = new TabPage();
            timmingStrip = new StatusStrip();
            mnu.SuspendLayout();
            ss.SuspendLayout();
            tc.SuspendLayout();
            tcProcesses.SuspendLayout();
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
            ssBtnState.Image = (Image)resources.GetObject("ssBtnState.Image");
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
            tc.Controls.Add(tcApplications);
            tc.Controls.Add(tcProcesses);
            tc.Controls.Add(tcServices);
            tc.Controls.Add(tpPerformance);
            tc.Controls.Add(tcNetworking);
            tc.Controls.Add(tcConnections);
            tc.Controls.Add(tcPorts);
            tc.Controls.Add(tcUsers);
            tc.ItemSize = new Size(70, 20);
            tc.Location = new Point(3, 27);
            tc.Margin = new Padding(0, 3, 0, 3);
            tc.Name = "tc";
            tc.Padding = new Point(6, 4);
            tc.SelectedIndex = 0;
            tc.Size = new Size(577, 508);
            tc.TabIndex = 1;
            // 
            // tcApplications
            // 
            tcApplications.BackColor = SystemColors.Control;
            tcApplications.Location = new Point(4, 24);
            tcApplications.Name = "tcApplications";
            tcApplications.Size = new Size(569, 480);
            tcApplications.TabIndex = 0;
            tcApplications.Text = "Applications";
            // 
            // tcProcesses
            // 
            tcProcesses.BackColor = SystemColors.Control;
            tcProcesses.Controls.Add(tabProcs);
            tcProcesses.Location = new Point(4, 24);
            tcProcesses.Name = "tcProcesses";
            tcProcesses.Size = new Size(569, 480);
            tcProcesses.TabIndex = 2;
            tcProcesses.Text = "Processes";
            // 
            // tabProcs
            // 
            tabProcs.Dock = DockStyle.Fill;
            tabProcs.InfoText = "";
            tabProcs.Location = new Point(0, 0);
            tabProcs.Name = "tabProcs";
            tabProcs.Size = new Size(569, 480);
            tabProcs.TabIndex = 0;
            // 
            // tcServices
            // 
            tcServices.BackColor = SystemColors.Control;
            tcServices.Location = new Point(4, 24);
            tcServices.Name = "tcServices";
            tcServices.Size = new Size(569, 480);
            tcServices.TabIndex = 3;
            tcServices.Text = "Services";
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
            // tcNetworking
            // 
            tcNetworking.BackColor = SystemColors.Control;
            tcNetworking.Location = new Point(4, 24);
            tcNetworking.Name = "tcNetworking";
            tcNetworking.Size = new Size(569, 480);
            tcNetworking.TabIndex = 4;
            tcNetworking.Text = "Networking";
            // 
            // tcConnections
            // 
            tcConnections.BackColor = SystemColors.Control;
            tcConnections.Location = new Point(4, 24);
            tcConnections.Name = "tcConnections";
            tcConnections.Size = new Size(569, 480);
            tcConnections.TabIndex = 5;
            tcConnections.Text = "Connections";
            // 
            // tcPorts
            // 
            tcPorts.BackColor = SystemColors.Control;
            tcPorts.Location = new Point(4, 24);
            tcPorts.Name = "tcPorts";
            tcPorts.Size = new Size(569, 480);
            tcPorts.TabIndex = 6;
            tcPorts.Text = "Ports";
            // 
            // tcUsers
            // 
            tcUsers.BackColor = SystemColors.Control;
            tcUsers.Location = new Point(4, 24);
            tcUsers.Name = "tcUsers";
            tcUsers.Size = new Size(569, 480);
            tcUsers.TabIndex = 7;
            tcUsers.Text = "Users";
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
            MainMenuStrip = mnu;
            Margin = new Padding(6);
            MinimumSize = new Size(480, 400);
            Name = "frmMain";
            Text = "sMk Task Manager - Next Gen";
            Load += OnLoadEventHandler;
            SizeChanged += OnSizeChangedEventHandler;
            mnu.ResumeLayout(false);
            mnu.PerformLayout();
            ss.ResumeLayout(false);
            ss.PerformLayout();
            tc.ResumeLayout(false);
            tcProcesses.ResumeLayout(false);
            tpPerformance.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
        private TabPage tcApplications;
        private TabPage tpPerformance;
        private Forms.tabPerformance tabPerf;
        private TabPage tcProcesses;
        private TabPage tcServices;
        private TabPage tcNetworking;
        private TabPage tcConnections;
        private TabPage tcPorts;
        private TabPage tcUsers;
        private StatusStrip timmingStrip;
        private Forms.tabProcesses tabProcs;
    }
}