using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InvoiceClient.Helper;
using InvoiceClient.Properties;
using InvoiceClient.TransferManagement;
using System.ServiceProcess;
using InvoiceClient.Agent;

namespace InvoiceClient.MainContent
{
    public partial class GoogleInvoiceServerConfigForDoubleClick : UserControl , ITabWorkItem
    {
        private ServerInspector _inspector;

        public GoogleInvoiceServerConfigForDoubleClick()
        {
            InitializeComponent();
            _inspector = InvoiceClientTransferManager.GetServerInspector(typeof(InvoicePDFInspectorForDoubleClick));
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            Program.Install(false, new String[0]);
            refreshServiceConfig();
        }

        private void btnUninstall_Click(object sender, EventArgs e)
        {
            Program.Install(true, new String[0]);
            refreshServiceConfig();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            InvoiceClientTransferManager.ServiceInstance.Start();
            btnRun.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            InvoiceClientTransferManager.ServiceInstance.Stop();
            btnRun.Enabled = true;
            btnStop.Enabled = false;
        }

        private void cbAutoInvService_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["IsAutoInvService"] = cbAutoInvService.Checked;
            _inspector.StartUp();
            MessageBox.Show("Action OK!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnAutoInvService_Click(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(AutoInvServiceInterval.Text, out interval))
            {
                Settings.Default["AutoInvServiceInterval"] = interval;
                _inspector.StartUp();
                MessageBox.Show("Action OK!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Interval must be an integer!!", "Retrieve GUI PDF", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                AutoInvServiceInterval.Focus();
            }

        }

        private void btnInvService_Click(object sender, EventArgs e)
        {
            List<String> pathInfo = new List<string>();
            _inspector.ExecutiveService(pathInfo);
            if (pathInfo.Count > 0)
            {
                if (MessageBox.Show("New GUI PDF arrivals!\r\nOpen Explorer to inspect?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    foreach (var item in pathInfo)
                    {
                        Win32.Shell.ShellExecute(IntPtr.Zero, "explore", item, null, null, Win32.User.SW_SHOW);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nothing available!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public string TabName
        {
            get { return "ServiceConfigTab"; }
        }

        public string TabText
        {
            get { return "Service"; }
        }

        private void InvoiceServerConfig_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                refreshServiceConfig();
                cbAutoInvService.Checked = Settings.Default.IsAutoInvService;
                AutoInvServiceInterval.Text = Settings.Default.AutoInvServiceInterval.ToString();
            }
        }

        private void refreshServiceConfig()
        {
            InvoiceClientTransferManager.ResetServiceController();
            btnInstall.Enabled = InvoiceClientTransferManager.ServiceInstance == null;
            btnUninstall.Enabled = !btnInstall.Enabled;

            if (InvoiceClientTransferManager.ServiceInstance != null)
            {
                btnStop.Enabled = InvoiceClientTransferManager.ServiceInstance.Status == ServiceControllerStatus.Running;
                btnRun.Enabled = !btnStop.Enabled;
            }
            else
            {
                btnStop.Enabled = false;
                btnRun.Enabled = false;
            }
        }
    }
}
