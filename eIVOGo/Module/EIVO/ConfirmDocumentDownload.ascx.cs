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
using Model.Locale;

namespace eIVOGo.Module.EIVO
{
    public partial class ConfirmDocumentDownload : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;
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
            this.PreRender += new EventHandler(ConfirmDocumentDownload_PreRender);
        }

        void ConfirmDocumentDownload_PreRender(object sender, EventArgs e)
        {
            prepareData();
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            signContext.DataToSign = dataToSign.Text;
        }


        public int? DocID
        {
            get
            {
                return (int?)ViewState["invoiceID"];
            }
            set
            {
                ViewState["invoiceID"] = value;
            }
        }

        protected virtual void prepareData()
        {
            if (DocID.HasValue)
            {
                var item = dsEntity.CreateDataManager().GetTable<CDS_Document>()
                    .Where(a => a.DocID == DocID).FirstOrDefault();
                if (item != null)
                {
                    StringBuilder sb = new StringBuilder("您欲授權發票資料可再下載成大平台傳輸格式:\r\n");
                    sb.Append("作業時間:").Append(DateTime.Now.ToString()).Append("\r\n");
                    switch ((Naming.DocumentTypeDefinition)item.DocType)
                    {
                        case Naming.DocumentTypeDefinition.E_Invoice:
                            sb.Append("發票號碼:").Append(item.InvoiceItem.TrackCode).Append(item.InvoiceItem.No).Append("\r\n");
                            break;
                        case Naming.DocumentTypeDefinition.E_Allowance:
                            sb.Append("折讓單號碼:").Append(item.InvoiceAllowance.AllowanceNumber).Append("\r\n");
                            break;
                        case Naming.DocumentTypeDefinition.E_InvoiceCancellation:
                            sb.Append("作廢發票號碼:").Append(item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceCancellation.CancellationNo).Append("\r\n");
                            break;
                        case Naming.DocumentTypeDefinition.E_AllowanceCancellation:
                            sb.Append("作廢折讓單號碼:").Append(item.DerivedDocument.ParentDocument.InvoiceAllowance.AllowanceNumber).Append("\r\n");
                            break;
                    }

                    dataToSign.Text = sb.ToString();
                }
            }
        }

        public void Show()
        {
            signContext.RegisterAutoSign();
            this.ModalPopupExtender.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (signContext.Verify())
            {
                save();
                //ModalPopupExtender.Hide();
                this.AjaxAlert("重設資料完成!!");
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

        protected virtual void save()
        {
            var mgr = dsEntity.CreateDataManager();
            var item = dsEntity.CreateDataManager().GetTable<CDS_Document>().Where(a => a.DocID == DocID).FirstOrDefault();
            if (item != null)
                item.ResetDocumentDispatch((Naming.DocumentTypeDefinition)item.DocType);
            mgr.SubmitChanges();
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender.Hide();
        }
    }
}