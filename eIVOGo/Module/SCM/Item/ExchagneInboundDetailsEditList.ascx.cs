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
    public partial class ExchagneInboundDetailsEditList : SCMEntityAttributeList<EXCHANGE_GOODS_INBOUND_DETAILS>
    {

        protected override PRODUCTS_DATA loadItem(EXCHANGE_GOODS_INBOUND_DETAILS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).First();
            item.PRODUCTS_DATA = _currentItem;
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
                    String defectiveText = Request[String.Format("GR_WW_DEFECTIVE_AMOUNT{0}", i)];
                    String quantityText = Request[String.Format("GR_WW_QUANTITY{0}", i)];
                    if (!String.IsNullOrEmpty(defectiveText) && !String.IsNullOrEmpty(quantityText))
                    {
                        item.GR_WW_QUANTITY = decimal.Parse(quantityText);
                        item.GR_WW_DEFECTIVE_AMOUNT = decimal.Parse(defectiveText);
                    }
                }
            }
        }

        protected override IEnumerable<int> getProductSN()
        {
            var prodSN = Items.Select(b => b.PRODUCTS_SN.Value).ToArray();
            return prodSN;
        }

    }
}