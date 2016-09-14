using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Helper;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.EIVO
{
    public partial class ImportInvoiceFile : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        private String _encodingName = "Big5";
        protected String _prefix = "Invoice";

        protected void Page_Load(object sender, EventArgs e)
        {
            
            _userProfile = WebPageUtility.UserProfile;
            GoogleInvoiceUploadManager mgr = _userProfile["importMgr"] as GoogleInvoiceUploadManager;
            if (mgr != null)
                mgr.Dispose();

            //FileUpload.Attributes.Add
            //    ("onchange", "return validateFileUpload(this);");
            var modal = btnConfirm.AttachWaitingMessage("正在處理中，請稍後...", false);
            btnConfirm.Attributes.Add
                ("onclick",
                    String.Format("if(validateFileUpload(document.getElementById('{0}'))) {{ {1} return true;}} return false;",
                        FileUpload.ClientID, modal.GetClientTriggerScript()));
        }

        [Bindable(true)]
        public String EncodingName
        {
            get
            {
                return _encodingName;
            }
            set
            {
                _encodingName = value;
            }
        }

        [Bindable(true)]
        public String Prefix
        {
            get
            {
                return _prefix;
            }
            set
            {
                _prefix = value;
            }
        }


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                String fileName = Path.Combine(Logger.LogDailyPath, String.Format("{0}({1:yyyyMMddHHmmssfff}).csv", _prefix, DateTime.Now));
                FileUpload.SaveAs(fileName);

                IGoogleUploadManager mgr = createUploadManager();
                if (mgr != null)
                {
                    mgr.ParseData(_userProfile, fileName, Encoding.GetEncoding(_encodingName));

                    //if (!mgr.IsValid)                                                   //0.讀CVS檔
                    //{
                    //    this.AjaxAlert("讀取失敗,CVS檔案格式錯誤!!");
                    //    return;
                    //}

                    _userProfile["importMgr"] = mgr;
                    Response.Redirect(NextAction.RedirectTo);
                }
            }
        }

        protected virtual IGoogleUploadManager createUploadManager()
        {
            return new GoogleInvoiceUploadManager();
        }
    }
}