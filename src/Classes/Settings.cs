using Microsoft.Win32;
using sMkTaskManager.Controls;
using System.Diagnostics;
using System.Runtime.Versioning;
namespace sMkTaskManager.Classes;

[SupportedOSPlatform("windows")]
internal static class Settings {
    private static readonly System.Text.UTF8Encoding _Encoding = new();
    private static readonly System.Text.StringBuilder _StringBuilder = new();
    public static string Filename = Application.ExecutablePath.Replace(".exe", ".cfg");
    public static string RegKey = "Software\\sMk Tools\\sMk TaskManager";

    public static bool RememberPositions = true;
    public static bool RememberActiveTab = true;
    public static bool StartMinimized = false;
    public static bool AlwaysOnTop = false;
    public static HighlightsClass Highlights = new();
    public static PerformanceClass Performance = new();
    public static NetworkingClass Networking = new();
    public static TrayClass Tray = new();
    public static ProcessDetailsClass ProcessDetails = new();
    public static MainWindowClass MainWindow = new();
    public static bool IconsInProcess = true;
    public static bool TimmingInStatus = true;
    public static bool ServicesInStatus = false;
    public static short UpdateMethod = 3;
    public static int UpdateSpeed = 1000;
    public static bool inFullScreen = false;
    public static int ActiveTab = 0;
    public static bool ToTrayWhenMinimized = false;
    public static bool ToTrayWhenClosed = false;
    public static bool MinimizeWhenClosing = false;
    public static bool DblClickToRestore = true;
    public static bool ShowAllProcess = false;
    public static bool ShowSummaryView = false;
    public static bool AlternateRowColors = false;
    public static List<string> CheckedInterfaces = new();
    public static List<int> CustomColors = new();

    public class HighlightsClass {
        public bool NewItems = true;
        public bool ChangingItems = true;
        public bool RemovedItems = true;
        public bool FrozenItems = true;
        public Color NewColor = Color.LightGreen;
        public Color ChangingColor = Color.PeachPuff;
        public Color RemovedColor = Color.LightCoral;
        public Color FrozenColor = Color.Orchid;
    }
    public class PerformanceClass {
        public bool Solid = true;
        public bool AntiAlias = false;
        public bool ShadeBackground = true;
        public bool DisplayAverages = false;
        public bool DisplayIndexes = false;
        public bool DisplayLegends = false;
        public bool DisplayOnHover = false;
        public bool LightColors = false;
        public int ValueSpacing = 4;
        public int GridSize = 10;
        public int VerticalGridStyle = 0;
        public Color VerticalGridColor = Color.Green;
        public int HorizontalGridStyle = 0;
        public Color HorizontalGridColor = Color.Green;
        public int AverageLineStyle = 5;
        public Color AverageLineColor = Color.Orange;
        public bool ShowKernelTime = true;
        public bool SeparateCPUs = false;
    }
    public class NetworkingClass {
        public bool Solid = false;
        public bool AntiAlias = false;
        public bool ShadeBackground = true;
        public bool DisplayAverages = true;
        public bool DisplayIndexes = true;
        public bool DisplayLegends = true;
        public bool LightColors = false;
        public int ValueSpacing = 4;
        public int GridSize = 10;
        public int VerticalGridStyle = 5;
        public Color VerticalGridColor = Color.Green;
        public int HorizontalGridStyle = 5;
        public Color HorizontalGridColor = Color.Green;
        public int AverageLineStyle = 5;
        public Color AverageLineColor = Color.Orange;
        public Color UploadColor = Color.Fuchsia;
        public Color DownloadColor = Color.Cyan;
        public bool KeepUpdating = false;
    }
    public class TrayClass {
        public bool ShowCPU = true;
        public bool ShowMemory = false;
        public bool ShowIO = false;
        public bool Combine = false;
        public int Background = 3;
        public int Border = 1;
    }
    public class ProcessDetailsClass {
        public Size Size = new(500, 550);
        public Point Location = new(0, 0);
        public int UpdateSpeed = 1;
        public int LastTab = 0;
    }
    public class MainWindowClass {
        public Size Size = new(760, 750);
        public Point Location = new(0, 0);
        public bool Maximized = false;
    }

