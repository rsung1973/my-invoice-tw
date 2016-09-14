using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.DataEntity;
using Model.Locale;
using DataAccessLayer.basis;
using Model.Schema.EIVO;
using Utility;
using System.Linq.Expressions;

namespace Model.InvoiceManagement
{
    public class B2CInvoiceManager : EIVOEntityManager<InvoiceItem>
    {
        public B2CInvoiceManager() : base() { }
        public B2CInvoiceManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        public Dictionary<int, Exception> SaveUploadInvoice(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    var invItem = item.Invoice[idx];
                    try
                    {
                        String invNo, trackCode;
                        if (invItem.InvoiceNumber.Length >= 10)
                        {
                            trackCode = invItem.InvoiceNumber.Substring(0, 2);
                            invNo = invItem.InvoiceNumber.Substring(2);
                        }
                        else
                        {
                            trackCode = null;
                            invNo = invItem.InvoiceNumber;
                        }

                        if (this.EntityList.Any(i => i.No == invNo && i.TrackCode == trackCode))
                        {
                            result.Add(idx, new Exception(String.Format("發票號碼已存在:{0}", invItem.InvoiceNumber)));
                            continue;
                        }

                        var seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();
                        if (seller == null)
                        {
                            result.Add(idx, new Exception(String.Format("賣方為非註冊店家,統一編號:{0}", invItem.SellerId)));
                            continue;
                        }

                        Organization donatory = null;
                        bool bPrinted = invItem.PrintMark == "Y";
                        if (invItem.DonateMark == "1")
                        {
                            if (bPrinted)
                            {
                                result.Add(idx, new Exception(String.Format("發票已列印後不能捐贈,發票號碼:{0}", invItem.InvoiceNumber)));
                                continue;
                            }
                            donatory = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.NPOBAN).FirstOrDefault();
                            if (donatory == null || !this.GetTable<InvoiceWelfareAgency>().Any(w => w.AgencyID == donatory.CompanyID && w.SellerID == seller.CompanyID))
                            {
                                result.Add(idx, new Exception(String.Format("發票受捐社福單位不符,統一編號:{0}", invItem.NPOBAN)));
                                continue;
                            }
                        }

                        InvoiceUserCarrier carrier = null;
                        if (bPrinted)
                        {
                            if (!String.IsNullOrEmpty(invItem.CarrierType) || !String.IsNullOrEmpty(invItem.CarrierId1) || !String.IsNullOrEmpty(invItem.CarrierId2))
                            {
                                result.Add(idx, new Exception(String.Format("發票已列印後不能指定歸戶,發票號碼:{0}", invItem.InvoiceNumber)));
                                continue;
                            }
                        }
                        else
                        {
                            carrier = checkInvoiceCarrier(invItem);
                            if (carrier == null)
                            {
                                result.Add(idx, new Exception(String.Format("發票歸戶載具或卡號不符,發票號碼:{0}", invItem.InvoiceNumber)));
                                continue;
                            }
                        }

                        InvoiceItem newItem = new InvoiceItem
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocDate = DateTime.Now,
                                DocType = (int)Naming.DocumentTypeDefinition.E_Invoice,
                                DocumentOwner = new DocumentOwner
                                {
                                    OwnerID = seller.CompanyID
                                }
                            },
                            DonateMark = invItem.DonateMark,
                            InvoiceBuyer = new InvoiceBuyer
                            {
                                Name = invItem.BuyerName,
                                ReceiptNo = invItem.BuyerId,
                                CustomerName = invItem.BuyerName
                            },
                            InvoiceDate = DateTime.ParseExact(String.Format("{0} {1}", invItem.InvoiceDate, invItem.InvoiceTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture),
                            InvoiceType = byte.Parse(invItem.InvoiceType),
                            No = invNo,
                            TrackCode = trackCode,
                            SellerID = seller.CompanyID,
                            InvoiceByHousehold = carrier != null ? new InvoiceByHousehold { InvoiceUserCarrier = carrier } : null,
                            InvoicePrintAssertion = bPrinted ? new InvoicePrintAssertion { PrintDate = DateTime.Now } : null,
                            RandomNo = invItem.RandomNumber,
                            InvoiceAmountType = new InvoiceAmountType
                            {
                                DiscountAmount = invItem.DiscountAmount,
                                SalesAmount = (invItem.TaxType == 1) ? invItem.SalesAmount :
                                        (invItem.TaxType == 2) ? invItem.ZeroTaxSalesAmount :
                                        (invItem.TaxType == 3) ? invItem.FreeTaxSalesAmount : invItem.SalesAmount,
                                TaxAmount = invItem.TaxAmount,
                                TaxRate = invItem.TaxRate,
                                TaxType = invItem.TaxType,
                                TotalAmount = invItem.TotalAmount,
                                TotalAmountInChinese = Utility.ValueValidity.MoneyShow(invItem.TotalAmount)
                            },
                            DonationID = donatory != null ? donatory.CompanyID : (int?)null
                        };

