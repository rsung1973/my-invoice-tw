using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.Workflow;
using Model.Security.MembershipManagement;
using Model.DataEntity;
using System.Text;
using System.Net.Mail;
using eIVOGo.Properties;
using System.Net;
using Utility;
using eIVOGo.Module.SAM;
using Model.Locale;

namespace eIVOGo
{
    public partial class getPassword_email : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.PID.Text))
            {
                if (this.CaptchaImg1.Verify())
                {
                    string pid = this.PID.Text.Trim();
                    if (doSendMail(pid))
                    {
                        //Uxnet.Web.WebUI.WebMessageBox.Alert(this.Page, "信件已送出至您的信箱!!");
                        //Response.Redirect("login.aspx");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('信件已送出至您的信箱!!'); location.href='login.aspx';", true);
                    }
                    else
                    {
                        this.lblMsg.Text = "無此帳號或帳號錯誤!!";
                    }
                }
                else
                {
                    this.lblMsg.Text = "驗証碼錯誤!!";
                }
            }
            else
            {
                this.lblMsg.Text = "請填入帳號!!";
            }
        }

        private Boolean doSendMail(string pid)
        {
            Boolean result = false;
            try
            {
                using (UserProfileManager mgr = new UserProfileManager())
                {
                    IQueryable<UserProfile> items = mgr.EntityList.Where(u => u.PID.Equals(pid) & u.UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete);
                    if (items.Count() > 0)
                    {
                        string _tempPassword = Utility.ExtensionMethods.CreateRandomPassword(6);
                        items.FirstOrDefault().Password2 = Utility.ValueValidity.MakePassword(_tempPassword);
                        items.FirstOrDefault().UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Wait_For_Check;

                        StringBuilder body = new StringBuilder();
                        MailMessage message = new MailMessage();
                        message.ReplyToList.Add(Settings.Default.ReplyTo);
                        message.From = new MailAddress(Settings.Default.WebMaster);
                        message.To.Add(items.FirstOrDefault().EMail);
                        message.Subject = "網際優勢電子發票獨立第三方平台 密碼變更通知信";
                        message.IsBodyHtml = true;

                        body.Append("本信件由 網際優勢電子發票獨立第三方平台 寄出，為本站之密碼變更通知信。<br><br>");
                        body.Append("(本信件為系統自動發出，請勿回覆本信件。)<br>");
                        body.Append("-------------------------------------------------<br>");
                        body.Append("會員帳號：").Append(items.FirstOrDefault().PID).Append("<br>");
                        body.Append("會員密碼：").Append(_tempPassword).Append("<br>");
                        body.Append("-------------------------------------------------<br>");
                        body.Append("請立即透過下方密碼變更連結 登入網際優勢電子發票獨立第三方平台 變更密碼 。<br><br>");
                        body.Append("密碼變更連結： ");
                        body.Append("<a href=").Append(Settings.Default.mailLinkAddress).Append(VirtualPathUtility.ToAbsolute("~/SAM/EditMyself.aspx")).Append("?active=aEfs45WE>會員密碼變更</a>");
                        body.Append("<br><br>電子發票獨立第三方平台 感謝您的使用");

                        message.Body = body.ToString();

                        SmtpClient smtpclient = new SmtpClient(Settings.Default.MailServer);
                        smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
                        smtpclient.Send(message);

                        mgr.SubmitChanges();
                        result = true;
                    }
                }                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return result;
        }
    }
}