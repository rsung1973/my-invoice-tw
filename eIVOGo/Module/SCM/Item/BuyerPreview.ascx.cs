using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM.Item
{
    public partial class BuyerPreview : SelectBuyer
    {
        protected override void OnInit(EventArgs e)
        {
            this.PreRender += new EventHandler(BuyerPreview_PreRender);
            buyer.ItemType = typeof(List<BUYER_DATA>);
        }

        void BuyerPreview_PreRender(object sender, EventArgs e)
        {
            if (buyer.DataItem != null)
            {
                rpBuyer.DataSource = buyer.DataItem;
                rpBuyer.DataBind();
            }
        }

    }
}