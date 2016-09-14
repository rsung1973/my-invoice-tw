using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.basis;
using Model.Locale;
using System.Runtime.Serialization;
using System.Data.Linq;

namespace Model.SCMDataEntity
{
    public static partial class ExtensionMethods
    {

    }

    public partial class SCMEntityManager<TEntity> : GenericManager<SCMEntityDataContext, TEntity>
        where TEntity : class,new()
    {
        public SCMEntityManager() : base() { }
        public SCMEntityManager(GenericManager<SCMEntityDataContext> manager) : base(manager) { }
    }

    public delegate TEntity CreateItemFunc<TEntity>(GenericManager<SCMEntityDataContext, TEntity> arg) where TEntity : class,new();

    public partial class PurchaseDataSource : LinqToSqlDataSource<SCMEntityDataContext, PURCHASE_ORDER> { }
    public partial class ProductsAttributeNameDataSource : LinqToSqlDataSource<SCMEntityDataContext, PRODUCTS_ATTRIBUTE_NAME> { }
    public partial class PurchaseOrderDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, PURCHASE_ORDER_DETAILS> { }
    public partial class SupplierDataSource : LinqToSqlDataSource<SCMEntityDataContext, SUPPLIER> { }
    public partial class WarehouseDataSource : LinqToSqlDataSource<SCMEntityDataContext, WAREHOUSE> { }
    public partial class ProductsDataSource : LinqToSqlDataSource<SCMEntityDataContext, PRODUCTS_DATA> { }
    public partial class SupplierProductsNumberDataSource : LinqToSqlDataSource<SCMEntityDataContext, SUPPLIER_PRODUCTS_NUMBER> { }
    public partial class ProductsAttributeMappingDataSource : LinqToSqlDataSource<SCMEntityDataContext, PRODUCTS_ATTRIBUTE_MAPPING> { }
    public partial class WarehouseWarrantDataSource : LinqToSqlDataSource<SCMEntityDataContext, WAREHOUSE_WARRANT> { }
    public partial class WarehouseWarrantDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, WAREHOUSE_WARRANT_DETAILS> { }
    public partial class SalesPromotionDataSource : LinqToSqlDataSource<SCMEntityDataContext, SALES_PROMOTION> { }
    public partial class SalesPromotionProductsDataSource : LinqToSqlDataSource<SCMEntityDataContext, SALES_PROMOTION_PRODUCTS> { }
    public partial class PW_MappingDataSource : LinqToSqlDataSource<SCMEntityDataContext, PRODUCTS_WAREHOUSE_MAPPING> { }
    public partial class MarketResourceDataSource : LinqToSqlDataSource<SCMEntityDataContext, MARKET_RESOURCE> { }
    public partial class MarketAttributeDataSource : LinqToSqlDataSource<SCMEntityDataContext, MARKET_ATTRIBUTE> { }
    public partial class BuyerDataSource : LinqToSqlDataSource<SCMEntityDataContext, BUYER_DATA> { }
    public partial class BuyerOrderDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, BUYER_ORDERS_DETAILS> { }
    public partial class BuyerOrdersDataSource : LinqToSqlDataSource<SCMEntityDataContext, BUYER_ORDERS> { }
    public partial class PurchaseOrderReturnDataSource : LinqToSqlDataSource<SCMEntityDataContext, PURCHASE_ORDER_RETURNED> { }
    public partial class PurchaseOrderReturnDetailDataSource : LinqToSqlDataSource<SCMEntityDataContext, PURCHASE_ORDER_RETURNED_DETAILS> { }
    public partial class DeliveryCompanyDataSource : LinqToSqlDataSource<SCMEntityDataContext, DELIVERY_COMPANY> { }
    public partial class BuyerShipmentDataSource : LinqToSqlDataSource<SCMEntityDataContext, BUYER_SHIPMENT> { }
    public partial class ExchangeGoodsDataSource : LinqToSqlDataSource<SCMEntityDataContext, EXCHANGE_GOODS> { }
    public partial class DocumentDataSource : LinqToSqlDataSource<SCMEntityDataContext, CDS_Document> { }
    public partial class ReturnedGoodsDataSource : LinqToSqlDataSource<SCMEntityDataContext, GOODS_RETURNED> { }
    public partial class ReturnedGoodsDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, GOODS_RETURNED_DETAILS> { }
    public partial class ExchangeInboundDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, EXCHANGE_GOODS_INBOUND_DETAILS> { }
    public partial class ExchangeOutboundDetailsDataSource : LinqToSqlDataSource<SCMEntityDataContext, EXCHANGE_GOODS_OUTBOND_DETAILS> { }
    public partial class WarehouseProductsTransactionLogDataSource : LinqToSqlDataSource<SCMEntityDataContext, WAREHOUSE_PRODUCTS_TRANSACTION_LOG> { }

    //public partial class BUYER_ORDERS
    //{ 
    //    [DataMember()]
    //    public EntitySet<BUYER_ORDERS_DETAILS> X_BUYER_ORDERS_DETAILS
    //    {
    //        get
    //        {
    //            return this._BUYER_ORDERS_DETAILS;
    //        }
    //        set
    //        {
    //            this._BUYER_ORDERS_DETAILS.Assign(value);
    //        }
    //    }

    //}

    public partial class PRODUCTS_DATA
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }

        [DataMember()]
        public Naming.DataItemSource DataStatus
        { get; set; }

    }

    public partial class PURCHASE_ORDER_RETURNED_DETAILS
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }

        [DataMember()]
        public Naming.DataItemStatus DataStatus
        { get; set; }

    }



    public partial class BUYER_ORDERS
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }

        public BUYER_ORDERS Clone()
        {
            return (BUYER_ORDERS)this.MemberwiseClone();
        }
    }

    public partial class PURCHASE_ORDER
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }

    public partial class SALES_PROMOTION
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }

    }

    public partial class WAREHOUSE
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }

    }

    public partial class PURCHASE_ORDER_RETURNED
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }

    public partial class CDS_Document
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }

    public partial class EXCHANGE_GOODS
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }

    public partial class WAREHOUSE_WARRANT
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }

    public partial class GOODS_RETURNED
    {
        [DataMember()]
        public Naming.DataItemSource DataFrom
        { get; set; }
    }
}



