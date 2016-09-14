using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using System.Drawing;

namespace eIVOGo.Module.EIVO
{
    public partial class ConfirmInvoiceCancelList : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            signContext.Launcher = btnOK;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
            this.PreRender += new EventHandler(ConfirmInvoiceCancelList_PreRender);
        }

        void ConfirmInvoiceCancelList_PreRender(object sender, EventArgs e)
        {
            prepareData();
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            signContext.DataToSign = String.Format("{0}\r\n作廢原因:{1}", dataToSign.Text, reason.Text);
        }

        public int? InvoiceID
        {
            get
            {
                return (int?)ViewState["InvoiceID"];
            }
            set
            {
                ViewState["InvoiceID"] = value;
            }
        }

        private void prepareData()
        {
            if (InvoiceID.HasValue)
            {
                var item = dsEntity.CreateDataManager().EntityList.Where(a => a.InvoiceID == InvoiceID).FirstOrDefault();
                if (item != null)
                {
                    StringBuilder sb = new StringBuilder("您欲開立的發票作廢資料如下:\r\n");
                    sb.Append("作業時間:").Append(DateTime.Now.ToString()).Append("\r\n");
                    sb.Append("發票開立日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceDate)).Append("\r\n");
                    sb.Append("開立發票營業人名稱:").Append(item.Organization.CompanyName).Append("\r\n");
                    sb.Append("統一編號:").Append(item.Organization.ReceiptNo).Append("\r\n");
                    sb.Append("發票號碼:").Append(item.TrackCode).Append(item.No).Append("\r\n");
                    dataToSign.Text = sb.ToString();
                }
            }
        }

        public void Show()
        {
            this.ModalPopupExtender.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(reason.Text))
            {
                WebMessageBox.AjaxAlert(this, "請填入作廢原因!!");
            }
            else if (signContext.Verify())
            {
                save();
                //ModalPopupExtender.Hide();
                this.AjaxAlert("發票作廢開立完成!!");
                if (Done != null)
                {
                    Done(this, new EventArgs());
                }
            }
            else
            {
                WebMessageBox.AjaxAlert(this, "驗簽失敗!!");
            }
        }

        private void save()
        {
            var mgr = dsEntity.CreateDataManager();
            mgr.GetTable<InvoiceCancellation>().InsertOnSubmit(new InvoiceCancellation
            {
                InvoiceID = this.InvoiceID.Value,
                CancelDate = DateTime.Now,
                Remark = reason.Text
            });
            mgr.SubmitChanges();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender.Hide();
        }
    }
}