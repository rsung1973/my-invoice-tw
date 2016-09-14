using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;
using eIVOGo.Module.SCM.Item;
using Model.SCM;
using Model.Locale;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM
{
    public partial class CreateBuyerOrder : SCMEntityEdit<BUYER_ORDERS>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            productQueryByPromotionName.BuildQuery = table =>
            {
                var pwMapping = table.Context.GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(w => w.WAREHOUSE_SN == getWarehouseSN());
                return table.Context.GetTable<SALES_PROMOTION_PRODUCTS>().Where(p => p.SALES_PROMOTION.SALES_PROMOTION_NAME.Contains(productQueryByPromotionName.FieldValue)).Select(p => p.PRODUCTS_DATA)
                    .Join(pwMapping, p => p.PRODUCTS_SN, w => w.PRODUCTS_SN, (p, w) => p);
            };

            productQueryByPromotionID.BuildQuery = table =>
            {
                var pwMapping = table.Context.GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(w => w.WAREHOUSE_SN == getWarehouseSN());
                return table.Context.GetTable<SALES_PROMOTION_PRODUCTS>().Where(p => p.SALES_PROMOTION.SALES_PROMOTION_SYMBOL.Contains(productQueryByPromotionID.FieldValue)).Select(p => p.PRODUCTS_DATA)
                    .Join(pwMapping, p => p.PRODUCTS_SN, w => w.PRODUCTS_SN, (p, w) => p);
            };

            productQueryByName.QueryExpr = p => p.PRODUCTS_NAME.Contains(productQueryByName.FieldValue) && p.PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == getWarehouseSN()) > 0;

            this.PreRender += new EventHandler(CreateBuyerOrder_PreRender);
            productQueryByName.Done += new EventHandler(productQueryByName_Done);
            productQueryByPromotionID.Done += new EventHandler(productQueryByName_Done);
            productQueryByPromotionName.Done += new EventHandler(productQueryByName_Done);
        }


        private int getWarehouseSN()
        {
            int warehouseSN = -1;
            int.TryParse(warehouse.SelectedValue, out warehouseSN);
            return warehouseSN;
        }

        void productQueryByName_Done(object sender, EventArgs e)
        {
            var items = ((ProductQuery)sender).Items;
            if (items != null)
            {
                ((BUYER_ORDERS)modelItem.DataItem).BUYER_ORDERS_DETAILS.AddRange(
                    items.Select(p => new BUYER_ORDERS_DETAILS
                    {
                        BO_QUANTITY = 0,
                        BO_UNIT_PRICE = p.SELL_PRICE,
                        PRODUCTS_SN = p.PRODUCTS_SN,
                        PRODUCTS_DATA = p
                    }).ToArray()
                    );
            }
        }

        void CreateBuyerOrder_PreRender(object sender, EventArgs e)
        {
            BUYER_ORDERS item = (BUYER_ORDERS)modelItem.DataItem;
            boDetails.UpdateData();
            if (!this.IsPostBack)
            {
                if (item.MARKET_RESOURCE_SN.HasValue)
                    marketResource.SelectedValue = item.MARKET_RESOURCE_SN.ToString();
                if (item.WAREHOUSE_SN > 0)
                {
                    warehouse.SelectedValue = item.WAREHOUSE_SN.ToString();
                }
                this.DataBind();
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            var prodItem = dsEntity.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE == Barcode.Text && p.PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == getWarehouseSN()) > 0).FirstOrDefault();
            if (prodItem != null)
            {
                ((BUYER_ORDERS)modelItem.DataItem).BUYER_ORDERS_DETAILS.Add(new BUYER_ORDERS_DETAILS
                {
                    BO_QUANTITY = 0,
                    BO_UNIT_PRICE = prodItem.SELL_PRICE,
                    PRODUCTS_SN = prodItem.PRODUCTS_SN,
                    PRODUCTS_DATA = prodItem
                });
            }
            else
            {
                this.AjaxAlert("料品資料不存在!!");
            }
        }

        protected void btnCalc_Click(object sender, EventArgs e)
        {
            boDetails.UpdateData();
            BUYER_ORDERS item = (BUYER_ORDERS)modelItem.DataItem;
            if (item.BUYER_ORDERS_DETAILS.Count > 0)
            {
                item.ORDERS_ORIGINAL_AMOUNT = item.BUYER_ORDERS_DETAILS.Sum(o => o.BO_QUANTITY * o.BO_UNIT_PRICE).Value;
                if (!String.IsNullOrEmpty(discountAmt.Text))
                {
                    item.ORDERS_DISCOUNT_AMOUNT = decimal.Parse(discountAmt.Text);
                    item.DISCOUNT_DESCRIPTION = discountDescription.Text;
                }
                item.ORDERS_AMOUNT = item.ORDERS_ORIGINAL_AMOUNT - item.ORDERS_DISCOUNT_AMOUNT;
            }

            divResult.Visible = true;
            divResult.DataBind();
            tblPreview.Visible = true;
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            BUYER_ORDERS item = (BUYER_ORDERS)modelItem.DataItem;
            if (marketResource.Item == null)
            {
                this.AjaxAlert("請選擇網購平台!!");
                return;
            }

            if (buyer.Items == null || buyer.Items.Count <= 0)
            {
                this.AjaxAlert("請選擇買受人!!");
                return;
            }

            item.BUYER_ORDER_TYPE = int.Parse(buyerOrderType.SelectedValue);
            item.MARKET_RESOURCE_SN = marketResource.Item.MARKET_RESOURCE_SN;
            item.MARKET_RESOURCE = marketResource.Item;
            if (marketResource.MappingItems != null)
            {
                item.ORDERS_MARKET_ATTRIBUTE_MAPPING.AddRange(marketResource.MappingItems);
            }
            item.WAREHOUSE_SN = getWarehouseSN();
            item.WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == item.WAREHOUSE_SN).First();

            Server.Transfer(ToPreviewBuyerOrder.TransferTo);
        }

        public override void PrepareDataFromDB(object keyValue)
        {
            int orderSN = (int)keyValue;
            BUYER_ORDERS item = (new BuyerOrdersManager(dsEntity.CreateDataManager())).GetBuyerOrder(o => o.BUYER_ORDERS_SN == orderSN);
            item.DataFrom = Naming.DataItemSource.FromDB;

            modelItem.DataItem = item;
            buyer.Items = dsEntity.CreateDataManager().GetTable<BUYER_DATA>().Where(b => b.BUYER_SN == item.BUYER_SN).ToList();
            boDetails.Items = item.BUYER_ORDERS_DETAILS;
            marketResource.MappingItems = item.ORDERS_MARKET_ATTRIBUTE_MAPPING;
        }

        protected override void prepareDataForViewState()
        {
            boDetails.Items = ((BUYER_ORDERS)modelItem.DataItem).BUYER_ORDERS_DETAILS;
            marketResource.MappingItems = ((BUYER_ORDERS)modelItem.DataItem).ORDERS_MARKET_ATTRIBUTE_MAPPING;
            marketResource.CheckMappingData();
        }

        protected override void prepareInitialData()
        {
            Item = new BUYER_ORDERS
            {
                ORDERS_DISCOUNT_AMOUNT = 0,
                ORDERS_ORIGINAL_AMOUNT = 0,
                TAX_AMOUNT = 0,
                ORDERS_AMOUNT = 0,
                TOTAL_AMOUNT = 0,
                WAREHOUSE_SN = -1,
                BUYER_ORDER_TYPE = (int)Naming.BuyerOrderTypeDefinition.一般消費者
            };

            boDetails.Items = ((BUYER_ORDERS)modelItem.DataItem).BUYER_ORDERS_DETAILS;
            marketResource.MappingItems = ((BUYER_ORDERS)modelItem.DataItem).ORDERS_MARKET_ATTRIBUTE_MAPPING;

        }
    }
}