using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

using com.uxb2b.util;
using Model.DataEntity;
using Model.Locale;
using Uxnet.Web.WebUI;
using eIVOGo.Properties;

namespace eIVOGo.Published
{
    public partial class RequestInvoicePaper : System.Web.UI.Page
    {
        protected InvoiceItem _item;
        protected String _queryString;
        protected void Page_Load(object sender, EventArgs e)
        {

            _queryString = Request.Params["QUERY_STRING"];
            if (!String.IsNullOrEmpty(_queryString))
            {
                int invoiceID;
                if (int.TryParse((new CipherDecipherSrv()).decipher(_queryString), out invoiceID))
                {
                    _item = dsEntity.CreateDataManager().EntityList.Where(i => i.InvoiceID == invoiceID).FirstOrDefault();
                }
            }

            checkItem();
        }

        private void checkItem()
        {
            if (_item == null)
            {
                WebMessageBox.Alert(Page, "發票資料不存在!!");
            }
            else
            {
                if (_item.CDS_Document.DocumentPrintLog.Any(p => p.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice))
                {
                    WebMessageBox.Alert(Page, "發票已列印!!");
                }
                else if (_item.InvoicePaperRequest != null)
                {
                    WebMessageBox.Alert(Page, "紙本發票已索取!!");
                }
                else
                {
                    var mgr = dsEntity.CreateDataManager();
                    _item.InvoicePaperRequest = new InvoicePaperRequest
                    {
                        RequestDate = DateTime.Now,
                        Token = _queryString
                    };
                    mgr.SubmitChanges();
                    WebMessageBox.Alert(Page, "紙本發票索取完成!!");

                    sendRequestAlert();
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "close", "window.close();", true);
        }

        private void sendRequestAlert()
        {
            ThreadPool.QueueUserWorkItem(obj =>
            {
                MailMessage message = new MailMessage();
                message.ReplyToList.Add(Settings.Default.ReplyTo);
                message.From = new MailAddress(Settings.Default.WebMaster);
                message.To.Add(Settings.Default.WebMaster);
                message.Subject = String.Format("紙本發票索取通知,發票號碼:{0}{1}", _item.TrackCode, _item.No);
                message.IsBodyHtml = true;

                message.Body = String.Format("發票號碼:{0}{1},已被索取紙本發票,請即刻處理!!", _item.TrackCode, _item.No); 

                SmtpClient smtpclient = new SmtpClient(Settings.Default.MailServer);
                smtpclient.Credentials = CredentialCache.DefaultNetworkCredentials;
                smtpclient.Send(message);

            });
        }
        
    }
}