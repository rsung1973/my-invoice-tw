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
using Services;
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
                if ((profile = LogonSvc.GetUserProfile(Model.UIType.WebUI)) == null)
                {
                    HttpContext context = HttpContext.Current;
                    context.Response.Redirect(FormsAuthentication.LoginUrl);
                }
                return profile;
            }
        }

        public static void Logout()
        {
            LogonSvc.UserLogout();
            HttpContext context = HttpContext.Current;
            context.Response.Redirect(FormsAuthentication.LoginUrl);
        }

        public static bool Logon
        {
            get
            {
                return LogonSvc.GetUserProfile(Model.UIType.WebUI) != null;
            }
        }


    }
}
