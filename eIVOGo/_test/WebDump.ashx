<%@ WebHandler Language="C#" Class="eIVOGo._test.WebDump" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Utility;

namespace eIVOGo._test
{
    /// <summary>
    /// Summary description for WebDump
    /// </summary>
    public class WebDump : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var Request = context.Request;
            var Response = context.Response;
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            System.Web.Script.Serialization.JavaScriptSerializer serializer = 
                new System.Web.Script.Serialization.JavaScriptSerializer();
            String json = Request.GetPostAsString();
            var values = serializer.Deserialize<ApplyCancellingInvoice[]>(json);
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
    
    public partial class ApplyCancellingInvoice
    {
        public int invoiceID { get; set; }
        public String cancelReason { get; set; }
    }
}