using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public class frmProcess_Files : Form {
    private Process? p = null;
    private ColumnHeader colName;
    private ColumnHeader colPath;
    private IContainer? components = null;

    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }

    private void InitializeComponent() {
        btnRelease = new Button();
        btnRefresh = new Button();
        btnClose = new Button();
        lblTotal = new Label();
        lv = new ListView();
        colName = new ColumnHeader();
        colPath = new ColumnHeader();
        SuspendLayout();
        // 
        // btnRelease
        // 
        btnRelease.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnRelease.Location = new Point(6, 203);
        btnRelease.Name = "btnRelease";
        btnRelease.Size = new Size(99, 23);
        btnRelease.TabIndex = 1;
        btnRelease.Text = "Release File(s)";
        btnRelease.UseVisualStyleBackColor = true;
        // 
        // btnRefresh
        // 
        btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnRefresh.Location = new Point(109, 203);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(100, 23);
        btnRefresh.TabIndex = 2;
        btnRefresh.Text = "Refresh List";
        btnRefresh.UseVisualStyleBackColor = true;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnClose.DialogResult = DialogResult.Cancel;
        btnClose.Location = new Point(453, 203);
        btnClose.Name = "btnClose";
        btnClose.Size = new Size(75, 23);
        btnClose.TabIndex = 4;
        btnClose.Text = "Close";
        btnClose.UseVisualStyleBackColor = true;
        btnClose.Click += btnClose_Click;
        // 
        // lblTotal
        // 
        lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblTotal.Location = new Point(215, 203);
        lblTotal.Name = "lblTotal";
        lblTotal.Size = new Size(232, 23);
        lblTotal.TabIndex = 3;
        lblTotal.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lv
        // 
        lv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lv.Columns.AddRange(new ColumnHeader[] { colName, colPath });
        lv.FullRowSelect = true;
        lv.Location = new Point(6, 6);
        lv.Name = "lv";
        lv.Size = new Size(522, 192);
        lv.TabIndex = 0;
        lv.UseCompatibleStateImageBehavior = false;
        lv.View = View.Details;
        // 
        // colName
        // 
        colName.Text = "Name";
        // 
        // colPath
        // 
        colPath.Text = "Path";
        // 
        // frmProcess_Files
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnClose;
        ClientSize = new Size(534, 231);
        Controls.Add(lblTotal);
        Controls.Add(btnClose);
        Controls.Add(btnRefresh);
        Controls.Add(btnRelease);
        Controls.Add(lv);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        Name = "frmProcess_Files";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Locked Files";
        ResumeLayout(false);
    }

    private Button btnRelease;
    private Button btnRefresh;
    private Button btnClose;
    private Label lblTotal;
    private ListView lv;

    public frmProcess_Files() {
        InitializeComponent();
        Load += OnLoad;
        KeyPress += OnKeyPress;
    }

    private void OnLoad(object? sender, EventArgs e) {
        LoadProcessFiles();
    }
    private void OnKeyPress(object? sender, KeyPressEventArgs e) {
        if (e.KeyChar == Convert.ToChar(Keys.Escape)) { Close(); }
    }
    private void btnClose_Click(object? sender, EventArgs e) {
        Close();
    }

    public int PID {
        get { return (p == null) ? 0 : p.Id; }
        set { p = Process.GetProcessById(value); if (Visible) { LoadProcessFiles(); } }
    }
    private void LoadProcessFiles() { }

}
