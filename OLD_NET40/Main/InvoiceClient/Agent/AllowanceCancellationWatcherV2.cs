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
    public class AllowanceCancellationWatcherV2 : AllowanceCancellationWatcher
    {
        public AllowanceCancellationWatcherV2(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadAllowanceCancellationV2(docInv).ConvertTo<Root>();
            return result;
        }

    }
}
