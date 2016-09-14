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
using Model.Locale;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class SingleShipmentPreviewFromExchange : SCMEntityPreview<CDS_Document>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int shipmentSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.DocID == shipmentSN).First();
            _item.DataFrom = Naming.DataItemSource.FromDB;
            modelItem.DataItem = _item;
            outboundDetails.Items = _item.EXCHANGE_GOODS.EXCHANGE_GOODS_OUTBOND_DETAILS;
        }

        protected override void prepareDataForViewState()
        {
            if (_item.DataFrom == Naming.DataItemSource.FromDB)
            {
                PrepareDataFromDB(_item.DocID);
            }
            else
            {
                var exchangeItem = dsEntity.CreateDataManager().GetTable<EXCHANGE_GOODS>().Where(b => b.EXCHANGE_GOODS_SN == _item.DocID).First();
                _item.EXCHANGE_GOODS = exchangeItem;
                _item.BUYER_SHIPMENT.DELIVERY_COMPANY = dsEntity.CreateDataManager().GetTable<DELIVERY_COMPANY>().Where(c => c.DELIVERY_COMPANY_SN == _item.BUYER_SHIPMENT.DELIVERY_COMPANY_SN).First();
                outboundDetails.Items = exchangeItem.EXCHANGE_GOODS_OUTBOND_DETAILS;
            }
        }
    }
}