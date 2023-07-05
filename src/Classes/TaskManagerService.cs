using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
namespace sMkTaskManager.Classes;

[SupportedOSPlatform("windows")]
internal class TaskManagerService : IEquatable<TaskManagerService>, INotifyPropertyChanged {

    private ServiceController _BaseService;
    private ServiceControllerStatus _StatusCode;
    private ServiceStartMode _StartupCode;
    private string _Ident, _PID = "", _Name = "", _Description = "", _Logon = "", _ImagePath = "", _CommandLine = "";
    private Color _BackColor = Color.Empty;
    private int _ImageIndex = 0;
    private bool _CancellingEvents = false;

    public TaskManagerService(string serviceIdent) {
        _Ident = serviceIdent;
        LastChanged = DateTime.Now.Ticks;
        LastUpdated = LastChanged;
    }

    /* Public Properties */
    public bool CanStop { get; set; }
    public bool CanStart { get; set; }
    public bool CanPause { get; set; }
    public bool CanResume { get; set; }
    public long LastUpdated { get; set; }
    public long LastChanged { get; set; }
    public string ID => _Ident;
    public string Ident => _Ident;

    public string PID { get => _PID; set { SetField(ref _PID, value); } }
    public string Name { get => _Name; set { SetField(ref _Name, value); } }
    public string Description { get => _Description; set { SetField(ref _Description, value); } }
    public string Logon {
        get => Shared.ToTitleCase(_Logon[(_Logon.IndexOf("\\") + 1)..].Trim().ToLower().Replace("service", " service").Replace("system", " system"));
        set { SetField(ref _Logon, value); }
    }
    public string ImagePath { get => _ImagePath; set { SetField(ref _ImagePath, value); } }
    public string CommandLine { get => _CommandLine; set { SetField(ref _CommandLine, value); } }
    public string Status => _StatusCode.ToString();
    public ServiceControllerStatus StatusCode { get => _StatusCode; set { SetField(ref _StatusCode, value, nameof(Status)); } }
    public string Startup => _StartupCode.ToString();
    public ServiceStartMode StartupCode { get => _StartupCode; set { SetField(ref _StartupCode, value, nameof(Startup)); } }
    public Color BackColor { get => _BackColor; set { SetField(ref _BackColor, value); } }
    public int ImageIndex { get => _ImageIndex; set { SetField(ref _ImageIndex, value); } }
    public ServiceController[] DependentServices => (_BaseService == null) ? Array.Empty<ServiceController>() : _BaseService.DependentServices;
    public ServiceController[] ServicesDependedOn => (_BaseService == null) ? Array.Empty<ServiceController>() : _BaseService.ServicesDependedOn;

