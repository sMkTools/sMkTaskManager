namespace sMkTaskManager;

partial class frmMain {
    private ToolStripMenuItem mnuMonitor, mnuComputer;

    private void Initialize_MainMenu() {
        mnu.SuspendLayout();

        mnuMonitor = new("&Monitor") { Name = "mnuMonitor" };
        mnuMonitor.DropDownItems.AddRange(GenerateMenuItems_Monitor());
        mnuMonitor.DropDownOpening += Menu_DropDownOpening;
        mnuMonitor.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnuComputer = new("&Computer") { Name = "mnuComputer" };
        mnuComputer.DropDownItems.AddRange(GenerateMenuItems_Computer());
        mnuComputer.DropDownOpening += Menu_DropDownOpening;
        mnuComputer.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnuFile.DropDownItems.Clear();
        mnuFile.DropDownItems.AddRange(GenerateMenuItems_File());
        mnuFile.DropDownOpening += Menu_DropDownOpening;
        mnuFile.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnuOptions.DropDownItems.Clear();
        mnuOptions.DropDownItems.AddRange(GenerateMenuItems_Options());
        mnuOptions.DropDownOpening += Menu_DropDownOpening;
        mnuOptions.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnuView.DropDownItems.Clear();
        mnuView.DropDownItems.AddRange(GenerateMenuItems_View());
        mnuView.DropDownOpening += Menu_DropDownOpening;
        mnuView.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnuHelp.DropDownItems.Clear();
        mnuHelp.DropDownItems.AddRange(GenerateMenuItems_Help());
        mnuHelp.DropDownOpening += Menu_DropDownOpening;
        mnuHelp.DropDownItemClicked += Mnu_DropDownItemClicked;

        mnu.ResumeLayout(false);
        mnu.PerformLayout();

    }

    private ToolStripItem[] GenerateMenuItems_File() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("&New Task (Run...)")   { Name = "mnuFile_NewTask"},
            new ToolStripMenuItem("New Task (Run As...)") { Name = "mnuFile_NewTaskAsUser"},
            new ToolStripSeparator(),
            mnuMonitor, mnuComputer,
            new ToolStripSeparator(),
            new ToolStripMenuItem("E&xit") { Name = "mnuFile_Exit"},
        };
    }
    private ToolStripItem[] GenerateMenuItems_Monitor() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("&Screensaver...")   { Name = "mnuMonitor_ScreenSaver"},
            new ToolStripMenuItem("Power &off...") { Name = "mnuMonitor_PowerOff"},
        };
    }
    private ToolStripItem[] GenerateMenuItems_Computer() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("Lock") { Name = "mnuComputer_Lock"},
            new ToolStripMenuItem("Log off") { Name = "mnuComputer_Logoff"},
            new ToolStripSeparator(),
            new ToolStripMenuItem("Sleep") { Name = "mnuComputer_Sleep"},
            new ToolStripMenuItem("Hibernate") { Name = "mnuComputer_Hibernate"},
            new ToolStripSeparator(),
            new ToolStripMenuItem("Restart") { Name = "mnuComputer_Restart"},
            new ToolStripMenuItem("Shutdown") { Name = "mnuComputer_Shutdown"},
            new ToolStripMenuItem("Force Shutdown") { Name = "mnuComputer_ForceShutdown"},
        };
    }
    private ToolStripItem[] GenerateMenuItems_Options() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("&Preferences...") { Name = "mnuOptions_Preferences",ShortcutKeys=Keys.F2},
            new ToolStripSeparator(),
            new ToolStripMenuItem("&Always On Top") { Name = "mnuOptions_OnTop"},
            new ToolStripMenuItem("H&ighlight Changes") { Name = "mnuOptions_Highlight"},
            new ToolStripMenuItem("&Hide When Minimized") { Name = "mnuOptions_HideMinimize"},
            new ToolStripMenuItem("&Minimize When Closing") { Name = "mnuOptions_MinimizeClose"},
            new ToolStripSeparator(),
            new ToolStripMenuItem("&Default Task Manager") { Name = "mnuOptions_DefaultTaskManager"},
            new ToolStripSeparator() { Tag="pl"},
            new ToolStripMenuItem("&Select Columns...") { Name = "mnuOptions_SelectColumns", Tag="pl"},
            new ToolStripMenuItem("Select S&ummary View...") { Name = "mnuOptions_SelectSummary", Tag="pl"},

        };
    }
    private ToolStripItem[] GenerateMenuItems_View() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("&Refresh Now") { Name = "mnuView_Refresh", ShortcutKeys=Keys.F5},
            new ToolStripSeparator(),
            new ToolStripMenuItem("Update Speed: &High") { Name = "mnuView_SpeedHigh"},
            new ToolStripMenuItem("Update Speed: &Normal") { Name = "mnuView_SpeedNormal"},
            new ToolStripMenuItem("Update Speed: &Low") { Name = "mnuView_SpeedLow"},
            new ToolStripSeparator(),
            new ToolStripMenuItem("&Pause Data") { Name = "mnuView_Pause", ShortcutKeys=Keys.F6},
        };
    }
    private ToolStripItem[] GenerateMenuItems_Help() {
        return new ToolStripItem[] {
            new ToolStripMenuItem("&About") { Name = "mnuHelp_About", ShortcutKeys=Keys.F1 },
            new ToolStripMenuItem("&Timmings") { Name = "mnuHelp_Timmings", ShortcutKeys=Keys.F9 },
        };
    }

    private void Menu_DropDownOpening(object? sender, EventArgs e) {
        var mnu = (ToolStripMenuItem)sender!;

    }
    private void Mnu_DropDownItemClicked(object? sender, ToolStripItemClickedEventArgs e) {
        if (e.ClickedItem == null) return;
        switch (e.ClickedItem.Name) {
            default: break;
        }
    }

}