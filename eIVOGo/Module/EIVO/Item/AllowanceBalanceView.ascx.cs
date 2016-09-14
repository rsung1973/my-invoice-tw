using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Utility;

namespace eIVOGo.Module.EIVO.Item
{
    public partial class AllowanceBalanceView : System.Web.UI.UserControl
    {
        protected InvoiceAllowance _item;
        protected char[] _totalAmtChar;

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
                if (_item != null)
                {
                    _totalAmtChar = ((int)(_item.TotalAmount.Value + _item.TaxAmount.Value)).GetChineseNumberSeries(8);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(AllowanceBalanceView_PreRender);
        }

        void AllowanceBalanceView_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                rpList.DataSource = _item.InvoiceAllowanceDetails.Select(d => d.InvoiceAllowanceItem);
                rpList.DataBind();
                this.DataBind();
            }
        }

    }
}