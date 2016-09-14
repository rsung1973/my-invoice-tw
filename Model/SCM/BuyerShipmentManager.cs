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
    public partial class BuyerShipmentManager : SCMEntityManager<BUYER_SHIPMENT>
    {
        public BuyerShipmentManager() : base() { }
        public BuyerShipmentManager(GenericManager<SCMEntityDataContext> mgr) : base(mgr) { }

        public CDS_Document Save(CDS_Document dataItem, String prefixNO)
        {
            int currentCount = EntityList.Count(p => p.SHIPMENT_DATETIME >= DateTime.Today);

            BUYER_SHIPMENT item = new BUYER_SHIPMENT
                    {
                        BUYER_SHIPMENT_SN = dataItem.BUYER_SHIPMENT.BUYER_SHIPMENT_SN,
                        DELIVERY_COMPANY_SN = dataItem.BUYER_SHIPMENT.DELIVERY_COMPANY_SN,
                        INVOICE_SN = dataItem.BUYER_SHIPMENT.INVOICE_SN,
                        BUYER_SHIPMENT_NUMBER = String.Format("{0}{1:yyyyMMdd}{2:0000}", prefixNO, dataItem.BUYER_SHIPMENT.SHIPMENT_DATETIME, ++currentCount),
                        POST_PRINT_STATUS = 0,
                        SHIPMENT_AMOUNT = dataItem.BUYER_SHIPMENT.SHIPMENT_AMOUNT,
                        SHIPMENT_DATETIME = dataItem.BUYER_SHIPMENT.SHIPMENT_DATETIME,
                        SHIPMENT_DISCOUNT_AMOUNT = dataItem.BUYER_SHIPMENT.SHIPMENT_DISCOUNT_AMOUNT,
                        SHIPMENT_ORIGINAL_AMOUNT = dataItem.BUYER_SHIPMENT.SHIPMENT_ORIGINAL_AMOUNT,
                        TAX_AMOUNT = dataItem.BUYER_SHIPMENT.TAX_AMOUNT,
                        TOTAL_AMOUNT = dataItem.BUYER_SHIPMENT.TOTAL_AMOUNT
                    };
            foreach (var b in dataItem.BUYER_SHIPMENT.BUYER_SHIPMENT_DETAILS)
            {
                new BUYER_SHIPMENT_DETAILS
                {
                    BUYER_SHIPMENT = item,
                    BO_DETAILS_SN = b.BO_DETAILS_SN,
                    BS_QUANTITY = b.BS_QUANTITY
                };
            }

            EntityList.InsertOnSubmit(item);
            this.SubmitChanges();
            item.CDS_Document.DataFrom = Naming.DataItemSource.FromDB;
            return item.CDS_Document;
        }

    }





}
