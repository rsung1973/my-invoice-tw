using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;
using System.Drawing;
using System.Drawing.Imaging;

namespace eIVOGo.Published
{
    /// <summary>
    /// Summary description for GetBarCode39
    /// </summary>
    public class GetBarCode39 : IHttpHandler
    {
        protected float _dpi = 600f;

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            response.Clear();
            response.ContentType = "image/Jpeg";
            //response.Buffer = true;
            //response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            //response.Expires = 0;
            response.Cache.SetCacheability(HttpCacheability.NoCache);

            String sCode = request.Params["QUERY_STRING"];
            if (!String.IsNullOrEmpty(sCode))
            {
                try
                {
                    using (Bitmap img = sCode.GetCode39(false))
                    {
                        img.SetResolution(_dpi, _dpi);
                        img.Save(response.OutputStream, ImageFormat.Jpeg);
                        img.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }

            //context.Response.CacheControl = "no-cache";
            //context.Response.AppendHeader("Pragma", "No-Cache");
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