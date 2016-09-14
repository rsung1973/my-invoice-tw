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
    public class AllowanceWatcherV2 : AllowanceWatcher
    {
        public AllowanceWatcherV2(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadAllowanceV2(docInv).ConvertTo<Root>();
            return result;
        }
    }
}
