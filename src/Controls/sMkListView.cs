using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using static sMkTaskManager.Controls.sMkListViewHelpers;
namespace sMkTaskManager.Controls;

[SupportedOSPlatform("windows")]
public class sMkListView : ListView {
    private readonly sMkColumnSorter _sorter = new();
    private BindingSource _dataSource = new();
    private PropertyDescriptorCollection _descriptors;
    private PropertyDescriptor? _keyDescriptor;
    private int _contextMenuSet = -1;
    private bool _alternateRowColors = false;
    private bool _needReSort = false;
    private long _lastKeyPress = DateTime.Now.Ticks;
    private string _lastKeyString = "";

    protected override void WndProc(ref Message m) {
        // We use this to prevent the context menu being displayed when right click on column headers.
        switch (m.Msg) {
            case 0x210: // WM_PARENTNOTIFY
                _contextMenuSet = 1; break;
            case 0x21: // WM_MOUSEACTIVATE
                _contextMenuSet += 1; break;
            case 0x7B: // WM_CONTEXTMENU
                // Cancel default context menu on the columnheader
                if (_contextMenuSet == 2) m.Msg = 0; break;
        }
        base.WndProc(ref m);
    }

    public sMkListView() : base() {
        Sortable = true;
        FullRowSelect = true;
        View = View.Details;
        DrawItem += OnDrawItem;
        DrawSubItem += OnDrawSubItem;
        DrawColumnHeader += OnDrawColumnHeader;
        ColumnClick += OnColumnClick;
        _dataSource.ListChanged += OnDataSource_ListChanged;
        _descriptors = _dataSource.GetItemProperties(null);
        ListViewItemSorter = _sorter;
    }

    public event EventHandler? ViewChanged;

    public bool SpaceFirstValue = false;
    public Color AlternateRowColor = SystemColors.Control;
    public bool AlternateRowColors {
        get { return _alternateRowColors; }
        set {
            if (value == _alternateRowColors) return;
            _alternateRowColors = value;
            OwnerDraw = value;
            Refresh();
        }
    }
    public bool Sortable {
        get { return !(ListViewItemSorter == null); }
        set {
            _sorter.SortColumn = 0;
            _sorter.Order = SortOrder.Ascending;
            Sorting = value ? _sorter.Order : SortOrder.None;
            ListViewItemSorter = value ? _sorter : null;
            if (value) SetSortIcons(ref _sorter.PrevColumn, 0);
        }
    }
    public BindingSource? DataSource {
        get => _dataSource;
        set {
            if ((value == null) || (_dataSource == value)) return;
            _dataSource = value;
            _dataSource.ListChanged += OnDataSource_ListChanged;
            _descriptors = _dataSource.GetItemProperties(null);
        }
    }
    public Type? ContentType;
    public new SortOrder Sorting {
        get { return Sortable ? base.Sorting : SortOrder.None; }
        set { if (Sortable) { base.Sorting = _sorter.Order = value; Sort(); } }
    }
    public int SortColumn {
        get { return (ListViewItemSorter == null) ? -1 : _sorter.SortColumn; }
        set {
            _sorter.SortColumn = value;
            Sort();
            SetSortIcons(ref _sorter.PrevColumn, _sorter.SortColumn);
            ListViewItemSorter = _sorter;
        }
    }
    public new View View {
        get { return base.View; }
        set {
            if (base.View != value) { base.View = value; ViewChanged?.Invoke(this, new EventArgs()); }
        }
    }
    public int ItemsCount {
        get => Items.Count;
    }

