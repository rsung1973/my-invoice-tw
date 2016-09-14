using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Uxnet.Web.WebUI;
using eIVOGo.Module.SCM.Item;
using eIVOGo.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class CreateExchangeGoods : System.Web.UI.UserControl
    {
        protected EXCHANGE_GOODS _item;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CreateExchangeGoods_PreRender);
            exchangeGoods.ItemType = typeof(EXCHANGE_GOODS);
            exchangeGoods.Load += new EventHandler(singleShipment_Load);
            productQueryByName.QueryExpr = p => p.PRODUCTS_NAME.Contains(productQueryByName.FieldValue) && p.PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == getWarehouseSN()) > 0;
            productQueryByName.Done += new EventHandler(productQueryByName_Done);
            outboundWarehouse.Selector.AutoPostBack = true;
            outboundWarehouse.SelectedIndexChanged += new EventHandler(Selector_SelectedIndexChanged);
        }

        private int getWarehouseSN()
        {
            return String.IsNullOrEmpty(outboundWarehouse.SelectedValue) ? -1 : int.Parse(outboundWarehouse.SelectedValue);
        }

        void Selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            _item.EXCHANGE_GOODS_OUTBOND_DETAILS.Clear();
        }

        void productQueryByName_Done(object sender, EventArgs e)
        {
            var items = ((ProductQuery)sender).Items;
            if (items != null)
            {
                _item.EXCHANGE_GOODS_OUTBOND_DETAILS.AddRange(
                    items.Select(p => new EXCHANGE_GOODS_OUTBOND_DETAILS
                    {
                        GR_BS_QUANTITY = 0,
                        BO_UNIT_PRICE = p.SELL_PRICE,
                        PRODUCTS_SN = p.PRODUCTS_SN,
                        PRODUCTS_DATA = p
                    }).ToArray()
                    );
            }
        }


        void singleShipment_Load(object sender, EventArgs e)
        {
            _item = (EXCHANGE_GOODS)exchangeGoods.DataItem;
            _item.BUYER_ORDERS = dsEntity.CreateDataManager().EntityList.Where(b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN).First();
            inboundDetails.Items = _item.EXCHANGE_GOODS_INBOUND_DETAILS;
            outboundDetails.Items = _item.EXCHANGE_GOODS_OUTBOND_DETAILS;
        }

        void CreateExchangeGoods_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                inboundDetails.UpdateData();
                this.DataBind();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToInquireExchange.TransferTo);
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            var prodItem = dsEntity.CreateDataManager().GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_BARCODE == Barcode.Text && p.PRODUCTS_WAREHOUSE_MAPPING.Count(w => w.WAREHOUSE_SN == getWarehouseSN()) > 0).FirstOrDefault();
            if (prodItem != null)
            {
                _item.EXCHANGE_GOODS_OUTBOND_DETAILS.Add(new EXCHANGE_GOODS_OUTBOND_DETAILS
                {
                    GR_BS_QUANTITY = 0,
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


        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Reason.Text))
            {
                this.AjaxAlert("請填入換貨原因!!");
                return;
            }
            _item.EG_REASON = Reason.Text;

            if (String.IsNullOrEmpty(inboundWarehouse.SelectedValue))
            {
                this.AjaxAlert("未設定入庫倉儲!!");
                return;
            }
            _item.INBOUND_WAREHOUSE_SN = int.Parse(inboundWarehouse.SelectedValue);

            if (_item.EXCHANGE_GOODS_OUTBOND_DETAILS.Count < 1)
            {
                this.AjaxAlert("尚未設定出貨項目!!");
                return;
            }
            _item.OUTBOUND_WAREHOUSE_SN = int.Parse(outboundWarehouse.SelectedValue);

           inboundDetails.UpdateData();
           var inboundItems = inboundDetails.GetSelectedItems(_item.EXCHANGE_GOODS_INBOUND_DETAILS);
           if (inboundItems.Count < 1)
           {
               this.AjaxAlert("尚未設定換貨項目!!");
               return;
           }

            _item.EXCHANGE_GOODS_INBOUND_DETAILS.Clear();
            _item.EXCHANGE_GOODS_INBOUND_DETAILS.AddRange(inboundItems);

            _item.INBOUND_WAREHOUSE_SN = int.Parse(inboundWarehouse.SelectedValue);
            _item.OUTBOUND_WAREHOUSE_SN = int.Parse(outboundWarehouse.SelectedValue);


            Server.Transfer(ToPreviewExchange.TransferTo);
        }
    }
}