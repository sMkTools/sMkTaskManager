namespace sMkTaskManager.Classes;

internal static class Shared {

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