    public void SetSort(int SortColumn, SortOrder Order) {
        _sorter.SortColumn = SortColumn;
        _sorter.Order = Order;
        Sorting = _sorter.Order;
        Sort();
        SetSortIcons(ref _sorter.PrevColumn, _sorter.SortColumn);
        ListViewItemSorter = _sorter;
    }
    public void SetSortIcons(ref int previouslySortedColumn, int newSortColumn) {
        IntPtr hHeader = GetHeaderHnd(Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
        IntPtr newColumn = new(newSortColumn);
        IntPtr prevColumn = new(previouslySortedColumn);
        HDITEM HDITEM;

        // Only update the previous item if it existed and if it was a different one.
        if (previouslySortedColumn != -1 && previouslySortedColumn != newSortColumn) {
            // Clear icon from the previous column.
            HDITEM = new HDITEM { mask = HDI_FORMAT };
            SendMessageItem(hHeader, HDM_GETITEM, prevColumn, ref HDITEM);
            HDITEM.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP;
            SendMessageItem(hHeader, HDM_SETITEM, prevColumn, ref HDITEM);
        }

        if (newSortColumn != -1) {
            // Set icon on the new column.
            HDITEM = new() { mask = HDI_FORMAT };
            SendMessageItem(hHeader, HDM_GETITEM, newColumn, ref HDITEM);
            if (_sorter.Order == SortOrder.Ascending) {
                HDITEM.fmt &= ~HDF_SORTDOWN;
                HDITEM.fmt |= HDF_SORTUP;
            } else {
                HDITEM.fmt &= ~HDF_SORTUP;
                HDITEM.fmt |= HDF_SORTDOWN;
            }
            SendMessageItem(hHeader, HDM_SETITEM, newColumn, ref HDITEM);
        }
        previouslySortedColumn = newSortColumn;
    }
    public void KeyJumper(string colTag, ref KeyPressEventArgs e) {
        if (Items.Count < 1) return;
        if (!Items[0].SubItems.ContainsKey(colTag)) return;

        if (DateTime.Now.Ticks - _lastKeyPress > 4000000) { // Allow 0.4secs
            _lastKeyString = e.KeyChar.ToString();
        } else {
            if (!(_lastKeyString == e.KeyChar.ToString())) { _lastKeyString += e.KeyChar.ToString(); }
        }
        _lastKeyPress = DateTime.Now.Ticks;
        e.Handled = true;

        sMkListViewItem? itmFound = null;
        foreach (sMkListViewItem itmKey in Items) {
            try {
                if (itmKey.SubItems[colTag]!.Text.ToString().ToLower().StartsWith(_lastKeyString.ToLower())) {
                    if (!itmKey.Selected) {
                        itmFound ??= itmKey;
                        if (SelectedItems.Count > 0) {
                            if (SelectedItems[0].Index < itmKey.Index) {
                                SelectedItems.Clear();
                                itmKey.Selected = true;
                                FocusedItem = itmKey;
                                itmKey.EnsureVisible();
                                return;
                            } else if (!SelectedItems[0].SubItems[colTag]!.Text.ToString().ToLower().StartsWith(_lastKeyString.ToLower())) {
                                SelectedItems.Clear();
                                itmKey.Selected = true;
                                itmKey.EnsureVisible();
                                FocusedItem = itmKey;
                                return;
                            }
                        }
                    }
                }
            } catch (Exception ex) { Shared.DebugTrap(ex, 011); }
        }
        if (itmFound != null) {
            SelectedItems.Clear();
            itmFound.Selected = true;
            FocusedItem = itmFound;
            itmFound.EnsureVisible();
        }
    }

    public void RemoveItemByKey(string Key) {
        try {
            Items.RemoveByKey(Key);
        } catch { }
    }

    protected void OnDrawItem(object? sender, DrawListViewItemEventArgs e) {
        if (AlternateRowColors) {
            if (!(e.ItemIndex % 2 == 0)) {
                // We set the alternate color, but only if it doesnt have any other color first.
                if (e.Item.BackColor.IsEmpty || e.Item.BackColor == BackColor) e.Item.BackColor = AlternateRowColor;
            } else {
                // We set the default color, but only if it has the alternate color, otherwise leave as is.
                if (e.Item.BackColor.Equals(AlternateRowColor)) { e.Item.BackColor = Color.Empty; }
            }
        } else if (e.Item.BackColor.Equals(AlternateRowColor)) {
            e.Item.BackColor = Color.Empty;
            // Try and see if we have a color to remove, this is causing some issues
        }
        e.DrawDefault = true;
    }
    protected void OnDrawSubItem(object? sender, DrawListViewSubItemEventArgs e) {
        e.DrawDefault = true;
    }
    protected void OnDrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e) {
        e.DrawDefault = true;
    }
    protected void OnColumnClick(object? sender, ColumnClickEventArgs e) {
        if (!Sortable) { return; }
        if (ListViewItemSorter == null) return;
        if (e.Column == _sorter.SortColumn) {
            // Reverse the current sort direction for this column.
            _sorter.Order = (_sorter.Order == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
        } else {
            // Set the column number that is to be sorted; default to ascending.
            _sorter.SortColumn = e.Column;
            _sorter.Order = SortOrder.Ascending;
        }
        // This causes the listview to jump back to original position, so dont set
        // Sorting = _sorter.Order;
        SetSortIcons(ref _sorter.PrevColumn, e.Column);
        Sort();
        ListViewItemSorter = _sorter;
    }

    private void OnDataSource_ListChanged(object? sender, ListChangedEventArgs e) {
        switch (e.ListChangedType) {
            case ListChangedType.ItemAdded: OnDataSource_RowAdded(e); break;
            case ListChangedType.ItemChanged: OnDataSource_RowChanged(e); break;
                // case ListChangedType.ItemDeleted: OnDataSource_RowDeleted(e); break;
        }
    }
    private void OnDataSource_RowAdded(ListChangedEventArgs e) {
        var row = _dataSource[e.NewIndex];
        // If its a string, treat as such and return.
        if (row is string @string) { Items.Add(@string); return; }
        // Otherwise create a sMkListViewItem and populate
        var itm = new sMkListViewItem();
        // Internal properties that the item could have to format the ListViewItem.
        foreach (PropertyDescriptor d in _descriptors) {
            if (d.Name == "ID") { _keyDescriptor = d; itm.Name = (string)d.GetValue(row)!; }
            if (d.Name == "BackColor") { itm.BackColor = (Color)d.GetValue(row)!; }
            if ((d.Name == "ImageKey") && ((string?)d.GetValue(row) != "")) { itm.ImageKey = (string)d.GetValue(row)!; }
            if ((d.Name == "ImageIndex") && ((int?)d.GetValue(row) > -1)) { itm.ImageIndex = (int)d.GetValue(row)!; }
        }
        // Populate each value of the item if the column exists.
        foreach (ColumnHeader c in Columns) {
            var value = "";
            c.Tag ??= "";
            if (c.Tag.ToString()!.Trim() != "") {
                foreach (PropertyDescriptor d in _descriptors) {
                    if (d.Name.ToLower() == c.Tag.ToString()?.ToLower()) {
                        try {
                            value = d.GetValue(row)?.ToString();
                        } catch (Exception ex) {
                            Trace.TraceWarning($"sMkListView - Cannot Read Value {d.Name} From Row Type {row.GetType}, Error: {ex.Message}");
                        }
                        break;
                    }
                }
            }
            if (c.Index == 0) {
                itm.Text = SpaceFirstValue ? " " + value : value;
            } else {
                itm.SubItems.Add(value).Name = c.Tag.ToString();
            }
            _needReSort = _needReSort || c.Index == SortColumn;
        }
        Items.Add(itm);
        if (_needReSort && Sortable) { _needReSort = false; Sort(); }
    }
    private void OnDataSource_RowChanged(ListChangedEventArgs e) {
        var row = _dataSource[e.NewIndex];
        if (Items.Count < 1) return;
        if (e.PropertyDescriptor == null || _keyDescriptor == null || row == null) return;
        try {
            switch (e.PropertyDescriptor?.Name) {
                case "BackColor": // Set The Back Color
                    if (Items.ContainsKey(_keyDescriptor.GetValue(row)!.ToString())) {
                        Items[_keyDescriptor.GetValue(row)!.ToString()].BackColor = (Color)e.PropertyDescriptor.GetValue(row)!;
                    }
                    return;
                case "ImageKey": // Set The Image Key
                    if (Items.ContainsKey(_keyDescriptor.GetValue(row)!.ToString())) {
                        Items[_keyDescriptor.GetValue(row)!.ToString()].ImageKey = (string)e.PropertyDescriptor.GetValue(row)!;
                    }
                    return;
                case "ImageIndex": // Set The Image Index
                    if (Items.ContainsKey(_keyDescriptor.GetValue(row)!.ToString())) {
                        Items[_keyDescriptor.GetValue(row)!.ToString()].ImageIndex = (int)e.PropertyDescriptor.GetValue(row)!;
                    }
                    return;
                default: // Set Values...
                    foreach (ColumnHeader c in Columns) {
                        if (e.PropertyDescriptor?.Name.ToLower() == c.Tag?.ToString()?.ToLower()) {
                            if (!Items.ContainsKey(_keyDescriptor.GetValue(row)!.ToString())) break;
                            // Used to catch an error in the ForceRefresh
                            // Items[_keyDescriptor.GetValue(row)!.ToString()].SubItems[c.Index].Text = e.PropertyDescriptor!.GetValue(row)!.ToString();
                            Items[_keyDescriptor.GetValue(row)!.ToString()].SubItems[c.Index].Text = ((c.DisplayIndex == 0 && SpaceFirstValue) ? " " : "") + e.PropertyDescriptor!.GetValue(row)?.ToString();
                            _needReSort = _needReSort || c.Index == SortColumn;
                            break;
                        }
                    }
                    break;
            }
        } catch (Exception ex) {
            Debug.WriteLine("Method: {0}, Error: {1}", MethodBase.GetCurrentMethod()?.Name, ex.ToString());
        }
        if (_needReSort && Sortable) { _needReSort = false; Sort(); }
    }
    // private void OnDataSource_RowDeleted(ListChangedEventArgs e) { }

    public void SetColumns(in ListViewItemCollection colItems) {
        if (colItems == null) return;
        BeginUpdate();
        int curPosition = 0;
        foreach (ListViewItem c in colItems) {
            if (!c.Checked && Columns.ContainsKey(c.Name)) {
                Columns.RemoveByKey(c.Name);
            } else if (c.Checked && !Columns.ContainsKey(c.Name)) {
                ColumnHeader newCol = new() {
                    Name = c.Name,
                    Text = c.ToolTipText,
                    Tag = c.Tag,
                    TextAlign = (HorizontalAlignment)c.IndentCount,
                    Width = c.ImageIndex
                };
                if (curPosition <= Columns.Count) {
                    Columns.Insert(curPosition, newCol);
                } else {
                    Columns.Add(newCol);
                }
                curPosition += 1;
            } else if (c.Checked && Columns.ContainsKey(c.Name)) {
                curPosition += 1;
            }
        }
        // As a safety measure we should also remove any other column that is on the list.
        foreach (ColumnHeader c in Columns) {
            if (!colItems.ContainsKey(c.Tag?.ToString())) Columns.Remove(c);
        }
        EndUpdate();
    }
    public int TotalColumnsWidth(int exceptColumn) {
        int res = 0;
        for (int i = 0; i < Columns.Count; i++) {
            if (i != exceptColumn) res += Columns[i].Width;
        }
        return res;
    }
}

public static class sMkListViewHelpers {
    public const int HDI_FORMAT = 0x4;
    public const int HDF_LEFT = 0x0;
    public const int HDF_STRING = 0x4000;
    public const int HDF_SORTUP = 0x400;
    public const int HDF_SORTDOWN = 0x200;
    public const int LVM_GETHEADER = 0x1000 + 31; // LVM_FIRST + 31
    public const int HDM_GETITEM = 0x1200 + 11;   // HDM_FIRST + 11
    public const int HDM_SETITEM = 0x1200 + 12;   // HDM_FIRST + 12

