using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.InvoiceManagement.enUS;
using Model.Locale;
using Model.Properties;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement
{
    public partial class GoogleInvoiceManager : InvoiceManagerV2
    {
        public static string AttachmentPoolPath
        {
            get;
            private set;
        }

        static GoogleInvoiceManager()
        {
            AttachmentPoolPath = Path.Combine(Logger.LogPath, Settings.Default.UploadedFilePath);

            if (!Directory.Exists(AttachmentPoolPath))
                Directory.CreateDirectory(AttachmentPoolPath);
        }


        public GoogleInvoiceManager()
            : base()
        {

        }
        public GoogleInvoiceManager(GenericManager<EIVOEntityDataContext> mgr)
            : base(mgr)
        {

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

                        if ((ex = invItem.CheckDataNumber(seller,this, out order)) != null)
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
                            result.Add(idx, new Exception("Not Set invoice word has run track or invoice number"));
                            continue;
                        }
                        else
                        {
                            newItem.InvoiceDate = DateTime.Now;
                        }

                        this.EntityList.InsertOnSubmit(newItem);
                        checkAttachmentFromPool(order);

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

        private void checkAttachmentFromPool(InvoicePurchaseOrder order)
        {
            //發票附件檢查

            #region 抓取暫存資料夾內檔案名稱

            var fileList = Directory.GetFiles(AttachmentPoolPath,String.Format("{0}*.*",order.OrderNo));
            if (fileList.Length > 0)
            {
                Dictionary<String, String> fileItems = new Dictionary<string, string>();

                //取得暫存資料夾底下檔案名稱
                foreach (var tempFile in fileList)
                {
                    String fileName = Path.GetFileName(tempFile);
                    String storedPath = Path.Combine(Logger.LogDailyPath, fileName);

                    fileItems.Add(tempFile, storedPath);
                    String keyName = Path.GetFileNameWithoutExtension(fileName);

                    order.InvoiceItem.CDS_Document
                        .Attachment.Add(new Attachment
                        {
                            KeyName = keyName,
                            StoredPath = storedPath
                        });
                }

                ThreadPool.QueueUserWorkItem(stateInfo =>
                    {
                        foreach (var item in fileItems)
                        {
                            if (File.Exists(item.Value))
                            {
                                File.Delete(item.Value);
                            }
                            File.Move(item.Key, item.Value);
                        }
                    });
            }
            #endregion
        }

        public override Dictionary<int, Exception> SaveUploadInvoiceCancellation(CancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.CancelInvoice != null && item.CancelInvoice.Length > 0)
            {
                EventItems = null;
                List<InvoiceItem> eventItems = new List<InvoiceItem>();

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

                        eventItems.Add(cancelItem.InvoiceItem);

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
                    eventItems.Select(i => i.InvoiceID).SendCancellingInvoiceNotification();
                }

                EventItems = eventItems;
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

                this.EventItems_Allowance = null;
                List<InvoiceAllowance> eventItems = new List<InvoiceAllowance>();

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
                            },                            
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

                        eventItems.Add(newItem);
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

        public override Dictionary<int, Exception> SaveUploadAllowanceCancellation(CancelAllowanceRoot root, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (root != null && root.CancelAllowance != null && root.CancelAllowance.Length > 0)
            {
                var tblAllowance = this.GetTable<InvoiceAllowance>();
                var tblCancel = this.GetTable<InvoiceAllowanceCancellation>();

                EventItems = null;
                List<InvoiceAllowance> eventItems = new List<InvoiceAllowance>();

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

                        eventItems.Add(cancelItem.InvoiceAllowance);
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
    }
}
