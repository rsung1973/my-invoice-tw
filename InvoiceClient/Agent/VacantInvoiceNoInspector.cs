using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Properties;
using Utility;
using Model.Schema.TXN;
using InvoiceClient.Helper;
using InvoiceClient.TransferManagement;
using Model.Schema.TurnKey.E0402;

namespace InvoiceClient.Agent
{

    public class VacantInvoiceNoInspector : ServerInspector
    {
        private DateTime _dateCounter;

        public VacantInvoiceNoInspector()
        {
            initializeCounter();
        }

        private void initializeCounter()
        {
            _dateCounter = DateTime.Now;
        }

        public bool ReceiveVacantInvoiceNo()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();
            bool bRun = false;
            try
            {
                String storedPath = Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.VacantInvoiceNoFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.VacantInvoiceNoFolder);
                storedPath.CheckStoredPath();

                Root token = this.CreateMessageToken("接收空白發票號碼");
                XmlDocument signedReq = token.ConvertToXml().Sign();
                //XmlNode doc = invSvc.GetCurrentYearInvoiceTrackCode(signedReq);

                var queryDate = DateTime.Today.AddMonths(-2);
                var queryPeriod = queryDate.Month / 2 + queryDate.Month % 2;
                var issuers = invSvc.GetCustomerIdListByAgent(signedReq);
                if (issuers != null)
                {
                    foreach (var issuer in issuers)
                    {
                        if (Directory.GetFiles(storedPath, String.Format("{0}_{1}_{2}*.xml", issuer, queryDate.Year, queryPeriod)).Length == 0)
                        {
                            var resultNode = invSvc.GetVacantInvoiceNo(signedReq, issuer);
                            if (resultNode != null)
                            {
                                BranchTrackBlank[] items = resultNode.ConvertTo<BranchTrackBlank[]>();
                                foreach (var blank in items)
                                {
                                    String path = Path.Combine(storedPath, String.Format("{0}_{1}_{2}_{3}.xml", issuer, queryDate.Year, queryPeriod, blank.Main.InvoiceTrack));
                                    blank.ConvertToXml().Save(path);
                                    bRun = true;
                                }
                            }
                        }
                    }
                }

                return bRun;

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return false;
        }

        public override void StartUp()
        {
            if (Settings.Default.IsAutoInvService && !_isRunning)
            {
                ThreadPool.QueueUserWorkItem(p =>
                {
                    while (Settings.Default.IsAutoInvService)
                    {
                        _isRunning = true;

                        ReceiveVacantInvoiceNo();

                        Thread.Sleep(Settings.Default.AutoInvServiceInterval > 0 ? Settings.Default.AutoInvServiceInterval * 60 * 1000 : 1800000);
                    }
                    _isRunning = false;
                });
            }
        }

        public override Type UIConfigType
        {
            get { return null; }
        }

        public override void ExecutiveService(List<string> pathInfo)
        {
            if (ReceiveVacantInvoiceNo())
                pathInfo.Add(Settings.Default.DownloadDataInAbsolutePath ? Settings.Default.VacantInvoiceNoFolder : Path.Combine(Settings.Default.InvoiceTxnPath, Settings.Default.VacantInvoiceNoFolder));

            base.ExecutiveService(pathInfo);
        }
    }
}
