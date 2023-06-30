using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Versioning;
using sMkTaskManager.Classes;

namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmColumns : Form {
    private readonly int EnlargeWidthBy = 100;
    public TaskManagerColumnTypes ListType { get; private set; }
    public ListView.ListViewItemCollection ColItems => lv.Items;

    public frmColumns() {
        InitializeComponent();
        ListType = TaskManagerColumnTypes.Process;
    }
    public frmColumns(TaskManagerColumnTypes Type) : this() {
        ListType = Type;
        LoadAllColumns();
    }

    private void frmColumns_Load(object sender, EventArgs e) {
        lv_SelectedIndexChanged(this, new EventArgs());
        lv.Columns[0].Width = -2;
        lv.Columns[0].Width -= 5;
        if (ListType == TaskManagerColumnTypes.Process) chkSmallIcons.Checked = true;
    }

    private void btnOk_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.OK;
        Close();
    }
    private void btnCancel_Click(object sender, EventArgs e) {
        DialogResult = DialogResult.Cancel;
        Close();

    }
    private void btnMoveUp_Click(object sender, EventArgs e) {
        if (lv.SelectedItems.Count > 0) {
            MoveListViewItem(true);
            lv.Focus();
        }

    }
    private void btnMoveDown_Click(object sender, EventArgs e) {
        if (lv.SelectedItems.Count > 0) {
            MoveListViewItem(false);
            lv.Focus();
        }

    }
    private void btnShow_Click(object sender, EventArgs e) {
        foreach (ListViewItem i in lv.SelectedItems) { i.Checked = true; }
        lv.Focus();
    }
    private void btnHide_Click(object sender, EventArgs e) {
        foreach (ListViewItem i in lv.SelectedItems) { i.Checked = false; }
        lv.Focus();

    }
    private void btnDefaults_Click(object sender, EventArgs e) {
        lv.BeginUpdate();
        LoadAllColumns(true);
        if (chkSmallIcons.Checked) {
            lv.Columns[0].Width = (lv.Width - 20) / 2;
        } else {
            lv.Columns[0].Width = -2;
            lv.Columns[0].Width -= 5;
        }
        lv.EndUpdate();
    }
    private void txtWidth_KeyPress(object sender, KeyPressEventArgs e) {
        e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back);
    }
    private void txtWidth_LostFocus(object sender, EventArgs e) {
        txtWidth.Text = txtWidth.Text.Trim();
        if (Shared.IsNumeric(txtWidth.Text)) {
            if (int.Parse(txtWidth.Text) < 1) { txtWidth.Text = "1"; }
            if (int.Parse(txtWidth.Text) > 300) { txtWidth.Text = "300"; }
            if (lv.SelectedItems.Count < 1) return;
            lv.SelectedItems[0].ImageIndex = int.Parse(txtWidth.Text);
        } else {
            txtWidth.Text = (lv.SelectedItems.Count > 0) ? lv.SelectedItems[0].ImageIndex.ToString() : "50";
        }
    }
    private void chkSmallIcons_CheckedChanged(object sender, EventArgs e) {
        lv.BeginUpdate();
        if (chkSmallIcons.Checked) {
            lv.View = View.SmallIcon;
            Width += (EnlargeWidthBy - 1);
            lv.Columns[0].Width = (lv.Width - 20) / 2;
            lv.Refresh();
            Width += 1;
            Cursor.Position = new Point(Cursor.Position.X + EnlargeWidthBy, Cursor.Position.Y);
        } else {
            Width -= EnlargeWidthBy;
            lv.View = View.Details;
            lv.Columns[0].Width = -2;
            lv.Columns[0].Width -= 5;
            Cursor.Position = new Point(Cursor.Position.X - EnlargeWidthBy, Cursor.Position.Y);
        }
        lv.EndUpdate();
    }
    private void lv_SelectedIndexChanged(object sender, EventArgs e) {
        txtWidth.Enabled = lv.SelectedItems.Count > 0;
        if (lv.SelectedItems.Count > 0) {
            txtWidth.Text = lv.SelectedItems[0].ImageIndex.ToString();
            btnHide.Enabled = lv.SelectedItems[0].Checked && !(lv.SelectedItems[0].ForeColor == Color.FromKnownColor(KnownColor.GrayText));
            btnShow.Enabled = !(lv.SelectedItems[0].Checked);
            btnMoveUp.Enabled = lv.SelectedItems[0].Index > 0 && !(lv.SelectedItems[0].ForeColor == Color.FromKnownColor(KnownColor.GrayText));
            btnMoveDown.Enabled = lv.SelectedItems[0].Index < lv.Items.Count - 1 && !(lv.SelectedItems[0].ForeColor == Color.FromKnownColor(KnownColor.GrayText));
            if (btnMoveUp.Enabled) { btnMoveUp.Enabled = lv.SelectedItems[0].Group.Equals(lv.Items[lv.SelectedItems[0].Index - 1].Group); }
            if (btnMoveDown.Enabled) { btnMoveDown.Enabled = lv.SelectedItems[0].Group.Equals(lv.Items[lv.SelectedItems[0].Index + 1].Group); }
            if (btnMoveUp.Enabled) { btnMoveUp.Enabled = !(lv.Items[lv.SelectedItems[0].Index - 1].ForeColor == Color.FromKnownColor(KnownColor.GrayText)); }
            if (btnMoveDown.Enabled) { btnMoveDown.Enabled = !(lv.Items[lv.SelectedItems[0].Index + 1].ForeColor == Color.FromKnownColor(KnownColor.GrayText)); }
        } else {
            btnHide.Enabled = false;
            btnShow.Enabled = false;
            btnMoveDown.Enabled = false;
            btnMoveUp.Enabled = false;
        }
    }
    private void lv_ItemCheck(object sender, ItemCheckEventArgs e) {
        if (lv.Items[e.Index].ForeColor == Color.FromKnownColor(KnownColor.GrayText)) {
            lv.Items[e.Index].ForeColor = Color.FromKnownColor(KnownColor.GrayText);
            e.NewValue = CheckState.Checked;
        }
    }
    private void lv_ItemChecked(object sender, ItemCheckedEventArgs e) {
        lv_SelectedIndexChanged(lv, new EventArgs());
    }
    private void lv_KeyDown(object sender, KeyEventArgs e) {
        if (e.Control && e.KeyCode == Keys.Up) { btnMoveUp.PerformClick(); e.Handled = true; }
        if (e.Control && e.KeyCode == Keys.Down) { btnMoveDown.PerformClick(); e.Handled = true; }
    }
    private void lv_SizeChanged(object sender, EventArgs e) {
        // TODO: Migrate and implement this at some point
        // sMkAPIs.ShowScrollBar(lv.Handle, 0, False);
    }

    private void MoveListViewItem(bool moveUp) {
        if (lv.SelectedItems.Count == 0) return;
        int selIdx = lv.SelectedItems[0].Index;
        int nexIdx = moveUp ? selIdx - 1 : selIdx + 1;

        // ignore MoveUp or MoveDown if needed (first/last)
        if (moveUp && selIdx == 0) return;
        if (!moveUp && selIdx == lv.Items.Count - 1) return;
        if (!lv.Items[nexIdx].Group.Equals(lv.Items[selIdx].Group)) return;

        ListViewItem cacheItem = (ListViewItem)lv.Items[nexIdx].Clone();
        string cacheName = lv.Items[nexIdx].Name;
        // move the subitems for the previous row to cache so we can move the selected row up
        lv.Items[nexIdx].Name = lv.Items[selIdx].Name;
        lv.Items[nexIdx].Text = lv.Items[selIdx].Text;
        lv.Items[nexIdx].Tag = lv.Items[selIdx].Tag;
        lv.Items[nexIdx].ToolTipText = lv.Items[selIdx].ToolTipText;
        lv.Items[nexIdx].ImageIndex = lv.Items[selIdx].ImageIndex;
        lv.Items[nexIdx].ImageKey = lv.Items[selIdx].ImageKey;
        lv.Items[nexIdx].StateImageIndex = lv.Items[selIdx].StateImageIndex;
        lv.Items[nexIdx].IndentCount = lv.Items[selIdx].IndentCount;
        lv.Items[nexIdx].ForeColor = lv.Items[selIdx].ForeColor;

        lv.Items[selIdx].Name = cacheName;
        lv.Items[selIdx].Text = cacheItem.Text;
        lv.Items[selIdx].Tag = cacheItem.Tag;
        lv.Items[selIdx].ToolTipText = cacheItem.ToolTipText;
        lv.Items[selIdx].ImageIndex = cacheItem.ImageIndex;
        lv.Items[selIdx].ImageKey = cacheItem.ImageKey;
        lv.Items[selIdx].StateImageIndex = cacheItem.StateImageIndex;
        lv.Items[selIdx].IndentCount = cacheItem.IndentCount;
        lv.Items[selIdx].ForeColor = cacheItem.ForeColor;

        lv.Items[nexIdx].Selected = true;
        lv.Items[nexIdx].Focused = true;
    }
    internal void LoadAllColumns(bool checkDefaults = false) {
        lv.Items.Clear();
        foreach (TaskManagerColumn c in TaskManagerColumn.GetColumnDefinition(ListType)) {
            ListViewGroup? itmGroup = null;
            foreach (ListViewGroup g in lv.Groups) {
                if (g.Header == c.Group) { itmGroup = g; break; }
            }
            if (itmGroup == null) {
                itmGroup = new() { Name = c.Group, Header = c.Group };
                lv.Groups.Add(itmGroup);
            }
            ListViewItem itm = new() {
                Name = c.Tag,
                Tag = c.Tag,
                Group = itmGroup,
                Text = c.Label,
                ToolTipText = c.Title,
                ImageIndex = c.Width,
                IndentCount = c.Align
            };
            if (checkDefaults) itm.Checked = c.Default;
            if (c.Fixed) {
                itm.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
                itm.Checked = true;
            }
            lv.Items.Add(itm);
        }
    }
    internal void LoadCheckedColumns(in ListView.ColumnHeaderCollection Columns) {
        foreach (ColumnHeader c in Columns) {
            if (lv.Items.ContainsKey(c.Tag!.ToString())) {
                lv.Items[c.Tag.ToString()].Checked = true;
                lv.Items[c.Tag.ToString()].ImageIndex = c.Width;
            }
        }
    }
}
