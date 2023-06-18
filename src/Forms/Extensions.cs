using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Principal;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static class Extensions {

    // Friendly functions to make it easier add items and separators to menus
    public static ToolStripMenuItem AddMenuItem(this ToolStripItemCollection parent, string text) => (ToolStripMenuItem)parent.Add(text);
    public static ToolStripSeparator AddSeparator(this ToolStripItemCollection parent) {
        var separator = new ToolStripSeparator();
        parent.Add(separator);
        return separator;
    }

    public static void SetDoubleBuffered(Control c) {
        if (SystemInformation.TerminalServerSession) return;
        PropertyInfo? aProp = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
        aProp?.SetValue(c, true, null);
    }

    public static void CascadingDoubleBuffer(Control c) {
        var p = c.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
        p?.SetValue(c, true, null);
        foreach (Control cc in c.Controls) CascadingDoubleBuffer(cc);
    }

    public static bool IsAdminRole() {
        try {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            return (new WindowsPrincipal(identity)).IsInRole(WindowsBuiltInRole.Administrator);
        } catch {
            return false;
        }
    }

    public static void StartMeasure(Stopwatch? tmr) {
        if (!Debugger.IsAttached) return;
        tmr ??= new();
        tmr.Start();
    }
    public static void StopMeasure(Stopwatch? tmr, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "") {
        if (!Debugger.IsAttached) return;
        tmr?.Stop();
        Debug.WriteLine($"- {methodName} Time: {tmr?.ElapsedMilliseconds}ms.");
    }


}
