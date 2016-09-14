using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;
using System.Web.SessionState;
using System.Drawing.Imaging;
using eIVOGo.Properties;

namespace eIVOGo.services
{
    /// <summary>
    ///CaptchaImg 的摘要描述
    /// </summary>
    public class CaptchaImg : IHttpHandler, IRequiresSessionState 
    {
        private string _code;

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["QUERY_STRING"] != null)
            {
                _code = (new com.uxb2b.util.CipherDecipherSrv()).decipher(context.Request["QUERY_STRING"]);
            }
            getimg(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void getimg(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            response.Clear();
            response.ContentType = "image/Png";
            using (Bitmap bmp = new Bitmap(120, 30))
            {
                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                int x3 = 0;
                int y3 = 0;
                int intNoiseWidth = 25;
                int intNoiseHeight = 15;
                Random rdn = new Random();
                Graphics g = Graphics.FromImage(bmp);

                //設定字型
                Font font = new Font("Courier New", 16, FontStyle.Bold);

                //設定圖片背景
                g.Clear(Color.CadetBlue);

                //產生雜點
                for (int i = 0; i < 100; i++)
                {
                    x1 = rdn.Next(0, bmp.Width);
                    y1 = rdn.Next(0, bmp.Height);
                    bmp.SetPixel(x1, y1, Color.DarkGreen);
                }

                //產生擾亂弧線
                for (int i = 0; i < 15; i++)
                {
                    x1 = rdn.Next(bmp.Width - intNoiseWidth);
                    y1 = rdn.Next(bmp.Height - intNoiseHeight);
                    x2 = rdn.Next(1, intNoiseWidth);
                    y2 = rdn.Next(1, intNoiseHeight);
                    x3 = rdn.Next(0, 45);
                    y3 = rdn.Next(-270, 270);
                    g.DrawArc(new Pen(Brushes.Gray), x1, y1, x2, y2, x3, y3);
                }
                
                //把GenPassword()方法換成你自己的密碼產生器，記得把產生出來的密碼存起來日後才能與user的輸入做比較。
                
                
                g.DrawString(_code, font, Brushes.Black, 3, 3);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Png );
                byte[] bmpBytes = ms.GetBuffer();
                bmp.Dispose();
                ms.Close();
                context.Response.BinaryWrite(bmpBytes);
                //context.Response.End();

            }

        }
    }
}