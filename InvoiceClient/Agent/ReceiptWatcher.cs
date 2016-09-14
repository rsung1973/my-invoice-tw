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
    public class ReceiptWatcher : InvoiceWatcher
    {
        public ReceiptWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.B2BUploadReceipt(docInv).ConvertTo<Root>();
            return result;
        }

        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("收據號碼:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("在上傳收據檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                ReceiptRoot invoice = docInv.ConvertTo<ReceiptRoot>();
                ReceiptRoot stored = new ReceiptRoot();
                stored.Receipt = rootInvoiceNo.Where(i => i.ItemIndexSpecified).Select(i => invoice.Receipt[i.ItemIndex]).ToArray();

                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, String.Format("{0}-{1:yyyyMMddHHmmssfff}.xml", Path.GetFileName(fileName), DateTime.Now)));
            }
        }


    }
}
