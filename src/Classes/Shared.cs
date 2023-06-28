namespace sMkTaskManager.Classes;

internal static class Shared {

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

    public static Array RedimPreserve(Array origArray, int desiredSize) {
        Type t = origArray.GetType().GetElementType()!;
        Array newArray = Array.CreateInstance(t, desiredSize);
        Array.Copy(origArray, 0, newArray, 0, Math.Min(origArray.Length, desiredSize));
        return newArray;
    }
}

internal class CpuUsage {
    private API.FILETIME _idleTime, _kernTime, _userTime;
    private long _oldCpuUsage = 0, _oldKernUsage = 0, _oldUserUsage = 0;
    private double _rawIdleTime, _rawKernTime, _rawUserTime;
    private int _CpuUsage, _UserUsage, _KernelUsage;
    private long _now;
    private readonly int Processors = Environment.ProcessorCount;

    public void Refresh(long sinceWhen) {
        API.GetSystemTimes(ref _idleTime, ref _kernTime, ref _userTime);
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

            //if (double.IsNaN(_rawIdleTime) || _rawIdleTime > int.MaxValue || _rawIdleTime < int.MinValue) _rawIdleTime = 0;
            //if (double.IsNaN(_rawKernTime) || _rawKernTime > int.MaxValue || _rawKernTime < int.MinValue) _rawKernTime = 0;
            //if (double.IsNaN(_rawUserTime) || _rawUserTime > int.MaxValue || _rawUserTime < int.MinValue) _rawUserTime = 0;

            _CpuUsage = (int)Math.Min(100, 100 - Math.Abs(_rawIdleTime));
            _UserUsage = (int)Math.Min(100, Math.Abs(_rawUserTime));
            _KernelUsage = (int)Math.Min(100, Math.Abs(_rawKernTime) - Math.Abs(_rawIdleTime));
        }
    }

    public int Usage => _CpuUsage;
    public int UserUsage => _UserUsage;
    public int KernelUsage => _KernelUsage;

}
