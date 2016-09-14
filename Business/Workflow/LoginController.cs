using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

using Model.Security.MembershipManagement;
using Services;
using Model.Resource;
using Model.Locale;

namespace Business.Workflow
{
    public class LoginController
    {
        public LoginController()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string RedirectToAsLoginSuccessfully { get; set; }

        public bool ProcessLogin(string pid, string password, out string msg)
        {
             UserProfileMember up = LogonSvc.CreateUserProfile(pid, password);
             if (up != null)
             {
                 if (up.Profile.UserProfileStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Wait_For_Check)
                 {
                     up.CurrentSiteMenu = "WaitForCheckMenu.xml";
                 }
             }
             return processLoginUsingRole(out msg, up);
        }

        public bool ProcessLogin(string pid)
        {
            string msg;
            UserProfileMember up = LogonSvc.CreateUserProfile(pid);
            return processLoginUsingRole(out msg, up);
        }

        public void ReloadUserProfile(string pid)
        {
            UserProfileMember up = LogonSvc.CreateUserProfile(pid);
            HttpContext.Current.Session[WebKey.USER_PROFILE] = up;
        }

        public bool NormalLogin(string pid)
        {
            bool bAuth = false;

            UserProfileMember up = LogonSvc.CreateUserProfile(pid);
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
            UserProfileMember up = LogonSvc.CreateUserProfile(signerCert);
            return processLoginUsingRole(out msg, up);
        }

        private bool processLoginUsingRole(out string msg, UserProfileMember up)
        {
            msg = null;
            bool bAuth = false;
            if (up != null)	//new login
            {
                HttpContext.Current.Session.Add(WebKey.USER_PROFILE, up);
                bAuth = true;
            }

            if (bAuth)
            {
                string url = FormsAuthentication.GetRedirectUrl(up.PID, false);
                FormsAuthentication.SetAuthCookie(up.PID, false);

                if (url != null && url.Length > 0 && !url.EndsWith("default.aspx"))
                {
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(up.PID, false);
                }
                else
                {
                    if (up.RoleIndex >= 0)
                    {
                        if (String.IsNullOrEmpty(RedirectToAsLoginSuccessfully))
                        {
                            HttpContext.Current.Response.Redirect("MainPage.aspx", true);
                        }
                        else
                        {
                            HttpContext.Current.Response.Redirect(RedirectToAsLoginSuccessfully, true);
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
