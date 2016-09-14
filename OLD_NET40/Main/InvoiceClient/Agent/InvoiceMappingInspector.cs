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

    public class InvoiceMappingInspector : ServerInspector
    {
        private DateTime _dateCounter;

        public InvoiceMappingInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
        }

        public IEnumerable<XmlNode> ReceiveInvoices()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("接收銷項電子發票");
                XmlDocument signedReq = token.ConvertToXml().Sign();
                XmlNode doc = invSvc.GetInvoicesMap(signedReq);
                if (doc != null)
                {
                    String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadInvoiceMapping : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceMapping);
                    storedPath.CheckStoredPath();
                    if (_dateCounter < DateTime.Now)
                    {
                        initializeCounter();
                    }
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);
                    var nodes = doc.SelectNodes("Response/InvoiceMapRoot");
                    foreach (XmlNode invNode in nodes)
                    {
                        String path = Path.Combine(storedPath, String.Format("{0:yyyyMMddHHmmssf}_InvoiceMap.xml", _dateCounter));
                        invNode.Save(path, settings);

                    }
                    return nodes.Cast<XmlNode>();
                }
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

                        ReceiveInvoices();

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
            IEnumerable<XmlNode> items = ReceiveInvoices();
            if (items != null && items.Count() > 0)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceMapping));

            base.ExecutiveService(pathInfo);
        }
    }
}
