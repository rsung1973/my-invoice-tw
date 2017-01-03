using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Model.Security.MembershipManagement;
using Model.Resource;
using Uxnet.Web.Module.Common;

namespace Business.Helper
{
    public class WebPageUtility : BaseWebPageUtility
    {
        public static UserProfileMember @UserProfile
        {
            get
            {
                UserProfileMember profile;
                if ((profile = HttpContext.Current.GetUser()) == null)
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
                return profile;
            }
        }

        public static void Logout()
        {
            HttpContext.Current.Logout();
            FormsAuthentication.RedirectToLoginPage();
        }

        public static bool Logon
        {
            get
            {
                return UserProfile != null;
            }
        }


    }
}
