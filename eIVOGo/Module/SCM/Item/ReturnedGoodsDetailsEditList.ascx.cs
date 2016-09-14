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
    public partial class ReturnedGoodsDetailsEditList : SCMEntityAttributeList<GOODS_RETURNED_DETAILS>
    {
        private List<PRODUCTS_DATA> _productItems = new List<PRODUCTS_DATA>();

        protected override PRODUCTS_DATA loadItem(GOODS_RETURNED_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            var boDetailItem = mgr.GetTable<BUYER_ORDERS_DETAILS>().Where(p => p.BO_DETAILS_SN == item.BO_DETAILS_SN).First();
            _currentItem = boDetailItem.PRODUCTS_DATA;
            item.BUYER_ORDERS_DETAILS = boDetailItem;
            _productItems.Add(_currentItem);
            return _currentItem;
        }

        public override void UpdateData()
        {
            String[] index = Request.GetItemSelection();
            if (index != null && index.Length > 0)
            {
                int[] idx = index.Select(s => int.Parse(s)).ToArray();
                foreach (var i in idx)
                {
                    var item = Items[i];
                    String quantityText = Request[String.Format("GR_QUANTITY{0}", i)];
                    if (!String.IsNullOrEmpty(quantityText))
                    {
                        item.GR_QUANTITY = decimal.Parse(quantityText);
                    }
                }
            }
        }

        protected override IEnumerable<int> getProductSN()
        {
            return _productItems.Select(p => p.PRODUCTS_SN).ToArray();
        }
    }
}