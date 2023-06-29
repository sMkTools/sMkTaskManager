using System.Diagnostics;

namespace sMkTaskManager.Forms;

public partial class frmColumns : Form {
    private readonly int EnlargeWidthBy = 100;
    public enum ColumnListType : int {
        Process = 1,
        Services = 2,
        Connections = 3
    }
    public ColumnListType ListType { get; set; }
    public ListView.ListViewItemCollection ColItems => lv.Items;

    public frmColumns() {
        InitializeComponent();
        ListType = ColumnListType.Process;
    }
    public frmColumns(ColumnListType Type) : this() {
        ListType = Type;
    }

    private void frmColumns_Load(object sender, EventArgs e) {
        lv_SelectedIndexChanged(this, new EventArgs());
        lv.Columns[0].Width = -2;
        lv.Columns[0].Width -= 5;
        if (ListType == ColumnListType.Process) chkSmallIcons.Checked = true;
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
            // MoveListViewItem(lv, true);
            lv.Focus();
        }

    }
    private void btnMoveDown_Click(object sender, EventArgs e) {
        if (lv.SelectedItems.Count > 0) {
            // MoveListViewItem(lv, false);
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
