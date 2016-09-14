using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using eIVOGo.Helper;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using Business.Helper;
using System.Text;
using eIVOGo.Module.UI;

namespace eIVOGo.Module.Inquiry
{
    public partial class PrintCInvoiceItemList : InvoiceItemList
    {
        private UserProfileMember _userProfile;
        protected String _successfulMsg = "列印作業即將進行，請稍後...!!";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CancelInvoiceItemList_PreRender);
            this.Load += new EventHandler(CancelInvoiceItemList_Load);
        }

        void CancelInvoiceItemList_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        void CancelInvoiceItemList_PreRender(object sender, EventArgs e)
        {
            this.btnPrint.Visible = dsInv.CurrentView.LastSelectArguments.TotalRowCount > 0;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            String[] ar = Request.GetItemSelection();
            if (ar != null && ar.Count() > 0)
            {
                _userProfile.EnqueueDocumentPrint(dsInv.CreateDataManager(), ar.Select(a => int.Parse(a)));

                LiteralControl lc = new LiteralControl(String.Format("<iframe src='{0}?printBack={1}' height='0' width='0'></iframe>", VirtualPathUtility.ToAbsolute("~/SAM/PrintInvoiceAsPDF.aspx"), Request["printBack"]));
                this.Controls.Add(lc);

                PopupModal modal = (PopupModal)this.LoadControl("~/Module/UI/PopupModal.ascx");
                modal.InitializeAsUserControl(this.Page);
                modal.TitleName = _successfulMsg;
                _successfulMsg = null;
                this.Controls.Add(modal);
                modal.Show();

                gvEntity.DataBind();
            }
            else
            {
                this.AjaxAlert("請選擇列印資料!!");
            }
        }
    }    
}