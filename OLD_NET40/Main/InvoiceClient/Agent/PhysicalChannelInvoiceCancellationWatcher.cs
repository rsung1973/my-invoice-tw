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
    public class PhysicalChannelInvoiceCancellationWatcher : InvoiceCancellationWatcher
    {
        public PhysicalChannelInvoiceCancellationWatcher(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.B2CUploadInvoiceCancellation(docInv).ConvertTo<Root>();
            return result;
        }
    }
}
