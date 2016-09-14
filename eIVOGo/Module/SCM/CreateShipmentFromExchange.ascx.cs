using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM
{
    public partial class CreateShipmentFromExchange : System.Web.UI.UserControl
    {
        protected CDS_Document _item;

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(CreateShipment_PreRender);
            singleShipment.ItemType = typeof(CDS_Document);
            singleShipment.Load += new EventHandler(singleShipment_Load);
        }

        protected virtual void singleShipment_Load(object sender, EventArgs e)
        {
            _item = (CDS_Document)singleShipment.DataItem;
            var exchangeItem = dsEntity.CreateDataManager().EntityList.Where(b => b.EXCHANGE_GOODS_SN == _item.DocID).First();
            _item.EXCHANGE_GOODS = exchangeItem;
            outboundDetails.Items = exchangeItem.EXCHANGE_GOODS_OUTBOND_DETAILS;
        }

        void CreateShipment_PreRender(object sender, EventArgs e)
        {
            if (_item != null)
            {
                if (_item.BUYER_SHIPMENT.DELIVERY_COMPANY_SN.HasValue)
                {
                    delivery.SelectedValue = _item.BUYER_SHIPMENT.DELIVERY_COMPANY_SN.ToString();
                }
                this.DataBind();
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Server.Transfer(ToInquireShipment.TransferTo);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(delivery.SelectedValue))
            {
                this.AjaxAlert("請選擇配送公司!!");
                return;
            }
            _item.BUYER_SHIPMENT.DELIVERY_COMPANY_SN = int.Parse(delivery.SelectedValue);
            Server.Transfer(ToPreviewShipment.TransferTo);
        }

    }
}