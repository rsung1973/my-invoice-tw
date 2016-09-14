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

    public class DispatchInvoiceInspector : ServerInspector
    {
        private DateTime _dateCounter;
        private int _invoiceCounter;
        private int _invoiceCancellationCounter;
        private int _allowanceCounter;
        private int _allowanceCancellationCounter;

        public DispatchInvoiceInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
            _invoiceCounter = 1;
            _invoiceCancellationCounter = 1;
            _allowanceCounter = 1;
            _allowanceCancellationCounter = 1;
        }

        public IEnumerable<XmlNode> GetIncomingInvoices()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載傳送大平台電子發票");

                XmlNode doc = invSvc.GetIncomingInvoices(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadInvoiceFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceFolder);
                    storedPath.CheckStoredPath();

                    if (_dateCounter < DateTime.Today)
                    {
                        initializeCounter();
                    }
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);

                    var nodes = doc.SelectNodes("Response/Invoice");
                    foreach (XmlNode invNode in nodes)
                    {
                        String path = Path.Combine(storedPath, String.Format("A0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", _dateCounter, _invoiceCounter++));
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

        public IEnumerable<XmlNode> GetIncomingInvoiceCancellations()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載傳送大平台作廢電子發票");

                XmlNode doc = invSvc.GetIncomingInvoiceCancellations(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadInvoiceCancellationFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceCancellationFolder);
                    storedPath.CheckStoredPath();

                    if (_dateCounter < DateTime.Today)
                    {
                        initializeCounter();
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);

                    var nodes = doc.SelectNodes("Response/CancelInvoice");
                    foreach (XmlNode invNode in nodes)
                    {
                        String path = Path.Combine(storedPath, String.Format("A0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", _dateCounter, _invoiceCancellationCounter++));
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

        public IEnumerable<XmlNode> GetIncomingAllowances()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載傳送大平台電子發票折讓證單");

                XmlNode doc = invSvc.GetIncomingAllowances(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadAllowanceFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceFolder);
                    storedPath.CheckStoredPath();

                    if (_dateCounter < DateTime.Today)
                    {
                        initializeCounter();
                    }
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);

                    var nodes = doc.SelectNodes("Response/Allowance");
                    foreach (XmlNode invNode in nodes)
                    {
                        String path = Path.Combine(storedPath, String.Format("B0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", _dateCounter, _allowanceCounter++));
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

        public IEnumerable<XmlNode> GetIncomingAllowanceCancellations()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載傳送大平台作廢電子發票折讓證明");

                XmlNode doc = invSvc.GetIncomingAllowanceCancellations(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.DownloadAllowanceCancellationFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceCancellationFolder);
                    storedPath.CheckStoredPath();

                    if (_dateCounter < DateTime.Today)
                    {
                        initializeCounter();
                    }

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);

                    var nodes = doc.SelectNodes("Response/CancelAllowance");
                    foreach (XmlNode invNode in nodes)
                    {
                        String path = Path.Combine(storedPath, String.Format("B0501-{0:yyyyMMddHHmmssf}-{1:00000}.xml", _dateCounter, _allowanceCancellationCounter++));
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

                        GetIncomingInvoices();
                        GetIncomingInvoiceCancellations();
                        GetIncomingAllowances();
                        GetIncomingAllowanceCancellations();

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
            IEnumerable<XmlNode> items;

            items = GetIncomingInvoices();
            if (items != null && items.Count() > 0)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceFolder));
            items = GetIncomingInvoiceCancellations();
            if (items != null && items.Count() > 0)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceCancellationFolder));
            items = GetIncomingAllowances();
            if (items != null && items.Count() > 0)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceFolder));
            items = GetIncomingAllowanceCancellations();
            if (items != null && items.Count() > 0)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceCancellationFolder));

            base.ExecutiveService(pathInfo);
        }
    }
}
