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
    public class B2BAllowanceWatcher : AllowanceWatcher
    {
        public B2BAllowanceWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.B2BUploadAllowance(docInv).ConvertTo<Root>();
            return result;
        }

        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, XmlDocument docInv, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => String.Format("折讓證明單號碼:{0}=>{1}", i.Value, i.Description));
                Logger.Warn(String.Format("在上傳折讓證明單檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));

                AllowanceRoot invoice = docInv.ConvertTo<AllowanceRoot>();
                AllowanceRoot stored = new AllowanceRoot();
                stored.Allowance = rootInvoiceNo.Where(i=>i.ItemIndexSpecified).Select(i => invoice.Allowance[i.ItemIndex]).ToArray();
                stored.ConvertToXml().Save(Path.Combine(_failedTxnPath, String.Format("{0}-{1:yyyyMMddHHmmssfff}.xml", Path.GetFileName(fileName), DateTime.Now)));
            }
        }
    }
}
