using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using InvoiceClient.Properties;
using Model.Schema.EIVO;
using InvoiceClient.Agent;

namespace InvoiceClient.TransferManagement
{
    public class MIGInvoiceTransferManager : ITransferManager
    {
        private InvoiceWatcher _InvoiceWatcher;
        private InvoiceWatcher _CancellationWatcher;

        public void EnableAll(String fullPath)
        {
            _InvoiceWatcher = new MIGInvoiceWatcher(Path.Combine(fullPath, Settings.Default.UploadC0401SellerInvoiceFolder));
            _InvoiceWatcher.StartUp();

            _CancellationWatcher = new MIGInvoiceCancellationWatcher(Path.Combine(fullPath, Settings.Default.UploadC0501InvoiceCancellationFolder));
            _CancellationWatcher.StartUp();
        }

        public void PauseAll()
        {
            if (_InvoiceWatcher != null)
            {
                _InvoiceWatcher.Dispose();
            }
            if (_CancellationWatcher != null)
            {
                _CancellationWatcher.Dispose();
            }
        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());

            return sb.ToString();

        }

        public void SetRetry()
        {
            _InvoiceWatcher.Retry();
            _CancellationWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.MIGInvoiceConfig); }
        }
    }
}
