using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using eIVOGo.Properties;
using System.Net;
using System.Text;

namespace eIVOGo.Published
{
    public partial class TestEMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(Settings.Default.WebMaster);
            message.To.Add(MailTo.Text);
            message.Subject = Subject.Text;
            message.IsBodyHtml = true;

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                message.Body = wc.DownloadString(Url.Text);
            }

            SmtpClient smtpclient = new SmtpClient(Settings.Default.MailServer);
            smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
            smtpclient.Send(message);

        }
    }
}