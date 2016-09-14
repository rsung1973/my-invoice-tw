using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InvoiceClient.Properties;
using System.Xml;
using Utility;
using InvoiceClient.Helper;
using Model.Schema.EIVO;
using System.Threading;
using Model.Schema.TXN;

namespace InvoiceClient.Agent
{
    public class InvoiceCancellationWatcher : InvoiceWatcher
    {
        public InvoiceCancellationWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadInvoiceCancellation(docInv).ConvertTo<Root>();
            return result;
        }

        public override String ReportError()
        {
            int count = Directory.GetFiles(_failedTxnPath).Length;
            return count > 0 ? String.Format("{0}筆作廢發票資料傳送失敗!!\r\n", count) : null;
        }

        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("作廢發票號碼:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("在上傳作廢發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                CancelInvoiceRoot invoice = docInv.ConvertTo<CancelInvoiceRoot>();
                CancelInvoiceRoot stored = new CancelInvoiceRoot();
                stored.CancelInvoice = rootInvoiceNo.Where(i => i.ItemIndexSpecified).Select(i => invoice.CancelInvoice[i.ItemIndex]).ToArray();

                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, fileName));
            }
        }

        protected override void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("在上傳作廢發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, message));
        }

    }
}
