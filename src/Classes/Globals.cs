namespace sMkTaskManager; 

internal static class Globals {
    internal static int bpi = 20; // Base PID Ignore
    internal static List<string> skipProcess = new List<string>(new[] { "audiodg" });
    internal static List<string> skipServices = new();

    internal static void OpenProcessForm(int PID) { }

    internal static void NotImplemented([System.Runtime.CompilerServices.CallerMemberName] string feature = "") {
        MessageBox.Show("This feature is not implemented yet.", feature, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

}
