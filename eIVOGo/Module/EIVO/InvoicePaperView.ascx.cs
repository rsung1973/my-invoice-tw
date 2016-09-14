using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;

namespace eIVOGo.Module.EIVO
{
    public partial class InvoicePaperView : System.Web.UI.UserControl
    {
        protected InvoiceItem _item;
        protected InvoiceBuyer _buyer;
        protected Organization _buyerOrg;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Bindable(true)]
        public int? InvoiceID
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(InvoicePrintView_PreRender);
        }

        void InvoicePrintView_PreRender(object sender, EventArgs e)
        {
            bindData();
        }

        public override void DataBind()
        {
            bindData();
        }

        private void bindData()
        {
            if (InvoiceID.HasValue)
            {
                var mgr = dsEntity.CreateDataManager();
                _item = mgr.EntityList.Where(i => i.InvoiceID == InvoiceID).FirstOrDefault();
                _buyer = _item.InvoiceBuyer;
                if (_buyer != null && _buyer.BuyerID.HasValue)
                    _buyerOrg = _buyer.Organization;

                receiptView.Item = _item;
                balanceView.Item = _item;
                balanceView.Visible = !_item.InvoiceBuyer.IsB2C();
                base.DataBind();
            }
        }
        
    }
}