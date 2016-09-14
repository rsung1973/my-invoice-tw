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
using Model.Locale;
using Model.SCM;
using eIVOGo.Properties;
using Model.Security.MembershipManagement;
using Business.Helper;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class BuyerOrderPreview : SCMEntityPreview<BUYER_ORDERS>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int orderSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.BUYER_ORDERS_SN == orderSN).First();
            modelItem.DataItem = _item;
            boDetails.Items = _item.BUYER_ORDERS_DETAILS;
            resourcePreview.MappingItems = _item.ORDERS_MARKET_ATTRIBUTE_MAPPING;
        }

        protected override void prepareDataForViewState()
        {
            boDetails.Items = _item.BUYER_ORDERS_DETAILS;
            resourcePreview.MappingItems = _item.ORDERS_MARKET_ATTRIBUTE_MAPPING;
            _item.WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == _item.WAREHOUSE_SN).First();
            _item.MARKET_RESOURCE = dsEntity.CreateDataManager().GetTable<MARKET_RESOURCE>().Where(m => m.MARKET_RESOURCE_SN == _item.MARKET_RESOURCE_SN).First();
        }
    }
}