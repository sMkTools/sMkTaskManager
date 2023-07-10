using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.Versioning;
using sMkTaskManager.Classes;
using sMkTaskManager.Controls;
namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
internal class tabNetworking : UserControl, ITaskManagerTab {
    private readonly Stopwatch _stopWatch = new();
    private readonly Dictionary<string, string> _IPstatsValues = new();
    private readonly Dictionary<string, string> _TCPstatsValues = new();
    private readonly Dictionary<string, string> _UDPstatsValues = new();
    private readonly Dictionary<string, string> _ICMPstatsValues = new();

    private TabControl tc;
    private TabPage tpUsage;
    private TabPage tpIP;
    private TabPage tpTCP;
    private TabPage tpUDP;
    private TabPage tpICMP;
    private sMkListView lvIP;
    private sMkListView lvTCP;
    private sMkListView lvUDP;
    private sMkListView lvICMP;
    private Button btnForceRefresh;

    public event EventHandler? ForceRefreshClicked;
    public event EventHandler? RefreshStarts;
    public event EventHandler? RefreshComplete;

    private IContainer? components = null;
    protected override void Dispose(bool disposing) {
        if (disposing && (components != null)) { components.Dispose(); }
        base.Dispose(disposing);
    }

    public tabNetworking() {
        InitializeComponent();
        InitializeExtras();
        Extensions.CascadingDoubleBuffer(this);
    }
    private void InitializeComponent() {
        components = new Container();
        tc = new TabControl();
        tpUsage = new TabPage();
        tpIP = new TabPage();
        lvIP = new sMkListView();
        tpTCP = new TabPage();
        lvTCP = new sMkListView();
        tpUDP = new TabPage();
        lvUDP = new sMkListView();
        tpICMP = new TabPage();
        lvICMP = new sMkListView();
        btnForceRefresh = new Button();
        tc.SuspendLayout();
        tpIP.SuspendLayout();
        tpTCP.SuspendLayout();
        tpUDP.SuspendLayout();
        tpICMP.SuspendLayout();
        SuspendLayout();
        // 
        // tc
        // 
        tc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tc.Appearance = TabAppearance.FlatButtons;
        tc.Controls.Add(tpUsage);
        tc.Controls.Add(tpIP);
        tc.Controls.Add(tpTCP);
        tc.Controls.Add(tpUDP);
        tc.Controls.Add(tpICMP);
        tc.Location = new Point(2, 6);
        tc.Margin = new Padding(6);
        tc.Name = "tc";
        tc.Padding = new Point(4, 3);
        tc.SelectedIndex = 0;
        tc.Size = new Size(596, 588);
        tc.TabIndex = 0;
        // 
        // tpUsage
        // 
        tpUsage.BackColor = SystemColors.Control;
        tpUsage.Location = new Point(4, 27);
        tpUsage.Margin = new Padding(0);
        tpUsage.Name = "tpUsage";
        tpUsage.Size = new Size(588, 557);
        tpUsage.TabIndex = 0;
        tpUsage.Text = "Network Usage";
        // 
        // tpIP
        // 
        tpIP.BackColor = SystemColors.Control;
        tpIP.Controls.Add(lvIP);
        tpIP.Location = new Point(4, 27);
        tpIP.Margin = new Padding(0);
        tpIP.Name = "tpIP";
        tpIP.Size = new Size(588, 557);
        tpIP.TabIndex = 1;
        tpIP.Text = "IP Statistics";
        // 
        // lvIP
        // 
        lvIP.AlternateRowColors = false;
        lvIP.Dock = DockStyle.Fill;
        lvIP.FullRowSelect = true;
        lvIP.GridLines = true;
        lvIP.LabelWrap = false;
        lvIP.Location = new Point(0, 0);
        lvIP.Margin = new Padding(0);
        lvIP.MultiSelect = false;
        lvIP.Name = "lvIP";
        lvIP.ShowItemToolTips = true;
        lvIP.Size = new Size(588, 557);
        lvIP.Sortable = true;
        lvIP.SortColumn = -1;
        lvIP.Sorting = SortOrder.Ascending;
        lvIP.TabIndex = 0;
        lvIP.UseCompatibleStateImageBehavior = false;
        lvIP.View = View.Details;
        // 
        // tpTCP
        // 
        tpTCP.BackColor = SystemColors.Control;
        tpTCP.Controls.Add(lvTCP);
        tpTCP.Location = new Point(4, 27);
        tpTCP.Margin = new Padding(0);
        tpTCP.Name = "tpTCP";
        tpTCP.Size = new Size(588, 557);
        tpTCP.TabIndex = 2;
        tpTCP.Text = "TCP Statistics";
        // 
        // lvTCP
        // 
        lvTCP.AlternateRowColors = false;
        lvTCP.Dock = DockStyle.Fill;
        lvTCP.FullRowSelect = true;
        lvTCP.GridLines = true;
        lvTCP.LabelWrap = false;
        lvTCP.Location = new Point(0, 0);
        lvTCP.Margin = new Padding(0);
        lvTCP.MultiSelect = false;
        lvTCP.Name = "lvTCP";
        lvTCP.ShowItemToolTips = true;
        lvTCP.Size = new Size(588, 557);
        lvTCP.Sortable = true;
        lvTCP.SortColumn = -1;
        lvTCP.Sorting = SortOrder.Ascending;
        lvTCP.TabIndex = 1;
        lvTCP.UseCompatibleStateImageBehavior = false;
        lvTCP.View = View.Details;
        // 
        // tpUDP
        // 
        tpUDP.BackColor = SystemColors.Control;
        tpUDP.Controls.Add(lvUDP);
        tpUDP.Location = new Point(4, 27);
        tpUDP.Margin = new Padding(0);
        tpUDP.Name = "tpUDP";
        tpUDP.Size = new Size(588, 557);
        tpUDP.TabIndex = 3;
        tpUDP.Text = "UDP Statistics";
        // 
        // lvUDP
        // 
        lvUDP.AlternateRowColors = false;
        lvUDP.Dock = DockStyle.Fill;
        lvUDP.FullRowSelect = true;
        lvUDP.GridLines = true;
        lvUDP.LabelWrap = false;
        lvUDP.Location = new Point(0, 0);
        lvUDP.Margin = new Padding(0);
        lvUDP.MultiSelect = false;
        lvUDP.Name = "lvUDP";
        lvUDP.ShowItemToolTips = true;
        lvUDP.Size = new Size(588, 557);
        lvUDP.Sortable = true;
        lvUDP.SortColumn = -1;
        lvUDP.Sorting = SortOrder.Ascending;
        lvUDP.TabIndex = 1;
        lvUDP.UseCompatibleStateImageBehavior = false;
        lvUDP.View = View.Details;
        // 
        // tpICMP
        // 
        tpICMP.BackColor = SystemColors.Control;
        tpICMP.Controls.Add(lvICMP);
        tpICMP.Location = new Point(4, 27);
        tpICMP.Margin = new Padding(0);
        tpICMP.Name = "tpICMP";
        tpICMP.Size = new Size(588, 557);
        tpICMP.TabIndex = 4;
        tpICMP.Text = "ICMP Statistics";
        // 
        // lvICMP
        // 
        lvICMP.AlternateRowColors = false;
        lvICMP.Dock = DockStyle.Fill;
        lvICMP.FullRowSelect = true;
        lvICMP.GridLines = true;
        lvICMP.LabelWrap = false;
        lvICMP.Location = new Point(0, 0);
        lvICMP.Margin = new Padding(0);
        lvICMP.MultiSelect = false;
        lvICMP.Name = "lvICMP";
        lvICMP.ShowItemToolTips = true;
        lvICMP.Size = new Size(588, 557);
        lvICMP.Sortable = true;
        lvICMP.SortColumn = -1;
        lvICMP.Sorting = SortOrder.Ascending;
        lvICMP.TabIndex = 1;
        lvICMP.UseCompatibleStateImageBehavior = false;
        lvICMP.View = View.Details;
        // 
        // btnForceRefresh
        // 
        btnForceRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnForceRefresh.Location = new Point(505, 6);
        btnForceRefresh.Margin = new Padding(3, 3, 0, 3);
        btnForceRefresh.Name = "btnForceRefresh";
        btnForceRefresh.Size = new Size(89, 23);
        btnForceRefresh.TabIndex = 5;
        btnForceRefresh.Text = "Force Refresh";
        // 
        // tabNetworking
        // 
        Controls.Add(btnForceRefresh);
        Controls.Add(tc);
        Name = "tabNetworking";
        Size = new Size(600, 600);
        tc.ResumeLayout(false);
        tpIP.ResumeLayout(false);
        tpTCP.ResumeLayout(false);
        tpUDP.ResumeLayout(false);
        tpICMP.ResumeLayout(false);
        ResumeLayout(false);
    }
    private void InitializeExtras() {
        KeyPress += OnKeyPress;
        SizeChanged += OnSizeChanged;
        VisibleChanged += OnVisibleChanged;
        tc.SelectedIndexChanged += onTabControlSelectedTabChanged;
        btnForceRefresh.Click += OnButtonClicked;
    }
    private void OnKeyPress(object? sender, KeyPressEventArgs e) { }
    private void OnVisibleChanged(object? sender, EventArgs e) { }
    private void OnSizeChanged(object? sender, EventArgs e) {
        foreach (TabPage tp in tc.TabPages) {
            if (tc.Width < 550) {
                tp.Text = tp.Text.Replace("Statistics", "Stats");
            } else {
                tp.Text = tp.Text.Replace("Stats", "Statistics");
            }
        }
    }
    private void OnButtonClicked(object? sender, EventArgs e) {
        if (sender == null) return;
        if (sender == btnForceRefresh) { BeginInvoke(Feature_ForceRefresh); return; }
    }
    private void onTabControlSelectedTabChanged(object? sender, EventArgs e) {
        if (!Shared.InitComplete || !Visible) return;
        if (tc.SelectedTab == tpIP && lvIP.Items.Count == 0) UpdateIPstats();
        if (tc.SelectedTab == tpTCP && lvTCP.Items.Count == 0) UpdateTCPstats();
        if (tc.SelectedTab == tpUDP && lvUDP.Items.Count == 0) UpdateUDPstats();
        if (tc.SelectedTab == tpICMP && lvICMP.Items.Count == 0) UpdateICMPstats();
    }

