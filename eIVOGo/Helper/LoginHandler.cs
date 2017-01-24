using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Business.Helper;
using Model.Locale;
using Model.Resource;
using Model.Security.MembershipManagement;

namespace eIVOGo.Helper
{
    public class LoginHandler
    {
        public LoginHandler()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string RedirectToAsLoginSuccessfully { get; set; }

        public bool ProcessLogin(string pid, string password, out string msg)
        {
             UserProfileMember up = UserProfileFactory.CreateInstance(pid, password);
             bool auth =  processLoginUsingRole(out msg, up);
             if (up != null)
             {
                if (up.Profile.UserProfileStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Wait_For_Check)
                {
                    up.CurrentSiteMenu = "WaitForCheckMenu.xml";
                    msg = VirtualPathUtility.ToAbsolute("~/UserProfile/EditMySelf?forCheck=True");
                }
             }
            return auth;
        }

        public bool ProcessLogin(string pid)
        {
            string msg;
            UserProfileMember up = UserProfileFactory.CreateInstance(pid);
            return processLoginUsingRole(out msg, up);
        }

        public void ReloadUserProfile(string pid)
        {
            UserProfileMember up = UserProfileFactory.CreateInstance(pid);
            HttpContext.Current.Session[WebKey.USER_PROFILE] = up;
        }

        public bool NormalLogin(string pid)
        {
            bool bAuth = false;

            UserProfileMember up = UserProfileFactory.CreateInstance(pid);
            if (up != null)	//new login
            {
                HttpContext.Current.Session.Add(WebKey.USER_PROFILE, up);
                bAuth = true;
            }

            if (bAuth)
            {
                string url = FormsAuthentication.GetRedirectUrl(pid, false);
                FormsAuthentication.SetAuthCookie(pid, false);

                if (url != null && url.Length > 0 && !url.EndsWith("default.aspx"))
                {
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(pid, false);
                }
                else
                {
                    if (up.RoleIndex >= 0)
                    {

                    }
                    else
                    {
                        bAuth = false;
                    }
                }
            }

            return bAuth;

        }


        public bool ProcessLogin(X509Certificate2 signerCert, out string msg)
        {
            UserProfileMember up = UserProfileFactory.CreateInstance(signerCert);
            return processLoginUsingRole(out msg, up);
        }

        private bool processLoginUsingRole(out string msg, UserProfileMember up)
        {
            msg = null;
            bool bAuth = false;
            if (up != null)	//new login
            {
                HttpContext.Current.SignOn(up);
                bAuth = true;
            }

            if (bAuth)
            {
                string url = FormsAuthentication.GetRedirectUrl(up.PID, false);
                FormsAuthentication.SetAuthCookie(up.PID, false);

                if (url != null && url.Length > 0 && !url.EndsWith("default.aspx"))
                {
                    //System.Web.Security.FormsAuthentication.RedirectFromLoginPage(up.PID, false);
                }
                else
                {
                    if (up.RoleIndex >= 0)
                    {
                        if (String.IsNullOrEmpty(RedirectToAsLoginSuccessfully))
                        {
                            //HttpContext.Current.Response.Redirect("MainPage.aspx", true);
                            msg = "~/Home/MainPage";
                        }
                        else
                        {
                            //HttpContext.Current.Response.Redirect(RedirectToAsLoginSuccessfully, true);
                            msg = RedirectToAsLoginSuccessfully;
                        }
                    }
                    else
                    {
                        bAuth = false;
                        msg = "使用者角色尚未被核定!!";
                    }
                }
            }
            else
            {
                msg = "系統找不到您的資料，請重新登入!!";
            }

            return bAuth;
        }
    }
}
