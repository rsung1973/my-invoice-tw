using System;
using System.Linq;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.SCM;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class WarehouseMaintain : InquireEntity<WAREHOUSE>
    {


        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SCM_SYSTEM/Warehouse_Maintain_Add.aspx");
        }

        protected override void buildQueryItem()
        {
            Expression<Func<WAREHOUSE, bool>> queryExpr = w => true;
            if (!String.IsNullOrEmpty(txtName.Text))
            {
                queryExpr = queryExpr.And(w => w.WAREHOUSE_NAME.Contains(txtName.Text.Trim()));
            }
            itemList.QueryExpr = queryExpr;
        }


        protected override System.Web.UI.UserControl _itemList
        {
            get { return itemList; }
        }

    }
}