    public void Feature_ForceRefresh() {
        Refresher(true);
        ForceRefreshClicked?.Invoke(this, EventArgs.Empty);
    }

    public sMkListView? ListView => null;
    public string Title { get; set; } = "Networking";
    public string Description { get; set; } = "Networking";
    public string TimmingKey => "Net";
    public long TimmingValue => _stopWatch.ElapsedMilliseconds;
    public bool CanSelectColumns => false;
    public TaskManagerColumnTypes ColumnType => TaskManagerColumnTypes.None;
    public void ForceRefresh() => ForceRefreshClicked?.Invoke(this, EventArgs.Empty);

    private void RefresherDoWork() {
        RefreshStarts?.Invoke(this, EventArgs.Empty);
        if (tc.SelectedTab == tpIP) UpdateIPstats();
        if (tc.SelectedTab == tpTCP) UpdateTCPstats();
        if (tc.SelectedTab == tpUDP) UpdateUDPstats();
        if (tc.SelectedTab == tpICMP) UpdateICMPstats();
        RefreshComplete?.Invoke(this, EventArgs.Empty);
    }
    public void Refresher(bool firstTime = false) {
        _stopWatch.Restart();
        if (Visible || firstTime) {
            if (InvokeRequired) {
                Invoke(RefresherDoWork);
            } else {
                RefresherDoWork();
            }
        }
        _stopWatch.Stop();
    }
    public void LoadSettings() { }
    public bool SaveSettings() { return true; }
    public void ApplySettings() { }

