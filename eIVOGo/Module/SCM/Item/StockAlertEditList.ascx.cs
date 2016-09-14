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
    public partial class StockAlertEditList : SCMEntityAttributeList<PRODUCTS_WAREHOUSE_MAPPING>
    {


        protected override PRODUCTS_DATA loadItem(PRODUCTS_WAREHOUSE_MAPPING item)
        {
            var mgr = dsEntity.CreateDataManager();
            _currentItem = mgr.GetTable<PRODUCTS_DATA>().Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).First();
            item.PRODUCTS_DATA = _currentItem;
            return _currentItem;
        }

        public override void UpdateData()
        {
            var mgr = dsEntity.CreateDataManager();
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                String priceText = Request[String.Format("PRODUCTS_PLAN_AMOUNT{0}", i)];
                String quantityText = Request[String.Format("PRODUCTS_SAFE_AMOUNT_PERCENTAGE{0}", i)];
                if (item.PW_MAPPING_SN != 0)
                {
                    if (!String.IsNullOrEmpty(priceText) && !String.IsNullOrEmpty(quantityText))
                    {
                        var det = mgr.GetTable<PRODUCTS_WAREHOUSE_MAPPING>().Where(sn => sn.PW_MAPPING_SN == item.PW_MAPPING_SN).FirstOrDefault();
                        det.PRODUCTS_PLAN_AMOUNT = int.Parse(priceText);
                        det.PRODUCTS_SAFE_AMOUNT_PERCENTAGE = decimal.Parse(quantityText);
                        mgr.SubmitChanges();
                    }
                }
                //if (!String.IsNullOrEmpty(priceText) && !String.IsNullOrEmpty(quantityText))
                //{
                //    item.PRODUCTS_PLAN_AMOUNT = int.Parse(priceText);
                //    item.PRODUCTS_SAFE_AMOUNT_PERCENTAGE = decimal.Parse(quantityText);
                //}
            }
        }

        protected override IEnumerable<int> getProductSN()
        {
            return Items.Select(m => m.PRODUCTS_SN).ToArray();
        }
    }
}