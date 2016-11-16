using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;
using System.Drawing;
using System.Drawing.Imaging;
//using ThoughtWorks.QRCode.Codec;
using MessagingToolkit.QRCode.Codec;
using System.Text;

namespace eIVOGo.Published
{
    /// <summary>
    /// Summary description for GetBarCode39
    /// </summary>
    public class GetQRCode : IHttpHandler
    {
        protected QRCodeEncoder.ENCODE_MODE _encoding = QRCodeEncoder.ENCODE_MODE.BYTE;
        protected int _scale = 4;
        protected int _version = 6;
        protected QRCodeEncoder.ERROR_CORRECTION _errorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
        protected float _dpi = 600f;

        public virtual void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            response.Clear();
            response.ContentType = "image/Jpeg";
            //response.Buffer = true;
            //response.ExpiresAbsolute = System.DateTime.Now.AddMilliseconds(0);
            //response.Expires = 0;
            response.Cache.SetCacheability(HttpCacheability.NoCache);

            initialize(request);

            String content = request.Params["text"];

            try
            {
                Bitmap img = createQRCode(content, _encoding, _scale, _version, _errorCorrect);
                img.SetResolution(_dpi, _dpi);
                img.Save(response.OutputStream, ImageFormat.Jpeg);
                img.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            //context.Response.CacheControl = "no-cache";
            //context.Response.AppendHeader("Pragma", "No-Cache");
        }

        protected void initialize(HttpRequest request)
        {
            if (!String.IsNullOrEmpty(request["enc"]))
            {
                Enum.TryParse(request["enc"], true, out _encoding);
            }

            if (!String.IsNullOrEmpty(request["scale"]))
            {
                int.TryParse(request["scale"], out _scale);
            }

            if (!String.IsNullOrEmpty(request["ver"]))
            {
                int.TryParse(request["ver"], out _version);
            }

            if (!String.IsNullOrEmpty(request["ec"]))
            {
                Enum.TryParse(request["ec"], true, out _errorCorrect);
            }

            if (!String.IsNullOrEmpty(request["dpi"]))
            {
                float.TryParse(request["dpi"], out _dpi);
            }
        }

        protected Bitmap createQRCode(String content, QRCodeEncoder.ENCODE_MODE encoding, int scale, int version, QRCodeEncoder.ERROR_CORRECTION errorCorrect)
        {
            if (String.IsNullOrEmpty(content))
            {
                return null;
            }

            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();

            qrCodeEncoder.QRCodeScale = scale;
            qrCodeEncoder.QRCodeVersion = version;
            qrCodeEncoder.QRCodeErrorCorrect = errorCorrect;
            qrCodeEncoder.CharacterSet = "UTF-8";

            return qrCodeEncoder.Encode(content);

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