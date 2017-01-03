using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.InvoiceManagement;
using Model.Schema.EIVO;
using Model.DataEntity;
using Utility;
using Model.Locale;
using Model.UploadManagement;

namespace Model.Helper
{
    public class ExceptionInfo
    {
        public OrganizationToken Token { get; set; }
        public Dictionary<int, Exception> ExceptionItems { get; set; }
        public InvoiceRoot InvoiceData { get; set; }
        public Model.Schema.MIG3_1.C0401.Invoice InvoiceData_C0401 { get; set; }
        public Model.Schema.MIG3_1.C0501.CancelInvoice CancelInvoiceData_C0501 { get; set; }
        public CancelInvoiceRoot CancelInvoiceData { get; set; }
        public AllowanceRoot AllowanceData { get; set; }
        public CancelAllowanceRoot CancelAllowanceData { get; set; }
        public IEnumerable<ItemUpload<InvoiceItem>> InvoiceError { get; set; }
        public IEnumerable<ItemUpload<InvoiceItem>> InvoiceCancellationError { get; set; }
        public IEnumerable<ItemUpload<Organization>> BusinessCounterpartError { get; set; }
    }

    public partial class ExceptionEventArgs : EventArgs
    {
        public int? CompanyID { get; set; }
        public String EMail {get;set;}
        public int MaxLogID { get; set; }
    }

    public static class ExceptionNotification
    {
        public static EventHandler<ExceptionEventArgs> SendExceptionNotification;
        public static Action<Exception> SendExceptionNotificationToSysAdmin;

        public static void SendNotification(object stateInfo)
        {
            ExceptionInfo info = stateInfo as ExceptionInfo;
            if (info == null)
                return;

            try
            {
                if (info.InvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoice(info);
                }
                else if (info.InvoiceData_C0401 != null)
                {
                    notifyExceptionWhenUploadInvoice_C0401(info);
                }
                else if (info.CancelInvoiceData != null)
                {
                    notifyExceptionWhenUploadCancellation(info);
                }
                else if (info.CancelInvoiceData_C0501 != null)
                {
                    notifyExceptionWhenUploadCancellation_C0501(info);
                }
                else if (info.AllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowance(info);
                }
                else if (info.CancelAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceCancellation(info);
                }
                else if (info.InvoiceError != null)
                {
                    notifyExceptionWhenUploadCsvInvoice(info);
                }
                else if (info.InvoiceCancellationError != null)
                {
                    notifyExceptionWhenUploadCsvInvoiceCancellation(info);
                }

                lock(typeof(ExceptionNotification))
                {
                    processNotification();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        public static void ProcessNotification()
        {
            try
            {
                lock (typeof(ExceptionNotification))
                {
                    processNotification();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void processNotification()
        {
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var items = mgr.GetTable<ExceptionLog>().Where(e => e.ExceptionReplication != null);
                int maxLogID = items.OrderByDescending(e => e.LogID).First().LogID;
                foreach (var item in items.GroupBy(i => i.CompanyID))
                {
                    SendExceptionNotification(mgr, new ExceptionEventArgs
                    {
                        CompanyID = item.Key,
                        EMail = item.ElementAt(0).Organization.ContactEmail,
                        MaxLogID = maxLogID
                    });
                }

                SendExceptionNotification(mgr, new ExceptionEventArgs
                {
                    MaxLogID = maxLogID
                    ///送給系統管理員接收全部異常資料
                    ///
                });

                mgr.GetTable<ExceptionReplication>().DeleteAllOnSubmit(items
                    .Where(i => i.LogID <= maxLogID)
                    .Select(i => i.ExceptionReplication));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoice(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    Message = e.Value.Message,
                    DataContent = info.InvoiceData.Invoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoice_C0401(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    Message = e.Value.Message,
                    DataContent = info.InvoiceData_C0401.GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadCsvInvoice(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.InvoiceError.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    Message = e.Status,
                    DataContent = e.DataContent,
                    IsCSV = true
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadCsvInvoiceCancellation(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.InvoiceCancellationError.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    Message = e.Status,
                    DataContent = e.DataContent,
                    IsCSV = true
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadAllowance(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    Message = e.Value.Message,
                    DataContent = info.AllowanceData.Allowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadCancellation(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    Message = e.Value.Message,
                    DataContent = info.CancelInvoiceData.CancelInvoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadCancellation_C0501(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    Message = e.Value.Message,
                    DataContent = info.CancelInvoiceData_C0501.GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadAllowanceCancellation(ExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                    Message = e.Value.Message,
                    DataContent = info.CancelAllowanceData.CancelAllowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

    }


}
