using System.ComponentModel;
using System.Runtime.Versioning;
using System.ServiceProcess;

namespace sMkTaskManager.Forms;

[DesignerCategory("Component"), SupportedOSPlatform("windows")]
public partial class frmService_Details : Form {
    private Classes.TaskManagerService? s;

    public frmService_Details() {
        InitializeComponent();
    }

    public string SID {
        get {
            return s == null ? "" : s.Ident;
        }
        set {
            if (!string.IsNullOrEmpty(value.Trim()) && value != SID) {
                s = null;
                foreach (ServiceController serv in ServiceController.GetServices()) {
                    if (value == serv.ServiceName) {
                        s = new Classes.TaskManagerService(value);
                        s.Load(serv);
                        break;
                    }
                }
                LoadServiceDetails();
                LoadServiceDependencies();
            }
        }
    }

    private void LoadServiceDetails() { }
    private void LoadServiceDependencies() { }

}
