<%@ WebHandler Language="C#" Class="eIVOGo.Helper.GetSample" %>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Model.DataEntity;
using ModelExtension.DataExchange;
using Utility;
using ClosedXML.Excel;

namespace eIVOGo.Helper
{
    /// <summary>
    /// Summary description for LoadZipCode
    /// </summary>
    public class GetSample : IHttpHandler
    {
        HttpResponse Response;
        HttpRequest Request;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            Response = context.Response;
            Request = context.Request;
            
            switch(Request["data"])
            {
                case "InvoiceBuyer":
                    createSample(() =>
                    {
                        var exchange = new InvoiceBuyerExchange();
                        using (XLWorkbook xls = exchange.GetSample())
                        {
                            xls.SaveAs(Response.OutputStream);
                        }
                    }, "修改買受人資料.xlsx");
                    break;
                    
                case "TrackCode":
                    createSample(() =>
                    {
                        var exchange = new TrackCodeExchange();
                        using (XLWorkbook xls = exchange.GetSample())
                        {
                            xls.SaveAs(Response.OutputStream);
                        }
                    }, "發票字軌資料.xlsx");
                    break;
            }

        }

        void createSample(Action creator,String fileName)
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode(fileName)));

            creator();

            Response.Flush();
            Response.End();
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