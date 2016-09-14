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

    public class WinningInvoiceInspector : ServerInspector
    {

        public WinningInvoiceInspector()
        {

        }

        public Model.Schema.EIVO.BonusInvoiceRoot GetIncomingWinningInvoices()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = this.CreateMessageToken("下載發票中獎清冊");

                XmlNode doc = invSvc.GetIncomingWinningInvoices(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    String storedPath = Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadWinningFolder);
                    storedPath.CheckStoredPath();

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.Encoding = Settings.Default.OutputEncodingWithoutBOM ? new UTF8Encoding() : Encoding.GetEncoding(Settings.Default.OutputEncoding);

                    Model.Schema.EIVO.BonusInvoiceRoot result = doc.ConvertTo<Model.Schema.EIVO.BonusInvoiceRoot>();
                    String path = Path.Combine(storedPath, String.Format("{0:yyyyMMddHHmmssf}_BonusInvoice.xml", DateTime.Now));
                    doc.Attributes.RemoveAll();
                    doc.Save(path, settings);
                    return result;
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
                        GetIncomingWinningInvoices();
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
            if (GetIncomingWinningInvoices() != null)
                pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadWinningFolder));

            base.ExecutiveService(pathInfo);
        }
    }
}
