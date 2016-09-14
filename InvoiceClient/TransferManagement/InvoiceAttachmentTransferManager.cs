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
    public class InvoiceAttachmentTransferManager : ITransferManager
    {
        private InvoiceWatcher _InvoiceWatcher;

        public void EnableAll(String fullPath)
        {
            _InvoiceWatcher = new InvoiceAttachmentWatcherForGoogle(Path.Combine(fullPath, Settings.Default.UploadAttachmentFolder));
            _InvoiceWatcher.StartUp();
        }

        public void PauseAll()
        {
            if (_InvoiceWatcher != null)
            {
                _InvoiceWatcher.Dispose();
            }
        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_InvoiceWatcher != null)
                sb.Append(_InvoiceWatcher.ReportError());
            return sb.ToString();

        }

        public void SetRetry()
        {
            _InvoiceWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.GoogleAttachmentConfig); }
        }
    }
}
