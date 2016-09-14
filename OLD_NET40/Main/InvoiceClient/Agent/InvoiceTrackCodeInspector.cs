using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Properties;
using Utility;
using Model.Schema.TXN;
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;

namespace InvoiceClient.Agent
{

    public class InvoiceTrackCodeInspector : ServerInspector
    {
        private DateTime _dateCounter;

        public InvoiceTrackCodeInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
        }

        public bool ReceiveYearTrackCode()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.TrackCodeFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.TrackCodeFolder);
                storedPath.CheckStoredPath();
                String path = Path.Combine(storedPath, String.Format("{0:yyyy}_TrackCode.xml", DateTime.Today));

                if (File.Exists(path))
                    return false;

                Root token = this.CreateMessageToken("接收年度發票字軌");
                XmlDocument signedReq = token.ConvertToXml().Sign();
                XmlNode doc = invSvc.GetCurrentYearInvoiceTrackCode(signedReq);
                if (doc != null)
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);
                    var node = doc.SelectSingleNode("Response/InvoiceTrackCodeRoot");
                    if (node != null)
                        node.Save(path, settings);

                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return false;
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

                        ReceiveYearTrackCode();

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
            if (ReceiveYearTrackCode())
                pathInfo.Add(Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.TrackCodeFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.TrackCodeFolder));

            base.ExecutiveService(pathInfo);
        }
    }
}
