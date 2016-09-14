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
    public partial class ProductManager : SCMEntityManager<PRODUCTS_DATA>
    {
        public ProductManager() : base() { }
        public ProductManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public void Save(PRODUCTS_DATA item,int supplierSN,String supplierProductNo)
        {
            var dataItem = item.DataFrom == Naming.DataItemSource.FromDB ? EntityList.Where(p => p.PRODUCTS_SN == item.PRODUCTS_SN).FirstOrDefault() : null;
            if (dataItem != null)
            {
                this.DeleteAll<PRODUCTS_ATTRIBUTE_MAPPING>(m => m.PRODUCTS_SN == dataItem.PRODUCTS_SN);
                this.DeleteAll<SUPPLIER_PRODUCTS_NUMBER>(s => s.PRODUCTS_SN == dataItem.PRODUCTS_SN);
            }
            else
            {
                dataItem = new PRODUCTS_DATA { };
                EntityList.InsertOnSubmit(dataItem);
            }

            dataItem.PRODUCTS_BARCODE = item.PRODUCTS_BARCODE;
            dataItem.PRODUCTS_NAME = item.PRODUCTS_NAME;
            dataItem.SELL_PRICE = item.SELL_PRICE;
            dataItem.BUY_PRICE = item.BUY_PRICE;

            foreach(var attr in item.PRODUCTS_ATTRIBUTE_MAPPING)
            {
                new PRODUCTS_ATTRIBUTE_MAPPING
                {
                    PRODUCTS_ATTR_VALUE = attr.PRODUCTS_ATTR_VALUE,
                    PRODUCTS_ATTR_NAME_SN = attr.PRODUCTS_ATTR_NAME_SN,
                    PRODUCTS_DATA = dataItem
                };
            }

            new SUPPLIER_PRODUCTS_NUMBER
            {
                PRODUCTS_DATA = dataItem,
                SUPPLIER_SN = supplierSN,
                SUPPLIER_PRODUCTS_NUMBER1 = supplierProductNo
            };

            this.SubmitChanges();

        }
    }

    public partial class BuyerOrdersManager : SCMEntityManager<BUYER_ORDERS>
    {
        public BuyerOrdersManager() : base() { }
        public BuyerOrdersManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public BUYER_ORDERS GetBuyerOrder(Expression<Func<BUYER_ORDERS,bool>> queryExpr)
        {
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<BUYER_ORDERS>(o => o.BUYER_ORDERS_DETAILS);
            ops.LoadWith<BUYER_ORDERS_DETAILS>(d => d.PRODUCTS_DATA);
            ops.LoadWith<BUYER_ORDERS_DETAILS>(d => d.SALES_PROMOTION_PRODUCTS);
            ops.LoadWith<BUYER_ORDERS>(o => o.ORDERS_MARKET_ATTRIBUTE_MAPPING);
            ops.LoadWith<BUYER_ORDERS>(o => o.MARKET_RESOURCE);
            _db.LoadOptions = ops;

            return this.EntityList.Where(queryExpr).FirstOrDefault();
        }

        public BUYER_ORDERS Save(BUYER_ORDERS buyerOrder, IList<BUYER_DATA> buyer, String prefixNO,int sellerID)
        {
            BUYER_ORDERS item=null;
            if (buyerOrder.DataFrom == Naming.DataItemSource.FromDB && buyer.Count == 1)
            {
                item = EntityList.Where(o => o.BUYER_ORDERS_SN == buyerOrder.BUYER_ORDERS_SN).First();
                item.BUYER_SN = buyer[0].BUYER_SN;
                this.DeleteAll<BUYER_ORDERS_DETAILS>(o => o.BUYER_ORDERS_SN == item.BUYER_ORDERS_SN);
                this.DeleteAll<ORDERS_MARKET_ATTRIBUTE_MAPPING>(m => m.BUYER_ORDERS_SN == item.BUYER_ORDERS_SN);
                assignBuyerOrderValue(buyerOrder, item);
            }
            else
            {
                int currentCount = EntityList.Count(p => p.BO_DATETIME >= DateTime.Today);
                DateTime now = DateTime.Now;
                buyerOrder.BO_DATETIME = now;

                foreach (var b in buyer)
                {
                    item = new BUYER_ORDERS
                    {
                        CDS_Document = new CDS_Document
                        {
                            DocDate = now,
                            DocType = (int)Naming.DocumentTypeDefinition.BuyerOrder,
                            DocumentOwner = new DocumentOwner
                            {
                                OwnerID = sellerID
                            },
                            CurrentStep = (int)Naming.DocumentStepDefinition.已開立
                        },
                        BO_DATETIME = now,
                        BUYER_ORDERS_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", prefixNO, now, ++currentCount),
                        BUYER_SN = b.BUYER_SN
                    };
                    EntityList.InsertOnSubmit(item);
                    assignBuyerOrderValue(buyerOrder, item);
                }
            }

            this.SubmitChanges();
            item.DataFrom = Naming.DataItemSource.FromDB;
            return item;
        }

        private void assignBuyerOrderValue(BUYER_ORDERS buyerOrder, BUYER_ORDERS item)
        {
            item.BUYER_ORDER_TYPE = buyerOrder.BUYER_ORDER_TYPE;
            foreach (var d in buyerOrder.BUYER_ORDERS_DETAILS)
            {
                var detailItem = new BUYER_ORDERS_DETAILS
                {
                    BO_QUANTITY = d.BO_QUANTITY,
                    BO_UNIT_PRICE = d.BO_UNIT_PRICE,
                    BUYER_ORDERS = item,
                    PRODUCTS_SN = d.PRODUCTS_SN,
                    SP_PRODUCTS_SN = d.SP_PRODUCTS_SN
                };
            }
            item.DISCOUNT_DESCRIPTION = buyerOrder.DISCOUNT_DESCRIPTION;
            item.MARKET_RESOURCE_SN = buyerOrder.MARKET_RESOURCE_SN;
            item.ORDERS_AMOUNT = buyerOrder.ORDERS_AMOUNT;
            item.ORDERS_DISCOUNT_AMOUNT = buyerOrder.ORDERS_DISCOUNT_AMOUNT;
            foreach (var m in buyerOrder.ORDERS_MARKET_ATTRIBUTE_MAPPING)
            {
                var attrItem = new ORDERS_MARKET_ATTRIBUTE_MAPPING
                {
                    BUYER_ORDERS = item,
                    MARKET_ATTR_NAME_SN = m.MARKET_ATTR_NAME_SN,
                    MARKET_ATTR_VALUE = m.MARKET_ATTR_VALUE
                };
            }
            item.ORDERS_ORIGINAL_AMOUNT = buyerOrder.ORDERS_ORIGINAL_AMOUNT;
            item.WAREHOUSE_SN = buyerOrder.WAREHOUSE_SN;
        }
    }


    public partial class SalesPromotionManager : SCMEntityManager<SALES_PROMOTION>
    {
        public SalesPromotionManager() : base() { }
        public SalesPromotionManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public SALES_PROMOTION Save(SALES_PROMOTION dataItem)
        {
            SALES_PROMOTION item = null;
            if (dataItem.DataFrom == Naming.DataItemSource.FromDB)
            {
                item = EntityList.Where(o => o.SALES_PROMOTION_SN == dataItem.SALES_PROMOTION_SN).First();
                item.DataFrom = Naming.DataItemSource.FromDB;
                this.DeleteAll<SALES_PROMOTION_PRODUCTS>(o => o.SALES_PROMOTION_SN == item.SALES_PROMOTION_SN);
            }
            else
            {
                item = new SALES_PROMOTION { };
                EntityList.InsertOnSubmit(item);
            }

            assignSalesPromotionValue(dataItem, item);

            this.SubmitChanges();
            return item;
        }

        private void assignSalesPromotionValue(SALES_PROMOTION dataItem, SALES_PROMOTION item)
        {
            item.SALES_PROMOTION_DISCOUNT = dataItem.SALES_PROMOTION_DISCOUNT;
            item.SALES_PROMOTION_NAME = dataItem.SALES_PROMOTION_NAME;
            item.SALES_PROMOTION_SYMBOL = dataItem.SALES_PROMOTION_SYMBOL;

            foreach (var d in dataItem.SALES_PROMOTION_PRODUCTS)
            {
                var detailItem = new SALES_PROMOTION_PRODUCTS
                {
                    SALES_PROMOTION = item,
                    PRODUCTS_SN = d.PRODUCTS_SN,
                    PROMOTION_QUANTITY = d.PROMOTION_QUANTITY,
                    SALES_PROMOTION_SELL_PRICE = d.SALES_PROMOTION_SELL_PRICE
                };
            }
        }
    }



}
