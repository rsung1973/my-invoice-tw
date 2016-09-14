using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

using InvoiceClient.Agent;
using InvoiceClient.Helper;
using InvoiceClient.Properties;
using Model.Schema.EIVO;
using Model.Schema.TXN;
using Utility;

namespace InvoiceClient.TransferManagement
{

    public abstract class ServerInspector
    {
        protected bool _isRunning;
        public abstract void StartUp();
        public abstract Type UIConfigType { get; }
        public ServerInspector ChainedInspector
        { get; set; }
        public virtual void ExecutiveService(List<String> pathInfo)
        {
            if (ChainedInspector != null)
            {
                ChainedInspector.ExecutiveService(pathInfo);
            }
        }

        public static Model.DataEntity.Organization GetRegisterdMember()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = invSvc.CreateMessageToken("讀取用戶端資料");
                XmlNode doc = invSvc.GetRegisteredMember(token.ConvertToXml().Sign());
                if (doc != null)
                {
                    return doc.DeserializeDataContract<Model.DataEntity.Organization>();
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            return null;
        }

        public static void AcknowledgeServer()
        {
            WS_Invoice.eInvoiceService invSvc = InvoiceWatcher.CreateInvoiceService();

            try
            {
                Root token = invSvc.CreateMessageToken("用戶端系統正在執行中");
                invSvc.AcknowledgeLivingReport(token.ConvertToXml().Sign());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
