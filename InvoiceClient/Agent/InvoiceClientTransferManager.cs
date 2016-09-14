using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using InvoiceClient.Properties;
using Model.Schema.EIVO;
using System.Xml;

namespace InvoiceClient.Agent
{
    public static class InvoiceClientTransferManager
    {
        private static InvoiceWelfareInspector _WelfareInspector = new InvoiceWelfareInspector();
        private static InvoiceServerInspector _ServerInspector = new InvoiceServerInspector();
        private static ServiceController _ServiceController;

        public static void StartUp(String fullPath)
        {
            ResetServiceController();
            B2CInvoiceTransferManager.PauseAll();
            PhysicalChannelInvoiceTransferManager.PauseAll();
            CsvInvoiceTransferManager.PauseAll();

            if (_ServiceController == null || _ServiceController.Status != ServiceControllerStatus.Running)
            {

                if (!Settings.Default.DisableB2CInvoiceCenterTab)
                {
                    B2CInvoiceTransferManager.EnableAll(fullPath);
                }
                if (!Settings.Default.DisablePhysicalChannelTab)
                {
                    PhysicalChannelInvoiceTransferManager.EnableAll(fullPath);
                }
                if (!Settings.Default.DisableCsvInvoiceCenterTab)
                {
                    CsvInvoiceTransferManager.EnableAll(fullPath);
                }
            }
        }

        public static void ResetServiceController()
        {
            if (Environment.UserInteractive)
            {
                _ServiceController = ServiceController.GetServices().Where(s => s.ServiceName == "InvoiceClientService").FirstOrDefault();
            }
        }

        public static void ClearServiceController()
        {
            _ServiceController = null;
        }

        public static ServiceController ServiceInstance
        {
            get
            {
                return _ServiceController;
            }
        }


        public static SocialWelfareAgenciesRoot UpdateWelfareAgency()
        {
            return _WelfareInspector.GetUpdatedData();
        }

        public static SocialWelfareAgenciesRoot GetWelfareAgency()
        {
            return _WelfareInspector.GetAll();
        }


        public static void SetAutoUpdateWelfareAgency()
        {
            _WelfareInspector.StartUp();
        }

        public static void SetAutoInvoiceService()
        {
            _ServerInspector.StartUp();
        }

        public static List<String> ExceuteInvoiceService()
        {
            List<String> pathInfo = new List<string>();
            IEnumerable<XmlNode> items;

            if (!Settings.Default.DisableDownloadDispatch)
            {
                items = _ServerInspector.GetIncomingInvoices();
                if (items != null && items.Count() > 0)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceFolder));
                items = _ServerInspector.GetIncomingInvoiceCancellations();
                if (items != null && items.Count() > 0)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceCancellationFolder));
                items = _ServerInspector.GetIncomingAllowances();
                if (items != null && items.Count() > 0)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceFolder));
                items = _ServerInspector.GetIncomingAllowanceCancellations();
                if (items != null && items.Count() > 0)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadAllowanceCancellationFolder));
            }

            ///實體通路不用自動對獎
            ///
            if (Settings.Default.DisablePhysicalChannelTab)
            {
                if (_ServerInspector.GetIncomingWinningInvoices() != null)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadWinningFolder));
            }

            if (!Settings.Default.DisableDownloadInvoiceMapping)
            {
                items = _ServerInspector.ReceiveInvoices();
                if (items != null && items.Count() > 0)
                    pathInfo.Add(Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.DownloadInvoiceMapping));
            }
            return pathInfo;
        }

    }
}
