using System;
using System.Linq;
using System.Text;

namespace sMkTaskManager.Extensions;

internal static class WinFormExtensions {

    // Friendly functions to make it easier add items and separators to menus
    public static ToolStripMenuItem AddMenuItem(this ToolStripItemCollection parent, string text) => (ToolStripMenuItem)parent.Add(text);
    public static ToolStripSeparator AddSeparator(this ToolStripItemCollection parent) {
        var separator = new ToolStripSeparator();
        parent.Add(separator);
        return separator;
    }


}
