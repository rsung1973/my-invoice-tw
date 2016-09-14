using System;
using System.Linq.Expressions;
using System.Web.UI;
using System.Linq;

using eIVOGo.Module.Base;
using Model.SCMDataEntity;
using Utility;
using Model.Security.MembershipManagement;
using Business.Helper;
using eIVOGo.Helper;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class ExchangeGoodsQueryForWarrant : InquireEntity<WAREHOUSE_WARRANT>
    {
        
        protected override void buildQueryItem()
        {
            //設定能顯示的發票為登入者的公司作廢發票
            //過濾使用者所填入的條件
            if (_item != null)
            {
                int[] detailSN = _item.WAREHOUSE_WARRANT_DETAILS.Select(w => w.EGI_DETAILS_SN.Value).ToArray();
                itemList.QueryExpr = p => p.EXCHANGE_GOODS_INBOUND_DETAILS.Any(d => detailSN.Contains(d.EGI_DETAILS_SN));
                modelItem.DataItem = null;
            }
            else
            {
                Expression<Func<EXCHANGE_GOODS, bool>> queryExpr = p => true;

                if (!String.IsNullOrEmpty(orderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_NUMBER == orderNo.Text);
                }
                if (!String.IsNullOrEmpty(buyerOrderNo.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_NUMBER == buyerOrderNo.Text);
                }

                if (!String.IsNullOrEmpty(barcode.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.EXCHANGE_GOODS_INBOUND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0
                        || b.EXCHANGE_GOODS_OUTBOND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_BARCODE == barcode.Text) > 0);
                }
                if (!String.IsNullOrEmpty(txtName.Text))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_ORDERS_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.EXCHANGE_GOODS_OUTBOND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0
                        || b.EXCHANGE_GOODS_INBOUND_DETAILS.Count(d => d.PRODUCTS_DATA.PRODUCTS_NAME.Contains(txtName.Text)) > 0);
                }

                if (!String.IsNullOrEmpty(buyerQuery.QueryArgument))
                {
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.BUYER_SN == buyerQuery.Item.BUYER_SN);
                }
                if (orderDateFrom.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_DATETIME >= orderDateFrom.DateTimeValue);
                }
                if (orderDateTo.HasValue)
                {
                    queryExpr = queryExpr.And(b => b.EXCHANGE_GOODS_DATETIME < orderDateTo.DateTimeValue.AddDays(1));
                }

                if (!String.IsNullOrEmpty(marketResource.SelectedValue))
                {
                    int marketSN = int.Parse(marketResource.SelectedValue);
                    queryExpr = queryExpr.And(b => b.BUYER_ORDERS.MARKET_RESOURCE_SN == marketSN);
                }

                if (!String.IsNullOrEmpty(warehouse.SelectedValue))
                {
                    int warehouseSN = int.Parse(warehouse.SelectedValue);
                    queryExpr = queryExpr.And(p => p.INBOUND_WAREHOUSE_SN == warehouseSN);
                }

                if (!String.IsNullOrEmpty(SupplierSN.SelectedValue))
                {
                    int sn = int.Parse(SupplierSN.SelectedValue);
                    queryExpr = queryExpr.And(p => p.EXCHANGE_GOODS_INBOUND_DETAILS.Count(d => d.PRODUCTS_DATA.SUPPLIER_PRODUCTS_NUMBER.Count(s => s.SUPPLIER_SN == sn) > 0) > 0);
                }

                queryExpr = queryExpr.And(b => !b.EG_WW_STATUS.HasValue || b.EG_BS_STATUS == 0);

                itemList.QueryExpr = queryExpr;
            }

            tblNext.Visible = true;

        }

        protected void rbChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            Server.Transfer(rbChange.SelectedValue);
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            var items = Request.GetItemSelection();
            if (items != null && items.Length > 0)
            {
                var item = createWarehouseWarrant(items.Select(s => int.Parse(s)).ToArray());
                modelItem.DataItem = item;
                Server.Transfer(ToCreateWarrant.TransferTo);
            }
            else
            {
                this.AjaxAlert("請選擇項目!!");
            }
        }


        private WAREHOUSE_WARRANT createWarehouseWarrant(int[] exchangeSN)
        {
            var mgr = dsEntity.CreateDataManager();

            DateTime now = DateTime.Now;
            var item = new WAREHOUSE_WARRANT
            {
                WW_DATETIME = now,
                SUPPLIER_SN = int.Parse(SupplierSN.SelectedValue),
                WAREHOUSE_SN = int.Parse(warehouse.SelectedValue)
            };

            foreach (var p in mgr.GetTable<EXCHANGE_GOODS_INBOUND_DETAILS>().Where(p => exchangeSN.Contains(p.EXCHANGE_GOODS_SN)))
            {
                var detailItem = new WAREHOUSE_WARRANT_DETAILS
                {
                    EGI_DETAILS_SN = p.EGI_DETAILS_SN,
                    RECEIPT_QUANTITY = 0,
                    WW_QUANTITY = p.GR_WW_QUANTITY,
                    WW_DEFECTIVE_QUANTITY = 0
                };
                item.WAREHOUSE_WARRANT_DETAILS.Add(detailItem);
            }

            return item;
        }




        protected override UserControl _itemList
        {
            get { return itemList; }
        }
    }
}