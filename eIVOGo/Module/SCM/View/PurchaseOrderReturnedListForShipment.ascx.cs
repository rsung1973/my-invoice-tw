using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using eIVOGo.Helper;
using System.Linq.Expressions;
using Model.SCMDataEntity;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class PurchaseOrderReturnedListForShipment : SCMEntityActionList<PURCHASE_ORDER_RETURNED>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doPrintPreview.DoAction = arg =>
                {
                    printPreview.Show(int.Parse(arg));
                };
        }

    }
}