    [StructLayout(LayoutKind.Sequential)]
    public struct HDITEM {
        public int mask;
        public int cxy;
        [MarshalAs(UnmanagedType.LPTStr)] public string pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public int fmt;
        public int lParam;
        public int iImage;
        public int iOrder;
    }
    [System.Security.SuppressUnmanagedCodeSecurity()]
    [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
    public static unsafe extern IntPtr GetHeaderHnd(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    [System.Security.SuppressUnmanagedCodeSecurity()]
    [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
    public static unsafe extern IntPtr SendMessageItem(IntPtr Handle, int msg, IntPtr wParam, ref HDITEM lParam);

}

public class sMkListViewSorter {
    // We use this class to enable sorting w/icons on regular ListView controls
    private static void ColumnClickHandler(object? sender, ColumnClickEventArgs e) {
        if (sender == null) return;
        var lv = (ListView)sender;
        if (lv.Tag == null) return;
        sMkColumnSorter cs = (sMkColumnSorter)lv.Tag;
        if (e.Column == cs.SortColumn) {
            cs.Order = (cs.Order == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending;
        } else {
            cs.SortColumn = e.Column;
            cs.Order = SortOrder.Ascending;
        }
        lv.Sorting = cs.Order;
        SetSortIcons(lv, ref cs.PrevColumn, e.Column);
        lv.Sort();
        lv.Tag = cs;
    }

    public static void EnableSorting(ListView lv) {
        sMkColumnSorter ListViewSorters;
        ListViewSorters = new sMkColumnSorter();
        lv.ListViewItemSorter = ListViewSorters;
        ListViewSorters.SortColumn = 0;
        ListViewSorters.Order = SortOrder.Ascending;
        lv.Sorting = ListViewSorters.Order;
        lv.Tag = ListViewSorters;
        SetSortIcons(lv, ref ListViewSorters.PrevColumn, 0);
        lv.ColumnClick += ColumnClickHandler;
    }
    public static void EnableSorting(sMkListView lv) {
        EnableSorting((ListView)lv);
    }
    public static void RemoveSorting(ListView lv) {
        lv.Tag = null;
        lv.Sorting = SortOrder.None;
        lv.ColumnClick -= ColumnClickHandler;
    }
    public static void RemoveSorting(sMkListView lv) {
        RemoveSorting((ListView)lv);
    }

    public static void SetSort(ref ListView lv, int SortColumn, SortOrder Order) {
        if (lv.Tag == null) return;
        sMkColumnSorter cs = (sMkColumnSorter)lv.Tag;
        cs.SortColumn = SortColumn;
        cs.Order = Order;
        lv.Sorting = cs.Order;
        SetSortIcons(lv, ref cs.PrevColumn, cs.SortColumn);
        lv.Sort();
        lv.Tag = cs;
    }
    public static void SetSortIcons(ListView lv, ref int previouslySortedColumn, int newSortColumn) {
        IntPtr hHeader = GetHeaderHnd(lv.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
        IntPtr newColumn = new(newSortColumn);
        IntPtr prevColumn = new(previouslySortedColumn);
        HDITEM HDITEM = new();

        // Only update the previous item if it existed and if it was a different one.
        if (previouslySortedColumn != -1 && previouslySortedColumn != newSortColumn) {
            // Clear icon from the previous column.
            HDITEM = new() { mask = HDI_FORMAT };
            SendMessageItem(hHeader, HDM_GETITEM, prevColumn, ref HDITEM);
            HDITEM.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP;
            SendMessageItem(hHeader, HDM_SETITEM, prevColumn, ref HDITEM);
        }

        // Set icon on the new column.
        HDITEM = new() { mask = HDI_FORMAT };
        SendMessageItem(hHeader, HDM_GETITEM, newColumn, ref HDITEM);
        if (lv.Sorting == SortOrder.Ascending) {
            HDITEM.fmt &= ~HDF_SORTDOWN;
            HDITEM.fmt |= HDF_SORTUP;
        } else {
            HDITEM.fmt &= ~HDF_SORTUP;
            HDITEM.fmt |= HDF_SORTDOWN;
        }
        SendMessageItem(hHeader, HDM_SETITEM, newColumn, ref HDITEM);
        previouslySortedColumn = newSortColumn;
    }

}

public class sMkColumnSorter : IComparer {
    public SortOrder Order = SortOrder.None;
    public int SortColumn = 0;
    public int PrevColumn = 0;
    private readonly CaseInsensitiveComparer ObjectCompare = new();
    private bool IsNumeric(string text) => double.TryParse(text, out _);
    private bool IsDate(string text) => DateTime.TryParse(text, out _);

    public int Compare(object? x, object? y) {
        if (x == null) return -1;
        if (y == null) return +1;
        if (SortColumn < 0) return 0;
        ListViewItem listviewX = (ListViewItem)x;
        ListViewItem listviewY = (ListViewItem)y;
        if (SortColumn > listviewX.SubItems.Count - 1 || SortColumn > listviewY.SubItems.Count - 1) return 0;
        string valX = ((ListViewItem)x).SubItems[SortColumn].Text;
        string valY = ((ListViewItem)y).SubItems[SortColumn].Text;
        try {
            // Compare the two items.
            int compareResult = CompareValues(valX, valY);
            if (compareResult == 0) {
                // If its equal, then call a second function to sort by level 2.
                compareResult = CompareLevel2(ref x, ref y);
            }
            if (Order == SortOrder.Ascending) {
                return compareResult; // Ascending sort is selected, return typical result of compare operation.
            } else if (Order == SortOrder.Descending) {
                return (-compareResult); // Descending sort is selected, return negative result of compare operation.
            } else {
                return 0; // Return '0' to indicate that they are equal.
            }
        } catch (Exception ex) {
            Debug.WriteLine("Error at Compare Function");
            Debug.Indent();
            Debug.WriteLine("ValueX was: {0} and ValueY was: {1}", valX, valY);
            Debug.WriteLine("Error was: {0}", ex.ToString(), null);
            Debug.Unindent();
            return 0;
        }
    }
    private int CompareLevel2(ref object x, ref object y) {
        return CompareValues(((ListViewItem)x).Text, ((ListViewItem)y).Text);
    }
    private int CompareLevel2(ref ListViewItem x, ref ListViewItem y) {
        return CompareValues(x.Text, y.Text);
    }
    private int CompareValues(string valX, string valY) {
        valX = CompareFixValues(valX);
        valY = CompareFixValues(valY);

        if (IsNumeric(valX) && IsNumeric(valY)) {
            return double.Parse(valX).CompareTo(double.Parse(valY));
        } else if (IsNumeric(valX.Replace("%", "").Trim()) && IsNumeric(valY.Replace("%", "").Trim())) {
            return double.Parse(valX.Replace("%", "").Trim()).CompareTo(double.Parse(valY.Replace("%", "").Trim()));
        } else if (IsDate(valX) && IsDate(valY)) {
            return DateTime.Parse(valX).CompareTo(DateTime.Parse(valY));
        } else {
            return ObjectCompare.Compare(valX, valY);
        }
    }
    private string CompareFixValues(string Value) {
        if (IsNumeric(Value)) return Value;
        //var retValue = Value.ToUpper().Replace(",", "");
        //if (retValue.EndsWith(".")) retValue = retValue[..^1];
        //if (retValue.EndsWith("+")) retValue = retValue[1..].Trim();
        var retValue = Value.ToUpper().Replace(",", "").TrimStart('+', '=').TrimEnd('.').Trim();
        if (retValue.EndsWith("BPS")) { retValue = retValue[..^3]; }
        if (IsNumeric(retValue)) return retValue;

        if (retValue.EndsWith("B") && IsNumeric(retValue.Replace("B", "").Trim())) {
            retValue = retValue.Replace("B", "").Trim();
        } else if ((retValue.EndsWith("K") && IsNumeric(retValue.Replace("K", "").Trim())) | (retValue.EndsWith("KB") && IsNumeric(retValue.Replace("KB", "").Trim())) | (retValue.EndsWith("KB/S") && IsNumeric(retValue.Replace("KB/S", "").Trim()))) {
            retValue = retValue.Replace("KB/S", "").Replace("KB", "").Replace("K", "").Trim();
            retValue = (double.Parse(retValue) * 1024).ToString();
        } else if ((retValue.EndsWith("M") && IsNumeric(retValue.Replace("M", "").Trim())) | (retValue.EndsWith("MB") && IsNumeric(retValue.Replace("MB", "").Trim())) | (retValue.EndsWith("MB/S") && IsNumeric(retValue.Replace("MB/S", "").Trim()))) {
            retValue = retValue.Replace("MB/S", "").Replace("MB", "").Replace("M", "").Trim();
            retValue = (double.Parse(retValue) * 1024 * 1024).ToString();
        } else if ((retValue.EndsWith("G") && IsNumeric(retValue.Replace("G", "").Trim())) | (retValue.EndsWith("GB") && IsNumeric(retValue.Replace("GB", "").Trim())) | (retValue.EndsWith("GB/S") && IsNumeric(retValue.Replace("GB/S", "").Trim()))) {
            retValue = retValue.Replace("GB/S", "").Replace("GB", "").Replace("G", "").Trim();
            retValue = (double.Parse(retValue) * 1024 * 1024 * 1024).ToString();
        } else {
            retValue = Value;
        }
        return retValue;
    }

}
