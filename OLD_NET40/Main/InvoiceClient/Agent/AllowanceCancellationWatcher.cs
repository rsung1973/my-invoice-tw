using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Helper;
using InvoiceClient.Properties;
using Model.Schema.EIVO;
using Model.Schema.TXN;
using Utility;

namespace InvoiceClient.Agent
{
    public class AllowanceCancellationWatcher : InvoiceWatcher
    {
        public AllowanceCancellationWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadAllowanceCancellation(docInv).ConvertTo<Root>();
            return result;
        }

        public override String ReportError()
        {
            int count = Directory.GetFiles(_failedTxnPath).Length;
            return count > 0 ? String.Format("{0}筆作廢發票折讓資料傳送失敗!!\r\n", count) : null;
        }

        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("作廢折讓證明單號碼:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("在上傳作廢折讓證明單檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                CancelAllowanceRoot invoice = docInv.ConvertTo<CancelAllowanceRoot>();
                CancelAllowanceRoot stored = new CancelAllowanceRoot();
                stored.CancelAllowance = rootInvoiceNo.Where(i => i.ItemIndexSpecified).Select(i => invoice.CancelAllowance[i.ItemIndex]).ToArray();

                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, String.Format("{0}-{1:yyyyMMddHHmmssfff}.xml", Path.GetFileName(fileName), DateTime.Now)));
            }
        }

        protected override void processError(string message, XmlDocument docInv, string fileName)
        {
            Logger.Warn(String.Format("在上傳作廢折讓證明單檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, message));
        }

    }
}
