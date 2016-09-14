using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using InvoiceClient.Agent;
using InvoiceClient.Properties;
using InvoiceClient.TransferManagement;

namespace InvoiceClient
{
    partial class InvoiceClientService : ServiceBase
    {
        public InvoiceClientService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            InvoiceClientTransferManager.StartUp(Settings.Default.InvoiceTxnPath);
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
