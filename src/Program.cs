using System.Diagnostics;
namespace sMkTaskManager;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal static class Program {

    [STAThread]
    static void Main() {
        // Check if we can run or just activate existing instance.
        if (CheckExistingInstance(Process.GetCurrentProcess())) return;

        ApplicationConfiguration.Initialize();

        // Check if we have admin privileges to run or not
        if (!Extensions.IsAdminRole()) {
            var str = "This application requires Elevated Privileges to function." + "\r\n" +
            "Running without them will cause the application to become unstable or crash." + "\r\n\r\n" +
            "Do you want to continue running this application anyway?";
            if (MessageBox.Show(str, "Required Elevated Privileges", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) {
                Application.Exit();
            }
        }
        // Spawn the main form.
        Application.Run(new frmMain());
    }

    private static bool CheckExistingInstance(Process curProcess) {
        foreach (string arg in Environment.GetCommandLineArgs()) {
            if (arg.ToUpper() == "/NEW" || arg.ToUpper() == "NEW") { return false; }
            if (arg.ToUpper() == "/NEWINSTANCE" || arg.ToUpper() == "NEWINSTANCE") { return false; }
        }

        foreach (Process p in Process.GetProcesses()) {
            try {
                if (p.Id < Shared.bpi) continue;
                if (p.SessionId != curProcess.SessionId) continue;
                if (p.Id == curProcess.Id) continue;
                if (p.MainModule == null) continue;
                if (p.MainModule.FileName == curProcess.MainModule!.FileName) {
                    Debug.WriteLine("Second Instance of: " + curProcess.MainModule.FileName);
                    // const int BSF_IGNORECURRENTTASK = 0x2;
                    // const int BSF_POSTMESSAGE = 0x10;
                    int BSM_APPLICATIONS = 0x8;
                    Shared.PrivateMsgID = Classes.API.RegisterWindowMessage(Application.ExecutablePath.Replace(@"\", "_"));
                    _ = Classes.API.BroadcastSystemMessage(0x2 | 0x10, ref BSM_APPLICATIONS, Shared.PrivateMsgID, 1, 0);
                    if (p.MainWindowHandle > 0) Classes.API.SetForegroundWindow(p.MainWindowHandle);
                    return true;
                }
            } catch { }
        }
        return false;
    }

}
