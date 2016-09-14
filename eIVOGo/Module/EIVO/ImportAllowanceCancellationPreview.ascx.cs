using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using eIVOGo.Module.Common;
using eIVOGo.Helper;

namespace eIVOGo.Module.EIVO
{
    public partial class ImportAllowanceCancellationPreview : ImportInvoicePreview
    {
        protected override void btnAddCode_Click(object sender, EventArgs e)
        {
            try
            {
                //檢核成功寫入資料庫
                if (_mgr is GoogleAllowanceCancellationUploadManager && _mgr.Save())
                {
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