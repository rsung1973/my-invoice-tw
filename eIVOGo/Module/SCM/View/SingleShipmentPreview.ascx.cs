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
    public partial class SingleShipmentPreview : SCMEntityPreview<CDS_Document>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int shipmentSN = (int)keyValue; 
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.DocID == shipmentSN).First();
            _item.DataFrom = Naming.DataItemSource.FromDB;
            modelItem.DataItem = _item;
            boDetails.Items = _item.BUYER_ORDERS.BUYER_ORDERS_DETAILS;
        }

        protected override void prepareDataForViewState()
        {
            if (_item.DataFrom == Naming.DataItemSource.FromDB)
            {
                PrepareDataFromDB(_item.DocID);
            }
            else
            {
                _item.BUYER_ORDERS = dsEntity.CreateDataManager().GetTable<BUYER_ORDERS>().Where(b => b.BUYER_ORDERS_SN == _item.DocID).First();
                _item.BUYER_SHIPMENT.DELIVERY_COMPANY = dsEntity.CreateDataManager().GetTable<DELIVERY_COMPANY>().Where(c => c.DELIVERY_COMPANY_SN == _item.BUYER_SHIPMENT.DELIVERY_COMPANY_SN).First();
                boDetails.Items = _item.BUYER_ORDERS.BUYER_ORDERS_DETAILS;
            }
        }
    }
}