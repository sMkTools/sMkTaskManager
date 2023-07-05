﻿using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static class Shared {
    private static string _SystemAccount = "";

    internal static frmMain MainForm;
    internal static bool InitComplete = false;
    internal static bool LoadComplete = false;
    internal static bool RealExit = true;
    internal static int bpi = 20; // Base PID Ignore
    internal static List<string> skipProcess = new(new[] { "audiodg" });
    internal static List<string> skipServices = new();
    internal static int CurrentSessionID = Process.GetCurrentProcess().SessionId;
    internal static int PrivateMsgID = 0;
    internal static string TotalProcessorsBin = "".PadLeft(Environment.ProcessorCount, '1');
    internal static string DebuggerCmd = "";

    public static bool IsNumeric(string value) => double.TryParse(value, out _);
    public static bool IsNumeric(this object value) => double.TryParse(Convert.ToString(value), out _);
    public static bool IsInteger(string value) => value.All(char.IsNumber);
    public static bool IsInteger(this object value) => Convert.ToString(value)!.All(char.IsNumber);
    public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable<T> => (min.CompareTo(value) <= 0) && (value.CompareTo(max) <= 0);

    public static string ToTitleCase(string text) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
    public static string TimeSpanToElapsed(TimeSpan lpTimeSpan) {
        return string.Format("{0,3:D2}:{1,2:D2}:{2,2:D2}", Convert.ToInt32(lpTimeSpan.Hours + (Math.Floor(lpTimeSpan.TotalDays) * 24)), lpTimeSpan.Minutes, lpTimeSpan.Seconds);
    }
    public static string TimeDiff(long startTime, short Format = 1) {
        TimeSpan upX = new(DateTime.Now.Ticks - startTime);
        return Format switch {
            1 => string.Format("{0}d {1,2:D2}:{2,2:D2}:{3,2:D2}", upX.Days, upX.Hours, upX.Minutes, upX.Seconds),
            2 => string.Format("{0}d {1,2:D2}h {2,2:D2}m", upX.Days, upX.Hours, upX.Minutes),
            3 => string.Format("{0}d {1,2:D2}h {2,2:D2}m {3,2:D2}s", upX.Days, upX.Hours, upX.Minutes, upX.Seconds),
            _ => "",
        };
    }

    internal static void NotImplemented([CallerMemberName] string feature = "") {
        MessageBox.Show("This feature is not implemented yet.", feature, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    internal static void DebugTrap(Exception e, int Code = 0, [CallerMemberName] string Method = "") {
        Debug.WriteLine($"*** Error at `{Method}` - Code: {Code} - Threw: {e.Message}");
        Debug.WriteLine(e.ToString());
    }

    internal static string GetSystemAccount() {
        if (_SystemAccount != "") return _SystemAccount;
        try {
            System.Security.Principal.SecurityIdentifier sid = new(System.Security.Principal.WellKnownSidType.LocalSystemSid, null);
            _SystemAccount = sid.Translate(typeof(System.Security.Principal.NTAccount)).ToString();

            // TODO: Validate this and implemente vbProperCase somehow.
            if (_SystemAccount.IndexOf("\\") > 0) _SystemAccount = _SystemAccount[(_SystemAccount.LastIndexOf("\\") + 1)..];
            _SystemAccount = ToTitleCase(_SystemAccount);
            Debug.WriteLine("Getting System Account... Result: " + _SystemAccount);
        } catch (Exception ex) {
            DebugTrap(ex);
            _SystemAccount = "Error";
        }

        return _SystemAccount!;
    }
    internal static void GetDebuggerCmd() {
        string SubKey = "Software\\Microsoft\\Windows NT\\CurrentVersion\\AeDebug";
        string _Debugger = "";
        Microsoft.Win32.RegistryKey ParentKey = Microsoft.Win32.Registry.LocalMachine;
        try {
            Microsoft.Win32.RegistryKey? Key = ParentKey.OpenSubKey(SubKey, false);
            if (Key != null && Key.GetValue("Debugger") != null && !string.IsNullOrEmpty(Key.GetValue("Debugger")!.ToString())) {
                _Debugger = Key.GetValue("Debugger")!.ToString()!.Trim();
            }
            Key?.Close();
        } catch { } finally { ParentKey.Close(); }

        if (!string.IsNullOrEmpty(_Debugger)) {
            try {
                if (_Debugger.Contains('"')) {
                    _Debugger = _Debugger.TrimStart('\"');
                    _Debugger = _Debugger[.._Debugger.IndexOf('"')];
                    _Debugger = _Debugger.Trim('"');
                }
            } catch { _Debugger = ""; }
        }
        DebuggerCmd = _Debugger.Trim();
    }
    internal static void SetStatusText(string value = "", ToolStripLabel? obj = null) {
        if (MainForm == null) FindOwnerForm();
        MainForm?.SetStatusText(value, obj);
    }
    internal static void FindOwnerForm() {
        if (MainForm == null) {
            foreach (Form f in Application.OpenForms) {
                if (f.Name == "frmMain") { MainForm = (frmMain)f; break; }
            }
        }
    }
    internal static void OpenProcessForm(int PID) {
        if (MainForm == null) FindOwnerForm();
        if (MainForm == null) return;
        bool xFound = false;
        MainForm.Cursor = Cursors.WaitCursor;
        foreach (Form child in MainForm!.OwnedForms) {
            if (child == null || child.Name == null) continue;
            if (child.Name.Equals("frmProc-" + PID)) {
                xFound = true;
                child.Focus(); child.Show();
                if (child.WindowState == FormWindowState.Minimized) child.WindowState = FormWindowState.Normal;
                break;
            }
        }
        if (!xFound) {
            Forms.frmProcess_Details frm = new() {
                Owner = MainForm,
                Name = "frmProc-" + PID,
                Tag = PID,
                PID = PID
            };
            frm.Show();
        }
        MainForm.Cursor = Cursors.Default;
    }
    internal static bool BusyCursor {
        get {
            return !(MainForm?.Cursor == Cursors.Default);
        }
        set {
            if (MainForm == null) FindOwnerForm();
            if (MainForm == null) return;
            MainForm.Cursor = value ? Cursors.AppStarting : Cursors.Default;
        }
    }

    public static void BitClear(ref long MyByte, long MyBit) {
        long BitMask;
        BitMask = Convert.ToInt64(Math.Pow(2, MyBit - 1));
        MyByte &= ~BitMask;
    }
    public static void BitSet(ref long MyByte, long MyBit) {
        long BitMask;
        BitMask = Convert.ToInt64(Math.Pow(2, MyBit - 1));
        MyByte |= BitMask;
    }
    public static void BitToggle(ref long MyByte, long MyBit) {
        long BitMask;
        BitMask = Convert.ToInt64(Math.Pow(2, MyBit - 1));
        MyByte ^= BitMask;
    }
    public static bool BitExamine(long MyByte, long MyBit) {
        long BitMask;
        BitMask = Convert.ToInt64(Math.Pow(2, MyBit - 1));
        return (MyByte & BitMask) > 0;
    }

}
internal class CpuUsage {
    private Classes.API.FILETIME _idleTime, _kernTime, _userTime;
    private long _oldCpuUsage = 0, _oldKernUsage = 0, _oldUserUsage = 0;
    private double _rawIdleTime, _rawKernTime, _rawUserTime;
    private int _CpuUsage, _UserUsage, _KernelUsage;
    private long _now;
    private readonly int Processors = Environment.ProcessorCount;

    public void Refresh(long sinceWhen) {
        Classes.API.GetSystemTimes(ref _idleTime, ref _kernTime, ref _userTime);
        _now = DateTime.Now.Ticks;

        if (_oldCpuUsage == 0) {
            _oldCpuUsage = _idleTime.Ticks; _oldKernUsage = _kernTime.Ticks; _oldUserUsage = _userTime.Ticks;
            _CpuUsage = 0; _UserUsage = 0; _KernelUsage = 0;
            return;
        }
        unchecked {
            _rawIdleTime = (((_idleTime.Ticks - _oldCpuUsage) * 100) / (sinceWhen - _now)) / Processors;
            _rawKernTime = (((_kernTime.Ticks - _oldKernUsage) * 100) / (sinceWhen - _now)) / Processors;
            _rawUserTime = (((_userTime.Ticks - _oldUserUsage) * 100) / (sinceWhen - _now)) / Processors;

            _oldCpuUsage = _idleTime.Ticks;
            _oldKernUsage = _kernTime.Ticks;
            _oldUserUsage = _userTime.Ticks;

            _CpuUsage = (int)Math.Min(100, 100 - Math.Abs(_rawIdleTime));
            _UserUsage = (int)Math.Min(100, Math.Abs(_rawUserTime));
            _KernelUsage = (int)Math.Min(100, Math.Abs(_rawKernTime) - Math.Abs(_rawIdleTime));
        }
    }

    public int Usage => _CpuUsage;
    public int UserUsage => _UserUsage;
    public int KernelUsage => _KernelUsage;

}
