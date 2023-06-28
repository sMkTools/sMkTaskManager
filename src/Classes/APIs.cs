﻿using System.Runtime.InteropServices;
using System.Text;
using static sMkTaskManager.Classes.API;

namespace sMkTaskManager.Classes;

internal unsafe static partial class API {

    #region "API Enums..."
    public enum NTSTATUS : uint {
        // Success
        Success = 0x00000000,
        Wait1 = 0x00000001,
        Wait2 = 0x00000002,
        Wait3 = 0x00000003,
        Wait63 = 0x0000003f,
        Abandoned = 0x00000080,
        AbandonedWait1 = 0x00000081,
        AbandonedWait2 = 0x00000082,
        AbandonedWait3 = 0x00000083,
        AbandonedWait63 = 0x000000bf,
        UserApc = 0x000000c0,
        KernelApc = 0x00000100,
        Alerted = 0x00000101,
        Timeout = 0x00000102,
        Pending = 0x00000103,
        Reparse = 0x00000104,
        MoreEntries = 0x00000105,
        NotAllAssigned = 0x00000106,
        SomeNotMapped = 0x00000107,
        OpLockBreakInProgress = 0x00000108,
        VolumeMounted = 0x00000109,
        RxActCommitted = 0x0000010a,
        NotifyCleanup = 0x0000010b,
        NotifyEnumDir = 0x0000010c,
        NoQuotasForAccount = 0x0000010d,
        PrimaryTransportConnectFailed = 0x0000010e,
        PageFaultTransition = 0x00000110,
        PageFaultDemandZero = 0x00000111,
        PageFaultCopyOnWrite = 0x00000112,
        PageFaultGuardPage = 0x00000113,
        PageFaultPagingFile = 0x00000114,
        CrashDump = 0x00000116,
        ReparseObject = 0x00000118,
        NothingToTerminate = 0x00000122,
        ProcessNotInJob = 0x00000123,
        ProcessInJob = 0x00000124,
        ProcessCloned = 0x00000129,
        FileLockedWithOnlyReaders = 0x0000012a,
        FileLockedWithWriters = 0x0000012b,
        // Informational
        ObjectNameExists = 0x40000000,
        ThreadWasSuspended = 0x40000001,
        WorkingSetLimitRange = 0x40000002,
        ImageNotAtBase = 0x40000003,
        RegistryRecovered = 0x40000009,
        // Warning
        Warning = 0x80000000,
        GuardPageViolation = 0x80000001,
        DatatypeMisalignment = 0x80000002,
        Breakpoint = 0x80000003,
        SingleStep = 0x80000004,
        BufferOverflow = 0x80000005,
        NoMoreFiles = 0x80000006,
        HandlesClosed = 0x8000000a,
        PartialCopy = 0x8000000d,
        DeviceBusy = 0x80000011,
        InvalidEaName = 0x80000013,
        EaListInconsistent = 0x80000014,
        NoMoreEntries = 0x8000001a,
        LongJump = 0x80000026,
        DllMightBeInsecure = 0x8000002b,
        // Error
        Error = 0xc0000000,
        Unsuccessful = 0xc0000001,
        NotImplemented = 0xc0000002,
        InvalidInfoClass = 0xc0000003,
        InfoLengthMismatch = 0xc0000004,
        AccessViolation = 0xc0000005,
        InPageError = 0xc0000006,
        PagefileQuota = 0xc0000007,
        InvalidHandle = 0xc0000008,
        BadInitialStack = 0xc0000009,
        BadInitialPc = 0xc000000a,
        InvalidCid = 0xc000000b,
        TimerNotCanceled = 0xc000000c,
        InvalidParameter = 0xc000000d,
        NoSuchDevice = 0xc000000e,
        NoSuchFile = 0xc000000f,
        InvalidDeviceRequest = 0xc0000010,
        EndOfFile = 0xc0000011,
        WrongVolume = 0xc0000012,
        NoMediaInDevice = 0xc0000013,
        NoMemory = 0xc0000017,
        NotMappedView = 0xc0000019,
        UnableToFreeVm = 0xc000001a,
        UnableToDeleteSection = 0xc000001b,
        IllegalInstruction = 0xc000001d,
        AlreadyCommitted = 0xc0000021,
        AccessDenied = 0xc0000022,
        BufferTooSmall = 0xc0000023,
        ObjectTypeMismatch = 0xc0000024,
        NonContinuableException = 0xc0000025,
        BadStack = 0xc0000028,
        NotLocked = 0xc000002a,
        NotCommitted = 0xc000002d,
        InvalidParameterMix = 0xc0000030,
        ObjectNameInvalid = 0xc0000033,
        ObjectNameNotFound = 0xc0000034,
        ObjectNameCollision = 0xc0000035,
        ObjectPathInvalid = 0xc0000039,
        ObjectPathNotFound = 0xc000003a,
        ObjectPathSyntaxBad = 0xc000003b,
        DataOverrun = 0xc000003c,
        DataLate = 0xc000003d,
        DataError = 0xc000003e,
        CrcError = 0xc000003f,
        SectionTooBig = 0xc0000040,
        PortConnectionRefused = 0xc0000041,
        InvalidPortHandle = 0xc0000042,
        SharingViolation = 0xc0000043,
        QuotaExceeded = 0xc0000044,
        InvalidPageProtection = 0xc0000045,
        MutantNotOwned = 0xc0000046,
        SemaphoreLimitExceeded = 0xc0000047,
        PortAlreadySet = 0xc0000048,
        SectionNotImage = 0xc0000049,
        SuspendCountExceeded = 0xc000004a,
        ThreadIsTerminating = 0xc000004b,
        BadWorkingSetLimit = 0xc000004c,
        IncompatibleFileMap = 0xc000004d,
        SectionProtection = 0xc000004e,
        EasNotSupported = 0xc000004f,
        EaTooLarge = 0xc0000050,
        NonExistentEaEntry = 0xc0000051,
        NoEasOnFile = 0xc0000052,
        EaCorruptError = 0xc0000053,
        FileLockConflict = 0xc0000054,
        LockNotGranted = 0xc0000055,
        DeletePending = 0xc0000056,
        CtlFileNotSupported = 0xc0000057,
        UnknownRevision = 0xc0000058,
        RevisionMismatch = 0xc0000059,
        InvalidOwner = 0xc000005a,
        InvalidPrimaryGroup = 0xc000005b,
        NoImpersonationToken = 0xc000005c,
        CantDisableMandatory = 0xc000005d,
        NoLogonServers = 0xc000005e,
        NoSuchLogonSession = 0xc000005f,
        NoSuchPrivilege = 0xc0000060,
        PrivilegeNotHeld = 0xc0000061,
        InvalidAccountName = 0xc0000062,
        UserExists = 0xc0000063,
        NoSuchUser = 0xc0000064,
        GroupExists = 0xc0000065,
        NoSuchGroup = 0xc0000066,
        MemberInGroup = 0xc0000067,
        MemberNotInGroup = 0xc0000068,
        LastAdmin = 0xc0000069,
        WrongPassword = 0xc000006a,
        IllFormedPassword = 0xc000006b,
        PasswordRestriction = 0xc000006c,
        LogonFailure = 0xc000006d,
        AccountRestriction = 0xc000006e,
        InvalidLogonHours = 0xc000006f,
        InvalidWorkstation = 0xc0000070,
        PasswordExpired = 0xc0000071,
        AccountDisabled = 0xc0000072,
        NoneMapped = 0xc0000073,
        TooManyLuidsRequested = 0xc0000074,
        LuidsExhausted = 0xc0000075,
        InvalidSubAuthority = 0xc0000076,
        InvalidAcl = 0xc0000077,
        InvalidSid = 0xc0000078,
        InvalidSecurityDescr = 0xc0000079,
        ProcedureNotFound = 0xc000007a,
        InvalidImageFormat = 0xc000007b,
        NoToken = 0xc000007c,
        BadInheritanceAcl = 0xc000007d,
        RangeNotLocked = 0xc000007e,
        DiskFull = 0xc000007f,
        ServerDisabled = 0xc0000080,
        ServerNotDisabled = 0xc0000081,
        TooManyGuidsRequested = 0xc0000082,
        GuidsExhausted = 0xc0000083,
        InvalidIdAuthority = 0xc0000084,
        AgentsExhausted = 0xc0000085,
        InvalidVolumeLabel = 0xc0000086,
        SectionNotExtended = 0xc0000087,
        NotMappedData = 0xc0000088,
        ResourceDataNotFound = 0xc0000089,
        ResourceTypeNotFound = 0xc000008a,
        ResourceNameNotFound = 0xc000008b,
        ArrayBoundsExceeded = 0xc000008c,
        FloatDenormalOperand = 0xc000008d,
        FloatDivideByZero = 0xc000008e,
        FloatInexactResult = 0xc000008f,
        FloatInvalidOperation = 0xc0000090,
        FloatOverflow = 0xc0000091,
        FloatStackCheck = 0xc0000092,
        FloatUnderflow = 0xc0000093,
        IntegerDivideByZero = 0xc0000094,
        IntegerOverflow = 0xc0000095,
        PrivilegedInstruction = 0xc0000096,
        TooManyPagingFiles = 0xc0000097,
        FileInvalid = 0xc0000098,
        InstanceNotAvailable = 0xc00000ab,
        PipeNotAvailable = 0xc00000ac,
        InvalidPipeState = 0xc00000ad,
        PipeBusy = 0xc00000ae,
        IllegalFunction = 0xc00000af,
        PipeDisconnected = 0xc00000b0,
        PipeClosing = 0xc00000b1,
        PipeConnected = 0xc00000b2,
        PipeListening = 0xc00000b3,
        InvalidReadMode = 0xc00000b4,
        IoTimeout = 0xc00000b5,
        FileForcedClosed = 0xc00000b6,
        ProfilingNotStarted = 0xc00000b7,
        ProfilingNotStopped = 0xc00000b8,
        NotSameDevice = 0xc00000d4,
        FileRenamed = 0xc00000d5,
        CantWait = 0xc00000d8,
        PipeEmpty = 0xc00000d9,
        CantTerminateSelf = 0xc00000db,
        InternalError = 0xc00000e5,
        InvalidParameter1 = 0xc00000ef,
        InvalidParameter2 = 0xc00000f0,
        InvalidParameter3 = 0xc00000f1,
        InvalidParameter4 = 0xc00000f2,
        InvalidParameter5 = 0xc00000f3,
        InvalidParameter6 = 0xc00000f4,
        InvalidParameter7 = 0xc00000f5,
        InvalidParameter8 = 0xc00000f6,
        InvalidParameter9 = 0xc00000f7,
        InvalidParameter10 = 0xc00000f8,
        InvalidParameter11 = 0xc00000f9,
        InvalidParameter12 = 0xc00000fa,
        MappedFileSizeZero = 0xc000011e,
        TooManyOpenedFiles = 0xc000011f,
        Cancelled = 0xc0000120,
        CannotDelete = 0xc0000121,
        InvalidComputerName = 0xc0000122,
        FileDeleted = 0xc0000123,
        SpecialAccount = 0xc0000124,
        SpecialGroup = 0xc0000125,
        SpecialUser = 0xc0000126,
        MembersPrimaryGroup = 0xc0000127,
        FileClosed = 0xc0000128,
        TooManyThreads = 0xc0000129,
        ThreadNotInProcess = 0xc000012a,
        TokenAlreadyInUse = 0xc000012b,
        PagefileQuotaExceeded = 0xc000012c,
        CommitmentLimit = 0xc000012d,
        InvalidImageLeFormat = 0xc000012e,
        InvalidImageNotMz = 0xc000012f,
        InvalidImageProtect = 0xc0000130,
        InvalidImageWin16 = 0xc0000131,
        LogonServer = 0xc0000132,
        DifferenceAtDc = 0xc0000133,
        SynchronizationRequired = 0xc0000134,
        DllNotFound = 0xc0000135,
        IoPrivilegeFailed = 0xc0000137,
        OrdinalNotFound = 0xc0000138,
        EntryPointNotFound = 0xc0000139,
        ControlCExit = 0xc000013a,
        PortNotSet = 0xc0000353,
        DebuggerInactive = 0xc0000354,
        CallbackBypass = 0xc0000503,
        PortClosed = 0xc0000700,
        MessageLost = 0xc0000701,
        InvalidMessage = 0xc0000702,
        RequestCanceled = 0xc0000703,
        RecursiveDispatch = 0xc0000704,
        LpcReceiveBufferExpected = 0xc0000705,
        LpcInvalidConnectionUsage = 0xc0000706,
        LpcRequestsNotAllowed = 0xc0000707,
        ResourceInUse = 0xc0000708,
        ProcessIsProtected = 0xc0000712,
        VolumeDirty = 0xc0000806,
        FileCheckedOut = 0xc0000901,
        CheckOutRequired = 0xc0000902,
        BadFileType = 0xc0000903,
        FileTooLarge = 0xc0000904,
        FormsAuthRequired = 0xc0000905,
        VirusInfected = 0xc0000906,
        VirusDeleted = 0xc0000907,
        TransactionalConflict = 0xc0190001,
        InvalidTransaction = 0xc0190002,
        TransactionNotActive = 0xc0190003,
        TmInitializationFailed = 0xc0190004,
        RmNotActive = 0xc0190005,
        RmMetadataCorrupt = 0xc0190006,
        TransactionNotJoined = 0xc0190007,
        DirectoryNotRm = 0xc0190008,
        CouldNotResizeLog = 0xc0190009,
        TransactionsUnsupportedRemote = 0xc019000a,
        LogResizeInvalidSize = 0xc019000b,
        RemoteFileVersionMismatch = 0xc019000c,
        CrmProtocolAlreadyExists = 0xc019000f,
        TransactionPropagationFailed = 0xc0190010,
        CrmProtocolNotFound = 0xc0190011,
        TransactionSuperiorExists = 0xc0190012,
        TransactionRequestNotValid = 0xc0190013,
        TransactionNotRequested = 0xc0190014,
        TransactionAlreadyAborted = 0xc0190015,
        TransactionAlreadyCommitted = 0xc0190016,
        TransactionInvalidMarshallBuffer = 0xc0190017,
        CurrentTransactionNotValid = 0xc0190018,
        LogGrowthFailed = 0xc0190019,
        ObjectNoLongerExists = 0xc0190021,
        StreamMiniversionNotFound = 0xc0190022,
        StreamMiniversionNotValid = 0xc0190023,
        MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
        CantOpenMiniversionWithModifyIntent = 0xc0190025,
        CantCreateMoreStreamMiniversions = 0xc0190026,
        HandleNoLongerValid = 0xc0190028,
        NoTxfMetadata = 0xc0190029,
        LogCorruptionDetected = 0xc0190030,
        CantRecoverWithHandleOpen = 0xc0190031,
        RmDisconnected = 0xc0190032,
        EnlistmentNotSuperior = 0xc0190033,
        RecoveryNotNeeded = 0xc0190034,
        RmAlreadyStarted = 0xc0190035,
        FileIdentityNotPersistent = 0xc0190036,
        CantBreakTransactionalDependency = 0xc0190037,
        CantCrossRmBoundary = 0xc0190038,
        TxfDirNotEmpty = 0xc0190039,
        IndoubtTransactionsExist = 0xc019003a,
        TmVolatile = 0xc019003b,
        RollbackTimerExpired = 0xc019003c,
        TxfAttributeCorrupt = 0xc019003d,
        EfsNotAllowedInTransaction = 0xc019003e,
        TransactionalOpenNotAllowed = 0xc019003f,
        TransactedMappingUnsupportedRemote = 0xc0190040,
        TxfMetadataAlreadyPresent = 0xc0190041,
        TransactionScopeCallbacksNotSet = 0xc0190042,
        TransactionRequiredPromotion = 0xc0190043,
        CannotExecuteFileInTransaction = 0xc0190044,
        TransactionsNotFrozen = 0xc0190045,
        MaximumNtStatus = 0xffffffff
    }
    public enum SYSTEM_INFORMATION_CLASS : int {
        SystemBasicInformation,                // q: SYSTEM_BASIC_INFORMATION
        SystemProcessorInformation,            // q: SYSTEM_PROCESSOR_INFORMATION
        SystemPerformanceInformation,          // q: SYSTEM_PERFORMANCE_INFORMATION
        SystemTimeOfDayInformation,            // q: SYSTEM_TIMEOFDAY_INFORMATION
        SystemPathInformation,
        SystemProcessInformation,              // q: SYSTEM_PROCESS_INFORMATION
        SystemCallCountInformation,            // q: SYSTEM_CALL_COUNT_INFORMATION
        SystemDeviceInformation,               // q: SYSTEM_DEVICE_INFORMATION
        SystemProcessorPerformanceInformation, // q: SYSTEM_PROCESSOR_PERFORMANCE_INFORMATION
        SystemFlagsInformation,                // q: SYSTEM_FLAGS_INFORMATION
        SystemCallTimeInformation,
        SystemModuleInformation,               // q: RTL_PROCESS_MODULES
        SystemLocksInformation,
        SystemStackTraceInformation,
        SystemPagedPoolInformation,
        SystemNonPagedPoolInformation,
        SystemHandleInformation,               // q: SYSTEM_HANDLE_INFORMATION
        SystemObjectInformation,               // q: SYSTEM_OBJECTTYPE_INFORMATION mixed with SYSTEM_OBJECT_INFORMATION
        SystemPageFileInformation,             // q: SYSTEM_PAGEFILE_INFORMATION
        SystemVdmInstemulInformation,
        SystemVdmBopInformation,
        SystemFileCacheInformation,            // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypeSystemCache)
        SystemPoolTagInformation,              // q: SYSTEM_POOLTAG_INFORMATION
        SystemInterruptInformation,            // q: SYSTEM_INTERRUPT_INFORMATION
        SystemDpcBehaviorInformation,          // q: SYSTEM_DPC_BEHAVIOR_INFORMATION; s: SYSTEM_DPC_BEHAVIOR_INFORMATION (requires SeLoadDriverPrivilege)
        SystemFullMemoryInformation,
        SystemLoadGdiDriverInformation,        // s (kernel-mode only)
        SystemUnloadGdiDriverInformation,      // s (kernel-mode only)
        SystemTimeAdjustmentInformation,       // q: SYSTEM_QUERY_TIME_ADJUST_INFORMATION; s: SYSTEM_SET_TIME_ADJUST_INFORMATION (requires SeSystemtimePrivilege)
        SystemSummaryMemoryInformation,
        SystemMirrorMemoryInformation,         // s (requires license value "Kernel-MemoryMirroringSupported") (requires SeShutdownPrivilege)
        SystemPerformanceTraceInformation,     // s
        SystemObsolete0,
        SystemExceptionInformation,            // q: SYSTEM_EXCEPTION_INFORMATION
        SystemCrashDumpStateInformation,       // s (requires SeDebugPrivilege)
        SystemKernelDebuggerInformation,       // q: SYSTEM_KERNEL_DEBUGGER_INFORMATION
        SystemContextSwitchInformation,        // q: SYSTEM_CONTEXT_SWITCH_INFORMATION
        SystemRegistryQuotaInformation,        // q: SYSTEM_REGISTRY_QUOTA_INFORMATION; s (requires SeIncreaseQuotaPrivilege)
        SystemExtendServiceTableInformation,   // s (requires SeLoadDriverPrivilege) // loads win32k only
        SystemPrioritySeperation,              // s (requires SeTcbPrivilege)
        SystemVerifierAddDriverInformation,    // s (requires SeDebugPrivilege)
        SystemVerifierRemoveDriverInformation, // s (requires SeDebugPrivilege)
        SystemProcessorIdleInformation,        // q: SYSTEM_PROCESSOR_IDLE_INFORMATION
        SystemLegacyDriverInformation,         // q: SYSTEM_LEGACY_DRIVER_INFORMATION
        SystemCurrentTimeZoneInformation,
        SystemLookasideInformation,            // q: SYSTEM_LOOKASIDE_INFORMATION
        SystemTimeSlipNotification,            // s (requires SeSystemtimePrivilege)
        SystemSessionCreate,
        SystemSessionDetach,
        SystemSessionInformation,
        SystemRangeStartInformation,           // q
        SystemVerifierInformation,             // q: SYSTEM_VERIFIER_INFORMATION; s (requires SeDebugPrivilege)
        SystemVerifierThunkExtend,             // s (kernel-mode only)
        SystemSessionProcessInformation,       // q: SYSTEM_SESSION_PROCESS_INFORMATION
        SystemLoadGdiDriverInSystemSpace,      // s (kernel-mode only) (same as SystemLoadGdiDriverInformation)
        SystemNumaProcessorMap,                // q
        SystemPrefetcherInformation,           // q: PREFETCHER_INFORMATION; s: PREFETCHER_INFORMATION // PfSnQueryPrefetcherInformation
        SystemExtendedProcessInformation,      // q: SYSTEM_PROCESS_INFORMATION
        SystemRecommendedSharedDataAlignment,  // q
        SystemComPlusPackage,                  // q; s
        SystemNumaAvailableMemory,             // 60
        SystemProcessorPowerInformation,       // q: SYSTEM_PROCESSOR_POWER_INFORMATION
        SystemEmulationBasicInformation,       // q
        SystemEmulationProcessorInformation,
        SystemExtendedHandleInformation,       // q: SYSTEM_HANDLE_INFORMATION_EX
        SystemLostDelayedWriteInformation,     // q: ULONG
        SystemBigPoolInformation,              // q: SYSTEM_BIGPOOL_INFORMATION
        SystemSessionPoolTagInformation,       // q: SYSTEM_SESSION_POOLTAG_INFORMATION
        SystemSessionMappedViewInformation,    // q: SYSTEM_SESSION_MAPPED_VIEW_INFORMATION
        SystemHotpatchInformation,             // q; s
        SystemObjectSecurityMode,              // q
        SystemWatchdogTimerHandler,            // s (kernel-mode only)
        SystemWatchdogTimerInformation,        // q (kernel-mode only); s (kernel-mode only)
        SystemLogicalProcessorInformation,     // q: SYSTEM_LOGICAL_PROCESSOR_INFORMATION
        SystemWow64SharedInformationObsolete,
        SystemRegisterFirmwareTableInformationHandler, // s (kernel-mode only)
        SystemFirmwareTableInformation,
        SystemModuleInformationEx,                  // q: RTL_PROCESS_MODULE_INFORMATION_EX
        SystemVerifierTriageInformation,
        SystemSuperfetchInformation,                // q: SUPERFETCH_INFORMATION; s: SUPERFETCH_INFORMATION // PfQuerySuperfetchInformation
        SystemMemoryListInformation,                // q: SYSTEM_MEMORY_LIST_INFORMATION; s: SYSTEM_MEMORY_LIST_COMMAND (requires SeProfileSingleProcessPrivilege)
        SystemFileCacheInformationEx,               // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (same as SystemFileCacheInformation)
        SystemThreadPriorityClientIdInformation,    // s: SYSTEM_THREAD_CID_PRIORITY_INFORMATION (requires SeIncreaseBasePriorityPrivilege)
        SystemProcessorIdleCycleTimeInformation,    // q: SYSTEM_PROCESSOR_IDLE_CYCLE_TIME_INFORMATION[]
        SystemVerifierCancellationInformation,      // name:wow64:whNT32QuerySystemVerifierCancellationInformation
        SystemProcessorPowerInformationEx,
        SystemRefTraceInformation,                  // q; s // ObQueryRefTraceInformation
        SystemSpecialPoolInformation,               // q; s (requires SeDebugPrivilege) // MmSpecialPoolTag, then MmSpecialPoolCatchOverruns != 0
        SystemProcessIdInformation,                 // q: SYSTEM_PROCESS_ID_INFORMATION
        SystemErrorPortInformation,                 // s (requires SeTcbPrivilege)
        SystemBootEnvironmentInformation,           // q: SYSTEM_BOOT_ENVIRONMENT_INFORMATION
        SystemHypervisorInformation,                // q; s (kernel-mode only)
        SystemVerifierInformationEx,                // q; s
        SystemTimeZoneInformation,                  // s (requires SeTimeZonePrivilege)
        SystemImageFileExecutionOptionsInformation, // s: SYSTEM_IMAGE_FILE_EXECUTION_OPTIONS_INFORMATION (requires SeTcbPrivilege)
        SystemCoverageInformation,                  // q; s // name:wow64:whNT32QuerySystemCoverageInformation; ExpCovQueryInformation
        SystemPrefetchPatchInformation,
        SystemVerifierFaultsInformation,            // s (requires SeDebugPrivilege)
        SystemSystemPartitionInformation,           // q: SYSTEM_SYSTEM_PARTITION_INFORMATION
        SystemSystemDiskInformation,                // q: SYSTEM_SYSTEM_DISK_INFORMATION
        SystemProcessorPerformanceDistribution,     // q: SYSTEM_PROCESSOR_PERFORMANCE_DISTRIBUTION
        SystemNumaProximityNodeInformation,         // q
        SystemDynamicTimeZoneInformation,           // q; s (requires SeTimeZonePrivilege)
        SystemCodeIntegrityInformation,             // q // SeCodeIntegrityQueryInformation
        SystemProcessorMicrocodeUpdateInformation,  // s
        SystemProcessorBrandString,                 // q // HaliQuerySystemInformation -> HalpGetProcessorBrandString, info class 23
        SystemVirtualAddressInformation,            // q: SYSTEM_VA_LIST_INFORMATION[]; s: SYSTEM_VA_LIST_INFORMATION[] (requires SeIncreaseQuotaPrivilege) // MmQuerySystemVaInformation
        SystemLogicalProcessorAndGroupInformation,  // q: SYSTEM_LOGICAL_PROCESSOR_INFORMATION_EX // since WIN7 // KeQueryLogicalProcessorRelationship
        SystemProcessorCycleTimeInformation,        // q: SYSTEM_PROCESSOR_CYCLE_TIME_INFORMATION[]
        SystemStoreInformation,                     // q; s // SmQueryStoreInformation
        SystemRegistryAppendString,                 // s: SYSTEM_REGISTRY_APPEND_STRING_PARAMETERS
        SystemAitSamplingValue,                     // s: ULONG (requires SeProfileSingleProcessPrivilege)
        SystemVhdBootInformation,                   // q: SYSTEM_VHD_BOOT_INFORMATION
        SystemCpuQuotaInformation,                  // q; s // PsQueryCpuQuotaInformation
        SystemNativeBasicInformation,
        SystemSpare1,
        SystemLowPriorityIoInformation,             // q: SYSTEM_LOW_PRIORITY_IO_INFORMATION
        SystemTpmBootEntropyInformation,            // q: TPM_BOOT_ENTROPY_NT_RESULT // ExQueryTpmBootEntropyInformation
        SystemVerifierCountersInformation,          // q: SYSTEM_VERIFIER_COUNTERS_INFORMATION
        SystemPagedPoolInformationEx,               // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypePagedPool)
        SystemSystemPtesInformationEx,              // q: SYSTEM_FILECACHE_INFORMATION; s (requires SeIncreaseQuotaPrivilege) (info for WorkingSetTypeSystemPtes)
        SystemNodeDistanceInformation,              // q
        SystemAcpiAuditInformation,                 // q: SYSTEM_ACPI_AUDIT_INFORMATION // HaliQuerySystemInformation -> HalpAuditQueryResults, info class 26
        SystemBasicPerformanceInformation,          // q: SYSTEM_BASIC_PERFORMANCE_INFORMATION // name:wow64:whNtQuerySystemInformation_SystemBasicPerformanceInformation
        SystemQueryPerformanceCounterInformation,   // q: SYSTEM_QUERY_PERFORMANCE_COUNTER_INFORMATION // since WIN7 SP1
        MaxSystemInfoClass
    }
    public enum PROCESS_INFORMATION_CLASS {
        ProcessBasicInformation,                 // q: PROCESS_BASIC_INFORMATION, PROCESS_EXTENDED_BASIC_INFORMATION
        ProcessQuotaLimits,                      // qs: QUOTA_LIMITS, QUOTA_LIMITS_EX
        ProcessIoCounters,                       // q: IO_COUNTERS
        ProcessVmCounters,                       // q: VM_COUNTERS, VM_COUNTERS_EX
        ProcessTimes,                            // q: KERNEL_USER_TIMES
        ProcessBasePriority,                     // s: KPRIORITY
        ProcessRaisePriority,                    // s: ULONG
        ProcessDebugPort,                        // q: HANDLE
        ProcessExceptionPort,                    // s: HANDLE
        ProcessAccessToken,                      // s: PROCESS_ACCESS_TOKEN
        ProcessLdtInformation,
        ProcessLdtSize,
        ProcessDefaultHardErrorMode,             // qs: ULONG
        ProcessIoPortHandlers,                   // (kernel-mode only)
        ProcessPooledUsageAndLimits,             // q: POOLED_USAGE_AND_LIMITS
        ProcessWorkingSetWatch,                  // q: PROCESS_WS_WATCH_INFORMATION[]; s: void
        ProcessUserModeIOPL,
        ProcessEnableAlignmentFaultFixup,        // s: BOOLEAN
        ProcessPriorityClass,                    // qs: PROCESS_PRIORITY_CLASS
        ProcessWx86Information,
        ProcessHandleCount,                      // q: ULONG, PROCESS_HANDLE_INFORMATION
        ProcessAffinityMask,                     // s: KAFFINITY
        ProcessPriorityBoost,                    // qs: ULONG
        ProcessDeviceMap,                        // qs: PROCESS_DEVICEMAP_INFORMATION, PROCESS_DEVICEMAP_INFORMATION_EX
        ProcessSessionInformation,               // q: PROCESS_SESSION_INFORMATION
        ProcessForegroundInformation,            // s: PROCESS_FOREGROUND_BACKGROUND
        ProcessWow64Information,                 // q: ULONG_PTR
        ProcessImageFileName,                    // q: UNICODE_STRING
        ProcessLUIDDeviceMapsEnabled,            // q: ULONG
        ProcessBreakOnTermination,               // qs: ULONG
        ProcessDebugObjectHandle,                // q: HANDLE
        ProcessDebugFlags,                       // qs: ULONG
        ProcessHandleTracing,                    // q: PROCESS_HANDLE_TRACING_QUERY; s: size 0 disables, otherwise enables
        ProcessIoPriority,                       // qs: ULONG
        ProcessExecuteFlags,                     // qs: ULONG
        ProcessResourceManagement,
        ProcessCookie,                           // q: ULONG
        ProcessImageInformation,                 // q: SECTION_IMAGE_INFORMATION
        ProcessCycleTime,                        // q: PROCESS_CYCLE_TIME_INFORMATION
        ProcessPagePriority,                     // q: ULONG
        ProcessInstrumentationCallback,
        ProcessThreadStackAllocation,            // qs: PROCESS_STACK_ALLOCATION_INFORMATION
        ProcessWorkingSetWatchEx,                // q: PROCESS_WS_WATCH_INFORMATION_EX[]
        ProcessImageFileNameWin32,               // q: UNICODE_STRING
        ProcessImageFileMapping,                 // q: HANDLE (input)
        ProcessAffinityUpdateMode,               // qs: PROCESS_AFFINITY_UPDATE_MODE
        ProcessMemoryAllocationMode,             // qs: PROCESS_MEMORY_ALLOCATION_MODE
        ProcessGroupInformation,                 // q: USHORT[]
        ProcessTokenVirtualizationEnabled,       // s: ULONG
        ProcessConsoleHostProcess,               // q: ULONG_PTR
        ProcessWindowInformation,                // q: PROCESS_WINDOW_INFORMATION
        ProcessHandleInformation,                // q: PROCESS_HANDLE_SNAPSHOT_INFORMATION // since WIN8
        ProcessMitigationPolicy,                 // s: PROCESS_MITIGATION_POLICY_INFORMATION
        ProcessDynamicFunctionTableInformation,
        ProcessHandleCheckingMode,
        ProcessKeepAliveCount,                   // q: PROCESS_KEEPALIVE_COUNT_INFORMATION
        ProcessRevokeFileHandles,                // s: PROCESS_REVOKE_FILE_HANDLES_INFORMATION
        MaxProcessInfoClass,
    }
    public enum THREAD_INFORMATION_CLASS {
        ThreadBasicInformation,                  // q: THREAD_BASIC_INFORMATION
        ThreadTimes,                             // q: KERNEL_USER_TIMES
        ThreadPriority,                          // s: KPRIORITY
        ThreadBasePriority,                      // s: LONG
        ThreadAffinityMask,                      // s: KAFFINITY
        ThreadImpersonationToken,                // s: HANDLE
        ThreadDescriptorTableEntry,
        ThreadEnableAlignmentFaultFixup,         // s: BOOLEAN
        ThreadEventPair,
        ThreadQuerySetWin32StartAddress,         // q: PVOID
        ThreadZeroTlsCell,
        ThreadPerformanceCount,                  // q: LARGE_INTEGER
        ThreadAmILastThread,                     // q: ULONG
        ThreadIdealProcessor,                    // s: ULONG
        ThreadPriorityBoost,                     // qs: ULONG
        ThreadSetTlsArrayAddress,
        ThreadIsIoPending,                       // q: ULONG
        ThreadHideFromDebugger,                  // s: void
        ThreadBreakOnTermination,                // qs: ULONG
        ThreadSwitchLegacyState,
        ThreadIsTerminated,                      // q: ULONG
        ThreadLastSystemCall,                    // q: THREAD_LAST_SYSCALL_INFORMATION
        ThreadIoPriority,                        // qs: ULONG
        ThreadCycleTime,                         // q: THREAD_CYCLE_TIME_INFORMATION
        ThreadPagePriority,                      // q: ULONG
        ThreadActualBasePriority,
        ThreadTebInformation,                    // q: THREAD_TEB_INFORMATION (requires THREAD_GET_CONTEXT + THREAD_SET_CONTEXT)
        ThreadCSwitchMon,
        ThreadCSwitchPmu,
        ThreadWow64Context,                      // q: WOW64_CONTEXT
        ThreadGroupInformation,                  // q: GROUP_AFFINITY
        ThreadUmsInformation,
        ThreadCounterProfiling,
        ThreadIdealProcessorEx,                  // q: PROCESSOR_NUMBER
        ThreadCpuAccountingInformation,          // since WIN8
        ThreadSwitchStackCheck,
        MaxThreadInfoClass,
    }
    public enum TOKEN_INFORMATION_CLASS {
        TokenUser = 1,
        TokenGroups,
        TokenPrivileges,
        TokenOwner,
        TokenPrimaryGroup,
        TokenDefaultDacl,
        TokenSource,
        TokenType,
        TokenImpersonationLevel,
        TokenStatistics,
        TokenRestrictedSids,
        TokenSessionId
    }
    public enum DumpTypes {
        DumpNormal = 0,
        DumpWithDataSegs = 1,
        DumpWithFullMemory = 2,
        DumpWithHandleData = 4,
        DumpFilterMemory = 8,
        DumpScanMemory = 10,
        DumpWithUnloadedModules = 20,
        DumpWithIndirectlyReferencedMemory = 40,
        DumpFilterModulePaths = 80,
        DumpWithProcessThreadData = 100,
        DumpWithPrivateReadWriteMemory = 200,
        DumpWithoutOptionalData = 400,
        DumpWithFullMemoryInfo = 800,
        DumpWithThreadInfo = 1000,
        DumpWithCodeSegs = 2000
    }
    [Flags()] public enum SnapshotFlags : int {
        HeapList = 0x1,
        Process = 0x2,
        Thread = 0x4,
        Module = 0x8,
        Module32 = 0x10,
        Inherit = unchecked((int)0x80000000),
        All = 0x1F
    }
    [Flags()] public enum ThreadAccessFlags : int {
        TERMINATE = 0x1,
        SUSPEND_RESUME = 0x2,
        GET_CONTEXT = 0x8,
        SET_CONTEXT = 0x10,
        SET_INFORMATION = 0x20,
        QUERY_INFORMATION = 0x40,
        SET_THREAD_TOKEN = 0x80,
        IMPERSONATE = 0x100,
        DIRECT_IMPERSONATION = 0x200
    }
    [Flags()] public enum ProcessAccessFlags : uint {
        All = 0x1F0FFF,
        Terminate = 0x1,
        CreateThread = 0x2,
        VMOperation = 0x8,
        VMRead = 0x10,
        VMWrite = 0x20,
        DupHandle = 0x40,
        SetInformation = 0x200,
        QueryInformation = 0x400,
        Synchronize = 0x100000
    }
    #endregion

