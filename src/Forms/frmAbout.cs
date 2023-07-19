using System.ComponentModel;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.Versioning;
namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmAbout : Form {
    private int m_AnimationIndex = 1;
    private Image m_AnimationImage;
    private int m_AnimationPercent = 0;
    private Rectangle m_Rectangle;
    private readonly ImageAttributes m_AnimationAttr = new();
    private readonly int m_RectOffset = 7;

    public frmAbout() {
        InitializeComponent();
    }

    private void frmAbout_Load(object sender, EventArgs e) {
        ilAnimation.Images.Clear();
        ilAnimation.Images.Add(Resources.Resources.AnimIcon_Cyan);
        ilAnimation.Images.Add(Resources.Resources.AnimIcon_Red);
        /* Set Version Numbers */
        lblVersion.Text = ProductName;
        Version version = Assembly.GetExecutingAssembly().GetName().Version!;
        lblVersion.Text = "v" + version?.Major + "." + version?.Minor + "." + version?.Build;
        lblBuild.Text = lblBuild.Text.Replace("xxxx1", version?.Revision.ToString());
        /* Set Build Time, if possible */
        try {
            long buildTime = long.Parse(ApplicationInfo.BuildMark);
            lblBuild.Text = lblBuild.Text.Replace("xxxx2", new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(buildTime).ToLocalTime().ToString());
        } catch { lblBuild.Text = lblBuild.Text.Replace("xxxx2", "n/a."); };
        /* Set the Info Line */
        string infoLine = "", config = ApplicationInfo.Configuration;
        if (config != "Release") { infoLine += Environment.NewLine + ApplicationInfo.Configuration; }
        infoLine += Environment.NewLine + (Environment.Is64BitProcess ? "64bit" : "32bit");
        lblInfo.Text = infoLine.Trim();
        /* Start the Animation */
        try {
            m_AnimationImage = ilAnimation.Images[1];
            pbIcon.Image = ilAnimation.Images[0];
            m_Rectangle = pbIcon.ClientRectangle;
            m_Rectangle.X = m_RectOffset;
            m_Rectangle.Y = m_RectOffset;
            m_Rectangle.Height -= (m_RectOffset * 2);
            m_Rectangle.Width -= (m_RectOffset * 2);
            tmrIcon.Start();
        } catch { }

    }
    private void frmAbout_KeyDown(object sender, KeyEventArgs e) {
        if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter) { Close(); }
    }
    private void frmAbout_FormClosing(object sender, FormClosingEventArgs e) {
        tmrIcon.Stop();
    }
    private void pbIcon_Paint(object sender, PaintEventArgs e) {
        if (!tmrIcon.Enabled) return;
        ColorMatrix mx = new() { Matrix33 = 1f / 255f * (255f * m_AnimationPercent / 100f) };
        m_AnimationAttr.SetColorMatrix(mx);
        e.Graphics.DrawImage(m_AnimationImage, m_Rectangle, m_RectOffset, m_RectOffset, m_AnimationImage.Width - (m_RectOffset * 2), m_AnimationImage.Height - (m_RectOffset * 2), GraphicsUnit.Pixel, m_AnimationAttr);
    }
    private void tmrIcon_Tick(object sender, EventArgs e) {
        m_AnimationPercent += 2;
        if (m_AnimationPercent > 100) {
            pbIcon.Image = ilAnimation.Images[m_AnimationIndex];
            m_AnimationPercent = 0;
            m_AnimationIndex += 1;
            if (m_AnimationIndex > 1) m_AnimationIndex = 0;
            m_AnimationImage = ilAnimation.Images[m_AnimationIndex];
        }
        pbIcon.Invalidate();
    }

    private class ApplicationInfo {
        public static string Company { get { return GetExecutingAssemblyAttribute<AssemblyCompanyAttribute>(a => a.Company); } }
        public static string Product { get { return GetExecutingAssemblyAttribute<AssemblyProductAttribute>(a => a.Product); } }
        public static string Copyright { get { return GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright); } }
        public static string Trademark { get { return GetExecutingAssemblyAttribute<AssemblyTrademarkAttribute>(a => a.Trademark); } }
        public static string Title { get { return GetExecutingAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title); } }
        public static string Description { get { return GetExecutingAssemblyAttribute<AssemblyDescriptionAttribute>(a => a.Description); } }
        public static string Configuration { get { return GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration); } }
        public static string FileVersion { get { return GetExecutingAssemblyAttribute<AssemblyFileVersionAttribute>(a => a.Version); } }
        public static string BuildMark { get { return GetExecutingAssemblyAttribute<BuildMarkAttribute>(a => a.BuildMark); } }
        public static string GitCommit { get { return GetExecutingAssemblyAttribute<GitCommitAttribute>(a => a.GitCommit); } }

        public static Version Version { get { return Assembly.GetExecutingAssembly().GetName().Version!; } }
        public static string VersionFull { get { return Version.ToString(); } }
        public static string VersionMajor { get { return Version.Major.ToString(); } }
        public static string VersionMinor { get { return Version.Minor.ToString(); } }
        public static string VersionBuild { get { return Version.Build.ToString(); } }
        public static string VersionRevision { get { return Version.Revision.ToString(); } }

        private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T))!;
            return value.Invoke(attribute);
        }
    }
}
