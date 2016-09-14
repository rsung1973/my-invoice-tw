using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Helper;
using InvoiceClient.Properties;
using Model.Schema.EIVO.B2B;
using Model.Schema.TXN;
using Utility;

namespace InvoiceClient.Agent
{
    public class B2BInvoiceCancellationWatcher : InvoiceCancellationWatcher
    {
        public B2BInvoiceCancellationWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.B2BUploadInvoiceCancellation(docInv).ConvertTo<Root>();
            return result;
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
                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, String.Format("{0}-{1:yyyyMMddHHmmssfff}.xml", Path.GetFileName(fileName), DateTime.Now)));
            }
        }
    }
}
