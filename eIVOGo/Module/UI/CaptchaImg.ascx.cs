using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Properties;
using Utility;

namespace eIVOGo.Module.UI
{
    public partial class CaptchaImg : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CaptchaImg_PreRender);
        }

        void CaptchaImg_PreRender(object sender, EventArgs e)
        {
            ValidCode = ValueValidity.CreateRandomStringCode(6);
            imgNum.Src = String.Format("{0}?{1}", VirtualPathUtility.ToAbsolute("~/Published/CaptchaImg.ashx"), (new com.uxb2b.util.CipherDecipherSrv()).cipher(ValidCode));
        }

        protected String ValidCode
        {
            get
            {
                return ViewState["code"] as String;
            }
            set
            {
                ViewState["code"] = value;
            }
        }

        public string ImgIDText
        {
            get { return this.txtImgID.Text; }
            set { this.txtImgID.Text = value; }
        }

        public bool Verify()
        {
            if (!String.IsNullOrEmpty(ValidCode))
            {
                if (this.txtImgID.Text.ToUpper() == ValidCode.ToUpper())
                {
                    return true;
                }
            }
            return false;
        }
    }
}