    /* Public Methods */
    public event PropertyChangedEventHandler? PropertyChanged;
    public bool Equals(TaskManagerService? other) => _Ident == other?._Ident;
    public override bool Equals(object? obj) => Equals(obj as TaskManagerService);
    public override int GetHashCode() => _Ident.GetHashCode();
    public void Load(ServiceController s) {
        _CancellingEvents = true;
        _BaseService = s;
        Update();
        _CancellingEvents = false;
    }
    public void Update() {
        try {
            LastUpdated = DateTime.Now.Ticks;
            _BaseService.Refresh();
            // Note: we get the description only the first time, thus Name is empty
            GetServiceInfo(Ident, string.IsNullOrEmpty(Name), out SERVICE_INFO si);
            Name = _BaseService.DisplayName;
            StatusCode = _BaseService.Status;
            StartupCode = si.StartType;
            Logon = si.Logon!;
            CommandLine = si.BinaryPathName!.Replace("\"", "");
            if (si.BinaryPathName.StartsWith('"')) {
                ImagePath = si.BinaryPathName[1..si.BinaryPathName.IndexOf('"', 1)];
            } else {
                ImagePath = si.BinaryPathName.Contains(' ') ? si.BinaryPathName[..si.BinaryPathName.IndexOf(' ')] : si.BinaryPathName;
            }
            CanPause = _BaseService.CanPauseAndContinue && _BaseService.Status == ServiceControllerStatus.Running;
            CanResume = _BaseService.CanPauseAndContinue && _BaseService.Status == ServiceControllerStatus.Paused;
            CanStop = _BaseService.CanStop && (_BaseService.Status == ServiceControllerStatus.Running || _BaseService.Status == ServiceControllerStatus.Paused);
            CanStart = (_BaseService.Status == ServiceControllerStatus.Stopped) && !(si.StartType == ServiceStartMode.Disabled);
            ImageIndex = (si.StartType == ServiceStartMode.Disabled) ? 2 : ((_BaseService.Status == ServiceControllerStatus.Running) ? 0 : 1);
            if (si.Description != "") { Description = si.Description!; }
            PID = (si.ProcessID == 0) ? "" : si.ProcessID.ToString();

            if (LastUpdated <= LastChanged && !_CancellingEvents) {
                BackColor = Settings.Highlights.ChangingItems ? Settings.Highlights.ChangingColor : Color.Empty;
            } else {
                BackColor = Color.Empty;
            }

        } catch (Exception ex) { Shared.DebugTrap(ex); }

    }
    public struct SERVICE_INFO {
        // Custom Structure to store all the values i really need
        public ServiceType ServiceType;
        public ServiceStartMode StartType;
        public int ErrorControl;
        public string BinaryPathName;
        public string LoadOrderGroup;
        public int TagId;
        public string Description;
        public string Logon;
        public uint ProcessID;
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
    private static bool GetServiceInfo(string serviceIdent, bool incDescription, out SERVICE_INFO si) {
        if (serviceIdent.Equals("")) { si = default; return false; }
        si = new SERVICE_INFO();
        API.SetLastError(0);
        // Open Handlers
        IntPtr hscManager = API.OpenSCManager(".", null, API.SERVICE_ACCESS.SERVICE_GENERIC_ALL);
        if (hscManager == IntPtr.Zero) { throw API.GetLastError(); }
        IntPtr hService = API.OpenService(hscManager, serviceIdent, API.SERVICE_ACCESS.SERVICE_QUERY_CONFIG | API.SERVICE_ACCESS.SERVICE_QUERY_STATUS);
        if (hService == IntPtr.Zero) { throw API.GetLastError(); }
        int bytesNeeded = 0;
        // Service Config
        API.QueryServiceConfig(hService, IntPtr.Zero, 0, ref bytesNeeded);
        IntPtr qscPtr = Marshal.AllocCoTaskMem(bytesNeeded);
        if (API.QueryServiceConfig(hService, qscPtr, bytesNeeded, ref bytesNeeded)) {
            API.QUERY_SERVICE_CONFIG qscs = (API.QUERY_SERVICE_CONFIG)Marshal.PtrToStructure(qscPtr, typeof(API.QUERY_SERVICE_CONFIG))!;
            si.ServiceType = qscs.dwServiceType;
            si.StartType = qscs.dwStartType;
            si.ErrorControl = qscs.dwErrorControl;
            si.BinaryPathName = qscs.lpBinaryPathName;
            si.LoadOrderGroup = qscs.lpLoadOrderGroup;
            si.TagId = qscs.dwTagId;
            si.Logon = (qscs.lpServiceStartName != "") ? qscs.lpServiceStartName : "LocalSystem";
            si.Description = "";
            Marshal.FreeCoTaskMem(qscPtr);

        } else { throw API.GetLastError(); }
        // Service Description
        if (incDescription) {
            API.QueryServiceConfig2(hService, 1, IntPtr.Zero, 0, ref bytesNeeded); // SERVICE_CONFIG_DESCRIPTION = 1
            IntPtr lpsdPtr = Marshal.AllocCoTaskMem(bytesNeeded);
            if (API.QueryServiceConfig2(hService, 1, lpsdPtr, bytesNeeded, ref bytesNeeded)) {
                API.QUERY_SERVICE_DESCRIPTION lpsd = (API.QUERY_SERVICE_DESCRIPTION)Marshal.PtrToStructure(lpsdPtr, typeof(API.QUERY_SERVICE_DESCRIPTION))!;
                si.Description = lpsd.lpDescription;
                Marshal.FreeCoTaskMem(lpsdPtr);
            }
        }
        // Service Status Process
        API.QueryServiceStatusEx(hService, 0, IntPtr.Zero, 0, ref bytesNeeded);
        IntPtr sspPtr = Marshal.AllocCoTaskMem(bytesNeeded);
        if (API.QueryServiceStatusEx(hService, 0, sspPtr, bytesNeeded, ref bytesNeeded)) {
            API.SERVICE_STATUS_PROCESS ssp = (API.SERVICE_STATUS_PROCESS)Marshal.PtrToStructure(sspPtr, typeof(API.SERVICE_STATUS_PROCESS))!;
            si.ProcessID = ssp.processID;
            Marshal.FreeCoTaskMem(sspPtr);
        }
        // Free Handles & Return
        API.CloseServiceHandle(hService);
        API.CloseServiceHandle(hscManager);
        return true;
    }

}

[SupportedOSPlatform("windows")]
internal class TaskManagerServicesCollection : BindingList<TaskManagerService> {
    public BindingSource DataExporter { get; init; }
    public TaskManagerServicesCollection() {
        DataExporter = new(this, null) { RaiseListChangedEvents = true };
        AllowRemove = true;
        RaiseListChangedEvents = true;
    }

    public bool Contains(string serviceIdent) {
        foreach (TaskManagerService i in Items) {
            if (i.Ident == serviceIdent) { return true; }
        }
        return false;
    }

    public TaskManagerService? GetService(string serviceIdent) {
        foreach (TaskManagerService i in Items) {
            if (i.Ident == serviceIdent) return i;
        }
        return null;
    }

}
