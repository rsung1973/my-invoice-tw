using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;

using Uxnet.Web.WebUI;

namespace eIVOGo.Published
{
    public partial class TestAll : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            trackCodeList.QueryExpr = t => true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(TestAll_PreRender);
            waitModal.ActionComplete += new EventHandler(waitModal_ActionComplete);
        }

        void waitModal_ActionComplete(object sender, EventArgs e)
        {
            btnWait.Text = "再等10秒...";
        }

        void TestAll_PreRender(object sender, EventArgs e)
        {
            trackCodeList.BindData();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                Response.Clear();
                Response.ContentType = "message/rfc822";
                Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", Server.UrlEncode("中文字碼表.txt")));

                using (StreamReader sr = new StreamReader(FileUpload1.PostedFile.InputStream, Encoding.ASCII))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        byte[] buf = BitConverter.GetBytes(UInt16.Parse(line, NumberStyles.HexNumber));
                        Response.OutputStream.WriteByte(buf[1]);
                        Response.OutputStream.WriteByte(buf[0]);
                    }
                }

                Response.End();
                
            }
        }

        protected void btnWait_Click(object sender, EventArgs e)
        {
            waitModal.Do(obj =>
                {
                    Thread.Sleep(10000);
                });
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(controlPath.Text))
            {
                UserControl uc = (UserControl)this.LoadControl(controlPath.Text);
                uc.InitializeAsUserControl(this.Page);
                this.Page.Controls.Add(uc);
            }
        }
    }
}