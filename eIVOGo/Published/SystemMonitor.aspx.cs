using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.InvoiceManagement;
using Model.DataEntity;
using Model.Locale;

namespace eIVOGo.Published
{
    public partial class SystemMonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(SystemMonitor_PreRender);
        }

        void SystemMonitor_PreRender(object sender, EventArgs e)
        {
            var mgr = dsEntity.CreateDataManager();
            var replication = mgr.GetTable<DocumentReplication>();
            var invItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice);
            gvInvoice.DataSource = invItems;
            gvInvoice.DataBind();

            var cancelItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation);
            gvCancel.DataSource = cancelItems;
            gvCancel.DataBind();
        }

        protected void btnGov_Click(object sender, EventArgs e)
        {
            GovPlatformFactory.Notify();
        }
    }
}