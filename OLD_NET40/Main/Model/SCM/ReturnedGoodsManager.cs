using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

using DataAccessLayer.basis;
using Model.Properties;
using Model.SCMDataEntity;
using Utility;
using Model.Locale;

namespace Model.SCM
{
    public partial class ReturnedGoodsManager : SCMEntityManager<GOODS_RETURNED>
    {
        public ReturnedGoodsManager() : base() { }
        public ReturnedGoodsManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public GOODS_RETURNED Save(GOODS_RETURNED dataItem, String prefixNO)
        {
            int currentCount = EntityList.Count(p => p.GOODS_RETURNED_DATETIME >= DateTime.Today);
            DateTime now = DateTime.Now;

            GOODS_RETURNED item = new GOODS_RETURNED
                    {
                        BUYER_ORDERS_SN = dataItem.BUYER_ORDERS_SN,
                        CDS_Document = new CDS_Document {
                            DocDate = now,
                            DocType = (int)Naming.DocumentTypeDefinition.OrderExchangeGoods,
                            CurrentStep = (int)Naming.DocumentStepDefinition.已開立
                        },
                        GR_REASON = dataItem.GR_REASON,
                        CANCEL_INVOICE_SN = dataItem.CANCEL_INVOICE_SN,
                        GOODS_RETURNED_DATETIME = now,
                        GOODS_RETURNED_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", prefixNO, now, ++currentCount),
                        WAREHOUSE_SN = dataItem.WAREHOUSE_SN,
                        GR_STATUS = dataItem.GR_STATUS
                    };

            foreach (var b in dataItem.GOODS_RETURNED_DETAILS)
            {
                new GOODS_RETURNED_DETAILS
                {
                    BO_DETAILS_SN = b.BO_DETAILS_SN,
                    GOODS_RETURNED = item,
                    GR_QUANTITY = b.GR_QUANTITY
                };
            }

            EntityList.InsertOnSubmit(item);
            this.SubmitChanges();
            item.DataFrom = Naming.DataItemSource.FromDB;
            return item;
        }

    }





}
