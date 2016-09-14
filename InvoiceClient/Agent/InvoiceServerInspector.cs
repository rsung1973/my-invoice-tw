using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Properties;
using Utility;
using Model.Schema.TXN;
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;

namespace InvoiceClient.Agent
{

    public class InvoiceServerInspector : ServerInspector
    {
        public InvoiceServerInspector()
        {

        }


        public override void StartUp()
        {
            if (!_isRunning)
            {
                ThreadPool.QueueUserWorkItem(p =>
                {
                    _isRunning = true;
                    AcknowledgeServer();
                    Thread.Sleep(Settings.Default.AutoInvServiceInterval > 0 ? Settings.Default.AutoInvServiceInterval * 60 * 1000 : 1800000);
                    _isRunning = false;
                    StartUp();
                });
            }
        }

        public override Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.InvoiceServerConfig); }
        }

        //public List<string> ExceuteInvoiceService()
        //{
        //    List<String> pathInfo = new List<string>();
        //    this.ExecutiveService(pathInfo);
        //    return pathInfo;
        //}
    }
}
