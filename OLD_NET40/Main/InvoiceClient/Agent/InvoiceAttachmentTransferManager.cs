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
    public static class InvoiceAttachmentTransferManager
    {
        private static InvoiceWatcher _InvoiceWatcher;

        public static void EnableAll(String fullPath)
        {
            _InvoiceWatcher = new InvoiceAttachmentWatcher(Path.Combine(fullPath, Settings.Default.UploadCheckedStatementFolder));
            _InvoiceWatcher.StartUp();
        }

        public static void PauseAll()
        {
            if (_InvoiceWatcher != null)
            {
                _InvoiceWatcher.Dispose();
            }
        }

        public static String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());

            return sb.ToString();

        }

        public static void SetRetry()
        {
            _InvoiceWatcher.Retry();
        }

    }
}
