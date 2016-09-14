using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Net;

using InvoiceClient.Properties;
using Utility;
using Model.Schema.TXN;
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;

namespace InvoiceClient.Agent
{

    public class InvoiceMailTrackingInspector : ServerInspector
    {
        private DateTime _dateCounter;

        protected String _prefix_name = "taiwan_uxb2b_mail_tracking_";
        protected String _prefix_name_returned = "taiwan_uxb2b_returned_mail_tracking_";

        public InvoiceMailTrackingInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
        }

        public String GetInvoiceMailTracking()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載發票掛號郵件追蹤號碼記錄檔");
                String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadInvoiceMailTracking : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceMailTracking);
                ValueValidity.CheckAndCreatePath(storedPath);
                //storedPath = ValueValidity.GetDateStylePath(storedPath);
                bool hasNew = false;
                String path = Path.Combine(Logger.LogDailyPath, _prefix_name + DateTime.Now.ToString("yyyyMMddHHmmssf") + ".csv");
                //                String path = Path.Combine(storedPath, String.Format("taiwan_uxb2b_mail_tracking_{0:yyyyMMddHHmmssf}.csv", DateTime.Now));
                XmlDocument signedReq = token.ConvertToXml().Sign();

                string[] items = invSvc.GetInvoiceMailTracking(signedReq, Settings.Default.ClientID);
                //  string[] items = { "2014/08/06,0000,12345678,012345,test1,test2", "2014/08/06,0001,12345678,012345,test1,test2"};//測試用
                if (items != null && items.Length > 0)
                {
                    hasNew = true;
                    string strValue = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Delivery Date| GoogleId| Invoice No| Mailing No| Contact Name| Contact Address");
                    sb.Append("\n");
                    foreach (var item in items)
                    {
                        sb.Append(item);
                        // sb.Append("\t,");
                        sb.Append("\t\n");

                    }

                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        File.WriteAllText(path, sb.ToString(), System.Text.Encoding.GetEncoding("big5"));
                        //sb..SaveAs(path);
                    }

                    String targetPath = Path.Combine(storedPath, Path.GetFileName(path));
                    if (PGPCrypto.Instance.Ready)
                    {
                        PGPCrypto.Instance.EncryptFile(path);
                        //File.Delete(path);
                        path += ".gpg";
                        targetPath += ".gpg";
                    }
                    //if (File.Exists(targetPath))
                    //{
                    //    File.Delete(targetPath);
                    //}
                    File.Copy(path, targetPath, true);

                }
                return hasNew ? storedPath : null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        public String GetInvoiceReturnedMail()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載發票掛號郵件退件重送記錄檔");
                String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadInvoiceReturnedMail : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceReturnedMail);
                ValueValidity.CheckAndCreatePath(storedPath);
                //storedPath = ValueValidity.GetDateStylePath(storedPath);
                bool hasNew = false;
                String path = Path.Combine(Logger.LogDailyPath, _prefix_name_returned + DateTime.Now.ToString("yyyyMMddHHmmssf") + ".csv");
                XmlDocument signedReq = token.ConvertToXml().Sign();

                string[] items = invSvc.GetInvoiceReturnedMail(signedReq, Settings.Default.ClientID);
               // string[] items = { "2014/08/06,0000,12345678,012345,test1,test2,test3", "2014/08/06,0001,12345678,012345,test1,test2,test3" };//測試用
                if (items != null && items.Length > 0)
                {
                    hasNew = true;
                    string strValue = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Resending Date| GoogleId| Invoice No| Mailing No| Contact Name| Contact Address| Remark");
                    sb.Append("\n");
                    foreach (var item in items)
                    {
                        sb.Append(item);
                        sb.Append("\t\n");

                    }
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        File.WriteAllText(path, sb.ToString(), System.Text.Encoding.GetEncoding("big5"));
                    }

                    String targetPath = Path.Combine(storedPath, Path.GetFileName(path));
                    if (PGPCrypto.Instance.Ready)
                    {
                        PGPCrypto.Instance.EncryptFile(path);
                        //File.Delete(path);
                        path += ".gpg";
                        targetPath += ".gpg";
                    }
                    //if (File.Exists(targetPath))
                    //{
                    //    File.Delete(targetPath);
                    //}
                    //File.Move(path, targetPath);
                    File.Copy(path, targetPath, true);

                }
                return hasNew ? storedPath : null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        public override void StartUp()
        {
            if (Settings.Default.IsAutoInvService && !_isRunning)
            {
                ThreadPool.QueueUserWorkItem(p =>
                {
                    while (Settings.Default.IsAutoInvService)
                    {
                        _isRunning = true;

                        GetInvoiceMailTracking();
                        GetInvoiceReturnedMail();
                        Thread.Sleep(Settings.Default.AutoInvServiceInterval > 0 ? Settings.Default.AutoInvServiceInterval * 60 * 1000 : 1800000);
                    }
                    _isRunning = false;
                });
            }
        }

        public override Type UIConfigType
        {
            get { return null; }
        }

        public override void ExecutiveService(List<string> pathInfo)
        {
            var path = GetInvoiceMailTracking();
            if (path != null)
                pathInfo.Add(path);
            path = GetInvoiceReturnedMail();
            if (path != null)
                pathInfo.Add(path);

            base.ExecutiveService(pathInfo);
        }
    }
}
