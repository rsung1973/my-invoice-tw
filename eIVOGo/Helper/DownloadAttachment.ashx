<%@ WebHandler Language="C#" Class="eIVOGo.Helper.DownloadAttachment" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Model.DataEntity;
using Utility;

namespace eIVOGo.Helper
{
    /// <summary>
    /// Summary description for LoadZipCode
    /// </summary>
    public class DownloadAttachment : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var Response = context.Response;
            var Request = context.Request;
            
            if(Request["keyName"]!=null)
            {
                using (Model.InvoiceManagement.InvoiceManager mgr = new Model.InvoiceManagement.InvoiceManager())
                {
                    var item = mgr.GetTable<Attachment>().Where(i => i.KeyName == Request["keyName"]).FirstOrDefault();

                    if (item != null)
                    {
                        Response.WriteFileAsDownload(item.StoredPath);
                    }

                }
            }

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