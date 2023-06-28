using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

namespace sMkTaskManager.Classes;

[SupportedOSPlatform("windows")]
internal class TaskManagerProcess : IEquatable<TaskManagerProcess>, INotifyPropertyChanged {
    private int _PID;
    private bool _CancellingEvents = false;
    private IntPtr _pHandle = IntPtr.Zero;

    public TaskManagerProcess(int pid) {
        _PID = pid;
        LastUpdated = DateTime.Now.Ticks;
        LastChanged = LastUpdated;
    }
    public TaskManagerProcess(IntPtr pid) : this(pid.ToInt32()) { }

    public string ID => _PID.ToString();
    public int PID => _PID;
    private Color _BackColor;
    public Color BackColor { get => _BackColor; set { SetField(ref _BackColor, value); } }
    public long LastUpdated { get; set; } = 0;
    public long PreviousUpdate { get; set; } = 0;
    public long LastChanged { get; set; } = 0;
    public int ImageIndex { get; set; } = -1;
    public string ImageKey { get; set; } = "";
    public bool IgnoreBackColor { get; set; } = false;

    private bool _Suspended;
    public bool Suspended { get => _Suspended; set { SetField(ref _Suspended, value); BackColor = value ? Settings.Highlights.FrozenColor : Color.Empty; } }
    /* Primary Basic Properties */
    private string _Name = "", _Description = "", _ImagePath = "", _CommandLine = "";
    private string _Affinity = "", _Priority = "", _Username = "", _RunTime = "", _CpuUsage = "";
    private uint _SessionID, _Handles, _Threads; private ulong _PageFaults;
    private int _GDIObjects, _UserObjects, _GDIObjectsPeak, _UserObjectsPeak;
    public string Name { get => _Name; set { SetField(ref _Name, value); } }
    public string Description { get => _Description; set { SetField(ref _Description, value); } }
    public string ImagePath { get => _ImagePath; set { SetField(ref _ImagePath, value); } }
    public string CommandLine { get => _CommandLine; set { SetField(ref _CommandLine, value); } }
    public string Affinity { get => _Affinity; set { SetField(ref _Affinity, value); } }
    public string Priority { get => _Priority; set { SetField(ref _Priority, value); } }
    public string Username { get => _Username; set { SetField(ref _Username, value); } }
    public string RunTime { get => (_PID == 0) ? "n/a." : _RunTime; set { SetField(ref _RunTime, value); } }
    public string CpuUsage { get => _CpuUsage; set { SetField(ref _CpuUsage, value); } }
    public uint SessionID { get => _Handles; set { SetField(ref _SessionID, value); } }
    public uint Handles { get => _SessionID; set { SetField(ref _Handles, value); } }
    public uint Threads { get => _Threads; set { SetField(ref _Threads, value); } }
    public ulong PageFaults { get => _PageFaults; set { SetField(ref _PageFaults, value); } }
    public int GDIObjects { get => _GDIObjects; set { SetField(ref _GDIObjects, value); } }
    public int UserObjects { get => _UserObjects; set { SetField(ref _UserObjects, value); } }
    public int GDIObjectsPeak { get => _GDIObjectsPeak; set { SetField(ref _GDIObjectsPeak, value); } }
    public int UserObjectsPeak { get => _UserObjectsPeak; set { SetField(ref _UserObjectsPeak, value); } }
    /* Times Related Properties */
    private TimeSpan _CreationTimeValue, _CpuTimeValue, _UserTimeValue, _KernelTimeValue, _prevCpuTimeValue;
    private TimeSpan CreationTimeValue { get => _CreationTimeValue; set { SetField(ref _CreationTimeValue, value); } }
    private string CreationTime => (_PID == 0) ? "n/a." : new DateTime(_CreationTimeValue.Ticks).ToString();
    private TimeSpan CpuTimeValue { get => _CpuTimeValue; set { SetField(ref _CpuTimeValue, value); } }
    public string CpuTime => (_PID == 0) ? "n/a." : Shared.TimeSpanToElapsed(_CpuTimeValue);
    private TimeSpan UserTimeValue { get => _UserTimeValue; set { SetField(ref _UserTimeValue, value); } }
    public string UserTime => (_PID == 0) ? "n/a." : Shared.TimeSpanToElapsed(_UserTimeValue);
    private TimeSpan KernelTimeValue { get => _KernelTimeValue; set { SetField(ref _KernelTimeValue, value); } }
    public string KernelTime => (_PID == 0) ? "n/a." : Shared.TimeSpanToElapsed(_KernelTimeValue);


