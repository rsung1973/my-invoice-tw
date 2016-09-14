using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.InvoiceManagement;
using Model.DataEntity;
using Model.Locale;

namespace Model.Helper
{
    public static partial class InvoiceNotification
    {
       
        static InvoiceNotification() 
        {
            ProcessMessage = () => 
            {
                using (InvoiceManager mgr = new InvoiceManager())
                {
                    mgr.ExecuteCommand("delete from DocumentReplication");

                    //var replication = mgr.GetTable<DocumentReplication>();
                    //var invItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice && !r.LastActionTime.HasValue);
                    //if (invItems.Count() > 0)
                    //{
                    //    replication.DeleteAllOnSubmit(invItems);
                    //    mgr.SubmitChanges();
                    //}

                    //var cancelItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation && !r.LastActionTime.HasValue);
                    //if (cancelItems.Count() > 0)
                    //{
                    //    replication.DeleteAllOnSubmit(cancelItems);
                    //    mgr.SubmitChanges();
                    //}

                    //var allowanceItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Allowance && !r.LastActionTime.HasValue);
                    //if (allowanceItems.Count() > 0)
                    //{
                    //    replication.DeleteAllOnSubmit(allowanceItems);
                    //    mgr.SubmitChanges();
                    //}

                    //var cancelAllowanceItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation && !r.LastActionTime.HasValue);
                    //if (cancelAllowanceItems.Count() > 0)
                    //{
                    //    replication.DeleteAllOnSubmit(cancelAllowanceItems);
                    //    mgr.SubmitChanges();
                    //}

                }
            };
        }

        public static Action ProcessMessage
        {
            get;
            set;
        }

        public static Action<IEnumerable<int>> NotifyIssuingInvoice
        {
            get;
            set;
        }

        public static void SendIssuingNotification(this IEnumerable<int> invoiceID)
        {
            NotifyIssuingInvoice(invoiceID);
        }

        public static Action<IEnumerable<int>> NotifyCancellingInvoice
        {
            get;
            set;
        }

        public static void SendCancellingInvoiceNotification(this IEnumerable<int> invoiceID)
        {
            NotifyCancellingInvoice(invoiceID);
        }


    }

    public class NotificationMesssage
    {
        public String Subject { get; set; }
        public String Content { get; set; }
    }

    public interface INotification
    {
        Func<int, NotificationMesssage> BuildMessageContent { get; set; }
        void ProcessMessage();
        Action<int, String, String> ExceptionHandler { get; set; }
    }

}
