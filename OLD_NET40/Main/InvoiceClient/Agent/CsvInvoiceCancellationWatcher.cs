using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Helper;
using InvoiceClient.Properties;
using Model.Schema.TXN;
using Utility;
using Model.Schema.EIVO;

namespace InvoiceClient.Agent
{
    public class CsvInvoiceCancellationWatcher : CsvInvoiceWatcher
    {

        public CsvInvoiceCancellationWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, System.Security.Cryptography.Pkcs.SignedCms docInv)
        {
            var result = invSvc.UploadInvoiceCancellationCmsCSV(docInv.Encode()).ConvertTo<Root>();
            return result;
        }

        protected override void processError(IEnumerable<RootResponseInvoiceNo> rootInvoiceNo, string fileName)
        {
            if (rootInvoiceNo != null && rootInvoiceNo.Count() > 0)
            {
                IEnumerable<String> message = rootInvoiceNo.Select(i => i.Description);
                Logger.Warn(String.Format("在上傳作廢發票檔({0})時,傳送失敗!!原因如下:\r\n{1}", fileName, String.Join("\r\n", message.ToArray())));
            }
        }


    }
}
