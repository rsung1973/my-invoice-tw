using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Helper;
using Model.Locale;
using DataAccessLayer.basis;
using System.Threading;
using Utility;

namespace ModelExtension.MessageManagement
{
    public partial class InvoiceWinningNotificationManager : GenericManager<MessageEntityDataContext, SMSNotificationQueue>, INotification
    {
        public InvoiceWinningNotificationManager() : base() { }
        public InvoiceWinningNotificationManager(GenericManager<MessageEntityDataContext> manager) : base(manager) { }

        public int Year { get; set; }
        public int MonthFrom { get; set; }
        public int MonthTo { get; set; }

        public void ProcessMessage()
        {
            var smsQueue = this.GetTable<SMSNotificationQueue>();
            DateTime now = DateTime.Now;

            var winningInvoices = this.GetTable<InvoiceWinningNumber>().Where(n => n.Year == Year && n.MonthFrom == (byte)MonthFrom && n.MonthTo == (byte)MonthTo)
                .Select(n => n.InvoiceItem)
                .Where(n => n.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS == true);

            foreach (var item in winningInvoices)
            {
                if (!String.IsNullOrEmpty(item.InvoiceBuyer.Phone))
                {
                    if (!smsQueue.Any(q => q.DocID == item.InvoiceID))
                    {
                        smsQueue.InsertOnSubmit(new SMSNotificationQueue
                        {
                            DocID = item.InvoiceID,
                            MessageID = (int)Naming.MessageTypeDefinition.發票中獎通知,
                            SubmitDate = now
                        });
                        this.SubmitChanges();
                    }
                }
            }

            sendSMS(null);

        }

        private void sendSMS(object stateInfo)
        {
            var logs = this.GetTable<SMSNotificationLog>();
            var items = this.GetTable<SMSNotificationQueue>().Where(q => q.MessageID == (int)Naming.MessageTypeDefinition.發票中獎通知);
            if (items.Count() > 0)
            {
                SMSHelper sms = new SMSHelper();
                if (sms.Start())
                {
                    String mobile;
                    foreach (var item in items.ToList())
                    {
                        var msg = buildMessageContent(item.DocID, out mobile);
                        if (!String.IsNullOrEmpty(mobile))
                        {
                            if (sms.SendSMS("發票中獎通知", msg, mobile) && sms.TotalUnsent == 0)
                            {
                                logs.InsertOnSubmit(new SMSNotificationLog
                                {
                                    DocID = item.DocID,
                                    MessageID = (int)Naming.MessageTypeDefinition.發票中獎通知,
                                    SubmitDate = DateTime.Now,
                                    SendingContent = msg,
                                    SendingMobil = mobile
                                });
                                this.SubmitChanges();
                            }
                            else if (ExceptionHandler != null)
                            {
                                ExceptionHandler(item.DocID, sms.GetDeliveryStatus(), msg);
                            }
                        }
                        this.EntityList.DeleteOnSubmit(item);
                        this.SubmitChanges();
                    }
                    sms.Close();
                }
            }
        }

        private String buildMessageContent(int docID, out String mobile)
        {
            InvoiceItem item = this.GetTable<InvoiceItem>().Where(i => i.InvoiceID == docID).First();
            mobile = item.InvoiceBuyer.Phone;
            StringBuilder msg = new StringBuilder();
            //msg.Append("中獎發票:" + Environment.NewLine);
            //msg.Append(Environment.NewLine);
            //msg.Append("親愛的客戶您好," + Environment.NewLine);
            //msg.Append("您在" + item.InvoiceSeller.CustomerName + "採購之電子發票已中獎," + Environment.NewLine);
            //msg.Append("發票將於10日內寄出," + Environment.NewLine);
            //msg.Append("發票號碼: " + item.TrackCode + item.No + Environment.NewLine);
            //msg.Append("開立日期: " + item.InvoiceDate + Environment.NewLine);
            //msg.Append("消費金額: $" + String.Format("{0:.##}", item.InvoiceAmountType.TotalAmount));
            msg.Append(item.InvoiceSeller.CustomerName).Append("通知\r\n");
            msg.Append("您的").Append(((Naming.B2BInvoiceDocumentTypeDefinition)item.CDS_Document.DocType).ToString());
            msg.Append(String.Format("{0}{1}", item.TrackCode, item.No)).Append("已中獎\r\n");
            msg.Append("紙本發票將於10日內寄給您\r\n");
            msg.Append("備註:").Append(String.Join("", item.InvoiceDetails.Select(d => d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)));
            return msg.ToString();
        }

        public Func<int, NotificationMesssage> BuildMessageContent
        {
            get;
            set;
        }

        public Action<int, String, String> ExceptionHandler
        {
            get;
            set;
        }
    }
}
