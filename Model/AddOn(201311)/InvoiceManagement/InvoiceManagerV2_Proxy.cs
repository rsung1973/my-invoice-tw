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
    public class InvoiceManagerV2_Proxy : InvoiceManagerV2
    {
        public InvoiceManagerV2_Proxy() : base() { }
        public InvoiceManagerV2_Proxy(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }


        public override Dictionary<int, Exception> SaveUploadInvoice(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
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


                        if ((ex = invItem.CheckBusiness_Proxy(this, owner, out seller)) != null)
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
                        if ((ex = invItem.CheckMandatoryFields_Proxy(this, owner, out invoice, out cancelDate)) != null)
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
                        if ((ex = allowanceItem.CheckBusiness_Proxy(this, owner, out seller)) != null)
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

                        if ((ex = item.CheckMandatoryFields_Proxy(this, owner, out allowance, out cancelDate)) != null)
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

        public override Dictionary<int, Exception> SaveUploadInvoiceAutoTrackNo(InvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();


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
                        if ((ex = invItem.CheckBusiness_Proxy(this, owner, out seller)) != null)
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
                        TrackNoManager trackNoMgr = new TrackNoManager(this, seller.CompanyID);
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
                        trackNoMgr.Dispose();
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


            return result;
        }
    }
}