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
    public partial class ReturnedGoodsPreview : SCMEntityPreview<GOODS_RETURNED>
    {

        public override void PrepareDataFromDB(object keyValue)
        {
            int returnSN = (int)keyValue;
            _item = dsEntity.CreateDataManager().EntityList.Where(s => s.GOODS_RETURNED_SN == returnSN).First();
            modelItem.DataItem = _item;
            returnDetails.Items = _item.GOODS_RETURNED_DETAILS;
        }

        protected override void prepareDataForViewState()
        {
            _item.BUYER_ORDERS = dsEntity.CreateDataManager().GetTable<BUYER_ORDERS>().Where(b => b.BUYER_ORDERS_SN == _item.BUYER_ORDERS_SN).First();
            _item.WAREHOUSE = dsEntity.CreateDataManager().GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == _item.WAREHOUSE_SN).First();
            returnDetails.Items = _item.GOODS_RETURNED_DETAILS;
        }
    }
}