    #region "API Structs..."
    [StructLayout(LayoutKind.Sequential)] public struct SYSTEM_PERFORMANCE_INFORMATION {
        public ulong IdleTime;
        public ulong IoReadTransferCount;
        public ulong IoWriteTransferCount;
        public ulong IoOtherTransferCount;
        public uint IoReadOperationCount;
        public uint IoWriteOperationCount;
        public uint IoOtherOperationCount;
        public uint AvailablePages;
        public uint CommittedPages;
        public uint CommitLimit;
        public uint PeakCommitment;
        public uint PageFaultCount;
        public uint CopyOnWriteCount;
        public uint TransitionCount;
        public uint CacheTransitionCount;
        public uint DemandZeroCount;
        public uint PageReadCount;
        public uint PageReadIoCount;
        public uint CacheReadCount;
        public uint CacheIoCount;
        public uint DirtyPagesWriteCount;
        public uint DirtyWriteIoCount;
        public uint MappedPagesWriteCount;
        public uint MappedWriteIoCount;
        public uint PagedPoolPages;
        public uint NonPagedPoolPages;
        public uint PagedPoolAllocs;
        public uint PagedPoolFrees;
        public uint NonPagedPoolAllocs;
        public uint NonPagedPoolFrees;
        public uint FreeSystemPtes;
        public uint ResidentSystemCodePage;
        public uint TotalSystemDriverPages;
        public uint TotalSystemCodePages;
        public uint NonPagedPoolLookasideHits;
        public uint PagedPoolLookasideHits;
        public uint AvailablePagedPoolPages;
        public uint ResidentSystemCachePage;
        public uint ResidentPagedPoolPage;
        public uint ResidentSystemDriverPage;
        public uint CcFastReadNoWait;
        public uint CcFastReadWait;
        public uint CcFastReadResourceMiss;
        public uint CcFastReadNotPossible;
        public uint CcFastMdlReadNoWait;
        public uint CcFastMdlReadWait;
        public uint CcFastMdlReadResourceMiss;
        public uint CcFastMdlReadNotPossible;
        public uint CcMapDataNoWait;
        public uint CcMapDataWait;
        public uint CcMapDataNoWaitMiss;
        public uint CcMapDataWaitMiss;
        public uint CcPinMappedDataCount;
        public uint CcPinReadNoWait;
        public uint CcPinReadWait;
        public uint CcPinReadNoWaitMiss;
        public uint CcPinReadWaitMiss;
        public uint CcCopyReadNoWait;
        public uint CcCopyReadWait;
        public uint CcCopyReadNoWaitMiss;
        public uint CcCopyReadWaitMiss;
        public uint CcMdlReadNoWait;
        public uint CcMdlReadWait;
        public uint CcMdlReadNoWaitMiss;
        public uint CcMdlReadWaitMiss;
        public uint CcReadAheadIos;
        public uint CcLazyWriteIos;
        public uint CcLazyWritePages;
        public uint CcDataFlushes;
        public uint CcDataPages;
        public uint ContextSwitches;
        public uint FirstLevelTbFills;
        public uint SecondLevelTbFills;
        public uint SystemCalls;
        // I need those, for unknown reason, but x86 needs them...
        public uint Dummy1;
        public uint Dummy2;
        public uint Dummy3;
    }
    [StructLayout(LayoutKind.Sequential)] public struct PERFORMANCE_INFORMATION {
        public uint cb;
        public UIntPtr CommitTotal;
        public UIntPtr CommitLimit;
        public UIntPtr CommitPeak;
        public UIntPtr PhysicalTotal;
        public UIntPtr PhysicalAvailable;
        public UIntPtr SystemCache;
        public UIntPtr KernelTotal;
        public UIntPtr KernelPaged;
        public UIntPtr KernelNonPaged;
        public UIntPtr PageSize;
        public uint HandleCount;
        public uint ProcessCount;
        public uint ThreadCount;
    }
    [StructLayout(LayoutKind.Sequential)] public struct PAGE_FILE_INFORMATION {
        public int Count;
        public int Reserved;
        public UIntPtr TotalSize;
        public UIntPtr TotalInUse;
        public UIntPtr PeakUsage;
    }
    [StructLayout(LayoutKind.Sequential)] public struct FILETIME {
        public uint dwLowDateTime;
        public uint dwHighDateTime;
        public readonly long Ticks => (((long)dwHighDateTime) << 32) | dwLowDateTime;
    }
    [StructLayout(LayoutKind.Sequential)] public struct PROCESS_IO_COUNTERS {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
    }
    [StructLayout(LayoutKind.Sequential)] public struct PROCESS_MEMORY_COUNTERS_EX {
        public uint cb;
        public uint PageFaultCount;              // Not Existant
        public long PeakWorkingSetSize;          // Process.GetCurrentProcess.PeakWorkingSet64
        public long WorkingSetSize;              // Process.GetCurrentProcess.WorkingSet64
        public long QuotaPeakPagedPoolUsage;     // Not Existant
        public long QuotaPagedPoolUsage;         // Process.GetCurrentProcess.PagedSystemMemorySize64
        public long QuotaPeakNonPagedPoolUsage;  // Not Existant
        public long QuotaNonPagedPoolUsage;      // Process.GetCurrentProcess.NonpagedSystemMemorySize64
        public long PagefileUsage;               // Process.GetCurrentProcess.PagedMemorySize64
        public long PeakPagefileUsage;           // Process.GetCurrentProcess.PeakPagedMemorySize64
        public long PrivateUsage;                // Process.GetCurrentProcess.PrivateMemorySize64
    }
    [StructLayout(LayoutKind.Sequential)] public struct TOKEN_USER {
        public SID_AND_ATTRIBUTES User;
    }
    [StructLayout(LayoutKind.Sequential)] public struct SID_AND_ATTRIBUTES {
        public IntPtr SID;
        public int Attributes;
    }

