namespace sMkTaskManager.Forms;
partial class frmAbout {

    private System.ComponentModel.IContainer components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent() {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
        gb = new GroupBox();
        lblBuild = new Label();
        lblWarning = new Label();
        lblCopyright = new Label();
        lblLine = new Label();
        lblVersion = new Label();
        lblTitle = new Label();
        pbIcon = new PictureBox();
        lblInfo = new Label();
        tmrIcon = new System.Windows.Forms.Timer(components);
        ilAnimation = new ImageList(components);
        gb.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pbIcon).BeginInit();
        SuspendLayout();
        // 
        // gb
        // 
        gb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        gb.Controls.Add(lblBuild);
        gb.Controls.Add(lblWarning);
        gb.Controls.Add(lblCopyright);
        gb.Controls.Add(lblLine);
        gb.Controls.Add(lblVersion);
        gb.Controls.Add(lblTitle);
        gb.Controls.Add(pbIcon);
        gb.Controls.Add(lblInfo);
        gb.Location = new Point(6, -2);
        gb.Name = "gb";
        gb.Size = new Size(398, 126);
        gb.TabIndex = 0;
        gb.TabStop = false;
        // 
        // lblBuild
        // 
        lblBuild.AutoSize = true;
        lblBuild.BackColor = Color.Transparent;
        lblBuild.Location = new Point(80, 39);
        lblBuild.Name = "lblBuild";
        lblBuild.Size = new Size(158, 15);
        lblBuild.TabIndex = 2;
        lblBuild.Text = "Revision: xxxx1 - Date: xxxx2";
        // 
        // lblWarning
        // 
        lblWarning.BackColor = Color.Transparent;
        lblWarning.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point);
        lblWarning.ForeColor = SystemColors.ControlDarkDark;
        lblWarning.Location = new Point(6, 83);
        lblWarning.Name = "lblWarning";
        lblWarning.Size = new Size(390, 40);
        lblWarning.TabIndex = 5;
        lblWarning.Text = resources.GetString("lblWarning.Text");
        // 
        // lblCopyright
        // 
        lblCopyright.AutoSize = true;
        lblCopyright.BackColor = Color.Transparent;
        lblCopyright.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point);
        lblCopyright.Location = new Point(80, 60);
        lblCopyright.Name = "lblCopyright";
        lblCopyright.Size = new Size(282, 15);
        lblCopyright.TabIndex = 4;
        lblCopyright.Text = "Copyright (c) 2023 sMk Designs - All Rights Reserved";
        // 
        // lblLine
        // 
        lblLine.BackColor = SystemColors.ControlText;
        lblLine.Location = new Point(80, 55);
        lblLine.Name = "lblLine";
        lblLine.Size = new Size(285, 2);
        lblLine.TabIndex = 3;
        // 
        // lblVersion
        // 
        lblVersion.AutoSize = true;
        lblVersion.BackColor = Color.Transparent;
        lblVersion.Font = new Font("Segoe UI", 16F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
        lblVersion.ForeColor = SystemColors.ActiveCaption;
        lblVersion.Location = new Point(264, 11);
        lblVersion.Name = "lblVersion";
        lblVersion.Size = new Size(70, 30);
        lblVersion.TabIndex = 1;
        lblVersion.Text = "v1.00";
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold, GraphicsUnit.Point);
        lblTitle.ForeColor = SystemColors.ControlDarkDark;
        lblTitle.Location = new Point(79, 11);
        lblTitle.Margin = new Padding(3, 0, 3, 5);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(188, 28);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "sMk Task Manager";
        // 
        // pbIcon
        // 
        pbIcon.BackColor = Color.Transparent;
        pbIcon.Location = new Point(4, 12);
        pbIcon.Margin = new Padding(3, 3, 10, 3);
        pbIcon.Name = "pbIcon";
        pbIcon.Size = new Size(64, 64);
        pbIcon.SizeMode = PictureBoxSizeMode.AutoSize;
        pbIcon.TabIndex = 0;
        pbIcon.TabStop = false;
        pbIcon.Paint += pbIcon_Paint;
        // 
        // lblInfo
        // 
        lblInfo.BackColor = Color.Transparent;
        lblInfo.ForeColor = Color.DarkRed;
        lblInfo.Location = new Point(298, 10);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(100, 43);
        lblInfo.TabIndex = 6;
        lblInfo.Text = "Debug";
        lblInfo.TextAlign = ContentAlignment.TopRight;
        // 
        // tmrIcon
        // 
        tmrIcon.Interval = 50;
        tmrIcon.Tick += tmrIcon_Tick;
        // 
        // ilAnimation
        // 
        ilAnimation.ColorDepth = ColorDepth.Depth32Bit;
        ilAnimation.ImageSize = new Size(64, 64);
        ilAnimation.TransparentColor = Color.Transparent;
        // 
        // frmAbout
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(410, 129);
        Controls.Add(gb);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmAbout";
        ShowInTaskbar = false;
        SizeGripStyle = SizeGripStyle.Hide;
        StartPosition = FormStartPosition.CenterParent;
        Text = "About";
        FormClosing += frmAbout_FormClosing;
        Load += frmAbout_Load;
        KeyDown += frmAbout_KeyDown;
        gb.ResumeLayout(false);
        gb.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pbIcon).EndInit();
        ResumeLayout(false);
    }

    private GroupBox gb;
    private System.Windows.Forms.Timer tmrIcon;
    private ImageList ilAnimation;
    private PictureBox pbIcon;
    private Label lblCopyright;
    private Label lblLine;
    private Label lblBuild;
    private Label lblVersion;
    private Label lblTitle;
    private Label lblInfo;
    private Label lblWarning;
}
