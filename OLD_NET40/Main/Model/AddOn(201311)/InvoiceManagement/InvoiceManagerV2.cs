using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.zhTW;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement
{
    public class InvoiceManagerV2 : InvoiceManager
    {
        public InvoiceManagerV2() : base() { }
        public InvoiceManagerV2(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        protected Func<Exception> _checkUploadInvoice;
        protected Func<Exception> _checkUploadAllowance;


        public override Dictionary<int, Exception> SaveUploadInvoice(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                EventItems = null;
                List<InvoiceItem> eventItem = new List<InvoiceItem>();

                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();

                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Invoice[idx];

                        Exception ex;
                        Organization seller;
                        //InvoicePurchaseOrder order;
                        String trackCode, invNo;
                        DateTime invoiceDate;

                        if ((ex = invItem.CheckInvoice(this, out trackCode, out invNo, out invoiceDate)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }


                        if ((ex = invItem.CheckBusiness(this, owner, out seller)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        //if ((ex = invItem.CheckDataNumber(this, out order)) != null)
                        //{
                        //    result.Add(idx, ex);
                        //    continue;
                        //}

                        if ((ex = invItem.CheckAmount()) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        #region 列印、捐贈、載具

                        bool all_printed = seller.OrganizationStatus.PrintAll == true;
                        bool print_mark = invItem.PrintMark == "y" || invItem.PrintMark == "Y";

                        InvoiceDonation donation;
                        if ((ex = invItem.CheckInvoiceDonation(seller, all_printed, print_mark, out donation)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceCarrier carrier;

                        if ((ex = invItem.CheckInvoiceCarrier(seller, donation, all_printed, print_mark, out carrier)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if ((ex = invItem.CheckMandatoryFields(seller, all_printed, print_mark)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        #endregion

                        IEnumerable<InvoiceProductItem> productItems;
                        if ((ex = invItem.CheckInvoiceProductItems(out productItems)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }


                        if (_checkUploadInvoice != null)
                        {
                            ex = _checkUploadInvoice();
                            if (ex != null)
                            {
                                result.Add(idx, ex);
                                continue;
                            }
                        }

                        InvoiceItem newItem = createInvoiceItem(owner, invItem, seller, null, print_mark, all_printed, carrier, donation, productItems);
                        newItem.No = invNo;
                        newItem.TrackCode = trackCode;
                        newItem.InvoiceDate = invoiceDate;

                        this.EntityList.InsertOnSubmit(newItem);
                        this.SubmitChanges();

                        eventItem.Add(newItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }

                if (eventItem.Count > 0)
                {
                    HasItem = true;
                    eventItem.Select(i => i.InvoiceID).SendIssuingNotification();
                }

                EventItems = eventItem;

            }
            return result;
        }

        public override Dictionary<int, Exception> SaveUploadInvoiceCancellation(CancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.CancelInvoice != null && item.CancelInvoice.Length > 0)
            {
                EventItems = null;
                var eventItem = new List<InvoiceItem>();

                for (int idx = 0; idx < item.CancelInvoice.Length; idx++)
                {
                    var invItem = item.CancelInvoice[idx];
                    try
                    {
                        Exception ex;
                        InvoiceItem invoice;
                        DateTime cancelDate;
                        if ((ex = invItem.CheckMandatoryFields(this, owner, out invoice, out cancelDate)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceCancellation cancelItem = new InvoiceCancellation
                        {
                            InvoiceItem = invoice,
                            CancellationNo = invItem.CancelInvoiceNumber,
                            Remark = invItem.Remark,
                            ReturnTaxDocumentNo = invItem.ReturnTaxDocumentNumber,
                            CancelDate = cancelDate,
                            CancelReason = invItem.CancelReason
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

                        eventItem.Add(invoice);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }

                if (eventItem.Count > 0)
                {
                    HasItem = true;
                    eventItem.Select(i => i.InvoiceID).SendCancellingInvoiceNotification();
                }

                EventItems = eventItem;

            }
            return result;
        }
        public Dictionary<int, Exception> SaveUploadInvoiceCancellation_C0501(Model.Schema.MIG3_1.C0501.CancelInvoice item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null)
            {
                int idx = 0;
                var invItem = item;
                EventItems = null;
                var eventItem = new List<InvoiceItem>();
                try
                {
                    Exception ex;
                    InvoiceItem invoice;
                    DateTime cancelDate;
                    if ((ex = invItem.CheckMandatoryFields(this, owner, out invoice, out cancelDate)) != null)
                    {
                        result.Add(idx, ex);
                    }

                    if (result.Count == 0)
                    {
                        InvoiceCancellation cancelItem = new InvoiceCancellation
                        {
                            InvoiceItem = invoice,
                            CancellationNo = invItem.CancelInvoiceNumber,
                            Remark = invItem.Remark,
                            ReturnTaxDocumentNo = invItem.ReturnTaxDocumentNumber,
                            CancelDate = cancelDate,
                            CancelReason = invItem.CancelReason
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

                        eventItem.Add(invoice);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    result.Add(idx, ex);
                }

                if (eventItem.Count > 0)
                {
                    HasItem = true;
                    eventItem.Select(i => i.InvoiceID).SendCancellingInvoiceNotification();
                }

                EventItems = eventItem;

            }

            return result;
        }


        public override Dictionary<int, Exception> SaveUploadAllowance(AllowanceRoot root, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (root != null && root.Allowance != null && root.Allowance.Length > 0)
            {
                var table = this.GetTable<InvoiceAllowance>();
                var tblOrg = this.GetTable<Organization>();
                var invTable = this.GetTable<InvoiceItem>();
                for (int idx = 0; idx < root.Allowance.Length; idx++)
                {
                    var allowanceItem = root.Allowance[idx];
                    try
                    {

                        Exception ex;
                        Organization seller;
                        if ((ex = allowanceItem.CheckBusiness(this, owner, out seller)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        DateTime allowanceDate;
                        if ((ex = allowanceItem.CheckMandatoryFields(this, out allowanceDate)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        ////賣方統一編號：針對Google
                        //if (_allowanceItem.SellerId == "27934855")
                        //{
                        //    if (String.IsNullOrEmpty(_allowanceItem.GoogleID))
                        //    {
                        //        result.Add(idx, new Exception("必須填入GoogleID"));
                        //        continue;
                        //    }

                        //    if (_allowanceItem.GoogleID.Length > 64)
                        //    {
                        //        result.Add(idx, new Exception(String.Format("GoogleID最長為64碼，傳送資料長度：{0}", _allowanceItem.GoogleID.Length)));
                        //        continue;
                        //    }
                        //}

                        List<InvoiceAllowanceItem> productItems;
                        if ((ex = allowanceItem.CheckAllowanceItem(this, out productItems)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }


                        InvoiceAllowance newItem = new InvoiceAllowance
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocDate = DateTime.Now,
                                DocType = (int)Naming.DocumentTypeDefinition.E_Allowance
                            },
                            AllowanceDate = allowanceDate,
                            AllowanceNumber = allowanceItem.AllowanceNumber,
                            AllowanceType = byte.Parse(allowanceItem.AllowanceType),
                            BuyerId = allowanceItem.BuyerId,
                            SellerId = allowanceItem.SellerId,
                            TaxAmount = allowanceItem.TaxAmount,
                            TotalAmount = allowanceItem.TotalAmount,
                            //InvoiceID =  invTable.Where(i=>i.TrackCode + i.No == item.AllowanceItem.Select(a=>a.OriginalInvoiceNumber).FirstOrDefault()).Select(i=>i.InvoiceID).FirstOrDefault(),
                            InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                            {
                                Name = allowanceItem.BuyerName,
                                ReceiptNo = allowanceItem.BuyerId,
                                CustomerID = String.IsNullOrEmpty(allowanceItem.GoogleId) ? "" : allowanceItem.GoogleId,
                                ContactName = allowanceItem.BuyerName,
                                CustomerName = allowanceItem.BuyerName
                            },
                            InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                            {
                                Name = seller.CompanyName,
                                ReceiptNo = seller.ReceiptNo,
                                Address = seller.Addr,
                                ContactName = seller.ContactName,
                                CustomerID = String.IsNullOrEmpty(allowanceItem.GoogleId) ? "" : allowanceItem.GoogleId,
                                CustomerName = seller.CompanyName,
                                EMail = seller.ContactEmail,
                                Fax = seller.Fax,
                                Phone = seller.Phone,
                                PersonInCharge = seller.UndertakerName,
                                SellerID = seller.CompanyID,
                            }
                        };

                        newItem.InvoiceAllowanceDetails.AddRange(productItems.Select(p => new InvoiceAllowanceDetail
                        {
                            InvoiceAllowanceItem = p
                        }));

                        if (owner != null)
                        {
                            newItem.CDS_Document.DocumentOwner = new DocumentOwner
                            {
                                OwnerID = owner.CompanyID,
                            };
                        }

                        table.InsertOnSubmit(newItem);
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

        public override Dictionary<int, Exception> SaveUploadAllowanceCancellation(CancelAllowanceRoot root, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (root != null && root.CancelAllowance != null && root.CancelAllowance.Length > 0)
            {
                this.EventItems_Allowance = null;
                List<InvoiceAllowance> eventItems = new List<InvoiceAllowance>();

                var tblAllowance = this.GetTable<InvoiceAllowance>();
                var tblCancel = this.GetTable<InvoiceAllowanceCancellation>();

                for (int idx = 0; idx < root.CancelAllowance.Length; idx++)
                {
                    var item = root.CancelAllowance[idx];
                    try
                    {
                        Exception ex;
                        InvoiceAllowance allowance;
                        DateTime cancelDate;

                        if ((ex = item.CheckMandatoryFields(this, owner, out allowance, out cancelDate)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceAllowanceCancellation cancelItem = new InvoiceAllowanceCancellation
                               {
                                   InvoiceAllowance = allowance,
                                   AllowanceID = allowance.AllowanceID,
                                   Remark = item.Remark,
                                   CancelDate = cancelDate,
                                   CancelReason = item.CancelReason,
                               };

                        var p = new DerivedDocument
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocDate = DateTime.Now,
                                DocType = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                            },
                            SourceID = allowance.AllowanceID,
                        };

                        this.GetTable<DerivedDocument>().InsertOnSubmit(p);
                        this.SubmitChanges();

                        eventItems.Add(allowance);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }

                EventItems_Allowance = eventItems;

            }
            return result;
        }

        public override Dictionary<int, Exception> SaveUploadInvoiceAutoTrackNo(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            TrackNoManager trackNoMgr = new TrackNoManager(this, owner.CompanyID);

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                //Organization donatory = owner.Organization.InvoiceWelfareAgencies.Select(w => w.WelfareAgency.Organization).FirstOrDefault();
                EventItems = null;
                List<InvoiceItem> eventItems = new List<InvoiceItem>();

                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    try
                    {
                        var invItem = item.Invoice[idx];

                        Exception ex;
                        Organization seller;
                        InvoicePurchaseOrder order;
                        if ((ex = invItem.CheckBusiness(this, owner, out seller)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if ((ex = invItem.CheckDataNumber(seller, this, out order)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if ((ex = invItem.CheckAmount()) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        #region 列印、捐贈、載具

                        bool all_printed = seller.OrganizationStatus.PrintAll == true;
                        bool print_mark = invItem.PrintMark == "y" || invItem.PrintMark == "Y";

                        InvoiceDonation donation;
                        if ((ex = invItem.CheckInvoiceDonation(seller, all_printed, print_mark, out donation)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceCarrier carrier;

                        if ((ex = invItem.CheckInvoiceCarrier(seller, donation, all_printed, print_mark, out carrier)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if ((ex = invItem.CheckMandatoryFields(seller, all_printed, print_mark)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        #endregion

                        IEnumerable<InvoiceProductItem> productItems;
                        if ((ex = invItem.CheckInvoiceProductItems(out productItems)) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        InvoiceItem newItem = createInvoiceItem(owner, invItem, seller, order, print_mark, all_printed, carrier, donation, productItems);

                        if (!trackNoMgr.CheckInvoiceNo(newItem))
                        {
                            result.Add(idx, new Exception("未設定發票字軌或發票號碼已用完"));
                            continue;
                        }
                        else
                        {
                            newItem.InvoiceDate = DateTime.Now;
                        }

                        this.EntityList.InsertOnSubmit(newItem);
                        this.SubmitChanges();

                        eventItems.Add(newItem);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        result.Add(idx, ex);
                    }
                }

                if (eventItems.Count > 0)
                {
                    HasItem = true;
                    eventItems.Select(i => i.InvoiceID).SendIssuingNotification();
                }
                EventItems = eventItems;
            }

            trackNoMgr.Dispose();
            return result;
        }

        protected InvoiceItem createInvoiceItem(OrganizationToken owner, InvoiceRootInvoice invItem, Organization seller, InvoicePurchaseOrder order, bool Final_printed, bool all_printed, InvoiceCarrier carrier, InvoiceDonation donation, IEnumerable<InvoiceProductItem> productItems)
        {
            InvoiceItem newItem = new InvoiceItem
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Invoice
                },
                DonateMark = donation == null ? "0" : "1",
                InvoiceType = byte.Parse(invItem.InvoiceType),
                //No = invNo,
                //TrackCode = trackCode,
                SellerID = seller.CompanyID,
                CustomsClearanceMark = invItem.CustomsClearanceMark,
                InvoiceSeller = new InvoiceSeller
                {
                    Name = seller.CompanyName,
                    ReceiptNo = seller.ReceiptNo,
                    Address = seller.Addr,
                    ContactName = seller.ContactName,
                    //CustomerID = String.IsNullOrEmpty(invItem.GoogleId) ? "" : invItem.GoogleId,
                    CustomerName = seller.CompanyName,
                    EMail = seller.ContactEmail,
                    Fax = seller.Fax,
                    Phone = seller.Phone,
                    PersonInCharge = seller.UndertakerName,
                    SellerID = seller.CompanyID,
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invItem.BuyerName,
                    ReceiptNo = invItem.BuyerId,
                    CustomerID = String.IsNullOrEmpty(invItem.GoogleId) ? "" : invItem.GoogleId,
                    ContactName = invItem.Contact != null ? (String.IsNullOrEmpty(invItem.Contact.Name) ? "" : invItem.Contact.Name) : "",
                    Address = invItem.Contact != null ? (String.IsNullOrEmpty(invItem.Contact.Address) ? "" : invItem.Contact.Address) : "",
                    Phone = invItem.Contact != null ? (String.IsNullOrEmpty(invItem.Contact.TEL) ? "" : invItem.Contact.TEL) : "",
                    EMail = invItem.Contact != null ? (String.IsNullOrEmpty(invItem.Contact.Email) ? "" : invItem.Contact.Email.Replace(',', ';').Replace(' ', ';').Replace('、', ';')) : "",
                    CustomerName = invItem.BuyerName,
                },
                //InvoiceByHousehold = carrier != null ? new InvoiceByHousehold { InvoiceUserCarrier = carrier } : null,
                //InvoicePrintAssertion = bPrinted ? new InvoicePrintAssertion { PrintDate = DateTime.Now } : null,
                RandomNo = String.IsNullOrEmpty(invItem.RandomNumber) ? ValueValidity.GenerateRandomCode(4) : invItem.RandomNumber,
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
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(invItem.TotalAmount),
                },
                //DonationID = donatory != null ? donatory.CompanyID : (int?)null,
                InvoiceCarrier = carrier,
                InvoiceDonation = donation,
                PrintMark = (invItem.BuyerId != "0000000000") == true ? "Y" : ((all_printed && !Final_printed) == true ? "N" : (Final_printed == true ? "Y" : "N")),
            };

            if (order != null)
            {
                newItem.InvoicePurchaseOrder = order;
            }

            newItem.InvoiceDetails.AddRange(productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct,
            }));

            if (owner != null)
            {
                newItem.CDS_Document.DocumentOwner = new DocumentOwner
                {
                    OwnerID = owner.CompanyID,
                };
            }
            return newItem;
        }


        public Dictionary<int, Exception> SaveUploadInvoice_C0401(Model.Schema.MIG3_1.C0401.Invoice invoice, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            List<InvoiceItem> eventItem = new List<InvoiceItem>();

            int idx = 0;
            try
            {
                Exception ex;
                Organization seller;
                String trackCode, invNo;
                DateTime invoiceDate;

                if ((ex = InvoiceRootValidator_C0401.CheckDataLength(invoice)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                if ((ex = InvoiceRootValidator_C0401.CheckInvoice(invoice, this, out trackCode, out invNo, out invoiceDate)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                if ((ex = InvoiceRootValidator_C0401.CheckBusiness(invoice, this, owner, out seller)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                if ((ex = invoice.CheckAmount()) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }


                #region 列印、捐贈、載具

                bool all_printed = seller.OrganizationStatus.PrintAll == true;
                bool print_mark = invoice.Main.PrintMark == "y" || invoice.Main.PrintMark == "Y";

                InvoiceDonation donation;
                if ((ex = Check_CarrierType_PrintMark_DonateMark.CheckInvoiceDonation_C0401(invoice, seller, all_printed, print_mark, out donation)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                InvoiceCarrier carrier;

                if ((ex = Check_CarrierType_PrintMark_DonateMark.CheckInvoiceCarrier_C0401(invoice, seller, donation, all_printed, print_mark, out carrier)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                if ((ex = Check_CarrierType_PrintMark_DonateMark.CheckMandatoryFields_C0401(invoice, seller, all_printed, print_mark)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                #endregion

                IEnumerable<InvoiceProductItem> productItems;
                if ((ex = InvoiceRootValidator_C0401.CheckInvoiceProductItems(invoice, out productItems)) != null)
                {
                    result.Add(idx, ex);
                    idx = idx + 1;
                }

                if (_checkUploadInvoice != null)
                {
                    ex = _checkUploadInvoice();
                    if (ex != null)
                    {
                        result.Add(idx, ex);
                        idx = idx + 1;
                    }
                }


                if (result.Count == 0)
                {
                    InvoiceItem newItem = ConvertToInvoiceItem(invoice, owner, invoiceDate, carrier, seller, all_printed, print_mark);
                    this.EntityList.InsertOnSubmit(newItem);
                    this.SubmitChanges();
                    eventItem.Add(newItem);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                result.Add(idx, ex);
            }

            if (eventItem.Count > 0)
            {
                HasItem = true;
                eventItem.Select(i => i.InvoiceID).SendIssuingNotification();
            }


            return result;
        }

        private InvoiceItem ConvertToInvoiceItem(Schema.MIG3_1.C0401.Invoice invoice, OrganizationToken owner, DateTime invoiceDate, InvoiceCarrier carrier, Organization seller, bool all_printed, bool Final_printed)
        {
            String invNo, trackCode;
            getInvoiceNo(invoice.Main.InvoiceNumber, out invNo, out trackCode);

            InvoiceItem newItem = new InvoiceItem
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    //CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invoice.Main.Buyer.Name,
                    ReceiptNo = invoice.Main.Buyer.Identifier,
                    ContactName = invoice.Main.Buyer.PersonInCharge,
                    Address = invoice.Main.Buyer.Address,
                    //CustomerID = invoice.Main.Buyer.CustomerNumber,
                    CustomerName = invoice.Main.Buyer.Name,
                    EMail = invoice.Main.Buyer.EmailAddress,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    PersonInCharge = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    RoleRemark = invoice.Main.Buyer.RoleRemark,
                    CustomerNumber = invoice.Main.Buyer.CustomerNumber,
                },
                InvoiceSeller = new InvoiceSeller
                {
                    Name = invoice.Main.Seller.Name,
                    ReceiptNo = invoice.Main.Seller.Identifier,
                    ContactName = invoice.Main.Seller.PersonInCharge,
                    Address = invoice.Main.Seller.Address,
                    CustomerID = invoice.Main.Seller.CustomerNumber,
                    CustomerName = invoice.Main.Seller.Name,
                    EMail = invoice.Main.Seller.EmailAddress,
                    Fax = invoice.Main.Seller.FacsimileNumber,
                    PersonInCharge = invoice.Main.Seller.PersonInCharge,
                    Phone = invoice.Main.Seller.TelephoneNumber,
                    RoleRemark = invoice.Main.Seller.RoleRemark,
                    SellerID = seller.CompanyID,
                },
                InvoiceDate = invoiceDate,
                InvoiceType = (byte)((int)invoice.Main.InvoiceType),
                No = invNo,
                TrackCode = trackCode,
                Organization = seller,
                BuyerRemark = invoice.Main.BuyerRemarkSpecified ? String.Format("{0}", (int)invoice.Main.BuyerRemark) : null,
                Category = invoice.Main.Category,
                CheckNo = invoice.Main.CheckNumber,
                DonateMark = ((int)invoice.Main.DonateMark).ToString(),
                InvoiceDonation = new InvoiceDonation
                {
                    AgencyCode = invoice.Main.NPOBAN,
                },
                CustomsClearanceMark = invoice.Main.CustomsClearanceMarkSpecified ? ((int)invoice.Main.CustomsClearanceMark).ToString() : null,
                GroupMark = invoice.Main.GroupMark,
                //PermitDate = String.IsNullOrEmpty(invoice.Main.PermitDate) ? (DateTime?)null : DateTime.ParseExact(invoice.Main.PermitDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                //PermitNumber = invoice.Main.PermitNumber,
                //PermitWord = invoice.Main.PermitWord,
                RelateNumber = invoice.Main.RelateNumber,
                Remark = invoice.Main.MainRemark,
                InvoiceCarrier = carrier,
                PrintMark = (invoice.Main.Buyer.Identifier != "0000000000") == true ? "Y" : ((all_printed && !Final_printed) == true ? "N" : (Final_printed == true ? "Y" : "N")),
                RandomNo = invoice.Main.RandomNumber,
                //TaxCenter = invoice.Main.TaxCenter,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = invoice.Amount.DiscountAmountSpecified ? invoice.Amount.DiscountAmount : (decimal?)null,
                    SalesAmount = (int)invoice.Amount.TaxType == 1 ? (decimal)invoice.Amount.SalesAmount :
                                   (int)invoice.Amount.TaxType == 2 ? (decimal)invoice.Amount.ZeroTaxSalesAmount :
                                   (int)invoice.Amount.TaxType == 3 ? (decimal)invoice.Amount.FreeTaxSalesAmount : invoice.Amount.SalesAmount,
                    TaxAmount = invoice.Amount.TaxAmount,
                    TaxType = (byte)((int)invoice.Amount.TaxType),
                    TotalAmount = invoice.Amount.TotalAmount,
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(invoice.Amount.TotalAmount),
                    TaxRate = invoice.Amount.TaxRate,
                    CurrencyID = invoice.Amount.CurrencySpecified ? (int)invoice.Amount.Currency : (int?)null,
                    ExchangeRate = invoice.Amount.ExchangeRateSpecified ? invoice.Amount.ExchangeRate : (decimal?)null,
                    OriginalCurrencyAmount = invoice.Amount.OriginalCurrencyAmountSpecified ? invoice.Amount.OriginalCurrencyAmount : (decimal?)null
                }
            };

            short seqNo = 0;

            var productItems = invoice.Details.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                ItemNo = i.SequenceNumber,
                Piece = (int?)i.Quantity,
                PieceUnit = i.Unit,
                UnitCost = i.UnitPrice,
                Remark = i.Remark,
                TaxType = newItem.InvoiceAmountType.TaxType,
                RelateNumber = i.RelateNumber,
                No = (seqNo++)
            }).ToList();

            newItem.InvoiceDetails.AddRange(productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct
            }));
            return newItem;
        }

    }
}