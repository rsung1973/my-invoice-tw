using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using Model.Locale;
using System.Linq.Expressions;

namespace eIVOGo.Module.UI
{
    public partial class GovPlatformNotification : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<DocumentReplication, bool>> QueryExpr
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(GovPlatformNotification_PreRender);
        }

        void GovPlatformNotification_PreRender(object sender, EventArgs e)
        {
            var mgr = dsEntity.CreateDataManager();
            IQueryable<DocumentReplication> replication = QueryExpr != null ? mgr.GetTable<DocumentReplication>().Where(QueryExpr) : mgr.GetTable<DocumentReplication>();
            var invItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice);
            gvInvoice.DataSource = invItems;
            gvInvoice.DataBind();

            var cancelItems = replication.Where(r => r.TypeID == (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation);
            gvCancel.DataSource = cancelItems;
            gvCancel.DataBind();
        }
    }
}