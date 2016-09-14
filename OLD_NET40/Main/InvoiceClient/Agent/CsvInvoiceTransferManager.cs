using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using InvoiceClient.Properties;
using Model.Schema.EIVO;

namespace InvoiceClient.Agent
{
    public static class CsvInvoiceTransferManager
    {
        private static InvoiceWatcher _InvoiceWatcher;
        private static InvoiceWatcher _CancellationWatcher;

        public static void EnableAll(String fullPath)
        {
            _InvoiceWatcher = new CsvInvoiceWatcher(Path.Combine(fullPath, Settings.Default.UploadCsvInvoiceFolder));
            _InvoiceWatcher.StartUp();

            _CancellationWatcher = new CsvInvoiceCancellationWatcher(Path.Combine(fullPath, Settings.Default.UploadCsvInvoiceCancellationFolder));
            _CancellationWatcher.StartUp();
        }

        public static void PauseAll()
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

        public static String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());

            return sb.ToString();

        }

        public static void SetRetry()
        {
            _InvoiceWatcher.Retry();
            _CancellationWatcher.Retry();
        }

    }
}
