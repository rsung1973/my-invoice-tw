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
    public partial class ImportInvoicePreview : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;
        protected IGoogleUploadManager _mgr;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            _mgr = _userProfile["importMgr"] as IGoogleUploadManager;
            if (_mgr == null)
            {
                Response.Redirect(PrevAction.RedirectTo);
            }



            var modal = btnAddCode.AttachWaitingMessage("正在處理中，請稍後...", false);
            btnAddCode.Attributes.Add
                ("onclick",
                    String.Format("if(confirm('確定匯入共{0}筆資料?')) {{ {1} return true;}} return false;",
                        _mgr.ItemCount, modal.GetClientTriggerScript()));


        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(ImportInvoicePreview_PreRender);
        }

        void ImportInvoicePreview_PreRender(object sender, EventArgs e)
        {
            btnAddCode.Visible = _mgr.IsValid;
        }


        protected virtual void btnAddCode_Click(object sender, EventArgs e)
        {
            try
            {
                //檢核成功寫入資料庫
                if (_mgr is GoogleInvoiceUploadManager && _mgr.Save())
                {
                   ((GoogleInvoiceUploadManager)_mgr).SendInvoiceMail();
                    Response.Redirect(NextAction.RedirectTo);
                }
                else
                { 
                     this.AjaxAlert("匯入失敗!!");
               }
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("匯入失敗，錯誤原因：" + ex.Message);
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(PrevAction.RedirectTo);
        }
    }
}