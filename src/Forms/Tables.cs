using sMkTaskManager.Classes;
using System.Diagnostics;
using System.Runtime.Versioning;

namespace sMkTaskManager;

[SupportedOSPlatform("windows")]
internal static class Tables {

    internal static Dictionary<string, Stopwatch> Timmings = new();

    internal static TaskManagerSystem System = new();

    internal static HashSet<string> ColsServices = new();
    internal static HashSet<string> ColsConnections = new();
    internal static HashSet<string> ColsSummary = new();

}
