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
    public class InvoiceTransferManagerV2ForGoogle : ITransferManager
    {

        private InvoiceWatcher _PreInvoiceWatcher;
        private InvoiceCancellationWatcherV2ForGoogle _CancellationWatcher;
        private AllowanceWatcherV2ForGoogle _AllowanceWatcher;
        private AllowanceCancellationWatcherV2ForGoogle _AllowanceCancellationWatcher;


        public void EnableAll(String fullPath)
        {



            _PreInvoiceWatcher = new InvoiceWatcherV2ForGoogle(Path.Combine(fullPath, Settings.Default.UploadPreInvoiceFolder));
            _PreInvoiceWatcher.StartUp();

            _CancellationWatcher = new InvoiceCancellationWatcherV2ForGoogle(Path.Combine(fullPath, Settings.Default.UploadInvoiceCancellationFolder));
            _CancellationWatcher.StartUp();

            _AllowanceWatcher = new AllowanceWatcherV2ForGoogle(Path.Combine(fullPath, Settings.Default.UploadAllowanceFolder));
            _AllowanceWatcher.StartUp();

            _AllowanceCancellationWatcher = new AllowanceCancellationWatcherV2ForGoogle(Path.Combine(fullPath, Settings.Default.UploadAllowanceCancellationFolder));
            _AllowanceCancellationWatcher.StartUp();
        }

        public void PauseAll()
        {
            if (_PreInvoiceWatcher != null)
            {
                _PreInvoiceWatcher.Dispose();
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
            if (_PreInvoiceWatcher != null)
                sb.Append(_PreInvoiceWatcher.ReportError());
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
            _PreInvoiceWatcher.Retry();
            _CancellationWatcher.Retry();
            _AllowanceWatcher.Retry();
            _AllowanceCancellationWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.GoogleInvoiceConfig); }
        }
    }
}
