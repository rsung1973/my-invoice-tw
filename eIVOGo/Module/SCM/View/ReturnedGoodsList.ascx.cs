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
    public partial class ReturnedGoodsList : SCMEntityActionList<GOODS_RETURNED>
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            doPrintOrder.DoAction = arg =>
            {
                printBuyerOrder.Show(int.Parse(arg));
            };

            doPrintReturn.DoAction = arg =>
            {
                printReturn.Show(int.Parse(arg));
            };

            doPrintInvoice.DoAction = arg =>
                {
                    printInvoice.Show(int.Parse(arg));
                };

        }

    }
}