    [StructLayout(LayoutKind.Sequential)] public struct ThreadEntry32 {
        public int dwSize;
        public int cntUsage;
        public int th32ThreadID;
        public int th32OwnerProcessID;
        public int tpBasePri;
        public int tpDeltaPri;
        public int dwFlags;
    }
    [StructLayout(LayoutKind.Sequential)] public struct ModuleEntry32 {
        public uint dwSize;
        public uint th32ModuleID;
        public uint th32ProcessID;
        public uint GlblcntUsage;
        public uint ProccntUsage;
        public IntPtr modBaseAddr;
        public uint modBaseSize;
        public IntPtr hModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string szModule;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExePath;
    }
    [StructLayout(LayoutKind.Sequential)] public struct ProcessEntry32 {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szExeFile;
    }

    [StructLayout(LayoutKind.Sequential)] public struct CLIENT_ID {
        public IntPtr UniqueProcess;
        public IntPtr UniqueThread;
    }
    [StructLayout(LayoutKind.Sequential)] public struct UNICODE_STRING {
        public ushort Length;
        public ushort MaximumLength;
        [MarshalAs(UnmanagedType.LPWStr)] public string Buffer;
    }
    [StructLayout(LayoutKind.Sequential)] public struct SYSTEM_PROCESS_INFORMATION {
        public uint NextEntryOffset;
        public uint NumberOfThreads;
        public ulong WorkingSetPrivateSize;
        public uint HardFaultCount;
        public uint NumberOfThreadsHighWatermark;
        public ulong CycleTime;
        public ulong CreateTime;
        public ulong UserTime;
        public ulong KernelTime;
        public UNICODE_STRING ImageName;
        public int BasePriority;
        public IntPtr UniqueProcessId;
        public IntPtr InheritedFromUniqueProcessId;
        public uint HandleCount;
        public uint SessionId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1, ArraySubType = UnmanagedType.U4)] public UIntPtr[] UniqueProcessKey;
        public UIntPtr PeakVirtualSize;
        public UIntPtr VirtualSize;
        public uint PageFaultCount;
        public UIntPtr PeakWorkingSetSize;
        public UIntPtr WorkingSetSize;
        public UIntPtr QuotaPeakPagedPoolUsage;
        public UIntPtr QuotaPagedPoolUsage;
        public UIntPtr QuotaPeakNonPagedPoolUsage;
        public UIntPtr QuotaNonPagedPoolUsage;
        public UIntPtr PagefileUsage;
        public UIntPtr PeakPagefileUsage;
        public UIntPtr PrivatePageCount;
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 250, ArraySubType = UnmanagedType.Struct)] public SYSTEM_EXTENDED_THREAD_INFORMATION[] Threads;
    }
    [StructLayout(LayoutKind.Sequential)] public struct SYSTEM_THREAD_INFORMATION {
        public ulong KernelTime;
        public ulong UserTime;
        public ulong CreateTime;
        public uint WaitTime;
        public IntPtr StartAddress;
        public CLIENT_ID ClientId;
        public int Priority;
        public int BasePriority;
        public uint ContextSwitchCount;
        public int State;
        public int WaitReason;
    }
    [StructLayout(LayoutKind.Sequential)] public struct SYSTEM_EXTENDED_THREAD_INFORMATION {
        public SYSTEM_THREAD_INFORMATION ThreadInfo;
        public IntPtr StackBase;
        public IntPtr StackLimit;
        public IntPtr Win32StartAddress;
        public IntPtr TebBase;
        public UIntPtr Reserved2;
        public UIntPtr Reserved3;
        public UIntPtr Reserved4;
    }
    #endregion

    [LibraryImport("ntdll.dll", SetLastError = true)]
    internal static partial NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, ref SYSTEM_PERFORMANCE_INFORMATION SystemInformation, int SystemInformationLength, out int returnLength);
    [System.Security.SuppressUnmanagedCodeSecurity()]
    [DllImport("ntdll.dll", SetLastError = true)]
    internal static extern NTSTATUS NtQuerySystemInformation(SYSTEM_INFORMATION_CLASS SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, out int returnLength);

    [LibraryImport("psapi.dll", SetLastError = true, EntryPoint = "EnumPageFilesW")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool EnumPageFiles(EnumPageFilesProc proc, IntPtr lpContext);
    public delegate bool EnumPageFilesProc(IntPtr lpContext, ref PAGE_FILE_INFORMATION Info, string Name);

    [LibraryImport("psapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetPerformanceInfo(ref PERFORMANCE_INFORMATION pPerformanceInformation, uint cb);

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetSystemTimes(ref FILETIME lpIdleTime, ref FILETIME lpKernelTime, ref FILETIME lpUserTime);

    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern unsafe bool ConvertStringSidToSid(string StringSid, ref IntPtr Sid);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern unsafe bool ConvertSidToStringSid(IntPtr pSID, ref string pStringSid);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern unsafe bool GetTokenInformation(IntPtr TokenHandle, TOKEN_INFORMATION_CLASS tokenInfoClass, IntPtr TokenInformation, int TokenInformationLength, ref uint ReturnLength);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern unsafe bool LookupAccountSid(string lpSystemName, IntPtr pSid, StringBuilder Account, ref int cbName, StringBuilder DomainName, ref int cbDomainName, ref int peUse);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe bool GetSystemTimes(ref TimeSpan lpIdleTime, ref TimeSpan lpKernelTime, ref TimeSpan lpUserTime);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe bool GetProcessHandleCount(IntPtr hProcess, ref int dwHandleCount);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern unsafe int GetGuiResources(IntPtr hProcess, int uiFlags);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("psapi.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetProcessMemoryInfo")]
    internal static extern unsafe bool GetProcessMemoryInfo(IntPtr hProcess, ref PROCESS_MEMORY_COUNTERS_EX counters, uint size = 80);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern unsafe bool GetProcessTimes(IntPtr hProcess, ref TimeSpan lpCreationTime, ref TimeSpan lpExitTime, ref TimeSpan lpKernelTime, ref TimeSpan lpUserTime);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("Kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "GetProcessIoCounters")]
    internal static extern unsafe bool GetProcessIoCounters(IntPtr hProcess, ref PROCESS_IO_COUNTERS lpIoCounters);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe int ProcessIdToSessionId(int dwProcessId, ref int pSessionId);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern unsafe bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern unsafe IntPtr OpenThread(ThreadAccessFlags dwDesiredAccess, bool bInheritHandle, IntPtr dwThreadId);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe bool TerminateProcess(IntPtr hProcess, uint uExitCode);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern unsafe uint ResumeThread(IntPtr hThread);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    internal static extern unsafe uint SuspendThread(IntPtr hThread);
    [System.Security.SuppressUnmanagedCodeSecurity()] [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern unsafe bool CloseHandle(IntPtr handle);


}
