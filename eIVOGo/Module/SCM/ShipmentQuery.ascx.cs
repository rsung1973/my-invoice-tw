using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class ShipmentQuery : InquireEntity<CDS_Document>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                itemList.QueryExpr = b => b.DocID == _item.DocID;
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<CDS_Document, bool>> queryExpr = p => p.BUYER_SHIPMENT != null;

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_SHIPMENT.BUYER_SHIPMENT_NUMBER == orderNo.Text);
                }
                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.GOODS_RETURNED.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.PURCHASE_ORDER_RETURNED.PURCHASE_ORDER_RETURNED_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.GOODS_RETURNED.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.PURCHASE_ORDER_RETURNED.PURCHASE_ORDER_RETURNED_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }
                if (!String.IsNullOrEmpty(buyerQuery.QueryArgument))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_SN == buyerQuery.Item.BUYER_SN || b.GOODS_RETURNED.BUYER_ORDERS.BUYER_SN == buyerQuery.Item.BUYER_SN);
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.BUYER_SHIPMENT.SHIPMENT_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.BUYER_SHIPMENT.SHIPMENT_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }
                if (!String.IsNullOrEmpty(delivery.SelectedValue))
                {
                    int sn = int.Parse(delivery.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_SHIPMENT.DELIVERY_COMPANY_SN==sn);
                }

                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    int warehouseSN = int.Parse(warehouse.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.WAREHOUSE_SN == warehouseSN || b.GOODS_RETURNED.BUYER_ORDERS.WAREHOUSE_SN==warehouseSN || b.PURCHASE_ORDER_RETURNED.WAREHOUSE_SN==warehouseSN );
                }
                switch(orderType.SelectedValue)
                {
                    case "102":
                        queryExpr = queryExpr.And(b => b.PURCHASE_ORDER_RETURNED != null);
                        break;
                    case "103":
                        queryExpr = queryExpr.And(b => b.BUYER_ORDERS != null);
                        break;
                    case "104":
                        queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS != null);
                        break;
                }
                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN || b.GOODS_RETURNED.BUYER_ORDERS.MARKET_RESOURCE_SN==marketSN);
                }

                itemList.QueryExpr = queryExpr;
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect(ToCreateShipment.TransferTo);
        }

        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}