    /* Memory Related Properties */
    private ulong _PagedPoolValue, _PagedPoolPeakValue, _NonPagedPoolValue, _NonPagedPoolPeakValue;
    private ulong _PagedMemoryValue, _PagedMemoryPeakValue, _VirtualMemoryValue, _VirtualMemoryPeakValue;
    private ulong _WorkingSetValue, _WorkingSetPeakValue, _WorkingSetPrivateValue, _WorkingSetShareableValue, _PrivateBytesValue;
    private ulong PagedPoolValue { get => _PagedPoolValue; set { SetField(ref _PagedPoolValue, value); } }
    public string PagedPool => FormatValue(_PagedPoolValue, FormatValueTypes.KBytes);
    private ulong PagedPoolPeakValue { get => _PagedPoolPeakValue; set { SetField(ref _PagedPoolPeakValue, value); } }
    public string PagedPoolPeak => FormatValue(_PagedPoolPeakValue, FormatValueTypes.KBytes);
    private ulong NonPagedPoolValue { get => _NonPagedPoolValue; set { SetField(ref _NonPagedPoolValue, value); } }
    public string NonPagedPool => FormatValue(_NonPagedPoolValue, FormatValueTypes.KBytes);
    private ulong NonPagedPoolPeakValue { get => _NonPagedPoolPeakValue; set { SetField(ref _NonPagedPoolPeakValue, value); } }
    public string NonPagedPoolPeak => FormatValue(_NonPagedPoolPeakValue, FormatValueTypes.KBytes);
    private ulong PagedMemoryValue { get => _PagedMemoryValue; set { SetField(ref _PagedMemoryValue, value); } }
    public string PagedMemory => FormatValue(_PagedMemoryValue, FormatValueTypes.KBytes);
    private ulong PagedMemoryPeakValue { get => _PagedMemoryPeakValue; set { SetField(ref _PagedMemoryPeakValue, value); } }
    public string PagedMemoryPeak => FormatValue(_PagedMemoryPeakValue, FormatValueTypes.KBytes);
    private ulong VirtualMemoryValue { get => _VirtualMemoryValue; set { SetField(ref _VirtualMemoryValue, value); } }
    public string VirtualMemory => FormatValue(_VirtualMemoryValue, FormatValueTypes.KBytes);
    private ulong VirtualMemoryPeakValue { get => _VirtualMemoryPeakValue; set { SetField(ref _VirtualMemoryPeakValue, value); } }
    public string VirtualMemoryPeak => FormatValue(_VirtualMemoryPeakValue, FormatValueTypes.KBytes);
    private ulong WorkingSetValue { get => _WorkingSetValue; set { SetField(ref _WorkingSetValue, value); } }
    public string WorkingSet => FormatValue(_WorkingSetValue, FormatValueTypes.KBytes);
    private ulong WorkingSetPeakValue { get => _WorkingSetPeakValue; set { SetField(ref _WorkingSetPeakValue, value); } }
    public string WorkingSetPeak => FormatValue(_WorkingSetPeakValue, FormatValueTypes.KBytes);
    private ulong WorkingSetPrivateValue { get => _WorkingSetPrivateValue; set { SetField(ref _WorkingSetPrivateValue, value); } }
    public string WorkingSetPrivate => FormatValue(_WorkingSetPrivateValue, FormatValueTypes.KBytes);
    private ulong WorkingSetShareableValue { get => _WorkingSetShareableValue; set { SetField(ref _WorkingSetShareableValue, value); } }
    public string WorkingSetShareable => FormatValue(_WorkingSetShareableValue, FormatValueTypes.KBytes);
    private ulong PrivateBytesValue { get => _PrivateBytesValue; set { SetField(ref _PrivateBytesValue, value); } }
    public string PrivateBytes => FormatValue(_PrivateBytesValue, FormatValueTypes.KBytes);
    /* I/O Related Properties */
    private ulong _ReadTransferValue, _WriteTransferValue, _OtherTransferValue;
    private ulong _ReadTransferDeltaValue, _WriteTransferDeltaValue, _OtherTransferDeltaValue;
    private ulong _ReadOperationsValue, _WriteOperationsValue, _OtherOperationsValue;
    private ulong _ReadOperationsDeltaValue, _WriteOperationsDeltaValue, _OtherOperationsDeltaValue;
    private ulong ReadTransferValue { get => _ReadTransferValue; set { SetField(ref _ReadTransferValue, value); } }
    public string ReadTransfer => FormatValue(_ReadTransferValue, FormatValueTypes.AutoBytes, true);
    private ulong ReadTransferDeltaValue { get => _ReadTransferDeltaValue; set { SetField(ref _ReadTransferDeltaValue, value); } }
    public string ReadTransferDelta => FormatValue(_ReadTransferDeltaValue, FormatValueTypes.AutoBytes, true);
    private ulong ReadOperationsValue { get => _ReadOperationsValue; set { SetField(ref _ReadOperationsValue, value); } }
    public string ReadOperations => FormatValue(_ReadOperationsValue, FormatValueTypes.AutoBytes, true);
    private ulong ReadOperationsDeltaValue { get => _ReadOperationsDeltaValue; set { SetField(ref _ReadOperationsDeltaValue, value); } }
    public string ReadOperationsDelta => FormatValue(_ReadOperationsDeltaValue, FormatValueTypes.AutoBytes, true);
    private ulong WriteTransferValue { get => _WriteTransferValue; set { SetField(ref _WriteTransferValue, value); } }
    public string WriteTransfer => FormatValue(_WriteTransferValue, FormatValueTypes.AutoBytes, true);
    private ulong WriteTransferDeltaValue { get => _WriteTransferDeltaValue; set { SetField(ref _WriteTransferDeltaValue, value); } }
    public string WriteTransferDelta => FormatValue(_WriteTransferDeltaValue, FormatValueTypes.AutoBytes, true);
    private ulong WriteOperationsValue { get => _WriteOperationsValue; set { SetField(ref _WriteOperationsValue, value); } }
    public string WriteOperations => FormatValue(_WriteOperationsValue, FormatValueTypes.AutoBytes, true);
    private ulong WriteOperationsDeltaValue { get => _WriteOperationsDeltaValue; set { SetField(ref _WriteOperationsDeltaValue, value); } }
    public string WriteOperationsDelta => FormatValue(_WriteOperationsDeltaValue, FormatValueTypes.AutoBytes, true);
    private ulong OtherTransferValue { get => _OtherTransferValue; set { SetField(ref _OtherTransferValue, value); } }
    public string OtherTransfer => FormatValue(_OtherTransferValue, FormatValueTypes.AutoBytes, true);
    private ulong OtherTransferDeltaValue { get => _OtherTransferDeltaValue; set { SetField(ref _OtherTransferDeltaValue, value); } }
    public string OtherTransferDelta => FormatValue(_OtherTransferDeltaValue, FormatValueTypes.Number, true);
    private ulong OtherOperationsValue { get => _OtherOperationsValue; set { SetField(ref _OtherOperationsValue, value); } }
    public string OtherOperations => FormatValue(_OtherOperationsValue, FormatValueTypes.Number, true);
    private ulong OtherOperationsDeltaValue { get => _OtherOperationsDeltaValue; set { SetField(ref _OtherOperationsDeltaValue, value); } }
    public string OtherOperationsDelta => FormatValue(_OtherOperationsDeltaValue, FormatValueTypes.Number, true);
    /* Disk Related Properties */
    private ulong _DiskReadValue, _DiskWriteValue, _DiskReadDeltaValue, _DiskWriteDeltaValue, _DiskReadRateValue, _DiskWriteRateValue;
    private ulong DiskReadValue { get => _DiskReadValue; set { SetField(ref _DiskReadValue, value); } }
    public string DiskRead => FormatValue(_DiskReadValue, FormatValueTypes.AutoBytes);
    private ulong DiskReadDeltaValue { get => _DiskReadDeltaValue; set { SetField(ref _DiskReadDeltaValue, value); } }
    public string DiskReadDelta => FormatValue(_DiskReadDeltaValue, FormatValueTypes.AutoBytes);
    private ulong DiskReadRateValue { get => _DiskReadRateValue; set { SetField(ref _DiskReadRateValue, value); } }
    public string DiskReadRate => FormatValue(_DiskReadRateValue, FormatValueTypes.Kbps);
    private ulong DiskWriteValue { get => _DiskWriteValue; set { SetField(ref _DiskWriteValue, value); } }
    public string DiskWrite => FormatValue(_DiskWriteValue, FormatValueTypes.AutoBytes);
    private ulong DiskWriteDeltaValue { get => _DiskWriteDeltaValue; set { SetField(ref _DiskWriteDeltaValue, value); } }
    public string DiskWriteDelta => FormatValue(_DiskWriteDeltaValue, FormatValueTypes.AutoBytes);
    private ulong DiskWriteRateValue { get => _DiskWriteRateValue; set { SetField(ref _DiskWriteRateValue, value); } }
    public string DiskWriteRate => FormatValue(_DiskWriteRateValue, FormatValueTypes.Kbps);
    /* Net Related Properties */
    private ulong _NetSentValue, _NetRcvdValue, _NetSentDeltaValue, _NetRcvdDeltaValue, _NetSentRateValue, _NetRcvdRateValue;
    private ulong NetSentValue { get => _NetSentValue; set { SetField(ref _NetSentValue, value); } }
    public string NetSent => FormatValue(_NetSentValue, FormatValueTypes.AutoBytes);
    private ulong NetSentDeltaValue { get => _NetSentDeltaValue; set { SetField(ref _NetSentDeltaValue, value); } }
    public string NetSentDelta => FormatValue(_NetSentDeltaValue, FormatValueTypes.AutoBytes);
    private ulong NetSentRateValue { get => _NetSentRateValue; set { SetField(ref _NetSentRateValue, value); } }
    public string NetSentRate => FormatValue(_NetSentRateValue, FormatValueTypes.Kbps);
    private ulong NetRcvdValue { get => _NetRcvdValue; set { SetField(ref _NetRcvdValue, value); } }
    public string NetRcvd => FormatValue(_NetRcvdValue, FormatValueTypes.AutoBytes);
    private ulong NetRcvdDeltaValue { get => _NetRcvdDeltaValue; set { SetField(ref _NetRcvdDeltaValue, value); } }
    public string NetRcvdDelta => FormatValue(_NetRcvdDeltaValue, FormatValueTypes.AutoBytes);
    private ulong NetRcvdRateValue { get => _NetRcvdRateValue; set { SetField(ref _NetRcvdRateValue, value); } }
    public string NetRcvdRate => FormatValue(_NetRcvdRateValue, FormatValueTypes.Kbps);
    /* Product Related Properties */
    private FileVersionInfo? _FileVersionInfo = null;
    public string ProductName => (_FileVersionInfo == null || _FileVersionInfo.ProductName == null) ? "n/a." : _FileVersionInfo.ProductName;
    public string ProductVersion => (_FileVersionInfo == null || _FileVersionInfo.ProductVersion == null) ? "n/a." : _FileVersionInfo.ProductVersion;
    public string ProductCompany => (_FileVersionInfo == null || _FileVersionInfo.CompanyName == null) ? "n/a." : _FileVersionInfo.CompanyName;
    public string ProductLanguage => (_FileVersionInfo == null || _FileVersionInfo.Language == null) ? "n/a." : _FileVersionInfo.Language;

