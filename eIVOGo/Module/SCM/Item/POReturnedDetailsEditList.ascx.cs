using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.SCMDataEntity;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.Module.Common;
using eIVOGo.Helper;
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.Item
{
    public partial class POReturnedDetailsEditList : SCMEntityAttributeList<PURCHASE_ORDER_RETURNED_DETAILS>
    {
        protected PRODUCTS_WAREHOUSE_MAPPING _mappingItem;

        protected override PRODUCTS_DATA loadItem(PURCHASE_ORDER_RETURNED_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).First();
            item.PRODUCTS_DATA = _currentItem;
            return _currentItem;
        }

        protected SUPPLIER_PRODUCTS_NUMBER getSupplierNO(PURCHASE_ORDER_RETURNED_DETAILS item)
        {
            loadItem(item);
            return dsEntity.CreateDataManager().GetTable<SUPPLIER_PRODUCTS_NUMBER>().Where(s => s.PRODUCTS_SN == _currentItem.PRODUCTS_SN
                && s.SUPPLIER_SN == item.PURCHASE_ORDER_RETURNED.SUPPLIER_SN).First();
        }

        protected PRODUCTS_WAREHOUSE_MAPPING getProductWarehouseMapping(PURCHASE_ORDER_RETURNED_DETAILS item)
        {
            _mappingItem = dsEntity.CreateDataManager().GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(s => s.PRODUCTS_SN == _currentItem.PRODUCTS_SN
                && s.WAREHOUSE_SN == item.PURCHASE_ORDER_RETURNED.WAREHOUSE_SN).First();
            return _mappingItem;
        }

        public override void UpdateData()
        {

        }

        protected override IEnumerable<int> getProductSN()
        {
            var prodSN = Items.Select(b => b.PRODUCTS_SN).ToArray();
            return prodSN;
        }
    }
}