    #region "Save Methods..."
    /*
    public bool SaveAll() {
        return SaveGeneral() && SaveHighlights() && SavePerformance() && SaveNetworking() & SaveTray();
    }
    public bool SaveGeneral() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(Convert.ToInt16(RememberPositions) + ",");
            _StringBuilder.Append(Convert.ToInt16(RememberActiveTab) + ",");
            _StringBuilder.Append(Convert.ToInt16(AlwaysOnTop) + ",");
            _StringBuilder.Append(Convert.ToInt16(IconsInProcess) + ",");
            _StringBuilder.Append(Convert.ToInt16(TimmingInStatus) + ",");
            _StringBuilder.Append(Convert.ToInt16(ServicesInStatus) + ",");
            _StringBuilder.Append(Convert.ToInt16(ShowSummaryView) + ",");
            _StringBuilder.Append(UpdateMethod + ",");
            _StringBuilder.Append(UpdateSpeed + ",");
            _StringBuilder.Append(Convert.ToInt16(inFullScreen) + ",");
            _StringBuilder.Append(ActiveTab + ",");
            _StringBuilder.Append(Convert.ToInt16(MinimizeWhenClosing) + ",");
            _StringBuilder.Append(Convert.ToInt16(ToTrayWhenMinimized) + ",");
            _StringBuilder.Append(Convert.ToInt16(ToTrayWhenClosed) + ",");
            _StringBuilder.Append(Convert.ToInt16(DblClickToRestore) + ",");
            _StringBuilder.Append(Convert.ToInt16(ShowAllProcess) + ",");
            _StringBuilder.Append(Convert.ToInt16(StartMinimized) + ",");
            _StringBuilder.Append(Convert.ToInt16(AlternateRowColors) + ",");
            return WriteReg("General", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveHighlights() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(Convert.ToInt16(Highlights.NewItems) + ",");
            _StringBuilder.Append(Highlights.NewColor.ToArgb() + ",");
            _StringBuilder.Append(Convert.ToInt16(Highlights.ChangingItems) + ",");
            _StringBuilder.Append(Highlights.ChangingColor.ToArgb() + ",");
            _StringBuilder.Append(Convert.ToInt16(Highlights.RemovedItems) + ",");
            _StringBuilder.Append(Highlights.RemovedColor.ToArgb() + ",");
            _StringBuilder.Append(Convert.ToInt16(Highlights.FrozenItems) + ",");
            _StringBuilder.Append(Highlights.FrozenColor.ToArgb() + ",");
            return WriteReg("Highlights", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SavePerformance() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(Convert.ToInt16(Performance.Solid) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.AntiAlias) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.ShadeBackground) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.DisplayAverages) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.DisplayIndexes) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.DisplayLegends) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.DisplayOnHover) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.LightColors) + ",");
            _StringBuilder.Append(Performance.ValueSpacing + ",");
            _StringBuilder.Append(Performance.GridSize + ",");
            _StringBuilder.Append(Performance.VerticalGridStyle + ",");
            _StringBuilder.Append(Performance.VerticalGridColor.ToArgb() + ",");
            _StringBuilder.Append(Performance.HorizontalGridStyle + ",");
            _StringBuilder.Append(Performance.HorizontalGridColor.ToArgb() + ",");
            _StringBuilder.Append(Performance.AverageLineStyle + ",");
            _StringBuilder.Append(Performance.AverageLineColor.ToArgb() + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.ShowKernelTime) + ",");
            _StringBuilder.Append(Convert.ToInt16(Performance.SeparateCPUs) + ",");
            return WriteReg("Performance", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveNetworking() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(Convert.ToInt16(Networking.Solid) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.AntiAlias) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.ShadeBackground) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.DisplayAverages) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.DisplayIndexes) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.DisplayLegends) + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.LightColors) + ",");
            _StringBuilder.Append(Networking.ValueSpacing + ",");
            _StringBuilder.Append(Networking.GridSize + ",");
            _StringBuilder.Append(Networking.VerticalGridStyle + ",");
            _StringBuilder.Append(Networking.VerticalGridColor.ToArgb() + ",");
            _StringBuilder.Append(Networking.HorizontalGridStyle + ",");
            _StringBuilder.Append(Networking.HorizontalGridColor.ToArgb() + ",");
            _StringBuilder.Append(Networking.AverageLineStyle + ",");
            _StringBuilder.Append(Networking.AverageLineColor.ToArgb() + ",");
            _StringBuilder.Append(Networking.UploadColor.ToArgb() + ",");
            _StringBuilder.Append(Networking.DownloadColor.ToArgb() + ",");
            _StringBuilder.Append(Convert.ToInt16(Networking.KeepUpdating) + ",");
            return WriteReg("Networking", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveTray() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(Convert.ToInt16(Tray.ShowCPU) + ",");
            _StringBuilder.Append(Convert.ToInt16(Tray.ShowMemory) + ",");
            _StringBuilder.Append(Convert.ToInt16(Tray.ShowIO) + ",");
            _StringBuilder.Append(Convert.ToInt16(Tray.Combine) + ",");
            _StringBuilder.Append(Tray.Background + ",");
            _StringBuilder.Append(Tray.Border + ",");
            return WriteReg("Tray", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveInterfaces() {
        _StringBuilder.Clear();
        try {
            foreach (string s in CheckedInterfaces) {
                _StringBuilder.Append(s + ",");
            }
            return WriteReg("Selected Nics", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveColsInformation(string strName, sMkListView LV) {
        _StringBuilder.Clear();
        try {
            int colCount = 0;
            while (colCount < LV.Columns.Count) {
                foreach (ColumnHeader c in LV.Columns) {
                    if (c.DisplayIndex != colCount) {
                        continue;
                    }
                    sMkListView.ColumnInformation cc = new(c.Tag?.ToString(), c.Text, c.TextAlign, c.Width) {
                        Index = c.DisplayIndex
                    };
                    if (LV.Sortable && LV.SortColumn == c.Index) {
                        cc.SortOrder = LV.Sorting;
                    }
                    _StringBuilder.AppendLine(cc.GetChunk);
                    colCount += 1;
                    cc = null;
                }
            }
            return WriteReg(strName, _Encoding.GetBytes(_StringBuilder.ToString()));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveSummaryColumns(ref sMkSummaryView SV) {
        _StringBuilder.Clear();
        try {
            foreach (string l in SV.RowsChunk) {
                _StringBuilder.AppendLine(l);
            }
            return WriteReg("colsSummary", _Encoding.GetBytes(_StringBuilder.ToString()));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveProcDetails() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(ProcessDetails.Size.Width + "," + ProcessDetails.Size.Height + ",");
            _StringBuilder.Append(ProcessDetails.Location.X + "," + ProcessDetails.Location.Y + ",");
            _StringBuilder.Append(ProcessDetails.UpdateSpeed + ",");
            _StringBuilder.Append(ProcessDetails.LastTab + ",");
            return WriteReg("winProcess", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveMainWindow() {
        _StringBuilder.Clear();
        try {
            _StringBuilder.Append(MainWindow.Size.Width + "," + MainWindow.Size.Height + ",");
            _StringBuilder.Append(MainWindow.Location.X + "," + MainWindow.Location.Y + ",");
            _StringBuilder.Append(Convert.ToInt32(MainWindow.Maximized) + ",");
            return WriteReg("winMain", FixStringToWrite(_StringBuilder));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public bool SaveCustomColors() {
        _StringBuilder.Clear();
        try {
            foreach (int c in CustomColors) {
                if (c == 16777215) continue;
                if (c == 0) continue;
                _StringBuilder.Append(c + ",");
            }
            return WriteReg("Custom Colors", _StringBuilder.ToString().TrimEnd(','));
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    */
    #endregion