    /* Public Methods */
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool Equals(TaskManagerProcess? other) => (_PID == other?._PID);
    public void Load(API.SYSTEM_PROCESS_INFORMATION spi, ref HashSet<string> vv, bool getNewSPI = false) {
        if (getNewSPI) GetSpecificSPI(_PID, out spi);
        _CancellingEvents = true;

        // Load values that are fixed to this process and wont change...
        if (_PID > Shared.bpi) {
            Name = spi.ImageName.Buffer ?? "";
            SessionID = spi.SessionId;


        } else {
            if (_PID == 0) { Name = "Idle"; Description = "Idle Processor"; }
            if (_PID == 4) { Name = "System"; Description = "NT Kernel & System"; }
            SessionID = 0;
            Priority = "Normal";
            Affinity = "All";
            Username = Shared.GetSystemAccount();
        }

        // And then update the rest of values...
        Update(spi, ref vv);
        _CancellingEvents = false;
    }
    public void Update(API.SYSTEM_PROCESS_INFORMATION spi, ref HashSet<string> vv, bool getNewSPI = false) {
        // vv is short for visibleValues and contains the cols definition we are showing, so we dont update if we dont need.
        if (getNewSPI) GetSpecificSPI(_PID, out spi);
        PreviousUpdate = LastUpdated;
        LastUpdated = DateTime.Now.Ticks;

        // Handles & Threads Count
        if (vv.Contains("Handles")) Handles = spi.HandleCount;
        if (vv.Contains("Threads")) Threads = spi.NumberOfThreads;
        // General Memory Values
        if (vv.Contains("WorkingSet")) WorkingSetValue = spi.WorkingSetSize;
        if (vv.Contains("WorkingSetPeak")) WorkingSetPeakValue = spi.PeakWorkingSetSize;
        if (vv.Contains("WorkingSetPrivate") || vv.Contains("WorkingSetShareable")) WorkingSetPrivateValue = spi.WorkingSetPrivateSize;
        if (vv.Contains("PagedMemory")) PagedMemoryValue = spi.PagefileUsage;
        if (vv.Contains("PagedMemoryPeak")) PagedMemoryPeakValue = spi.PeakPagefileUsage;
        if (vv.Contains("PagedPool")) PagedPoolValue = spi.QuotaPagedPoolUsage;
        if (vv.Contains("PagedPoolPeak")) PagedPoolPeakValue = spi.QuotaPeakPagedPoolUsage;
        if (vv.Contains("NonPagedPool")) NonPagedPoolValue = spi.QuotaNonPagedPoolUsage;
        if (vv.Contains("NonPagedPoolPeak")) NonPagedPoolPeakValue = spi.QuotaPeakNonPagedPoolUsage;
        if (vv.Contains("PageFaults")) PageFaults = spi.PageFaultCount;
        if (vv.Contains("PrivateBytes")) PrivateBytesValue = spi.PrivatePageCount;
        if (vv.Contains("VirtualMemory")) VirtualMemoryValue = spi.VirtualSize;
        if (vv.Contains("VirtualMemoryPeak")) VirtualMemoryPeakValue = spi.PeakVirtualSize;
        // I/O Counters Values
        // TODO: Warning, these columns changed definitions from Transfers to Transfer
        if (vv.Contains("ReadTransfer") || vv.Contains("ReadTransferDelta")) {
            ReadTransferDeltaValue = (ReadTransferValue == 0) ? 0 : ReadTransferValue - spi.ReadTransferCount;
            ReadTransferValue = spi.ReadTransferCount;
        }
        if (vv.Contains("ReadOperations") || vv.Contains("ReadOperationsDelta")) {
            ReadOperationsDeltaValue = (ReadOperationsValue == 0) ? 0 : ReadOperationsValue - spi.ReadOperationCount;
            ReadOperationsValue = spi.ReadOperationCount;
        }
        if (vv.Contains("WriteTransfer") || vv.Contains("WriteTransferDelta")) {
            WriteTransferDeltaValue = (WriteTransferValue == 0) ? 0 : WriteTransferValue - spi.WriteTransferCount;
            WriteTransferValue = spi.WriteTransferCount;
        }
        if (vv.Contains("WriteOperations") || vv.Contains("WriteOperationsDelta")) {
            WriteOperationsDeltaValue = (WriteOperationsValue == 0) ? 0 : WriteOperationsValue - spi.WriteOperationCount;
            WriteOperationsValue = spi.WriteOperationCount;
        }
        if (vv.Contains("OtherTransfer") || vv.Contains("OtherTransferDelta")) {
            OtherTransferDeltaValue = (OtherTransferValue == 0) ? 0 : OtherTransferValue - spi.OtherTransferCount;
            OtherTransferValue = spi.OtherTransferCount;
        }
        if (vv.Contains("OtherOperations") || vv.Contains("OtherOperationsDelta")) {
            OtherOperationsDeltaValue = (OtherOperationsValue == 0) ? 0 : OtherOperationsValue - spi.OtherOperationCount;
            OtherOperationsValue = spi.OtherOperationCount;
        }
        // Process Times
        if (vv.Contains("CpuUsage") || vv.Contains("CpuTime") || vv.Contains("UserTime") || vv.Contains("KernelTime") || vv.Contains("CreationTime") || vv.Contains("RunTime")) {
            if (_CreationTimeValue.Ticks == 0) {
                CreationTimeValue = new TimeSpan(DateTime.FromFileTime(Convert.ToInt64(spi.CreateTime)).Ticks);
            }
            RunTime = Shared.TimeDiff(CreationTimeValue.Ticks, 2);
            KernelTimeValue = new TimeSpan(Convert.ToInt64(spi.KernelTime));
            UserTimeValue = new TimeSpan(Convert.ToInt64(spi.UserTime));
            CpuTimeValue = KernelTimeValue + UserTimeValue;

            // CPU Usage
            if (_PID == 0) { /* GetSystemTimes(_CpuTimeValue, _KernelTimeValue, _KernelTimeValue); */ }
            if (_prevCpuTimeValue.Ticks == 0) _prevCpuTimeValue = _CpuTimeValue;
            double _rawCpuUsage = (double)(_CpuTimeValue.Ticks - _prevCpuTimeValue.Ticks) * 100 / (DateTime.Now.Ticks - PreviousUpdate) / Environment.ProcessorCount;
            CpuUsage = _rawCpuUsage.ToString("00.0");
            _prevCpuTimeValue = _CpuTimeValue;
        }




        // TODO: Implement the rest, this will do for now though..


        // Define whatever or not we should change the item color
        if (!IgnoreBackColor) {
            if (LastUpdated <= LastChanged && !_CancellingEvents) {
                if (Suspended && Settings.Highlights.FrozenItems) {
                    BackColor = Settings.Highlights.FrozenColor;
                } else {
                    BackColor = Settings.Highlights.ChangingItems ? Settings.Highlights.ChangingColor : Color.Empty;
                }
            } else if (Suspended) {
                if (Settings.Highlights.FrozenItems) BackColor = Settings.Highlights.FrozenColor;
            } else {
                BackColor = Color.Empty;
            }
        }


    }

