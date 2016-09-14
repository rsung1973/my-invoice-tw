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
    public class InvoiceTransferManagerForAgent : ITransferManager
    {
        private InvoiceWatcher _InvoiceWatcher;
        private InvoiceWatcher _PreInvoiceWatcher;
        private InvoiceCancellationWatcher _CancellationWatcher;
        private AllowanceWatcher _AllowanceWatcher;
        private AllowanceCancellationWatcher _AllowanceCancellationWatcher;
        private InvoiceEnterpriseWatcher _EnterpriseWatcher;



        public void EnableAll(String fullPath)
        {
            _EnterpriseWatcher = new InvoiceEnterpriseWatcher(Path.Combine(fullPath, Settings.Default.UploadInvoiceEnterprise));
            _EnterpriseWatcher.StartUp();

            _InvoiceWatcher = new InvoiceWatcherV2(Path.Combine(fullPath, Settings.Default.UploadSellerInvoiceFolder));
            _InvoiceWatcher.StartUp();

            _PreInvoiceWatcher = new PreInvoiceWatcherV2(Path.Combine(fullPath, Settings.Default.UploadPreInvoiceFolder));
            _PreInvoiceWatcher.StartUp();

            _CancellationWatcher = new InvoiceCancellationWatcherV2(Path.Combine(fullPath, Settings.Default.UploadInvoiceCancellationFolder));
            _CancellationWatcher.StartUp();

            _AllowanceWatcher = new AllowanceWatcherV2(Path.Combine(fullPath, Settings.Default.UploadAllowanceFolder));
            _AllowanceWatcher.StartUp();

            _AllowanceCancellationWatcher = new AllowanceCancellationWatcherV2(Path.Combine(fullPath, Settings.Default.UploadAllowanceCancellationFolder));
            _AllowanceCancellationWatcher.StartUp();

            _InvoiceWatcher.InitializeDependency( _EnterpriseWatcher);
            _PreInvoiceWatcher.InitializeDependency(_EnterpriseWatcher);
            _CancellationWatcher.InitializeDependency(_InvoiceWatcher);
            _AllowanceCancellationWatcher.InitializeDependency( _AllowanceWatcher);
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
            if (_PreInvoiceWatcher != null)
            {
                _PreInvoiceWatcher.Dispose();
            }
            if (_AllowanceWatcher != null)
            {
                _AllowanceWatcher.Dispose();
            }
            if (_AllowanceCancellationWatcher != null)
            {
                _AllowanceCancellationWatcher.Dispose();
            }
            if (_EnterpriseWatcher != null)
            {
                _EnterpriseWatcher.Dispose();
            }

        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());
            if (_PreInvoiceWatcher != null)
                sb.Append(_PreInvoiceWatcher.ReportError());
            if (_AllowanceWatcher != null)
                sb.Append(_AllowanceWatcher.ReportError());
            if (_AllowanceCancellationWatcher != null)
                sb.Append(_AllowanceCancellationWatcher.ReportError());
            if (_EnterpriseWatcher != null)
                sb.Append(_EnterpriseWatcher.ReportError());

            return sb.ToString();

        }

        public void SetRetry()
        {
            _InvoiceWatcher.Retry();
            _CancellationWatcher.Retry();
            _PreInvoiceWatcher.Retry();
            _AllowanceWatcher.Retry();
            _AllowanceCancellationWatcher.Retry();
            _EnterpriseWatcher.Retry();

        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.B2CInvoiceCenterConfig); }
        }
    }
}
