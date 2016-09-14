using System;
using System.Linq.Expressions;
using System.Web.UI;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;

namespace eIVOGo.Module.SCM
{
    public partial class GoodsMaintain : InquireEntity<PRODUCTS_DATA>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            Expression<Func<PRODUCTS_DATA, bool>> queryExpr = p => true;
            if (!String.IsNullOrEmpty(txtName.Text))
            {
                queryExpr = queryExpr.And(p => p.PRODUCTS_NAME.Contains(txtName.Text.Trim()));
            }
            if (!String.IsNullOrEmpty(txtRno.Text))
            {
                queryExpr = queryExpr.And(p => p.PRODUCTS_BARCODE == txtRno.Text.Trim());
            }
            itemList.QueryExpr = queryExpr;
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/SCM_SYSTEM/Goods_Maintain_Add.aspx", true);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}