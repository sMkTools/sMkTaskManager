using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Versioning;
using sMkTaskManager.Classes;
using sMkTaskManager.Controls;
namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public class tabProcesses : UserControl {

    private IContainer? components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }

    public tabProcesses() {
        InitializeComponent();
        InitializeContextMenu();
        Extensions.CascadingDoubleBuffer(this);
    }
    private void InitializeComponent() {
        components = new Container();
        lv = new sMkListView();
        lvc_PID = new ColumnHeader();
        lvc_Name = new ColumnHeader();
        cms = new ContextMenuStrip(components);
        btnDetails = new Button();
        btnProperties = new Button();
        btnKill = new Button();
        btnForceRefresh = new Button();
        lblText = new Label();
        btnAllUsers = new CheckBox();
        SuspendLayout();
        // 
        // lv
        // 
        lv.AllowColumnReorder = true;
        lv.AlternateRowColors = false;
        lv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lv.Columns.AddRange(new ColumnHeader[] { lvc_PID, lvc_Name });
        lv.ContextMenuStrip = cms;
        lv.FullRowSelect = true;
        lv.GridLines = true;
        lv.Location = new Point(6, 6);
        lv.Margin = new Padding(6, 6, 6, 0);
        lv.Name = "lv";
        lv.ShowGroups = false;
        lv.Size = new Size(588, 559);
        lv.Sortable = true;
        lv.SortColumn = 0;
        lv.Sorting = SortOrder.Ascending;
        lv.TabIndex = 0;
        lv.UseCompatibleStateImageBehavior = false;
        lv.View = View.Details;
        lv.ColumnReordered += OnListViewColumnReordered;
        lv.SelectedIndexChanged += OnListViewSelectedIndexChanged;
        lv.KeyDown += OnListViewKeyDown;
        lv.KeyPress += OnListViewKeyPress;
        lv.MouseDoubleClick += OnListViewMouseDoubleClick;
        // 
        // lvc_PID
        // 
        lvc_PID.Tag = "PID";
        lvc_PID.Text = "PID";
        // 
        // lvc_Name
        // 
        lvc_Name.Tag = "Name";
        lvc_Name.Text = "Name";
        lvc_Name.Width = 150;
        // 
        // cms
        // 
        cms.Name = "cms";
        cms.Size = new Size(61, 4);
        // 
        // btnDetails
        // 
        btnDetails.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnDetails.Enabled = false;
        btnDetails.Location = new Point(6, 571);
        btnDetails.Margin = new Padding(6, 6, 0, 6);
        btnDetails.Name = "btnDetails";
        btnDetails.Size = new Size(75, 23);
        btnDetails.TabIndex = 1;
        btnDetails.Text = "Details";
        btnDetails.UseVisualStyleBackColor = true;
        btnDetails.Click += OnButtonClicked;
        // 
        // btnProperties
        // 
        btnProperties.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnProperties.Enabled = false;
        btnProperties.Location = new Point(84, 571);
        btnProperties.Margin = new Padding(3, 6, 0, 6);
        btnProperties.Name = "btnProperties";
        btnProperties.Size = new Size(75, 23);
        btnProperties.TabIndex = 2;
        btnProperties.Text = "Properties";
        btnProperties.UseVisualStyleBackColor = true;
        btnProperties.Click += OnButtonClicked;
        // 
        // btnKill
        // 
        btnKill.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnKill.Enabled = false;
        btnKill.Location = new Point(162, 571);
        btnKill.Margin = new Padding(3, 6, 0, 6);
        btnKill.Name = "btnKill";
        btnKill.Size = new Size(78, 23);
        btnKill.TabIndex = 3;
        btnKill.Text = "Kill Process";
        btnKill.UseVisualStyleBackColor = true;
        btnKill.Click += OnButtonClicked;
        // 
        // btnForceRefresh
        // 
        btnForceRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnForceRefresh.Location = new Point(504, 571);
        btnForceRefresh.Margin = new Padding(0, 6, 6, 6);
        btnForceRefresh.Name = "btnForceRefresh";
        btnForceRefresh.Size = new Size(90, 23);
        btnForceRefresh.TabIndex = 6;
        btnForceRefresh.Text = "Force Refresh";
        btnForceRefresh.UseVisualStyleBackColor = true;
        btnForceRefresh.Click += OnButtonClicked;
        // 
        // lblText
        // 
        lblText.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lblText.AutoEllipsis = true;
        lblText.ForeColor = SystemColors.HotTrack;
        lblText.Location = new Point(338, 575);
        lblText.Name = "lblText";
        lblText.Size = new Size(163, 16);
        lblText.TabIndex = 5;
        // 
        // btnAllUsers
        // 
        btnAllUsers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnAllUsers.Appearance = Appearance.Button;
        btnAllUsers.Location = new Point(243, 571);
        btnAllUsers.Name = "btnAllUsers";
        btnAllUsers.Size = new Size(92, 23);
        btnAllUsers.TabIndex = 4;
        btnAllUsers.Text = "Show all Users";
        btnAllUsers.TextAlign = ContentAlignment.MiddleCenter;
        btnAllUsers.UseVisualStyleBackColor = true;
        btnAllUsers.CheckedChanged += OnButtonClicked;
        // 
        // tabProcesses
        // 
        Controls.Add(btnAllUsers);
        Controls.Add(lblText);
        Controls.Add(btnForceRefresh);
        Controls.Add(btnKill);
        Controls.Add(btnProperties);
        Controls.Add(btnDetails);
        Controls.Add(lv);
        Name = "tabProcesses";
        Size = new Size(600, 600);
        ResumeLayout(false);
    }

    private void InitializeContextMenu() {
        cms.Items.Clear();
        cms.Items.AddMenuItem("&More Details").Name = "Details";
        cms.Items.AddMenuItem("File Prope&rties").Name = "Properties";
        cms.Items.AddMenuItem("&Open Location").Name = "Location";
        cms.Items.AddSeparator();
        cms.Items.AddMenuItem("&Kill Process").Name = "Kill";
        cms.Items.AddMenuItem("Free&ze Process").Name = "Pause";
        cms.Items.AddMenuItem("&Resume Process").Name = "Resume";
        cms.Items.AddSeparator();
        cms.Items.AddMenuItem("&Debug Process").Name = "Debug";
        cms.Items.AddMenuItem("Create D&ump File").Name = "Dump";
        cms.Items.AddSeparator();
        cms.Items.AddMenuItem("Set &Priority").Name = "Priority";
        cms.Items.AddMenuItem("Set &Affinity...").Name = "Affinity";
        cms.Items.AddSeparator();
        cms.Items.AddMenuItem("Search &Online").Name = "Online";
        cms.Items.AddMenuItem("Reveal &Windows").Name = "Windows";
        cms.Items.AddMenuItem("Show &Locked Files").Name = "Files";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("RealTime").Name = "Priority1";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("High").Name = "Priority2";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("Above Normal").Name = "Priority3";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("Normal").Name = "Priority4";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("Below Normal").Name = "Priority5";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownItems.AddMenuItem("Low").Name = "Priority6";
        ((ToolStripMenuItem)cms.Items["Priority"]).DropDownOpening += OnContextPriorityOpening;
        cms.Opening += OnContextOpening;
        cms.ItemClicked += OnContextItemClicked;
    }

    internal Button btnDetails;
    internal Button btnProperties;
    internal Button btnKill;
    internal Button btnForceRefresh;
    internal Label lblText;
    internal ColumnHeader lvc_PID;
    internal ColumnHeader lvc_Name;
    internal ContextMenuStrip cms;
    internal CheckBox btnAllUsers;
    internal sMkListView lv;

    private void OnContextOpening(object? sender, CancelEventArgs e) {
        e.Cancel = lv.SelectedItems.Count == 0;

        try {
            // We should not allow actions on PID < bpi
            for (int i = 0; i < lv.SelectedItems.Count; i++) {
                if ((i + 1) > lv.SelectedItems.Count) { break; }
                if (int.Parse(lv.SelectedItems[i].Name) < Shared.bpi) {
                    if (lv.SelectedItems.Count == 1 && i == 0) {
                        // If its the only one selected then dont deselect it, just cancel 
                        e.Cancel = true;
                        return;
                    }
                    // Otherwise unselect it
                    lv.SelectedItems[i].Selected = false;
                    i -= 1;
                }
            }
            e.Cancel = lv.SelectedItems.Count == 0;
        } catch { Debug.WriteLine("Error Code 008a"); }

        cms.Items["Kill"].Enabled = lv.SelectedItems.Count >= 1;
        cms.Items["Pause"].Enabled = lv.SelectedItems.Count >= 1;
        cms.Items["Resume"].Enabled = lv.SelectedItems.Count >= 1;
        cms.Items["Priority"].Enabled = lv.SelectedItems.Count >= 1;
        cms.Items["Affinity"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Details"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Properties"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Location"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Debug"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Dump"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Files"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Online"].Enabled = lv.SelectedItems.Count == 1;
        cms.Items["Windows"].Enabled = lv.SelectedItems.Count == 1;
    }
    private void OnContextPriorityOpening(object? sender, EventArgs e) {
        ProcessPriorityClass curPriority = 0;
        foreach (ListViewItem itm in lv.SelectedItems) {
            try {
                if (itm.Name == "" || !Shared.IsInteger(itm.Name)) continue;
                if (curPriority == 0) {
                    // This will trigger for the first item, so we save it
                    curPriority = Process.GetProcessById(int.Parse(itm.Name)).PriorityClass;
                    continue;
                }
                if (curPriority != Process.GetProcessById(int.Parse(itm.Name)).PriorityClass) {
                    // If any of the other process is diffferent, we set it to 0 and leave, they are not all equal.
                    curPriority = 0;
                    break;
                }
            } catch { Debug.WriteLine("Error Code 008b"); }
        }
        try {
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority1"]).Checked = curPriority == ProcessPriorityClass.RealTime;
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority2"]).Checked = curPriority == ProcessPriorityClass.High;
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority3"]).Checked = curPriority == ProcessPriorityClass.AboveNormal;
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority4"]).Checked = curPriority == ProcessPriorityClass.Normal;
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority5"]).Checked = curPriority == ProcessPriorityClass.BelowNormal;
            ((ToolStripMenuItem)((ToolStripMenuItem)sender!).DropDownItems["Priority6"]).Checked = curPriority == ProcessPriorityClass.Idle;
        } catch { Debug.WriteLine("Error Code 008c"); }
    }

    private void OnContextItemClicked(object? sender, ToolStripItemClickedEventArgs e) {
        if (e.ClickedItem == null) return;
        if (!e.ClickedItem.Enabled) return;
        switch (e.ClickedItem.Name) {
            case "Details": Feature_OpenDetails(); break;
            case "Properties": Feature_OpenFileProperties(); break;
            case "Location": Feature_OpenFileLocation(); break;
            case "Kill": Feature_ProcessKill(); break;
            case "Pause": Feature_ProcessPause(); break;
            case "Resume": Feature_ProcessResume(); break;
            case "Debug": Feature_ProcessDebug(); break;
            case "Dump": Feature_ProcessDump(); break;
            case "Affinity": Feature_ProcessAffinity(); break;
            case "Priority1": Feature_ProcessSetPriority(ProcessPriorityClass.RealTime); break;
            case "Priority2": Feature_ProcessSetPriority(ProcessPriorityClass.High); break;
            case "Priority3": Feature_ProcessSetPriority(ProcessPriorityClass.AboveNormal); break;
            case "Priority4": Feature_ProcessSetPriority(ProcessPriorityClass.Normal); break;
            case "Priority5": Feature_ProcessSetPriority(ProcessPriorityClass.BelowNormal); break;
            case "Priority6": Feature_ProcessSetPriority(ProcessPriorityClass.Idle); break;
            case "Online": Feature_OpenOnline(); break;
            case "Windows": Feature_RevealWindows(); break;
            case "Files": Feature_OpenLockedFiles(); break;
            default: break;
        }
    }
    private void OnButtonClicked(object? sender, EventArgs e) {
        if (sender == null) return;
        if (sender == btnDetails) { Feature_OpenDetails(); }
        if (sender == btnProperties) { Feature_OpenFileProperties(); }
        if (sender == btnKill) { Feature_ProcessKill(); }
        if (sender == btnForceRefresh) { Feature_ForceRefresh(); }
        if (sender == btnAllUsers) { Feature_ForceRefresh(); Settings.ShowAllProcess = btnAllUsers.Checked; }
    }
    private void OnListViewKeyDown(object? sender, KeyEventArgs e) {
        if (e.Control && e.KeyCode == Keys.A) {
            e.Handled = true;
            foreach (sMkListViewItem itm in lv.Items) { itm.Selected = true; }
        }
        if (e.KeyCode == Keys.Enter && !e.Handled) {
            e.Handled = true;
            btnDetails.PerformClick();
        }
    }
    private void OnListViewKeyPress(object? sender, KeyPressEventArgs e) {
        lv.KeyJumper("Name", ref e);
    }
    private void OnListViewColumnReordered(object? sender, ColumnReorderedEventArgs e) {
        if (e.Header!.Text == "PID") {
            e.Cancel = true;
        } else if (e.NewDisplayIndex == 0) {
            e.Cancel = true;
        }
    }
    private void OnListViewMouseDoubleClick(object? sender, MouseEventArgs e) {
        if (lv.SelectedItems.Count > 0 && e.Button == MouseButtons.Left) { Feature_OpenDetails(); }
    }
    private void OnListViewSelectedIndexChanged(object? sender, EventArgs e) {
        btnDetails.Enabled = lv.SelectedItems.Count == 1;
        btnProperties.Enabled = lv.SelectedItems.Count == 1;
        btnKill.Enabled = lv.SelectedItems.Count > 0;
        // TODO: ShowProcessSummary Not implemented yet
        //if (ShowProcessSummary && lv.SelectedItems.Count > 0) {
        //    TaskManagerProcess thisProcess = m_Processes.GetProcess(int.Parse(lv.SelectedItems[0].Name));
        //    if (thisProcess != null) {
        //        proc_SummaryView.Values = thisProcess.GetValuesFromTags(proc_SummaryView.RowsTags);
        //    }
        //    thisProcess = null;
        //} else if (ShowProcessSummary && proc_ListView.SelectedItems.Count == 0) {
        //    proc_SummaryView.ClearValues();
        //}
        RefreshInfoText();
    }

    public void Feature_ForceRefresh() { Shared.NotImplemented(); }
    public void Feature_OpenDetails() {
        if (lv.SelectedItems.Count > 0) {
            if (int.Parse(lv.SelectedItems[0].Name) < Shared.bpi) { return; }
            Shared.OpenProcessForm(int.Parse(lv.SelectedItems[0].Name));
        }
    }
    public void Feature_OpenFileProperties() { Shared.NotImplemented(); }
    public void Feature_OpenFileLocation() { Shared.NotImplemented(); }
    public void Feature_OpenLockedFiles() { Shared.NotImplemented(); }
    public void Feature_RevealWindows() { Shared.NotImplemented(); }
    public void Feature_OpenOnline() { Shared.NotImplemented(); }
    public void Feature_ProcessKill() { Shared.NotImplemented(); }
    public void Feature_ProcessPause() { Shared.NotImplemented(); }
    public void Feature_ProcessResume() { Shared.NotImplemented(); }
    public void Feature_ProcessDebug() { Shared.NotImplemented(); }
    public void Feature_ProcessDump() { Shared.NotImplemented(); }
    public void Feature_ProcessAffinity() { Shared.NotImplemented(); }
    public void Feature_ProcessSetPriority(ProcessPriorityClass newPriority) { Shared.NotImplemented(); }
    public ProcessPriorityClass Feature_ProcessGetPriority() { return ProcessPriorityClass.Normal; }

    public bool AllUsers => btnAllUsers.Checked;
    public void RefreshInfoText() {
        lblText.Text = string.Format("Total: {0}, Selected: {1}", lv.Items.Count, lv.SelectedItems.Count);
    }
    public string InfoText {
        get { return lblText.Text; }
        set { lblText.Text = value; }
    }
}