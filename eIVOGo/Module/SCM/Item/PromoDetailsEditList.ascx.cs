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
using eIVOGo.Module.Base;

namespace eIVOGo.Module.SCM.Item
{
    public partial class PromoDetailsEditList : SCMEntityAttributeList<SALES_PROMOTION_PRODUCTS>
    {


        protected override PRODUCTS_DATA loadItem(SALES_PROMOTION_PRODUCTS item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).First();
            item.PRODUCTS_DATA = _currentItem;
            return _currentItem;
        }


        public override void UpdateData()
        {
            for(int i=0;i<Items.Count;i++)
            {
                var item = Items[i];
                String priceText = Request[String.Format("SALES_PROMOTION_SELL_PRICE{0}", i)];
                String quantityText = Request[String.Format("PROMOTION_QUANTITY{0}", i)];
                if (!String.IsNullOrEmpty(priceText))
                {                    
                    item.SALES_PROMOTION_SELL_PRICE = decimal.Parse(priceText);
                }

                if ( !String.IsNullOrEmpty(quantityText))
                {
                    item.PROMOTION_QUANTITY = int.Parse(quantityText);
                }
            }
        }

        protected override IEnumerable<int> getProductSN()
        {
            return Items.Select(b => b.PRODUCTS_SN).ToArray();           
        }
    }
}