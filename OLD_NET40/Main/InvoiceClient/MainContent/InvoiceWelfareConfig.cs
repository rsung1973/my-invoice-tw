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
using Model.Schema.EIVO;
using InvoiceClient.Agent;

namespace InvoiceClient.MainContent
{
    public partial class InvoiceWelfareConfig : UserControl , ITabWorkItem
    {
        private InvoiceWelfareInspector _inspector;

        public InvoiceWelfareConfig()
        {
            InitializeComponent();
            _inspector = (InvoiceWelfareInspector)InvoiceClientTransferManager.GetServerInspector(typeof(InvoiceWelfareInspector));
        }

        public string TabName
        {
            get { return "WelfareTab"; }
        }

        public string TabText
        {
            get { return "社福機構"; }
        }

        private void btnAutoWelfare_Click(object sender, EventArgs e)
        {
            int interval;
            if (int.TryParse(AutoWelfareInterval.Text, out interval))
            {
                Settings.Default["AutoWelfareInterval"] = interval;
                _inspector.StartUp();
                MessageBox.Show("設定完成!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("請輸入整數的間隔數值!!", "自動更新社福機購資料", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                AutoWelfareInterval.Focus();
            }
        }

        private void btnUpdateWelfare_Click(object sender, EventArgs e)
        {
            SocialWelfareAgenciesRoot welfareItem = _inspector.GetUpdatedData();
            if (welfareItem != null)
            {
                WelfareInfo.Text = String.Join("//--------------------------------\r\n", welfareItem.SocialWelfareAgencies
                    .Select(a => String.Format("機構代碼：{0}\r\n統一編號：{1}\r\n機構名稱：{2}\r\n地　　址：{3}\r\n連絡電話：{4}\r\n電子郵件：{5}\r\n",
                        a.Code, a.Ban, a.Name, a.Address, a.TEL, a.Email)).ToArray());
                MessageBox.Show("更新完成!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                WelfareInfo.Text = String.Empty;
                MessageBox.Show("尚未發現新的社福機構資料!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbAutoWelfare_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default["IsAutoWelfare"] = cbAutoWelfare.Checked;
            if (cbAutoWelfare.Checked)
            {
                _inspector.StartUp();
                MessageBox.Show("設定完成!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnGetWelfareAgency_Click(object sender, EventArgs e)
        {
            SocialWelfareAgenciesRoot welfareItem = _inspector.GetAll();
            if (welfareItem != null)
            {
                WelfareInfo.Text = String.Join("//--------------------------------\r\n", welfareItem.SocialWelfareAgencies
                    .Select(a => String.Format("機構代碼：{0}\r\n統一編號：{1}\r\n機構名稱：{2}\r\n地　　址：{3}\r\n連絡電話：{4}\r\n電子郵件：{5}\r\n",
                        a.Code, a.Ban, a.Name, a.Address, a.TEL, a.Email)).ToArray());
                MessageBox.Show("取得社福機構資料完成!!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                WelfareInfo.Text = String.Empty;
                MessageBox.Show("尚未指定社福機構資料!!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void InvoiceWelfareConfig_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                cbAutoWelfare.Checked = Settings.Default.IsAutoWelfare;
                AutoWelfareInterval.Text = Settings.Default.AutoWelfareInterval.ToString();
            }
        }
    }
}
