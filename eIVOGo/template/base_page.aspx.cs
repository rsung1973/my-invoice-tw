using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using System.Web.Mvc;

namespace eIVOGo.template
{
    public partial class base_page : ViewPage
    {
        public const String PAGE_ALERT_ITEM_KEY = "pageAlert";
        private UserProfileMember _userProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (_userProfile["msg"] != null)
            {
                this.AjaxAlert((String)_userProfile["msg"]);
                _userProfile["msg"] = null;
            }
            else if (Page.PreviousPage != null && Page.PreviousPage.Items[PAGE_ALERT_ITEM_KEY] != null)
            {
                this.AjaxAlert((String)Page.PreviousPage.Items[PAGE_ALERT_ITEM_KEY]);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.Error += new EventHandler(base_page_Error);
        }

        //void base_page_Error(object sender, EventArgs e)
        //{
        //    Exception ex = Server.GetLastError();
        //    if (ex != null)
        //    {
        //        Logger.Error(ex);
        //    }
        //}


    }
}