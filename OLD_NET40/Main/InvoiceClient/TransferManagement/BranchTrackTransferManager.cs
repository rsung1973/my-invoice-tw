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
    public class BranchTrackTransferManager : ITransferManager
    {
        private BranchTrackWatcher _BranchTrackWatcher;

        public void EnableAll(String fullPath)
        {
            _BranchTrackWatcher = new BranchTrackWatcher(Path.Combine(fullPath, Settings.Default.UploadBranchTrackFolder));
            _BranchTrackWatcher.StartUp();
        }

        public void PauseAll()
        {
            if (_BranchTrackWatcher != null)
            {
                _BranchTrackWatcher.Dispose();
            }
        }

        public String ReportError()
        {
            StringBuilder sb = new StringBuilder();
            if (_BranchTrackWatcher != null)
                sb.Append(_BranchTrackWatcher.ReportError());
            return sb.ToString();

        }

        public void SetRetry()
        {
            _BranchTrackWatcher.Retry();
        }



        public Type UIConfigType
        {
            get { return null; }
        }
    }
}
