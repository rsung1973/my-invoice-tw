using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Workflow;

namespace eIVOGo
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.PID.Text = "guest";
            //this.PWD.Text = "12345";
        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            //this.PWD.Text = "12345";
            //this.PID.Text = Request["rule"].ToString();
            
            if (this.CaptchaImg1.Verify())
            {
                LoginController loginController = new LoginController();

                string msg;

                if (!loginController.ProcessLogin(PID.Text, PWD.Text, out msg))
                {
                    lblMsg.Text = msg;
                    this.CaptchaImg1.ImgIDText = "";
                }
            }
            else
            {
                this.lblMsg.Text = "驗証碼錯誤";
                this.CaptchaImg1.ImgIDText = "";
            }
        }
    }
}