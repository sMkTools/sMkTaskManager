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
            tabPage1 = new TabPage();
            tpPerformance = new TabPage();
            tabPerformance = new Forms.tabPerformance();
            mnu.SuspendLayout();
            ss.SuspendLayout();
            tc.SuspendLayout();
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
            // timer1
            // 
            timer1.Enabled = false;
            timer1.Tick += timer1_Tick;
            // 
            // tc
            // 
            tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tc.Controls.Add(tabPage1);
            tc.Controls.Add(tpPerformance);
            tc.ItemSize = new Size(69, 20);
            tc.Location = new Point(3, 27);
            tc.Margin = new Padding(0, 3, 0, 3);
            tc.Name = "tc";
            tc.Padding = new Point(6, 4);
            tc.SelectedIndex = 0;
            tc.Size = new Size(577, 508);
            tc.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(569, 480);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // tpPerformance
            // 
            tpPerformance.BackColor = SystemColors.Control;
            tpPerformance.Controls.Add(tabPerformance);
            tpPerformance.Location = new Point(4, 24);
            tpPerformance.Name = "tpPerformance";
            tpPerformance.Padding = new Padding(0, 2, 0, 0);
            tpPerformance.Size = new Size(569, 480);
            tpPerformance.TabIndex = 1;
            tpPerformance.Text = "Performance";
            // 
            // tabPerformance
            // 
            tabPerformance.Dock = DockStyle.Fill;
            tabPerformance.Location = new Point(0, 2);
            tabPerformance.Name = "tabPerformance";
            tabPerformance.Size = new Size(569, 478);
            tabPerformance.TabIndex = 0;
            // 
            // frmMain
            // 
            ClientSize = new Size(584, 561);
            Controls.Add(tc);
            Controls.Add(ss);
            Controls.Add(mnu);
            MainMenuStrip = mnu;
            Margin = new Padding(6);
            MinimumSize = new Size(480, 400);
            Name = "frmMain";
            Text = "sMk Task Manager - Next Gen";
            Load += frmMain_Load;
            mnu.ResumeLayout(false);
            mnu.PerformLayout();
            ss.ResumeLayout(false);
            ss.PerformLayout();
            tc.ResumeLayout(false);
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
        private TabPage tabPage1;
        private TabPage tpPerformance;
        private Forms.tabPerformance tabPerformance;
    }
}