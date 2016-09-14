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
    public class InvoiceWatcherV2ForGoogle : InvoiceWatcherForGoogle
    {
        public InvoiceWatcherV2ForGoogle(String fullPath)
            : base(fullPath)
        {

        }

        protected override Root processUpload(WS_Invoice.eInvoiceService invSvc, XmlDocument docInv)
        {
            var result = invSvc.UploadInvoiceAutoTrackNoByClient(docInv,Settings.Default.ClientID).ConvertTo<Root>();
            return result;
        }

    }
}
