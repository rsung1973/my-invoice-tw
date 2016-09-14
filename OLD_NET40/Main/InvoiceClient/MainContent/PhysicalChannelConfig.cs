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
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;

namespace InvoiceClient.MainContent
{
    public partial class PhysicalChannelConfig : UserControl,ITabWorkItem
    {
        private ITransferManager _transferMgr;

        public PhysicalChannelConfig()
        {
            InitializeComponent();
            _transferMgr = InvoiceClientTransferManager.GetTransferManager(typeof(PhysicalChannelTransferManager));
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
            _transferMgr.PauseAll();
            JobStatus.Text = "系統狀態：已停止...";
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            _transferMgr.SetRetry();
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
            FailedInvoiceInfo.Text = _transferMgr.ReportError();
            btnRetry.Enabled = !String.IsNullOrEmpty(FailedInvoiceInfo.Text);
        }

        private void PhysicalChannelConfig_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                RefreshContent();
        }

        public string TabName
        {
            get { return "PhysicalChannelTab"; }
        }

        public string TabText
        {
            get { return "實體通路電子發票"; }
        }
    }
}