    /* Private Methods */
    private void OnPropertyChanged(PropertyChangedEventArgs e) { PropertyChanged?.Invoke(this, e); }
    private void SetField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "", string[]? alsoNotifyProperties = null) {
        if (!EqualityComparer<T>.Default.Equals(field, newValue)) {
            field = newValue;
            // Since all properties ending with Value should be read as the NonValue<string> Property
            if (propertyName.EndsWith("Value")) propertyName = propertyName.Replace("Value", "");
            if (!_CancellingEvents) OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            if (!_CancellingEvents && alsoNotifyProperties != null) {
                foreach (string ap in alsoNotifyProperties) { OnPropertyChanged(new PropertyChangedEventArgs(ap)); }
            }
        }
    }

    private string FormatValue(ulong value, FormatValueTypes format, bool skipBPI = false) {
        if (skipBPI && _PID < Shared.bpi) { return "n/a."; }
        switch (format) {
            case FormatValueTypes.Literal: return value.ToString();
            case FormatValueTypes.Number: return string.Format("{0:#,0}", value);
            case FormatValueTypes.Bytes: return string.Format("{0:#,0}", value) + " B";
            case FormatValueTypes.KBytes: return string.Format("{0:#,0}", value / 1024) + " K";
            case FormatValueTypes.MBytes: return string.Format("{0:#,0}", value / 1024 / 1024) + " M";
            case FormatValueTypes.Kbps: return string.Format("{0:#,0.0}", value / 1024) + " Kb/s";
            case FormatValueTypes.AutoBytes:
                if (value < 10240) {
                    return string.Format("{0:#,0}", value) + " B";
                } else if (value < 1024000) {
                    return string.Format("{0:#,0}", value / 1024) + " K";
                } else {
                    return string.Format("{0:#,0}", value / 1024 / 1024) + " M";
                }
            default: return value.ToString();
        }

    }
    private enum FormatValueTypes {
        Literal = 0,
        Number = 1,
        Bytes = 11,
        KBytes = 12,
        MBytes = 13,
        AutoBytes = 15,
        Kbps = 22,
    }

    private void GetSpecificSPI(int PID, out API.SYSTEM_PROCESS_INFORMATION spi) {
        spi = new();
        IntPtr hmain = IntPtr.Zero;
        if (GetProcessesPointer(ref hmain)) {
            spi = (API.SYSTEM_PROCESS_INFORMATION)Marshal.PtrToStructure(hmain, typeof(API.SYSTEM_PROCESS_INFORMATION))!;
            IntPtr lastOffset = hmain;
            while (spi.NextEntryOffset >= 0) {
                if (spi.UniqueProcessId == PID) {
                    Array.Resize(ref spi.Threads, (int)spi.NumberOfThreads);
                    break;
                }
                if (spi.NextEntryOffset > 0) {
                    lastOffset += (IntPtr)spi.NextEntryOffset;
                    spi = (API.SYSTEM_PROCESS_INFORMATION)Marshal.PtrToStructure(lastOffset, typeof(API.SYSTEM_PROCESS_INFORMATION))!;
                } else {
                    break;
                }
            }
            Marshal.FreeHGlobal(hmain);
        }
    }
    internal static bool GetProcessesPointer(ref IntPtr Handle) {
        int hMainInfoSize = 1000000;
        Handle = Marshal.AllocHGlobal(hMainInfoSize);
        API.NTSTATUS NTstatus = API.NtQuerySystemInformation(API.SYSTEM_INFORMATION_CLASS.SystemExtendedProcessInformation, Handle, hMainInfoSize, out int nLength);
        while (NTstatus == API.NTSTATUS.InfoLengthMismatch) { // STATUS_INFO_LENGTH_MISMATCH
            hMainInfoSize = nLength;
            Marshal.FreeHGlobal(Handle);
            Handle = Marshal.AllocHGlobal(nLength);
            NTstatus = API.NtQuerySystemInformation(API.SYSTEM_INFORMATION_CLASS.SystemExtendedProcessInformation, Handle, hMainInfoSize, out nLength);
        }
        return (NTstatus == 0);
    }

    /* TODO: These functions are ugly, inspect */
    private string GetAffinitiesString(IntPtr bitMask) {
        StringBuilder m_StringBuilder = new();
        // TODO: This is horrible, find a better way
        // string binMask = ConversionHelper.StrReverse(Convert.ToString(bitMask.ToInt32(), 2));
        string binMask = Convert.ToString(bitMask.ToInt32(), 2);
        for (int i = 0; i < binMask.Length; i++) {
            if (binMask[i] == '1') {
                m_StringBuilder.Append((i + 1) + ",");
            }
        }
        return m_StringBuilder.ToString().Trim(',').Trim();
    }
    private bool DumpUserInfo(IntPtr pToken, ref IntPtr SID) {
        bool result = false;
        IntPtr procToken = IntPtr.Zero;
        try {
            if (API.OpenProcessToken(pToken, 0x8, ref procToken)) {
                result = ProcessTokenToSid(procToken, ref SID);
                API.CloseHandle(procToken);
            }
        } catch (Exception ex) {
            Debug.WriteLine("DumpUserInfo Said Error: " + ex.Message);
        }
        return result;
    }
    private bool ProcessTokenToSid(IntPtr token, ref IntPtr SID) {
        bool result = false;
        IntPtr tu = Marshal.AllocHGlobal(256);
        uint _ReturnLength = 0;
        try {
            result = API.GetTokenInformation(token, API.TOKEN_INFORMATION_CLASS.TokenUser, tu, 256, ref _ReturnLength);
            if (result) SID = ((API.TOKEN_USER)Marshal.PtrToStructure(tu, typeof(API.TOKEN_USER))!).User.SID;
        } catch (Exception ex) {
            Debug.WriteLine("ProcessTokenToSid Said Error: " + ex.Message);
        } finally {
            Marshal.FreeHGlobal(tu);
        }
        return result;
    }
    private string ConvertSidToUserName(ref IntPtr pSid, bool withDomain = false) {
        string result = "w32 Error";
        try {
            int l_UserNameLength = 160;
            int l_DomainLength = 160;
            StringBuilder l_UserName = new(l_UserNameLength);
            StringBuilder l_Domain = new(l_DomainLength);
            int sidUse = 0;
            API.LookupAccountSid(string.Empty, pSid, l_UserName, ref l_UserNameLength, l_Domain, ref l_DomainLength, ref sidUse);
            result = withDomain ? l_Domain.ToString() + "\\" + l_UserName.ToString() : l_UserName.ToString();
            l_UserName.Clear(); l_Domain.Clear();
        } catch (Exception ex) {
            Debug.WriteLine("ConvertSidToUserName Said Error: " + ex.Message);
            result = "w32 Error";
        }
        return result;
    }
    private void ConvertSidToUserName(ref IntPtr pSid, out string result, bool withDomain = false) {
        try {
            int l_UserNameLength = 160;
            int l_DomainLength = 160;
            StringBuilder l_UserName = new(l_UserNameLength);
            StringBuilder l_Domain = new(l_DomainLength);
            int sidUse = 0;
            API.LookupAccountSid(string.Empty, pSid, l_UserName, ref l_UserNameLength, l_Domain, ref l_DomainLength, ref sidUse);
            result = withDomain ? l_Domain.ToString() + "\\" + l_UserName.ToString() : l_UserName.ToString();
            l_UserName.Clear(); l_Domain.Clear();
        } catch (Exception ex) {
            Debug.WriteLine("ConvertSidToUserName Said Error: " + ex.Message);
            result = "w32 Error";
        }
    }
}

