namespace sMkTaskManager.Forms;

partial class frmProcess_Details {

    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }
    private void InitializeComponent() {
        tc = new TabControl();
        btnOK = new Button();
        btnCancel = new Button();
        lblSpeed = new Label();
        tbSpeed = new TrackBar();
        tpGeneral = new TabPage();
        tpPerformance = new TabPage();
        tpIO = new TabPage();
        tpDiskNet = new TabPage();
        tpModules = new TabPage();
        tpThreads = new TabPage();
        tpLocked = new TabPage();
        tc.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)tbSpeed).BeginInit();
        SuspendLayout();
        // 
        // tc
        // 
        tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tc.Controls.Add(tpGeneral);
        tc.Controls.Add(tpPerformance);
        tc.Controls.Add(tpIO);
        tc.Controls.Add(tpDiskNet);
        tc.Controls.Add(tpModules);
        tc.Controls.Add(tpThreads);
        tc.Controls.Add(tpLocked);
        tc.Location = new Point(7, 7);
        tc.Margin = new Padding(0);
        tc.Multiline = true;
        tc.Name = "tc";
        tc.SelectedIndex = 0;
        tc.Size = new Size(500, 467);
        tc.SizeMode = TabSizeMode.FillToRight;
        tc.TabIndex = 0;
        // 
        // btnOK
        // 
        btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnOK.Location = new Point(353, 482);
        btnOK.Margin = new Padding(0);
        btnOK.Name = "btnOK";
        btnOK.Size = new Size(75, 23);
        btnOK.TabIndex = 3;
        btnOK.Text = "OK";
        btnOK.UseVisualStyleBackColor = true;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(432, 482);
        btnCancel.Margin = new Padding(0);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(75, 23);
        btnCancel.TabIndex = 4;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        // 
        // lblSpeed
        // 
        lblSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblSpeed.Location = new Point(12, 478);
        lblSpeed.Name = "lblSpeed";
        lblSpeed.Size = new Size(116, 15);
        lblSpeed.TabIndex = 1;
        lblSpeed.Text = "Update Speed";
        // 
        // tbSpeed
        // 
        tbSpeed.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        tbSpeed.AutoSize = false;
        tbSpeed.Location = new Point(4, 489);
        tbSpeed.Maximum = 30;
        tbSpeed.Name = "tbSpeed";
        tbSpeed.Size = new Size(124, 23);
        tbSpeed.TabIndex = 2;
        tbSpeed.TabStop = false;
        tbSpeed.TickFrequency = 3;
        tbSpeed.TickStyle = TickStyle.None;
        tbSpeed.Value = 20;
        // 
        // tpGeneral
        // 
        tpGeneral.Location = new Point(4, 24);
        tpGeneral.Name = "tpGeneral";
        tpGeneral.Size = new Size(492, 439);
        tpGeneral.TabIndex = 0;
        tpGeneral.Text = "General";
        tpGeneral.UseVisualStyleBackColor = true;
        // 
        // tpPerformance
        // 
        tpPerformance.Location = new Point(4, 24);
        tpPerformance.Name = "tpPerformance";
        tpPerformance.Size = new Size(488, 439);
        tpPerformance.TabIndex = 1;
        tpPerformance.Text = "Performance";
        tpPerformance.UseVisualStyleBackColor = true;
        // 
        // tpIO
        // 
        tpIO.Location = new Point(4, 24);
        tpIO.Name = "tpIO";
        tpIO.Size = new Size(488, 439);
        tpIO.TabIndex = 2;
        tpIO.Text = "I/O Usage";
        tpIO.UseVisualStyleBackColor = true;
        // 
        // tpDiskNet
        // 
        tpDiskNet.Location = new Point(4, 24);
        tpDiskNet.Name = "tpDiskNet";
        tpDiskNet.Size = new Size(488, 439);
        tpDiskNet.TabIndex = 3;
        tpDiskNet.Text = "Disk & Network";
        tpDiskNet.UseVisualStyleBackColor = true;
        // 
        // tpModules
        // 
        tpModules.Location = new Point(4, 24);
        tpModules.Name = "tpModules";
        tpModules.Size = new Size(488, 439);
        tpModules.TabIndex = 4;
        tpModules.Text = "Modules";
        tpModules.UseVisualStyleBackColor = true;
        // 
        // tpThreads
        // 
        tpThreads.Location = new Point(4, 24);
        tpThreads.Name = "tpThreads";
        tpThreads.Size = new Size(488, 439);
        tpThreads.TabIndex = 5;
        tpThreads.Text = "Threads";
        tpThreads.UseVisualStyleBackColor = true;
        // 
        // tpLocked
        // 
        tpLocked.Location = new Point(4, 24);
        tpLocked.Name = "tpLocked";
        tpLocked.Size = new Size(488, 439);
        tpLocked.TabIndex = 6;
        tpLocked.Text = "Locked Files";
        tpLocked.UseVisualStyleBackColor = true;
        // 
        // frmProcess_Details
        // 
        AcceptButton = btnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCancel;
        ClientSize = new Size(514, 512);
        Controls.Add(lblSpeed);
        Controls.Add(tbSpeed);
        Controls.Add(btnCancel);
        Controls.Add(btnOK);
        Controls.Add(tc);
        DoubleBuffered = true;
        Icon = Resources.Resources.frmProcess_Details;
        MinimumSize = new Size(380, 450);
        Name = "frmProcess_Details";
        SizeGripStyle = SizeGripStyle.Show;
        Text = "sMk Task Manager - Process Details";
        tc.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)tbSpeed).EndInit();
        ResumeLayout(false);
    }

    private TabControl tc;
    private Button btnOK;
    private Button btnCancel;
    private Label lblSpeed;
    private TrackBar tbSpeed;
    private TabPage tpGeneral;
    private TabPage tpPerformance;
    private TabPage tpIO;
    private TabPage tpDiskNet;
    private TabPage tpModules;
    private TabPage tpThreads;
    private TabPage tpLocked;
}