    #region "Load Methods..."
    public static bool LoadAll() {
        return LoadGeneral() && LoadHighlights() && LoadPerformance() && LoadNetworking() & LoadTray();
    }
    public static bool LoadGeneral() {
        try {
            string[] ChunkValues = ReadReg("General", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            RememberPositions = ChunkValues[0] != "0";
            RememberActiveTab = ChunkValues[1] != "0";
            AlwaysOnTop = ChunkValues[2] != "0";
            IconsInProcess = ChunkValues[3] != "0";
            TimmingInStatus = ChunkValues[4] != "0";
            ServicesInStatus = ChunkValues[5] != "0";
            ShowSummaryView = ChunkValues[6] != "0";
            UpdateMethod = short.Parse(ChunkValues[7]);
            UpdateSpeed = int.Parse(ChunkValues[8]);
            inFullScreen = ChunkValues[9] != "0";
            ActiveTab = int.Parse(ChunkValues[10]);
            MinimizeWhenClosing = ChunkValues[11] != "0";
            ToTrayWhenMinimized = ChunkValues[12] != "0";
            ToTrayWhenClosed = ChunkValues[13] != "0";
            DblClickToRestore = ChunkValues[14] != "0";
            ShowAllProcess = ChunkValues[15] != "0";
            StartMinimized = ChunkValues[16] != "0";
            AlternateRowColors = ChunkValues[17] != "0";
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }

    }
    public static bool LoadHighlights() {
        try {
            string[] ChunkValues = ReadReg("Highlights", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            Highlights.NewItems = ChunkValues[0] != "0";
            Highlights.NewColor = Color.FromArgb(int.Parse(ChunkValues[1]));
            Highlights.ChangingItems = ChunkValues[2] != "0";
            Highlights.ChangingColor = Color.FromArgb(int.Parse(ChunkValues[3]));
            Highlights.RemovedItems = ChunkValues[4] != "0";
            Highlights.RemovedColor = Color.FromArgb(int.Parse(ChunkValues[5]));
            Highlights.FrozenItems = ChunkValues[6] != "0";
            Highlights.FrozenColor = Color.FromArgb(int.Parse(ChunkValues[7]));
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadPerformance() {
        try {
            string[] ChunkValues = ReadReg("Performance", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            Performance.Solid = ChunkValues[0] != "0";
            Performance.AntiAlias = ChunkValues[1] != "0";
            Performance.ShadeBackground = ChunkValues[2] != "0";
            Performance.DisplayAverages = ChunkValues[3] != "0";
            Performance.DisplayIndexes = ChunkValues[4] != "0";
            Performance.DisplayLegends = ChunkValues[5] != "0";
            Performance.DisplayOnHover = ChunkValues[6] != "0";
            Performance.LightColors = ChunkValues[7] != "0";
            Performance.ValueSpacing = int.Parse(ChunkValues[8]);
            Performance.GridSize = int.Parse(ChunkValues[9]);
            Performance.VerticalGridStyle = int.Parse(ChunkValues[10]);
            Performance.VerticalGridColor = Color.FromArgb(int.Parse(ChunkValues[11]));
            Performance.HorizontalGridStyle = int.Parse(ChunkValues[12]);
            Performance.HorizontalGridColor = Color.FromArgb(int.Parse(ChunkValues[13]));
            Performance.AverageLineStyle = int.Parse(ChunkValues[14]);
            Performance.AverageLineColor = Color.FromArgb(int.Parse(ChunkValues[15]));
            Performance.ShowKernelTime = ChunkValues[16] != "0";
            Performance.SeparateCPUs = ChunkValues[17] != "0";
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadNetworking() {
        try {
            string[] ChunkValues = ReadReg("Networking", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            Networking.Solid = ChunkValues[0] != "0";
            Networking.AntiAlias = ChunkValues[1] != "0";
            Networking.ShadeBackground = ChunkValues[2] != "0";
            Networking.DisplayAverages = ChunkValues[3] != "0";
            Networking.DisplayIndexes = ChunkValues[4] != "0";
            Networking.DisplayLegends = ChunkValues[5] != "0";
            Networking.LightColors = ChunkValues[6] != "0";
            Networking.ValueSpacing = int.Parse(ChunkValues[7]);
            Networking.GridSize = int.Parse(ChunkValues[8]);
            Networking.VerticalGridStyle = int.Parse(ChunkValues[9]);
            Networking.VerticalGridColor = Color.FromArgb(int.Parse(ChunkValues[10]));
            Networking.HorizontalGridStyle = int.Parse(ChunkValues[11]);
            Networking.HorizontalGridColor = Color.FromArgb(int.Parse(ChunkValues[12]));
            Networking.AverageLineStyle = int.Parse(ChunkValues[13]);
            Networking.AverageLineColor = Color.FromArgb(int.Parse(ChunkValues[14]));
            Networking.UploadColor = Color.FromArgb(int.Parse(ChunkValues[15]));
            Networking.DownloadColor = Color.FromArgb(int.Parse(ChunkValues[16]));
            Networking.KeepUpdating = ChunkValues[17] != "0";
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadTray() {
        try {
            string[] ChunkValues = ReadReg("Tray", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            Tray.ShowCPU = ChunkValues[0] != "0";
            Tray.ShowMemory = ChunkValues[1] != "0";
            Tray.ShowIO = ChunkValues[2] != "0";
            Tray.Combine = ChunkValues[3] != "0";
            Tray.Background = int.Parse(ChunkValues[4]);
            Tray.Border = int.Parse(ChunkValues[5]);
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadMainWindow() {
        try {
            string[] ChunkValues = ReadReg("winMain", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            MainWindow.Size = new Size(int.Parse(ChunkValues[0]), int.Parse(ChunkValues[1]));
            MainWindow.Location = new Point(int.Parse(ChunkValues[2]), int.Parse(ChunkValues[3]));
            MainWindow.Maximized = ChunkValues[4] != "0";
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadColsInformation(string strName, sMkListView lv, ref HashSet<string> Destination) {
        try {
            byte[] bt = ReadReg(strName, ""u8.ToArray());
            string allValues = (bt.Length > 10) ? allValues = _Encoding.GetString(bt) : GetInitialValues(strName);

            foreach (string colValues in allValues.Split(Environment.NewLine)) {
                if (colValues.Trim().Length < 5) continue;
                sMkListView.ColumnInformation c = new(colValues.Trim());
                // Check if a column with that Tag is already created on the ListView
                int colExist = -1;
                foreach (ColumnHeader col in lv.Columns) {
                    if (col.Tag?.ToString()!.ToLower() == c.Tag.ToString().ToLower()) { colExist = col.Index; }
                }
                if (colExist == -1) {
                    // If the column is not on the control then add it, This is used for Process & Services
                    ColumnHeader newCol = new() {
                        Name = lv.Name + "_" + c.Tag.ToString(),
                        Tag = c.Tag,
                        Text = c.Text,
                        Width = c.Width,
                        TextAlign = c.Align
                    };
                    lv.Columns.Insert(c.Index, newCol);
                } else {
                    // Otherwise just set the position and width, This is used for Connections and Ports
                    lv.Columns[colExist].Name = lv.Name + "_" + c.Tag.ToString();
                    lv.Columns[colExist].Width = c.Width;
                    lv.Columns[colExist].DisplayIndex = c.Index;
                }
                // Check if we need to sort by this column, and do it.
                if (lv.Sortable && !(c.SortOrder == SortOrder.None)) { lv.SetSort(c.Index, c.SortOrder); }
            }
        } catch (Exception e) {
            Globals.DebugLine(e); return false;
        } finally {
            Destination.Clear();
            foreach (ColumnHeader c in lv.Columns) { Destination.Add(c.Tag!.ToString()!); }
        }
        return true;
    }
    public static bool LoadInterfaces() {
        try {
            string[] ChunkValues = ReadReg("Selected Nics", "").Split(',');
            CheckedInterfaces.Clear();
            foreach (string s in ChunkValues) {
                CheckedInterfaces.Add(s);
            }
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    public static bool LoadCustomColors() {
        try {
            string[] ChunkValues = ReadReg("Custom Colors", "9498256,8421616,12180223,14053594").Split(",".ToCharArray());
            CustomColors.Clear();
            for (int i = 0; i < ChunkValues.Length; i++) {
                CustomColors.Add(int.Parse(ChunkValues[i]));
            }
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }


    /*
    public bool LoadSummaryColumns(ref sMkSummaryView SV) {
        try {
            byte[] bt = ReadReg("colsSummary", new[] { 0 });
            string allValues = null;
            if (bt.Length > 10) {
                allValues = m_Encoding.GetString(bt);
            } else {
                allValues = GetInitialValues("colsSummary");
            }
            string[] spValues = null;
            SV.SuspendLayout();
            SV.ClearRows();
            SV.Columns = 1;
            foreach (string colValues in allValues.Split(Environment.NewLine)) {
                if (colValues.Trim().Length < 5) {
                    continue;
                }
                spValues = colValues.Split('|');
                if (spValues.Count() < 3) {
                    continue;
                }
                if (!NumericHelper.IsNumeric(spValues[0])) {
                    continue;
                }
                if (int.Parse(spValues[0]) < 1) {
                    spValues[0] = 1.ToString();
                }
                if (int.Parse(spValues[0]) > 3) {
                    spValues[0] = 3.ToString();
                }
                SV.Columns = Math.Max(SV.Columns, int.Parse(spValues[0]));
                SV.AddRow(int.Parse(spValues[0]), spValues[1], spValues[2]);
            }
            SV.ResumeLayout();
            spValues = null;
            allValues = null;
            bt = null;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
        return true;
    }
    public bool LoadProcDetails() {
        try {
            string[] ChunkValues = ReadReg("winProcess", "").Split(',');
            if (ChunkValues.Length < 3) return false;
            ProcessDetails.Size = new Size(int.Parse(ChunkValues[0]), int.Parse(ChunkValues[1]));
            ProcessDetails.Location = new Point(int.Parse(ChunkValues[2]), int.Parse(ChunkValues[3]));
            ProcessDetails.UpdateSpeed = int.Parse(ChunkValues[4]);
            ProcessDetails.LastTab = int.Parse(ChunkValues[5]);
            return true;
        } catch (Exception e) { Globals.DebugLine(e); return false; }
    }
    */
    #endregion

    public static string GetInitialValues(string strName) {
        System.Text.StringBuilder retValue = new();
        if (strName == "colsProcess") {
            retValue.AppendLine("|PID,PID,51,0,1,0,0|");
            retValue.AppendLine("|Name,Name,117,0,0,1,0|");
            retValue.AppendLine("|CPU,CpuUsage,40,2,0,2,0|");
            retValue.AppendLine("|Description,Description,164,0,0,3,0|");
            retValue.AppendLine("|Priority,Priority,60,0,0,4,0|");
            retValue.AppendLine("|Username,Username,91,0,0,5,0|");
            retValue.AppendLine("|Mem Usage,Memory,71,1,0,6,0|");
            retValue.AppendLine("|Mem Peak,MemoryPeak,75,1,0,7,0|");
            retValue.AppendLine("|Virtual Memory,VirtualMemory,86,1,0,8,0|");
            retValue.AppendLine("|PF Memory,PagedMemory,71,1,0,9,0|");
            retValue.AppendLine("|Handles,Handles,55,1,0,10,0|");
            retValue.AppendLine("|Threads,Threads,55,1,0,11,0|");
            retValue.AppendLine("|GDI Objects,GDIObjects,70,1,0,12,0|");
            retValue.AppendLine("|User Objects,UserObjects,75,1,0,13,0|");
            retValue.AppendLine("|I/O Read Bytes,ReadTransfers,90,1,0,14,0|");
            retValue.AppendLine("|I/O Write Bytes,WriteTransfers,90,1,0,15,0|");
            retValue.AppendLine("|Page Faults,PageFaults,71,1,0,16,0|");
            retValue.AppendLine("|Run Time,RunTime,81,1,0,17,0|");
            retValue.AppendLine("|Image Path,ImagePath,150,0,0,18,0|");
        } else if (strName == "colsServices") {
            retValue.AppendLine("|Name,Name,188,0,1,0,0|");
            retValue.AppendLine("|Status,Status,52,0,0,1,0|");
            retValue.AppendLine("|PID,PID,36,0,0,2,0|");
            retValue.AppendLine("|Startup,Startup,59,0,0,3,0|");
            retValue.AppendLine("|Logon As,Logon,91,0,0,4,0|");
            retValue.AppendLine("|Command Line,CommandLine,230,0,0,5,0|");
            retValue.AppendLine("|Internal,Ident,162,0,0,6,0|");
        } else if (strName == "colsConnections") {
            retValue.AppendLine("|PID,PID,60,0,1,0,0|");
            retValue.AppendLine("|Process,ProcessName,75,0,0,1,0|");
            retValue.AppendLine("|Proto,Protocol,55,0,1,2,0|");
            retValue.AppendLine("|Local Address,LocalAddr,94,0,0,3,0|");
            retValue.AppendLine("|Local Port,LocalPort,60,0,0,4,0|");
            retValue.AppendLine("|Remote Address,RemoteAddr,102,0,0,5,0|");
            retValue.AppendLine("|Remote Port,RemotePort,75,0,0,6,0|");
            retValue.AppendLine("|State,StateString,70,0,0,7,0|");
            retValue.AppendLine("|Sent,Sent,57,1,0,8,0|");
            retValue.AppendLine("|Received,Received,67,1,0,9,0|");
        } else if (strName == "colsSummary") {
            retValue.AppendLine("1|Threads|Threads");
            retValue.AppendLine("1|Handles|Handles");
            retValue.AppendLine("1|GDI Objects|GDIObjects");
            retValue.AppendLine("1|User Objects|UserObjects");
            retValue.AppendLine("2|DiskRead|DiskRead");
            retValue.AppendLine("2|dRead Delta|DiskReadDelta");
            retValue.AppendLine("2|DiskWrite|DiskWrite");
            retValue.AppendLine("2|dWrite Delta|DiskWriteDelta");
            retValue.AppendLine("3|Net Sent|NetSent");
            retValue.AppendLine("3|nSent Delta|NetSentDelta");
            retValue.AppendLine("3|Net Rec|NetReceived");
            retValue.AppendLine("3|nRec Delta|NetReceivedDelta");
        }
        return retValue.ToString();
    }

    #region "Registry Write Methods..."
    /*
    private bool WriteReg(string ValueName, bool Value) {
        return ActualWriteReg(ValueName, (Value ? -1 : 0), Microsoft.Win32.RegistryValueKind.String);
    }
    private bool WriteReg(string ValueName, string Value) {
        return ActualWriteReg(ValueName, Value, Microsoft.Win32.RegistryValueKind.String);
    }
    private bool WriteReg(string ValueName, int Value) {
        return ActualWriteReg(ValueName, Value, Microsoft.Win32.RegistryValueKind.String);
    }
    private bool WriteReg(string ValueName, byte[] Value) {
        return ActualWriteReg(ValueName, Value, Microsoft.Win32.RegistryValueKind.Binary);
    }
    private bool WriteReg(string ValueName, Array Value) {
        return ActualWriteReg(ValueName, Value, Microsoft.Win32.RegistryValueKind.MultiString);
    }
    private bool ActualWriteReg(string ValueName, object Value, Microsoft.Win32.RegistryValueKind ValueKind = Microsoft.Win32.RegistryValueKind.String) {
        Microsoft.Win32.RegistryKey ParentKey = Microsoft.Win32.Registry.CurrentUser;
        Microsoft.Win32.RegistryKey Key;
        try {
            Key = ParentKey.OpenSubKey(RegKey, true);
            Key ??= ParentKey.CreateSubKey(RegKey);
            Key.SetValue(ValueName, Value, ValueKind);
            Key.Close();
            ParentKey.Close();
            return true;
        } catch {
            // INSTANT C# TASK: Calls to the VB 'Err' function are not converted by Instant C#:
            Debug.WriteLine("ActualWriteReg: " + Microsoft.VisualBasic.Information.Err().GetException.ToString());
            ParentKey.Close();
            return false;
        }
    }
    private string FixStringToWrite(System.Text.StringBuilder Original) {
        return Original.ToString().Replace("-1,", "1,").TrimEnd(',');
    }

    */
    #endregion

    private static T ReadReg<T>(string valueName, T defaultValue) {
        T retValue = defaultValue;
        RegistryKey ParentKey = Registry.CurrentUser;
        try {
            var Key = ParentKey.OpenSubKey(RegKey, false);
            if (Key == null || Key.GetValue(valueName) == null) {
                retValue = defaultValue;
            } else if (string.IsNullOrEmpty(Key.GetValue(valueName)!.ToString()!.Trim())) {
                retValue = defaultValue;
            } else {
                retValue = (T)Key.GetValue(valueName)!;
            }
            Key?.Close();
        } catch (Exception e) {
            Globals.DebugLine(e);
            retValue = defaultValue;
        } finally {
            ParentKey.Close();
        }
        return retValue;
    }


    private static string oldReadReg(string ValueName, string DefaultValue) {
        string retValue = DefaultValue;
        RegistryKey ParentKey = Registry.CurrentUser;
        try {
            var Key = ParentKey.OpenSubKey(RegKey, false);
            if (Key == null || Key.GetValue(ValueName) == null) {
                retValue = DefaultValue;
            } else if (string.IsNullOrEmpty(Key.GetValue(ValueName)!.ToString()!.Trim())) {
                retValue = DefaultValue;
            } else {
                retValue = Key.GetValue(ValueName)!.ToString()!;
            }
            Key?.Close();
        } catch (Exception e) {
            Globals.DebugLine(e);
            retValue = DefaultValue;
        } finally {
            ParentKey.Close();
        }
        return retValue;

    }
    private static int oldReadReg(string ValueName, int DefaultValue) {
        return int.Parse(ReadReg(ValueName, DefaultValue.ToString()));
    }
    private static bool oldReadReg(string ValueName, bool DefaultValue) {
        return ReadReg(ValueName, DefaultValue ? "1" : "0") != "0";
    }
    private static byte[] oldReadReg(string ValueName, byte[] DefaultValue) {
        byte[]? retValue = null;
        RegistryKey ParentKey = Registry.CurrentUser;
        try {
            var Key = ParentKey.OpenSubKey(RegKey, true);
            if (Key == null || Key.GetValue(ValueName) == null) {
                retValue = DefaultValue;
            } else {
                retValue = (byte[])Key.GetValue(ValueName)!;
            }
            Key?.Close();
        } catch {
            retValue = DefaultValue;
        } finally {
            ParentKey.Close();
        }
        return retValue;
    }

}
