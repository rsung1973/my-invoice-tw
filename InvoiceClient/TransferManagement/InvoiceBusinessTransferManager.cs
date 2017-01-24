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
    public class InvoiceBusinessTransferManager : ITransferManager
    {
        private InvoiceWatcher _BuyerWatcher;

        public void EnableAll(String fullPath)
        {
            _BuyerWatcher = new InvoiceBuyerWatcher(Path.Combine(fullPath, Settings.Default.UploadCsvBuyerFolder));
            _BuyerWatcher.StartUp();
        }

        public void PauseAll()
        {
            if (_BuyerWatcher != null)
            {
                _BuyerWatcher.Dispose();
            }
        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_BuyerWatcher != null)
                sb.Append(_BuyerWatcher.ReportError());

            return sb.ToString();

        }

        public void SetRetry()
        {
            _BuyerWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return typeof(InvoiceClient.MainContent.CsvInvoiceCenterConfig); }
        }
    }
}
