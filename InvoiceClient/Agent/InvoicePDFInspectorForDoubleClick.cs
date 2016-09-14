using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Net;

using InvoiceClient.Properties;
using Utility;
using Model.Schema.TXN;
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;

namespace InvoiceClient.Agent
{

    public class InvoicePDFInspectorForDoubleClick : InvoicePDFInspector
    {

        public InvoicePDFInspectorForDoubleClick() : base()
        {
            _prefix_name = "GOOG_AR_IN_TAIWAN_UXB2B_GUI_PDF_";
        }

        public override Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.GoogleInvoiceServerConfigForDoubleClick); }
        }

    }
}
