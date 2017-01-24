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
using System.Security.Cryptography.Pkcs;

namespace InvoiceClient.Agent
{
    public class InvoiceBuyerWatcher : CsvInvoiceWatcher
    {

        public InvoiceBuyerWatcher(String fullPath)
            : base(fullPath)
        {

        }


        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, SignedCms docInv)
        {
            var result = invSvc.UploadInvoiceBuyerCmsCSV(docInv.Encode()).ConvertTo<Root>();
            return result;
        }

    }
}
