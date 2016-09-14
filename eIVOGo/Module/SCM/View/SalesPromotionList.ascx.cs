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
    public partial class SalesPromotionList : SCMEntityList<SALES_PROMOTION>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            doEdit.DoAction = arg =>
                {
                    edit(arg);
                };
            doCreate.DoAction = arg =>
            {

            };
        }

        private void edit(String keyValue)
        {
            Page.Items["id"] = int.Parse(keyValue);
            Server.Transfer(ToEditSalesPromotion.TransferTo);
        }

        private void delete(int keyValue)
        {
            dsEntity.CreateDataManager().DeleteAny(r => r.SALES_PROMOTION_SN == keyValue);
        }

    }
}