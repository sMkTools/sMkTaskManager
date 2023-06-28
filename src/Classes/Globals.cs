using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static class Globals {
    private static string _SystemAccount = "";
    internal static int bpi = 20; // Base PID Ignore
    internal static List<string> skipProcess = new List<string>(new[] { "audiodg" });
    internal static List<string> skipServices = new();
    internal static int CurrentSessionID = Process.GetCurrentProcess().SessionId;
    internal static void OpenProcessForm(int PID) { }

    internal static void NotImplemented([CallerMemberName] string feature = "") {
        MessageBox.Show("This feature is not implemented yet.", feature, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    internal static void DebugLine(Exception e, int Code=0, [CallerMemberName] string Method = "") {
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
