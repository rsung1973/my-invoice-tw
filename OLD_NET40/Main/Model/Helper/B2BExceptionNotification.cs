using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.InvoiceManagement;
using Model.DataEntity;
using Utility;
using Model.Locale;
using Model.Schema.EIVO.B2B;

namespace Model.Helper
{
    public class B2BExceptionInfo
    {
        public OrganizationToken Token { get; set; }
        public Dictionary<int, Exception> ExceptionItems { get; set; }
        public SellerInvoiceRoot InvoiceData { get; set; }
        public CancelInvoiceRoot CancelInvoiceData { get; set; }
        public AllowanceRoot AllowanceData { get; set; }
        public CancelAllowanceRoot CancelAllowanceData { get; set; }
        public BuyerInvoiceRoot BuyerInvoiceData { get; set; }
        public ReceiptRoot ReceiptData { get; set; }
        public CancelReceiptRoot CancelReceiptData { get; set; }
        public ReturnInvoiceRoot ReturnInvoiceData { get; set; }
        public ReturnCancelInvoiceRoot ReturnCancelInvoiceData { get; set; }
        public ReturnAllowanceRoot ReturnAllowanceData { get; set; }
        public ReturnCancelAllowanceRoot ReturnCancelAllowanceData { get; set; }
        public DeleteInvoiceRoot DeleteInvoiceData { get; set; }
        public DeleteCancelInvoiceRoot DeleteCancelInvoiceData { get; set; }
        public DeleteAllowanceRoot DeleteAllowanceData { get; set; }
        public DeleteCancelAllowanceRoot DeleteCancelAllowanceData { get; set; }
    }

    public static class B2BExceptionNotification
    {
        public static EventHandler<ExceptionEventArgs> SendExceptionNotification;

        public static void SendNotification(object stateInfo)
        {
            B2BExceptionInfo info = stateInfo as B2BExceptionInfo;
            if (info == null)
                return;

            try
            {
                if (info.InvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoice(info);
                }
                else if (info.CancelInvoiceData != null)
                {
                    notifyExceptionWhenUploadCancellation(info);
                }
                else if (info.AllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowance(info);
                }
                else if (info.CancelAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceCancellation(info);
                }
                else if (info.BuyerInvoiceData != null)
                {
                    notifyExceptionWhenUploadBuyerInvoice(info);
                }
                else if (info.ReceiptData != null)
                {
                    notifyExceptionWhenUploadReceipt(info);
                }
                else if (info.CancelReceiptData != null)
                {
                    notifyExceptionWhenUploadReceiptCancellation(info);
                }
                else if (info.ReturnInvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoiceReturn(info);
                }
                else if (info.ReturnCancelInvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoiceCancellationReturn(info);
                }
                else if (info.ReturnAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceReturn(info);
                }
                else if (info.ReturnCancelAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceCancellationReturn(info);
                }
                else if (info.DeleteInvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoiceDelete(info);
                }
                else if (info.DeleteCancelInvoiceData != null)
                {
                    notifyExceptionWhenUploadInvoiceCancellationDelete(info);
                }
                else if (info.DeleteAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceDelete(info);
                }
                else if (info.DeleteCancelAllowanceData != null)
                {
                    notifyExceptionWhenUploadAllowanceCancellationDelete(info);
                }

                processNotification();
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
                foreach (var item in items.GroupBy(i => i.CompanyID))
                {
                    SendExceptionNotification(mgr, new ExceptionEventArgs
                    {
                        CompanyID = item.Key,
                        EMail = item.ElementAt(0).Organization.ContactEmail
                    });
                }

                SendExceptionNotification(mgr, new ExceptionEventArgs
                {
                    ///送給系統管理員接收全部異常資料
                    ///
                });

                mgr.GetTable<ExceptionReplication>().DeleteAllOnSubmit(items.Select(i => i.ExceptionReplication));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoiceReturn(B2BExceptionInfo info)
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
                    DataContent = info.ReturnInvoiceData.ReturnInvoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoiceDelete(B2BExceptionInfo info)
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
                    DataContent = info.DeleteInvoiceData.DeleteInvoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoiceCancellationReturn(B2BExceptionInfo info)
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
                    DataContent = info.ReturnCancelInvoiceData.ReturnCancelInvoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadInvoiceCancellationDelete(B2BExceptionInfo info)
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
                    DataContent = info.DeleteCancelInvoiceData.DeleteCancelInvoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadAllowanceReturn(B2BExceptionInfo info)
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
                    DataContent = info.ReturnAllowanceData.ReturnAllowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadAllowanceDelete(B2BExceptionInfo info)
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
                    DataContent = info.DeleteAllowanceData.DeleteAllowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadAllowanceCancellationReturn(B2BExceptionInfo info)
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
                    DataContent = info.ReturnCancelAllowanceData.ReturnCancelAllowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadAllowanceCancellationDelete(B2BExceptionInfo info)
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
                    DataContent = info.DeleteCancelAllowanceData.DeleteCancelAllowance[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadInvoice(B2BExceptionInfo info)
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


        private static void notifyExceptionWhenUploadBuyerInvoice(B2BExceptionInfo info)
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
                    DataContent = info.BuyerInvoiceData.Invoice[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }


        private static void notifyExceptionWhenUploadAllowance(B2BExceptionInfo info)
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


        private static void notifyExceptionWhenUploadCancellation(B2BExceptionInfo info)
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

        private static void notifyExceptionWhenUploadAllowanceCancellation(B2BExceptionInfo info)
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

        private static void notifyExceptionWhenUploadReceipt(B2BExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.B2BInvoiceDocumentTypeDefinition.收據,
                    Message = e.Value.Message,
                    DataContent = info.ReceiptData.Receipt[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }

        private static void notifyExceptionWhenUploadReceiptCancellation(B2BExceptionInfo info)
        {
            int? companyID = info.Token != null ? info.Token.CompanyID : (int?)null;
            using (InvoiceManager mgr = new InvoiceManager())
            {
                var table = mgr.GetTable<ExceptionLog>();

                table.InsertAllOnSubmit(info.ExceptionItems.Select(e => new ExceptionLog
                {
                    CompanyID = companyID,
                    LogTime = DateTime.Now,
                    TypeID = (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢收據,
                    Message = e.Value.Message,
                    DataContent = info.CancelReceiptData.CancelReceipt[e.Key].GetXml()
                }));
                mgr.SubmitChanges();
            }
        }



    }


}
