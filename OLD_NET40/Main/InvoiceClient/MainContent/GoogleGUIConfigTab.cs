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

namespace InvoiceClient.MainContent
{
    public partial class GoogleGUIConfigTab : UserControl, ITabWorkItem
    {
        public GoogleGUIConfigTab()
        {
            InitializeComponent();
        }

        private void btnReceiptNo_Click(object sender, EventArgs e)
        {
            Settings.Default["SellerReceiptNo"] = SellerReceiptNo.Text;
            MessageBox.Show("Action OK!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnServerUrl_Click(object sender, EventArgs e)
        {
            Settings.Default["InvoiceClient_WS_Invoice_eInvoiceService"] = ServerUrl.Text;
            MessageBox.Show("Action OK!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public string TabName
        {
            get { return "SystemConfigTab"; }
        }

        public string TabText
        {
            get { return "Configuration"; }
        }

        private void SystemConfigTab_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ServerUrl.Text = Settings.Default.InvoiceClient_WS_Invoice_eInvoiceService;
                InvoiceTxnPath.Text = Settings.Default.InvoiceTxnPath;
                SellerReceiptNo.Text = Settings.Default.SellerReceiptNo;
            }

        }

        private void btnTxnPath_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Settings.Default.InvoiceTxnPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default["InvoiceTxnPath"] = dialog.SelectedPath;
                    MessageBox.Show("Action OK!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
