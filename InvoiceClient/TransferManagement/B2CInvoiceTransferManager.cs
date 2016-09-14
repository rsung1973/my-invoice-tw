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
    public class B2CInvoiceTransferManager : ITransferManager
    {
        private InvoiceWatcher _InvoiceWatcher;
        private InvoiceCancellationWatcher _CancellationWatcher;
        private AllowanceWatcher _AllowanceWatcher;
        private AllowanceCancellationWatcher _AllowanceCancellationWatcher;


        public void EnableAll(String fullPath)
        {

            _InvoiceWatcher = new InvoiceWatcher(Path.Combine(fullPath, Settings.Default.UploadInvoiceFolder));
            _InvoiceWatcher.StartUp();

            _CancellationWatcher = new InvoiceCancellationWatcher(Path.Combine(fullPath, Settings.Default.UploadInvoiceCancellationFolder));
            _CancellationWatcher.StartUp();

            _AllowanceWatcher = new AllowanceWatcher(Path.Combine(fullPath, Settings.Default.UploadAllowanceFolder));
            _AllowanceWatcher.StartUp();

            _AllowanceCancellationWatcher = new AllowanceCancellationWatcher(Path.Combine(fullPath, Settings.Default.UploadAllowanceCancellationFolder));
            _AllowanceCancellationWatcher.StartUp();
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
            if (_AllowanceWatcher != null)
            {
                _AllowanceWatcher.Dispose();
            }
            if (_AllowanceCancellationWatcher != null)
            {
                _AllowanceCancellationWatcher.Dispose();
            }
        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());
            if (_AllowanceWatcher != null)
                sb.Append(_AllowanceWatcher.ReportError());
            if (_AllowanceCancellationWatcher != null)
                sb.Append(_AllowanceCancellationWatcher.ReportError());
            return sb.ToString();

        }

        public void SetRetry()
        {
            _InvoiceWatcher.Retry();
            _CancellationWatcher.Retry();
            _AllowanceWatcher.Retry();
            _AllowanceCancellationWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.B2CInvoiceCenterConfig); }
        }
    }
}
