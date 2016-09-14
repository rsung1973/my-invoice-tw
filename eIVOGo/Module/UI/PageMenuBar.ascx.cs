using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Business.Helper;
using Model.Resource;
using Model.Security.MembershipManagement;

namespace eIVOGo.Module.UI
{
    public partial class PageMenuBar : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (String.IsNullOrEmpty(_userProfile.CurrentSiteMenu))
            {
                WebPageUtility.Logout();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            TreeMenu1.Logout += new EventHandler(menuBar_Logout);
            TreeMenu1.DataPathBound += new MenuEventHandler(menuBar_DataPathBound);
            this.PreRender += new EventHandler(PageMenuBar_PreRender);
        }

        void PageMenuBar_PreRender(object sender, EventArgs e)
        {
            TreeMenu1.AutoAddWorkItem = true;
            TreeMenu1.BindData(_userProfile.CurrentSiteMenu, _userProfile.MenuDataPath);
        }

        void menuBar_DataPathBound(object sender, MenuEventArgs  e)
        {
            _userProfile.MenuDataPath = TreeMenu1.MenuDataPath;
        }

        void menuBar_Logout(object sender, EventArgs e)
        {
            WebPageUtility.Logout();
        }
    }
}