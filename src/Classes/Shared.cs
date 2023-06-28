using System.Diagnostics;
using System.Runtime.Versioning;
using System.Runtime.CompilerServices;
namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static class Shared {
    private static string _SystemAccount = "";

    internal static int bpi = 20; // Base PID Ignore
    internal static List<string> skipProcess = new List<string>(new[] { "audiodg" });
    internal static List<string> skipServices = new();
    internal static int CurrentSessionID = Process.GetCurrentProcess().SessionId;
    internal static void OpenProcessForm(int PID) { }

    public static bool IsNumeric(string value) => double.TryParse(value, out _);
    public static bool IsNumeric(this object value) => double.TryParse(Convert.ToString(value), out _);
    public static bool IsInteger(string value) => value.All(char.IsNumber);
    public static bool IsInteger(this object value) => Convert.ToString(value)!.All(char.IsNumber);

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

        System.Security.Principal.SecurityIdentifier sid = new(System.Security.Principal.WellKnownSidType.LocalSystemSid, null);
        System.Security.Principal.NTAccount acc = (System.Security.Principal.NTAccount)sid.Translate(Type.GetType("System.Security.Principal.NTAccount")!);

        // TODO: Validate this and implemente vbProperCase somehow.
        if (acc.Value.IndexOf("\\") > 0) {
            _SystemAccount = acc.Value[(acc.Value.LastIndexOf("\\") + 1)..];
        } else {
            _SystemAccount = acc.Value;
        }

        Debug.WriteLine("Getting System Account... Result: " + acc.Value);

        return _SystemAccount!;
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
