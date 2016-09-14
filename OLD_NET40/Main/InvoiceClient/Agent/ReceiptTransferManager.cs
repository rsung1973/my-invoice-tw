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
    public static class ReceiptTransferManager
    {
        private static InvoiceWatcher _receiptWatcher;
        private static ReceiptCancellationWatcher _CancellationWatcher;

        public static void EnableAll(String fullPath)
        {
            _receiptWatcher = new ReceiptWatcher(Path.Combine(fullPath, Settings.Default.B2BUploadReceiptFolder));
            _receiptWatcher.StartUp();
            _CancellationWatcher = new ReceiptCancellationWatcher(Path.Combine(fullPath, Settings.Default.B2BUploadReceiptCancellationFolder));
            _CancellationWatcher.StartUp();
        }

        public static void PauseAll()
        {
            if (_receiptWatcher != null)
            {
                _receiptWatcher.Dispose();
            }
            if (_CancellationWatcher != null)
            {
                _CancellationWatcher.Dispose();
            }
        }

        public static String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_receiptWatcher != null)
                sb.Append(_receiptWatcher.ReportError());
            if (_CancellationWatcher != null)
                sb.Append(_CancellationWatcher.ReportError());

            return sb.ToString();

        }

        public static void SetRetry()
        {
            _receiptWatcher.Retry();
            _CancellationWatcher.Retry();
        }

    }
}
