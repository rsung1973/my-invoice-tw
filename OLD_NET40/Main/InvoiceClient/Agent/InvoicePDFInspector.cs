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
using System.Diagnostics;

namespace InvoiceClient.Agent
{

    public class InvoicePDFInspector : ServerInspector
    {
        protected DateTime _dateCounter;

        protected String _prefix_name = "taiwan_uxb2b_scanned_gui_pdf_";

        public InvoicePDFInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
        }

       public String GetSaleInvoices()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載電子發票PDF");
                String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadSaleInvoiceFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadSaleInvoiceFolder);
                ValueValidity.CheckAndCreatePath(storedPath);
                //storedPath = ValueValidity.GetDateStylePath(storedPath);

                String tmpPath = Path.Combine(Logger.LogDailyPath, Guid.NewGuid().ToString());

                bool hasNew = false;

                XmlDocument signedReq = token.ConvertToXml().Sign();
                string[] items = invSvc.ReceiveContentAsPDF(signedReq, Settings.Default.ClientID);
                if (items != null && items.Length > 0)
                {
                    hasNew = true;
                    Directory.CreateDirectory(tmpPath);

                    using (WebClient wc = new WebClient())
                    {

                        foreach (var item in items)
                        {
                            com.uxb2b.util.CipherDecipherSrv cipher = new com.uxb2b.util.CipherDecipherSrv();
                            String[] paramValue = cipher.decipher(item.Split('?')[1]).Split(':');
                            String invNo = paramValue[0];

                            String pdfFile = Path.Combine(tmpPath,
                                _prefix_name + paramValue[1] + "_" + invNo + ".pdf");
                            //String pdfFile = Path.Combine(storedPath, string.Format("{0}{1}.pdf", "taiwan_uxb2b_scanned_gui_pdf_", invNo));

                            token = this.CreateMessageToken("已下載電子發票PDF:" + invNo);// 
                            signedReq = token.ConvertToXml().Sign();
                            var content = signedReq.UploadData(item);
                            if (content != null && content.Length > 0)
                            {
                                content.SaveAs(pdfFile);
                                bool DeleteTempForReceivePDF = invSvc.DeleteTempForReceivePDF(signedReq, item);

                                //String targetPath = Path.Combine(storedPath, Path.GetFileName(pdfFile));
                                //if (PGPCrypto.Instance.Ready)
                                //{
                                //    PGPCrypto.Instance.EncryptFile(pdfFile);
                                //    File.Delete(pdfFile);
                                //    pdfFile += ".gpg";
                                //    targetPath += ".gpg";
                                //}

                                //if (File.Exists(targetPath))
                                //{
                                //    File.Delete(targetPath);
                                //}
                                //File.Move(pdfFile, targetPath);
                            }
                        }
                    }
                }

                if(hasNew)
                {
                    Process proc = Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ZipPDF.bat"),
                        String.Format("{0}{1:yyyyMMddHHmmssffff} \"{2}\" \"{3}\"", _prefix_name, DateTime.Now, tmpPath, storedPath));
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

                        ServerInspector.AcknowledgeServer();
                        GetSaleInvoices();

                        Thread.Sleep(Settings.Default.AutoInvServiceInterval > 0 ? Settings.Default.AutoInvServiceInterval * 60 * 1000 : 1800000);
                    }
                    _isRunning = false;
                });
            }
        }

        public override Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.GoogleInvoiceServerConfig); }
        }

        public override void ExecutiveService(List<string> pathInfo)
        {
            var path = this.GetSaleInvoices();
            if (path!=null)
                pathInfo.Add(path);
            
            base.ExecutiveService(pathInfo);
        }
    }
}
