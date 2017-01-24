using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Utility;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Uxnet.Web.WebUI;

namespace Business.Helper
{
    public static class LoginExtension
    {

        public static void SignOn(this HttpContextBase context, UserProfileMember profile)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(profile.PID, false, HttpContext.Current.Session.Timeout);
            context.Response.SetCookie(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
            context.ClearCache();
            context.SetCacheValue("userProfile", profile);

            HttpCookie cookie;
            cookie = new HttpCookie("userID", profile.PID);
            cookie.Expires = DateTime.Now.AddMinutes(context.Session.Timeout);
            context.Response.SetCookie(cookie);
        }

        public static void SignOn(this HttpContext context, UserProfileMember profile)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(profile.PID, false, HttpContext.Current.Session.Timeout);
            context.Response.SetCookie(new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket)));
            context.ClearCache();
            context.SetCacheValue("userProfile", profile);

            HttpCookie cookie;
            cookie = new HttpCookie("userID", profile.PID);
            cookie.Expires = DateTime.Now.AddMinutes(context.Session.Timeout);
            context.Response.SetCookie(cookie);
        }

        public static void ClearCache(this HttpContextBase context)
        {
            DataModelCache caching = new DataModelCache(context);
            caching.Clear();
        }
        public static void ClearCache(this HttpContext context)
        {
            HttpContextDataModelCache caching = new HttpContextDataModelCache(context);
            caching.Clear();
        }


        public static void SetCacheValue(this HttpContextBase context, CachingKey keyName, Object value)
        {
            context.SetCacheValue(keyName.ToString(), value);
        }

        public static void RemoveCache(this HttpContextBase context, CachingKey keyName)
        {
            context.SetCacheValue(keyName.ToString(), null);
        }


        public static Object GetCacheValue(this HttpContextBase context, CachingKey keyName)
        {
            return context.GetCacheValue(keyName.ToString());
        }

        public static void SetCacheValue(this HttpContextBase context,String keyName,Object value)
        {
            DataModelCache caching = new DataModelCache(context);
            caching[keyName] = value;
        }

        public static void SetCacheValue(this HttpContext context, String keyName, Object value)
        {
            HttpContextDataModelCache caching = new HttpContextDataModelCache(context);
            caching[keyName] = value;
        }


        public static Object GetCacheValue(this HttpContextBase context, String keyName)
        {
            DataModelCache caching = new DataModelCache(context);
            return caching[keyName];
        }


        public static String MakePassword(this String password)
        {
            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.Default.GetBytes(password)));
        }

        public static void Logout(this HttpContextBase context)
        {
            context.ClearCache();
            FormsAuthentication.SignOut();
            context.Response.SetCookie(new HttpCookie(FormsAuthentication.FormsCookieName, ""));
        }

        public static void Logout(this HttpContext context)
        {
            context.ClearCache();
            FormsAuthentication.SignOut();
            context.Response.SetCookie(new HttpCookie(FormsAuthentication.FormsCookieName, ""));
        }


        public static UserProfileMember GetUser(this HttpContextBase context)
        {
            UserProfileMember profile = (UserProfileMember)context.GetCacheValue("userProfile");
            if (profile == null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    profile = UserProfileFactory.CreateInstance(context.User.Identity.Name);
                    context.SetCacheValue("userProfile", profile);
                }
                //else
                //{
                //    FormsAuthentication.RedirectToLoginPage();
                //}
            }
            return profile;
        }

        public static UserProfileMember GetUser(this HttpContext context)
        {
            HttpContextDataModelCache caching = new HttpContextDataModelCache(context);
            UserProfileMember profile = (UserProfileMember)caching["userProfile"];
            if (profile == null)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    profile = UserProfileFactory.CreateInstance(context.User.Identity.Name);
                    context.SetCacheValue("userProfile", profile);
                }
                //else
                //{
                //    FormsAuthentication.RedirectToLoginPage();
                //}
            }
            return profile;
        }

    }
}