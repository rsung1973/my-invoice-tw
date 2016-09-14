using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.View
{
    public partial class EditWarehouseWarrant : SCMEntityPreview<WAREHOUSE_WARRANT>
    {

        private void initializeData()
        {
            var mgr = dsWW.CreateDataManager();
            _item.SUPPLIER = mgr.GetTable<SUPPLIER>().Where(s => s.SUPPLIER_SN == _item.SUPPLIER_SN).First();
            _item.WAREHOUSE = mgr.GetTable<WAREHOUSE>().Where(w => w.WAREHOUSE_SN == _item.WAREHOUSE_SN).First();
            itemList.Items = _item.WAREHOUSE_WARRANT_DETAILS;

            foreach (var detailItem in _item.WAREHOUSE_WARRANT_DETAILS)
            {
                if (detailItem.PO_DETAILS_SN.HasValue)
                {
                    var item = mgr.GetTable<PURCHASE_ORDER_DETAILS>().Where(d => d.PO_DETAILS_SN == detailItem.PO_DETAILS_SN).First();
                    detailItem.PURCHASE_ORDER_DETAILS = item;
                }
                else if (detailItem.EGI_DETAILS_SN.HasValue)
                {
                    var item = mgr.GetTable<EXCHANGE_GOODS_INBOUND_DETAILS>().Where(g => g.EGI_DETAILS_SN == detailItem.EGI_DETAILS_SN).First();
                    detailItem.EXCHANGE_GOODS_INBOUND_DETAILS = item;
                }
                else if (detailItem.GR_DETAILS_SN.HasValue)
                {
                    var item = mgr.GetTable<GOODS_RETURNED_DETAILS>().Where(r => r.GR_DETAILS_SN == detailItem.GR_DETAILS_SN).First();
                    detailItem.GOODS_RETURNED_DETAILS = item;
                }
            }
        }

        public override void PrepareDataFromDB(object keyValue)
        {
            int warehouseWarrantSN = (int)keyValue;            
            _item = dsWW.CreateDataManager().EntityList.Where(w => w.WAREHOUSE_WARRANT_SN == warehouseWarrantSN).First();
            modelItem.DataItem = _item;
            initializeData();
        }

        protected override void prepareDataForViewState()
        {
            initializeData();
        }
    }
}