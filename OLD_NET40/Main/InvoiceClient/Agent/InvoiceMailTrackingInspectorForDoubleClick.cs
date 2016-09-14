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

    public class InvoiceMailTrackingInspectorForDoubleClick : InvoiceMailTrackingInspector
    {

        public InvoiceMailTrackingInspectorForDoubleClick() :base()
        {
            _prefix_name = "GOOG_AR_IN_TAIWAN_UXB2B_GUI_MAIL_TRACKING_";
            _prefix_name_returned = "GOOG_AR_IN_TAIWAN_UXB2B_GUI_RETURNED_MAIL_TRACKING_";
        }

    }
}
