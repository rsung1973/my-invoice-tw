using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Model.InvoiceManagement;
using Utility;
using Uxnet.Web.WebUI;
using Model.SCM;
using eIVOGo.Properties;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class ExchangeGoodsPreview : SCMEntityPreview<EXCHANGE_GOODS>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int exchangeSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.EXCHANGE_GOODS_SN == exchangeSN).First();
            modelItem.DataItem = _item;
            inboundDetails.Items = _item.EXCHANGE_GOODS_INBOUND_DETAILS;
            outboundDetails.Items = _item.EXCHANGE_GOODS_OUTBOND_DETAILS;
        }

        protected override void prepareDataForViewState()
        {
            _item.BUYER_ORDERS = dsEntity.CreateDataManager().GetTable<BUYER_ORDERS>().Where(b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN).First();
            _item.INBOUND_WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == _item.INBOUND_WAREHOUSE_SN).First();
            _item.OUTBOUND_WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == _item.OUTBOUND_WAREHOUSE_SN).First();
            inboundDetails.Items = _item.EXCHANGE_GOODS_INBOUND_DETAILS;
            outboundDetails.Items = _item.EXCHANGE_GOODS_OUTBOND_DETAILS;
        }
    }
}