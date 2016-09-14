using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Helper;
using Model.Locale;
using DataAccessLayer.basis;
using System.Threading;
using Utility;
using ModelExtension.Properties;

namespace ModelExtension.MessageManagement
{
    public partial class SMSManager : GenericManager<MessageEntityDataContext, SMSNotificationQueue>, INotification
    {
        public SMSManager() : base() { }
        public SMSManager(GenericManager<MessageEntityDataContext> manager) : base(manager) { }

        private List<String> _mobile = new List<string>();

        public String MobileNo 
        { 
            get
            {
                return String.Join(",", _mobile);
            }
            set
            {
                _mobile.Clear();
                _mobile.AddRange(value.Split(',',';'));
            }
        }

        public List<String> Mobile
        {
            get
            {
                return _mobile;
            }
        }

        public int? OwnerID { get; set; }

        public void ProcessMessage()
        {
            ThreadPool.QueueUserWorkItem(this.sendSMS);
        }

        private void sendSMS(object stateInfo)
        {
            if (BuildMessageContent == null)
                return;
            var msg = BuildMessageContent(-1);

            var logs = this.GetTable<SMSNotificationLog>();
            SMSHelper sms = new SMSHelper();
            if (sms.Start())
            {
                foreach (var mobile in _mobile)
                {
                    if (!String.IsNullOrEmpty(mobile))
                    {
                        if (sms.SendSMS(msg.Subject, msg.Content, mobile) && sms.TotalUnsent == 0)
                        {
                            logs.InsertOnSubmit(new SMSNotificationLog
                            {
                                OwnerID = OwnerID,
                                MessageID = (int)Naming.MessageTypeDefinition.客服訊息通知,
                                SubmitDate = DateTime.Now,
                                SendingContent = msg.Content,
                                SendingMobil = mobile
                            });
                            this.SubmitChanges();
                        }
                        else if (ExceptionHandler != null)
                        {
                            ExceptionHandler(-1, String.Format("{0}({1})", sms.GetDeliveryStatus(), mobile), msg.Content);
                        }
                    }
                    this.SubmitChanges();
                }
                sms.Close();
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

        public static bool AlertToLowerCredit(out double credit)
        {
            credit = 0;
            bool bResult =false;
            SMSHelper sms = new SMSHelper();
            if (sms.Start())
            {
                if (sms.GetCredit())
                {
                    bResult = sms.Credit < Settings.Default.MinimumCreditAlert;
                    credit = sms.Credit;
                }
                sms.Close();
            }
            return bResult;
        }
    }
}
