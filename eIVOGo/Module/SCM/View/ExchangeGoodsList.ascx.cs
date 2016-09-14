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
    public partial class ExchangeGoodsList : SCMEntityActionList<EXCHANGE_GOODS>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doPrintExchange.DoAction = arg =>
                {
                    printExchange.Show(int.Parse(arg));
                };
            doPrintOrder.DoAction = arg =>
                {
                    printBuyerOrder.Show(int.Parse(arg));
                };
            doPrintInvoice.DoAction = arg =>
            {
                printInvoice.Show(int.Parse(arg));
            };

        }

        protected override void edit(String keyValue)
        {
            Page.Items["id"] = int.Parse(keyValue);
            Server.Transfer(ToEditBuyerOrder.TransferTo);
        }

    }
}