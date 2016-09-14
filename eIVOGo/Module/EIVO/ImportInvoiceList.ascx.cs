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
using System.ComponentModel;
using eIVOGo.Module.EIVO.Export;

namespace eIVOGo.Module.EIVO
{
    public partial class ImportInvoiceList : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        protected IGoogleUploadManager _mgr;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            _mgr = _userProfile["importMgr"] as IGoogleUploadManager;
            if (_mgr == null)
            {
                Response.Redirect(NextAction.RedirectTo);
            }

            btnPrint.PrintControls.Add(uploadList);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnPrint.BeforeClick += new EventHandler(btnPrint_BeforeClick);
        }

        void btnPrint_BeforeClick(object sender, EventArgs e)
        {
            uploadList.AllowPaging = false;
        }

    }
}