    private void StatsSetValueNames() {
        if (_IPstatsValues.Count == 0) {
            _IPstatsValues.Add("Interfaces", "dwNumIf");
            _IPstatsValues.Add("IP Addresses", "dwNumAddr");
            _IPstatsValues.Add("IP Forwarding", "dwForwarding");
            _IPstatsValues.Add("Routes", "dwNumRoutes");
            _IPstatsValues.Add("Default TTL", "dwDefaultTTL");
            _IPstatsValues.Add("Datagrams Received", "dwReasmTimeout");
            _IPstatsValues.Add("Datagrams Forwarded", "dwInAddrErrors");
            _IPstatsValues.Add("Header Errors", "dwInReceives");
            _IPstatsValues.Add("Address Errors", "dwInHdrErrors");
            _IPstatsValues.Add("Unknown Protocols Receive", "dwForwDatagrams");
            _IPstatsValues.Add("Received Datagrams Discarded", "dwInUnknownProtos");
            _IPstatsValues.Add("Received Datagrams Delivered", "dwInDiscards");
            _IPstatsValues.Add("Output Requests", "dwInDelivers");
            _IPstatsValues.Add("Routing Discards", "dwOutRequests");
            _IPstatsValues.Add("Output Packets Discarded", "dwRoutingDiscards");
            _IPstatsValues.Add("Output Packet No Route", "dwOutDiscards");
            _IPstatsValues.Add("Reassembly Time-out", "dwOutNoRoutes");
            _IPstatsValues.Add("Reassembly Required", "dwReasmReqds");
            _IPstatsValues.Add("Reassembly Success", "dwReasmOks");
            _IPstatsValues.Add("Reassembly Failures", "dwReasmFails");
            _IPstatsValues.Add("Datagrams Successfully Fragmented", "dwFragOks");
            _IPstatsValues.Add("Datagrams Failing Fragmentation", "dwFragFails");
            _IPstatsValues.Add("Frags Created", "dwFragCreates");
        }
        if (_TCPstatsValues.Count == 0) {
            _TCPstatsValues.Add("Active Opens", "dwActiveOpens");
            _TCPstatsValues.Add("Passive Opens", "dwPassiveOpens");
            _TCPstatsValues.Add("Current Connection Count", "dwCurrEstab");
            _TCPstatsValues.Add("Failed Connection Attempts", "dwAttemptFails");
            _TCPstatsValues.Add("Established Connection Reset", "dwEstabResets");
            _TCPstatsValues.Add("Cumulative Connections", "dwNumConns");
            _TCPstatsValues.Add("Maximum Connections", "dwMaxConn");
            _TCPstatsValues.Add("Segments Received", "dwInSegs");
            _TCPstatsValues.Add("Segments Sent", "dwOutSegs");
            _TCPstatsValues.Add("Segments Retransmitte", "dwRetransSegs");
            _TCPstatsValues.Add("Segments Sent With Reset Flag", "dwOutRsts");
            _TCPstatsValues.Add("Time-out Algorithm", "dwRtoAlgorithm");
            _TCPstatsValues.Add("Min Retransmission Time-out", "dwRtoMin");
            _TCPstatsValues.Add("Max Retransmission Time-out", "dwRtoMax");
            _TCPstatsValues.Add("Errors", "dwInErrs");
        }
        if (_UDPstatsValues.Count == 0) {
            _UDPstatsValues.Add("Datagrams Received", "dwInDatagrams");
            _UDPstatsValues.Add("Datagrams Sent", "dwOutDatagrams");
            _UDPstatsValues.Add("Receive Error", "dwInErrors");
            _UDPstatsValues.Add("No Ports", "dwNoPorts");
            _UDPstatsValues.Add("Entries In UDP Listener Table", "dwNumAddrs");
        }
        if (_ICMPstatsValues.Count == 0) {
            _ICMPstatsValues.Add("Messages", "dwMsgs");
            _ICMPstatsValues.Add("Errors", "dwErrors");
            _ICMPstatsValues.Add("Destination Unreachable", "dwDestUnreachs");
            _ICMPstatsValues.Add("Time Exceeded", "dwTimeExcds");
            _ICMPstatsValues.Add("Parameter Problems", "dwParmProbs");
            _ICMPstatsValues.Add("Source Quenches", "dwSrcQuenchs");
            _ICMPstatsValues.Add("Redirects", "dwRedirects");
            _ICMPstatsValues.Add("Echos", "dwEchos");
            _ICMPstatsValues.Add("Echo Replies", "dwEchoReps");
            _ICMPstatsValues.Add("Timestamps", "dwTimestamps");
            _ICMPstatsValues.Add("Timestamp Replies", "dwTimestampReps");
            _ICMPstatsValues.Add("Address Masks", "dwAddrMasks");
            _ICMPstatsValues.Add("Address Mask Replies", "dwAddrMaskReps");
        }
    }
    private void UpdateIPstats() {
        try {
            ListViewItem? i;
            API.MIB_IPSTATS s4 = new();
            API.MIB_IPSTATS s6 = new();
            TaskManagerNetwork.GetIPStatistics(ref s4, ref s6);
            if (_IPstatsValues.Count == 0) StatsSetValueNames();
            // Set Initial Columns
            if (lvIP.Columns.Count == 0) {
                lvIP.Columns.Add("IP Parameter", 250);
                lvIP.Columns.Add("IPv4 Value", 100, HorizontalAlignment.Right);
                lvIP.Columns.Add("IPv6 Value", 100, HorizontalAlignment.Right);
            }
            // Add or Update all Values
            lvIP.SuspendLayout();
            foreach (KeyValuePair<string, string> e in _IPstatsValues) {
                if (lvIP.Items.ContainsKey(e.Value)) {
                    i = lvIP.Items[e.Value];
                } else {
                    i = new ListViewItem { Name = e.Value, Text = e.Key };
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    lvIP.Items.Add(i);
                }
                string newValue1 = s4.GetType().GetField(e.Value)!.GetValue(s4)!.ToString()!;
                string newValue2 = s6.GetType().GetField(e.Value)!.GetValue(s6)!.ToString()!;
                if (i.SubItems[1].Text.Equals(newValue1) && i.SubItems[2].Text.Equals(newValue2)) {
                    if (i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                } else {
                    if (!Settings.Highlights.ChangingItems && i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                    if (Settings.Highlights.ChangingItems && !i.SubItems[1].Text.Equals("n/a.") && !i.SubItems[2].Text.Equals("n/a.")) i.BackColor = Settings.Highlights.ChangingColor;
                    i.SubItems[1].Text = newValue1;
                    i.SubItems[2].Text = newValue2;
                }
            }
        } catch (Exception ex) { Shared.DebugTrap(ex, 121); } finally { lvIP.ResumeLayout(); }
    }
    private void UpdateTCPstats() {
        try {
            ListViewItem? i;
            API.MIB_TCPSTATS t4 = new();
            API.MIB_TCPSTATS t6 = new();
            TaskManagerNetwork.GetTCPStatistics(ref t4, ref t6);
            if (_TCPstatsValues.Count == 0) StatsSetValueNames();
            // Set Initial Columns
            if (lvTCP.Columns.Count == 0) {
                lvTCP.Columns.Add("TCP Parameter", 250);
                lvTCP.Columns.Add("IPv4 Value", 100, HorizontalAlignment.Right);
                lvTCP.Columns.Add("IPv6 Value", 100, HorizontalAlignment.Right);
            }
            lvTCP.SuspendLayout();
            // Add or Update TCP Values
            foreach (KeyValuePair<string, string> e in _TCPstatsValues) {
                if (lvTCP.Items.ContainsKey(e.Value)) {
                    i = lvTCP.Items[e.Value];
                } else {
                    i = new ListViewItem { Name = e.Value, Text = e.Key };
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    lvTCP.Items.Add(i);
                }
                string newValue1 = t4.GetType().GetField(e.Value)!.GetValue(t4)!.ToString()!;
                string newValue2 = t6.GetType().GetField(e.Value)!.GetValue(t6)!.ToString()!;
                if (i.SubItems[1].Text.Equals(newValue1) && i.SubItems[2].Text.Equals(newValue2)) {
                    if (i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                } else {
                    if (!Settings.Highlights.ChangingItems && i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                    if (Settings.Highlights.ChangingItems && !i.SubItems[1].Text.Equals("n/a.") && !i.SubItems[2].Text.Equals("n/a.")) i.BackColor = Settings.Highlights.ChangingColor;
                    i.SubItems[1].Text = newValue1;
                    i.SubItems[2].Text = newValue2;
                }
            }
        } catch (Exception ex) { Shared.DebugTrap(ex, 122); } finally { lvTCP.ResumeLayout(); }
    }
    private void UpdateUDPstats() {
        try {
            ListViewItem? i;
            API.MIB_UDPSTATS u4 = new();
            API.MIB_UDPSTATS u6 = new();
            TaskManagerNetwork.GetUDPStatistics(ref u4, ref u6);
            if (_UDPstatsValues.Count == 0) StatsSetValueNames();
            // Set Initial Columns
            if (lvUDP.Columns.Count == 0) {
                lvUDP.Columns.Add("UDP Parameter", 250);
                lvUDP.Columns.Add("IPv4 Value", 100, HorizontalAlignment.Right);
                lvUDP.Columns.Add("IPv6 Value", 100, HorizontalAlignment.Right);
            }
            lvUDP.SuspendLayout();
            // Add or Update UDP Values
            foreach (KeyValuePair<string, string> e in _UDPstatsValues) {
                if (lvUDP.Items.ContainsKey(e.Value)) {
                    i = lvUDP.Items[e.Value];
                } else {
                    i = new ListViewItem { Name = e.Value, Text = e.Key };
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    lvUDP.Items.Add(i);
                }
                string newValue1 = u4.GetType().GetField(e.Value)!.GetValue(u4)!.ToString()!;
                string newValue2 = u6.GetType().GetField(e.Value)!.GetValue(u6)!.ToString()!;
                if (i.SubItems[1].Text.Equals(newValue1) && i.SubItems[2].Text.Equals(newValue2)) {
                    if (i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                } else {
                    if (!Settings.Highlights.ChangingItems && i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                    if (Settings.Highlights.ChangingItems && !i.SubItems[1].Text.Equals("n/a.") && !i.SubItems[2].Text.Equals("n/a.")) i.BackColor = Settings.Highlights.ChangingColor;
                    i.SubItems[1].Text = newValue1;
                    i.SubItems[2].Text = newValue2;
                }
            }
        } catch (Exception ex) { Shared.DebugTrap(ex, 123); } finally { lvUDP.ResumeLayout(); }
    }
    private void UpdateICMPstats() {
        try {
            ListViewItem? i;
            API.MIB_ICMPINFO s4 = new();
            TaskManagerNetwork.GetICMPv4Statistics(ref s4);
            if (_ICMPstatsValues.Count == 0) StatsSetValueNames();
            // Set Initial Columns
            if (lvICMP.Columns.Count == 0) {
                lvICMP.Columns.Add("ICMP Parameter", 250);
                lvICMP.Columns.Add("Received", 100, HorizontalAlignment.Right);
                lvICMP.Columns.Add("Sent", 100, HorizontalAlignment.Right);
            }
            lvICMP.SuspendLayout();
            // Add or Update all Values
            foreach (KeyValuePair<string, string> e in _ICMPstatsValues) {
                if (lvICMP.Items.ContainsKey(e.Value)) {
                    i = lvICMP.Items[e.Value];
                } else {
                    i = new ListViewItem { Name = e.Value, Text = e.Key };
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    i.SubItems.Add("n/a.");
                    lvICMP.Items.Add(i);
                }
                string newValue1 = s4.icmpInStats.GetType().GetField(e.Value)!.GetValue(s4.icmpInStats)!.ToString()!;
                string newValue2 = s4.icmpOutStats.GetType().GetField(e.Value)!.GetValue(s4.icmpOutStats)!.ToString()!;
                if (i.SubItems[1].Text.Equals(newValue1) && i.SubItems[2].Text.Equals(newValue2)) {
                    if (i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                } else {
                    if (!Settings.Highlights.ChangingItems && i.BackColor == Settings.Highlights.ChangingColor) i.BackColor = Color.Empty;
                    if (Settings.Highlights.ChangingItems && !i.SubItems[1].Text.Equals("n/a.") && !i.SubItems[2].Text.Equals("n/a.")) i.BackColor = Settings.Highlights.ChangingColor;
                    i.SubItems[1].Text = newValue1;
                    i.SubItems[2].Text = newValue2;
                }
            }
        } catch (Exception ex) { Shared.DebugTrap(ex, 124); } finally { lvICMP.ResumeLayout(); }
    }

}
