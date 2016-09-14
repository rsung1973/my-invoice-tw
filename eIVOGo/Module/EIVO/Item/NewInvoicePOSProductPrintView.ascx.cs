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
    public partial class NewInvoicePOSProductPrintView : System.Web.UI.UserControl
    {
        protected InvoiceProduct _item;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Bindable(true)]
        public InvoiceProduct Item
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

        //public override void DataBind()
        //{
        //    if (Item != null)
        //    {
        //        base.DataBind();
        //    }
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoiceProductPrintView_PreRender);
        }

        void InvoiceProductPrintView_PreRender(object sender, EventArgs e)
        {
            if (Item != null)
            {
                rpList.DataSource = Item.InvoiceProductItem;
                rpList.DataBind();
                this.DataBind();
            }
        }

    }
}