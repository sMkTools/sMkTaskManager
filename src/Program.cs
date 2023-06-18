namespace sMkTaskManager;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
internal static class Program {

    [STAThread]
    static void Main() {
        // To customize application configuration such as set high DPI settings or default font, see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        if (!Extensions.IsAdminRole()) {
            var str = "This application requires Elevated Privileges to function." + "\r\n" +
            "Running without them will cause the application to become unstable or crash." + "\r\n\r\n" +
            "Do you want to continue running this application anyway?";
            if (MessageBox.Show(str, "Required Elevated Privileges", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No) {
                Application.Exit();
            }
        }

        Application.Run(new frmMain());
    }

}
