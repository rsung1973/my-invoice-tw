using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.SCMDataEntity;
using DataAccessLayer.basis;
using Model.Locale;


namespace Model.SCM
{
    public partial class WarehouseManager : SCMEntityManager<WAREHOUSE>
    {
        public WarehouseManager() : base() { }
        public WarehouseManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public WAREHOUSE SaveProductsMapping(WAREHOUSE dataItem)
        {
            WAREHOUSE item = null;

            item = EntityList.Where(o => o.WAREHOUSE_SN == dataItem.WAREHOUSE_SN).First();
            item.DataFrom = Naming.DataItemSource.FromDB;
            this.DeleteAll<PRODUCTS_WAREHOUSE_MAPPING>(o => o.WAREHOUSE_SN == item.WAREHOUSE_SN);

            assignProductsWarehouseMappingValue(dataItem, item);

            this.SubmitChanges();
            return item;
        }

        private void assignProductsWarehouseMappingValue(WAREHOUSE dataItem, WAREHOUSE item)
        {

            foreach (var d in dataItem.PRODUCTS_WAREHOUSE_MAPPING)
            {
                var detailItem = new PRODUCTS_WAREHOUSE_MAPPING
                {
                    WAREHOUSE = item,
                    PRODUCTS_SN = d.PRODUCTS_SN,
                    PRODUCTS_PLAN_AMOUNT = d.PRODUCTS_PLAN_AMOUNT,
                    PRODUCTS_SAFE_AMOUNT_PERCENTAGE = d.PRODUCTS_SAFE_AMOUNT_PERCENTAGE
                };
            }
        }
    }

}
