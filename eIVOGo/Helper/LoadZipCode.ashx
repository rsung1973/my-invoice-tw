<%@ WebHandler Language="C#" Class="eIVOGo.Helper.LoadZipCode" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Helper
{
    /// <summary>
    /// Summary description for LoadZipCode
    /// </summary>
    public class LoadZipCode : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var Response = context.Response;
            var Request = context.Request;

            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                Response.Output.WriteLine(client.DownloadString("http://zip5.5432.tw/zip5json.py?" + Request.Params["QUERY_STRING"]));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}