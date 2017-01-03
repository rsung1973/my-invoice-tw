using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Utility;

namespace eIVOGo
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
    : base()
        {
            this.AuthenticateRequest += Global_AuthenticateRequest;
            this.AuthorizeRequest += Global_AuthorizeRequest;
        }

        void Global_AuthorizeRequest(object sender, EventArgs e)
        {

        }

        void Global_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Context.User == null)
            {
                HttpCookie cookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    FormsIdentity identity = new FormsIdentity(ticket);
                    Context.User = new ClaimsPrincipal(identity);
                }
            }
        }

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Uxnet.Com.Helper.JobScheduler.StartUp();
            eIVOGo.Module.SAM.SystemMonitorControl.StartUp();
            eIVOGo.Published.eInvoiceService.StartUp();


        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            var ex = Server.GetLastError();
            if (ex != null)
                Logger.Error(ex);
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
