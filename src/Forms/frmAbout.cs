using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using sMkTaskManager.Properties;
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
        ilAnimation.Images.Add(Resources.AnimIcon_Cyan);
        ilAnimation.Images.Add(Resources.AnimIcon_Red);
        Version AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
        lblVersion.Text = "v" + AssemblyVersion?.Major + "." + AssemblyVersion?.Minor + "." + AssemblyVersion?.Build;
        lblBuild.Text = lblBuild.Text.Replace("xxxx1", AssemblyVersion?.Revision.ToString());
        lblBuild.Text = lblBuild.Text.Replace("xxxx2", AssemblyBuildDate(AssemblyVersion!).ToString());

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

    private static DateTime AssemblyBuildDate(Version AssemblyVersion) {
        DateTime dt = DateTime.Now;
        try {
            dt = DateTime.Parse("01/01/2000").AddDays(AssemblyVersion.Build).AddSeconds(AssemblyVersion.Revision * 2);
            if (dt > DateTime.Now || AssemblyVersion.Build < 730 || AssemblyVersion.Revision == 0) {
                dt = File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
        } catch { }
        return dt;
    }


}
