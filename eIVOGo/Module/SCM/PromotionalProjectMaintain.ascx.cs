using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Base;
using Model.SCM;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;

namespace eIVOGo.Module.SCM
{
    public partial class PromotionalProjectMaintain : InquireEntity<SALES_PROMOTION>
    {

        protected override void buildQueryItem()
        {
            //if (_item != null)
            //{
            //    itemList.QueryExpr = p => p.SALES_PROMOTION_SN == _item.SALES_PROMOTION_SN;
            //    modelItem.DataItem = null;
            //}
            //else
            //{
                Expression<Func<SALES_PROMOTION, bool>> queryExpr = p => true;

                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(p => p.SALES_PROMOTION_NAME.Contains(txtName.Text));
                }
                itemList.QueryExpr = queryExpr;
            //}
        }

        protected override System.Web.UI.UserControl _itemList
        {
            get { return itemList; }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToAddSalesPromo.TransferTo);
        }
    }
}