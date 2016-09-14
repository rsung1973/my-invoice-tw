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
    public partial class ExchangeGoodsManager : SCMEntityManager<EXCHANGE_GOODS>
    {
        public ExchangeGoodsManager() : base() { }
        public ExchangeGoodsManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public EXCHANGE_GOODS Save(EXCHANGE_GOODS dataItem, String prefixNO)
        {
            int currentCount = EntityList.Count(p => p.EXCHANGE_GOODS_DATETIME >= DateTime.Today);
            DateTime now = DateTime.Now;

            EXCHANGE_GOODS item = new EXCHANGE_GOODS
                    {
                        BUYER_ORDERS_SN = dataItem.BUYER_ORDERS_SN,
                        CDS_Document = new CDS_Document {
                            DocDate = now,
                            DocType = (int)Naming.DocumentTypeDefinition.OrderExchangeGoods,
                            CurrentStep = (int)Naming.DocumentStepDefinition.已開立
                        },
                        EG_REASON = dataItem.EG_REASON,
                        EXCHANGE_GOODS_DATETIME = now,
                        EXCHANGE_GOODS_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", prefixNO, now, ++currentCount),
                        INBOUND_WAREHOUSE_SN = dataItem.INBOUND_WAREHOUSE_SN,
                        OUTBOUND_WAREHOUSE_SN = dataItem.OUTBOUND_WAREHOUSE_SN
                    };

            foreach (var b in dataItem.EXCHANGE_GOODS_INBOUND_DETAILS)
            {
                new EXCHANGE_GOODS_INBOUND_DETAILS
                {
                    BO_UNIT_PRICE = b.BO_UNIT_PRICE,
                    EXCHANGE_GOODS = item,
                    GR_WW_DEFECTIVE_AMOUNT = b.GR_WW_DEFECTIVE_AMOUNT,
                    GR_WW_QUANTITY = b.GR_WW_QUANTITY,
                    PRODUCTS_SN = b.PRODUCTS_SN
                };
            }

            foreach (var b in dataItem.EXCHANGE_GOODS_OUTBOND_DETAILS)
            {
                new EXCHANGE_GOODS_OUTBOND_DETAILS
                {
                    BO_UNIT_PRICE = b.BO_UNIT_PRICE,
                    EXCHANGE_GOODS = item,
                    GR_BS_QUANTITY = b.GR_BS_QUANTITY,
                    PRODUCTS_SN = b.PRODUCTS_SN
                };
            }


            EntityList.InsertOnSubmit(item);
            this.SubmitChanges();
            return item;
        }

    }





}
