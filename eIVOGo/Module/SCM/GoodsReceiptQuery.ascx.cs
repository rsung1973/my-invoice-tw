using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;

namespace eIVOGo.Module.SCM
{
    public partial class GoodsReceiptQuery : InquireEntity<WAREHOUSE_WARRANT>
    {

        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                itemList.QueryExpr = b => b.WAREHOUSE_WARRANT_SN == _item.WAREHOUSE_WARRANT_SN;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<WAREHOUSE_WARRANT, bool>> queryExpr = p => true;

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.WAREHOUSE_WARRANT_NUMBER == orderNo.Text);
                }

                switch(orderType.SelectedValue)
                {
                    case "100":
                        queryExpr = queryExpr.And(b => b.WAREHOUSE_WARRANT_DETAILS.Count(w => w.PO_DETAILS_SN.HasValue) > 0);
                        break;
                    case "104":
                        queryExpr = queryExpr.And(b => b.WAREHOUSE_WARRANT_DETAILS.Count(w => w.GR_DETAILS_SN.HasValue || w.EGI_DETAILS_SN.HasValue) > 0);
                        break;
                }
                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    queryExpr = queryExpr.And(d => d.WAREHOUSE_SN == int.Parse(warehouse.SelectedValue));
                }
                if (!String.IsNullOrEmpty(supplierProductNo.Text))
                {
                    queryExpr = queryExpr.And(d => d.SUPPLIER.SUPPLIER_PRODUCTS_NUMBER.Count(s => s.SUPPLIER_PRODUCTS_NUMBER1.Contains(supplierProductNo.Text)) > 0);
                }

                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(d => d.WAREHOUSE_WARRANT_DETAILS.Count(w => w.PURCHASE_ORDER_DETAILS.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text
                        || w.GOODS_RETURNED_DETAILS.BUYER_ORDERS_DETAILS.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text
                        || w.EXCHANGE_GOODS_INBOUND_DETAILS.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(d => d.WAREHOUSE_WARRANT_DETAILS.Count(w => w.PURCHASE_ORDER_DETAILS.SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)
                        || w.GOODS_RETURNED_DETAILS.BUYER_ORDERS_DETAILS.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)
                        || w.EXCHANGE_GOODS_INBOUND_DETAILS.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.WW_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.WW_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                if (!String.IsNullOrEmpty(docStatus.SelectedValue))
                {
                    queryExpr = queryExpr.And(d => d.CDS_Document.CurrentStep == int.Parse(docStatus.SelectedValue));
                }

                itemList.QueryExpr = queryExpr;
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(ToCreateWarrant.TransferTo);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}