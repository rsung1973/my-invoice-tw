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
    public partial class InvoiceNotificationManager : GenericManager<MessageEntityDataContext, SMSNotificationQueue>, INotification
    {
        public InvoiceNotificationManager() : base() { }
        public InvoiceNotificationManager(GenericManager<MessageEntityDataContext> manager) : base(manager) { }

        public void ProcessMessage()
        {
            var smsQueue = this.GetTable<SMSNotificationQueue>();
            DateTime now = DateTime.Now;
            foreach (var item in this.GetTable<DocumentReplication>().Select(r => r.CDS_Document)
                .Where(d => d.DocType == (int)Naming.DocumentTypeDefinition.E_Invoice
                    && d.DocumentOwner.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS == true && d.InvoiceItem.InvoiceBuyer.ReceiptNo.Equals("0000000000")))
            {
                if (!smsQueue.Any(q => q.DocID == item.DocID))
                {
                    smsQueue.InsertOnSubmit(new SMSNotificationQueue
                    {
                        DocID = item.DocID,
                        MessageID = (int)Naming.MessageTypeDefinition.發票開立通知,
                        SubmitDate = now
                    });
                    this.SubmitChanges();
                }
            }

            this.ExecuteCommand("delete from DocumentReplication");
            sendSMS(null);
//            ThreadPool.QueueUserWorkItem(this.sendSMS);

        }

        private void sendSMS(object stateInfo)
        {
            var logs = this.GetTable<SMSNotificationLog>();
            var items = this.GetTable<SMSNotificationQueue>().Where(q => q.MessageID == (int)Naming.MessageTypeDefinition.發票開立通知);
            if (items.Count() > 0 && BuildMessageContent != null)
            {
                SMSHelper sms = new SMSHelper();
                if (sms.Start())
                {
                    foreach (var item in items.ToList())
                    {
                        var msg = BuildMessageContent(item.DocID);
                        var mobile = item.CDS_Document.GetCounterpartMobile();
                        if (!String.IsNullOrEmpty(mobile))
                        {
                            if (sms.SendSMS(msg.Subject, msg.Content, mobile) && sms.TotalUnsent == 0)
                            {
                                logs.InsertOnSubmit(new SMSNotificationLog
                                {
                                    DocID = item.DocID,
                                    MessageID = (int)Naming.MessageTypeDefinition.發票開立通知,
                                    SubmitDate = DateTime.Now,
                                    SendingContent = msg.Content,
                                    SendingMobil = mobile
                                });
                                this.SubmitChanges();
                            }
                            else if (ExceptionHandler != null)
                            {
                                ExceptionHandler(item.DocID, sms.GetDeliveryStatus(), null);
                            }
                        }
                        this.EntityList.DeleteOnSubmit(item);
                        this.SubmitChanges();
                    }
                    sms.Close();
                }
            }
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
