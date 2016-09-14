using System;
using System.Linq;
using System.Collections.Generic;

using Business.Helper;
using eIVOGo.Helper;
using Model.InvoiceManagement;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using eIVOGo.Module.Common;
using Model.DataEntity;

namespace eIVOGo.Module.EIVO
{
    public partial class ImportInvoiceCancellationPreview : ImportInvoicePreview
    {

        protected override void btnAddCode_Click(object sender, EventArgs e)
        {
            try
            {
                //檢核成功寫入資料庫
                if (_mgr is GoogleInvoiceCancellationUploadManager && _mgr.Save())
                {
                    //((GoogleInvoiceCancellationUploadManager)_mgr).ItemList.Where(i => i.Invoice.InvoiceBuyer.IsB2C()).Select(i => i.Invoice.InvoiceID).SendGoogleInvoiceCancellationMail();
                    ((GoogleInvoiceCancellationUploadManager)_mgr).ItemList.Select(i => i.Invoice.InvoiceID).SendGoogleInvoiceCancellationMail();
                    Response.Redirect(NextAction.RedirectTo);
                }
                else
                {
                    this.AjaxAlert("匯入失敗!!");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("匯入失敗，錯誤原因：" + ex.Message);
            }
        }
    }
}