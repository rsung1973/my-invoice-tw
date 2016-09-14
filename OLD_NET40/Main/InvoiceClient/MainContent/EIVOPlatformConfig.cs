using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InvoiceClient.Agent;
using InvoiceClient.Properties;

namespace InvoiceClient.MainContent
{
    public partial class EIVOPlatformConfig : UserControl
    {
        public EIVOPlatformConfig()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            btnSend.Enabled  = false;
            btnPause.Enabled = true;
            InvoiceClientTransferManager.StartUp(Settings.Default.InvoiceTxnPath);
            JobStatus.Text = "系統狀態：執行中...";
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            btnSend.Enabled = true;
            btnPause.Enabled = false;
            InvoiceCenterTransferManager.PauseAll();
            JobStatus.Text = "系統狀態：已停止...";
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            InvoiceCenterTransferManager.SetRetry();
            FailedInvoiceInfo.Text = String.Empty;
            btnRetry.Enabled = false;
            MessageBox.Show("已重新執行!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshContent();
        }

        public void RefreshContent()
        {
            FailedInvoiceInfo.Text = InvoiceCenterTransferManager.ReportError();
            btnRetry.Enabled = !String.IsNullOrEmpty(FailedInvoiceInfo.Text);
        }

        private void InvoiceCenterConfig_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                RefreshContent();
        }
    }
}
