using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;
using System.ComponentModel;

namespace eIVOGo.Module.SCM.Item
{
    public partial class SelectBuyer : System.Web.UI.UserControl
    {
        private bool _onlyOne = true;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IList<BUYER_DATA> Items
        {
            get
            {
                return (IList<BUYER_DATA>)buyer.DataItem;
            }
            set
            {
                buyer.DataItem = value;
            }
        }

        [Bindable(true)]
        public bool OnlyOne
        {
            get
            {
                return _onlyOne;
            }
            set
            {
                _onlyOne = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(SelectBuyer_PreRender);
            buyerItem.Done += new EventHandler(buyerItem_Done);
            buyer.ItemType = typeof(List<BUYER_DATA>);
        }

        void buyerItem_Done(object sender, EventArgs e)
        {
            if (buyer.DataItem == null)
                buyer.DataItem = new List<BUYER_DATA>();

            if (!((IList<BUYER_DATA>)buyer.DataItem).Any(a => a.BUYER_SN == buyerItem.Item.BUYER_SN))
            {
                if (_onlyOne)
                {
                    ((IList<BUYER_DATA>)buyer.DataItem).Clear();
                }
                ((IList<BUYER_DATA>)buyer.DataItem).Add(buyerItem.Item);
            }
        }

        void SelectBuyer_PreRender(object sender, EventArgs e)
        {
            if (buyer.DataItem != null)
            {
                gvEntity.Visible = true;
                gvEntity.DataSource = buyer.DataItem;
                gvEntity.DataBind();
            }
        }

    }
}