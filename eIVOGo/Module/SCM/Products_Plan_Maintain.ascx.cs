using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Utility;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.Security.MembershipManagement;
using Business.Helper;
using Uxnet.Web.WebUI;
using Model.InvoiceManagement;
using System.IO;
using eIVOGo.Module.Common;
using System.Text.RegularExpressions;
using System.Text;

using Model.SCM;
using Model.SCMDataEntity;
using eIVOGo.Module.Base;


namespace eIVOGo.Module.SCM
{
    public partial class Products_Plan_Maintain : InquireEntity<WAREHOUSE>
    {

        protected override void buildQueryItem()
        {
            if (_item != null)
            {
                itemList.QueryExpr = p => p.WAREHOUSE_SN == _item.WAREHOUSE_SN;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<PRODUCTS_WAREHOUSE_MAPPING, bool>> queryExpr = p => true;

                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    queryExpr = queryExpr.And(p => p.WAREHOUSE_SN == int.Parse(warehouse.SelectedValue));
                }

                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(p => p.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text));
                }

                if (!String.IsNullOrEmpty(this.txtBarCode.Text))
                {
                    queryExpr = queryExpr.And(p => p.PRODUCTS_DATA.PRODUCTS_BARCODE == txtBarCode.Text);
                }

                if (this.rdbStatus.SelectedIndex == 1)
                {
                    queryExpr = queryExpr.And(p => p.PRODUCTS_PLAN_AMOUNT != 0 | p.PRODUCTS_SAFE_AMOUNT_PERCENTAGE != 0);
                }
                else if (this.rdbStatus.SelectedIndex == 2)
                {
                    queryExpr = queryExpr.And(p => p.PRODUCTS_PLAN_AMOUNT == 0 & p.PRODUCTS_SAFE_AMOUNT_PERCENTAGE == 0);
                }

                itemList.QueryExpr = queryExpr;
            }
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