[SupportedOSPlatform("windows")]
internal class TaskManagerProcessCollection : BindingList<TaskManagerProcess> {
    public BindingSource DataExporter { get; init; }
    public TaskManagerProcessCollection() {
        DataExporter = new(this, null) { RaiseListChangedEvents = true };
        AllowRemove = true;
        RaiseListChangedEvents = true;
    }

    // TODO: These should be improved, maybe use LinQ.
    public bool Contains(IntPtr PID) => Contains(PID.ToInt32());
    public bool Contains(int PID) {
        foreach (TaskManagerProcess i in Items) {
            if (i.PID == PID) return true;
        }
        return false;
    }

    public bool Contains(IntPtr PID, ref TaskManagerProcess theProcess) => Contains(PID.ToInt32(), ref theProcess);
    public bool Contains(int PID, ref TaskManagerProcess theProcess) {
        foreach (TaskManagerProcess i in Items) {
            if (i.PID == PID) { theProcess = i; return true; }
        }
        return false;
    }

    public TaskManagerProcess? GetProcess(IntPtr PID) => GetProcess(PID.ToInt32());
    public TaskManagerProcess? GetProcess(int PID) {
        foreach (TaskManagerProcess i in Items) {
            if (i.PID == PID) return i;
        }
        return null;
    }

}
