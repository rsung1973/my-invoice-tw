using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;
using System.ComponentModel;

namespace eIVOGo.Module.EIVO.Item
{
    public partial class InvoiceAllowanceMailPrintView : System.Web.UI.UserControl
    {
        protected InvoiceAllowance _item;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [Bindable(true)]
        public InvoiceAllowance Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoiceAllowenceMailPrintView_PreRender);
        }

        void InvoiceAllowenceMailPrintView_PreRender(object sender, EventArgs e)
        {
            if (Item != null)
            {
                rpList.DataSource = Item.InvoiceAllowanceDetails;
                rpList.DataBind();
            }
        }
    }
}