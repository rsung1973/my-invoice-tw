using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.SCMDataEntity;
using DataAccessLayer.basis;
using Model.Locale;


namespace Model.SCM
{
    public partial class WarehouseWarrantManager : SCMEntityManager<WAREHOUSE_WARRANT>
    {
        public WarehouseWarrantManager() : base() { }
        public WarehouseWarrantManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }


        public WAREHOUSE_WARRANT Save(WAREHOUSE_WARRANT dataItem, int sellerID, String prefixNO)
        {
            int currentCount = EntityList.Count(p => p.WW_DATETIME >= DateTime.Today);
            DateTime now = DateTime.Now;

            WAREHOUSE_WARRANT item = new WAREHOUSE_WARRANT
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = now,
                    DocType = (int)Naming.DocumentTypeDefinition.OrderExchangeGoods,
                    DocumentOwner = new DocumentOwner { 
                        OwnerID = sellerID
                    }
                },
                SUPPLIER_SN = dataItem.SUPPLIER_SN,
                WAREHOUSE_SN = dataItem.WAREHOUSE_SN,
                WW_DATETIME = now,
                WAREHOUSE_WARRANT_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", prefixNO, now, ++currentCount)
            };

            foreach (var b in dataItem.WAREHOUSE_WARRANT_DETAILS)
            {
                new WAREHOUSE_WARRANT_DETAILS
                {
                    EGI_DETAILS_SN = b.EGI_DETAILS_SN,
                    GR_DETAILS_SN = b.GR_DETAILS_SN,
                    PO_DETAILS_SN = b.PO_DETAILS_SN,
                    RECEIPT_QUANTITY = b.RECEIPT_QUANTITY,
                    WAREHOUSE_WARRANT = item,
                    WW_DEFECTIVE_QUANTITY = b.WW_DEFECTIVE_QUANTITY,
                    WW_QUANTITY = b.WW_QUANTITY
                };
            }

            EntityList.InsertOnSubmit(item);
            this.SubmitChanges();
            item.DataFrom = Naming.DataItemSource.FromDB;
            return item;
        }


    }

}