                        var productItems = invItem.InvoiceItem.Select(i => new InvoiceProductItem
                        {
                            InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                            CostAmount = i.Amount,
                            ItemNo = i.Item,
                            Piece = (int?)i.Quantity,
                            PieceUnit = i.Unit,
                            UnitCost = i.UnitPrice,
                            Remark = i.Remark,
                            TaxType = i.TaxType
                        });

                        newItem.InvoiceDetails.AddRange(productItems.Select(p => new InvoiceDetail
                        {
                            InvoiceProduct = p.InvoiceProduct
                        }));

                        this.EntityList.InsertOnSubmit(newItem);

                        this.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }
            }

            return result;
        }

        private InvoiceUserCarrier checkInvoiceCarrier(InvoiceRootInvoice invItem)
        {
            Expression<Func<InvoiceUserCarrier, bool>> expr = null;
            if (!String.IsNullOrEmpty(invItem.CarrierId1))
            {
                expr = c => c.CarrierNo == invItem.CarrierId1;
            }
            if (!String.IsNullOrEmpty(invItem.CarrierId2))
            {
                if (expr == null)
                {
                    expr = c => c.CarrierNo2 == invItem.CarrierId2;
                }
                else
                {
                    expr = expr.And(c => c.CarrierNo2 == invItem.CarrierId2);
                }
            }

            if (expr == null)
                return null;

            var carrier = this.GetTable<InvoiceUserCarrier>().Where(expr).FirstOrDefault();

            if (carrier == null)
            {
                if (String.IsNullOrEmpty(invItem.CarrierType))
                    return null;
                var carrierType = this.GetTable<InvoiceUserCarrierType>().Where(t => t.CarrierType == invItem.CarrierType).FirstOrDefault();
                if (carrierType == null)
                {
                    //carrierType = new InvoiceUserCarrierType
                    //{
                    //    CarrierType = invItem.CarrierType
                    //};
                    return null;
                }
                carrier = new InvoiceUserCarrier
                    {
                        InvoiceUserCarrierType = carrierType,
                        CarrierNo = invItem.CarrierId1,
                        CarrierNo2 = invItem.CarrierId2
                    };
            }

            return carrier;
        }

        public Dictionary<int, Exception> SaveUploadInvoiceCancellation(CancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.CancelInvoice != null && item.CancelInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.CancelInvoice.Length; idx++)
                {
                    var invItem = item.CancelInvoice[idx];
                    try
                    {

                        String invNo, trackCode;
                        if (invItem.CancelInvoiceNumber.Length >= 10)
                        {
                            trackCode = invItem.CancelInvoiceNumber.Substring(0, 2);
                            invNo = invItem.CancelInvoiceNumber.Substring(2);
                        }
                        else
                        {
                            trackCode = null;
                            invNo = invItem.CancelInvoiceNumber;
                        }
                        var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

                        if (invoice == null)
                        {
                            result.Add(idx, new Exception(String.Format("發票號碼不存在:{0}", invItem.CancelInvoiceNumber)));
                            continue;
                        }

                        if (invoice.InvoiceCancellation != null)
                        {
                            result.Add(idx, new Exception(String.Format("作廢發票已存在,發票號碼:{0}", invItem.CancelInvoiceNumber)));
                            continue;
                        }

                        InvoiceCancellation cancelItem = new InvoiceCancellation
                        {
                            InvoiceItem = invoice,
                            CancellationNo = invItem.CancelInvoiceNumber,
                            Remark = invItem.Remark,
                            ReturnTaxDocumentNo = invItem.ReturnTaxDocumentNumber,
                            CancelDate = DateTime.ParseExact(String.Format("{0} {1}", invItem.CancelDate, invItem.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
                        };

                        var doc = new DerivedDocument
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                                DocDate = DateTime.Now,
                                DocumentOwner = new DocumentOwner
                                {
                                    OwnerID = owner.CompanyID
                                }
                            },
                            SourceID = invoice.InvoiceID
                        };
                        this.GetTable<DerivedDocument>().InsertOnSubmit(doc);

                        this.SubmitChanges();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }
            }

            return result;
        }

        public IEnumerable<WelfareReplication> GetUpdatedWelfareAgenciesForSeller(String receiptNo)
        {
            return this.GetTable<WelfareReplication>().Where(w => w.InvoiceWelfareAgency.Organization.ReceiptNo == receiptNo);
        }

        public IEnumerable<InvoiceWelfareAgency> GetWelfareAgenciesForSeller(String receiptNo)
        {
            return this.GetTable<InvoiceWelfareAgency>().Where(w => w.Organization.ReceiptNo == receiptNo);
        }


    }
}
