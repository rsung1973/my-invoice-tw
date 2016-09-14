using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Utility;
using System.ComponentModel;
using System.Linq.Expressions;

namespace eIVOGo.Module.EIVO.Item
{
    public partial class InvoiceAllowanceMailView : System.Web.UI.UserControl
    {
        protected InvoiceAllowance _item;
        protected Organization _sellerOrg;

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
                _sellerOrg = null;
                printView.Item = _item;
                if (_item != null)
                {
                    _sellerOrg = _item.InvoiceItem.Organization;
                }
            }
        }
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoiceAllowenceMailView_PreRender);
        }

        void InvoiceAllowenceMailView_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                //rpList.DataSource = _item.InvoiceDetails;
                //rpList.DataBind();
                this.DataBind();
            }
        }
    }
}