using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Helper;
using Model.Locale;
using Model.Properties;
using Model.Schema.EIVO.B2B;
using Utility;
using System.Xml;
using System.Xml.Linq;


namespace Model.InvoiceManagement
{
    public partial class B2BInvoiceManager : InvoiceManager
    {
        private X509Certificate2 _signerCert;
        private bool _useSigner;

        public B2BInvoiceManager() : base() { }
        public B2BInvoiceManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

        public X509Certificate2 PrepareSignerCertificate(Organization org)
        {
            _useSigner = org.OrganizationStatus.Entrusting == true;
            var signerToken = org.OrganizationStatus.UserToken;
            if (_useSigner && signerToken != null)
            {
                _signerCert = new X509Certificate2(Convert.FromBase64String(signerToken.PKCS12), signerToken.Token.ToString().Substring(0, 8));
            }
            return _signerCert;
        }

        public virtual Dictionary<int, Exception> SaveUploadInvoice(SellerInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    var invItem = item.Invoice[idx];
                    try
                    {
                        Exception ex;
                        InvoiceItem newItem = ConvertToInvoiceItem(owner, invItem, out ex);

                        if (newItem == null || (ex = invItem.UploadInvoiceSpecializedCheck()) != null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (owner.CompanyID == newItem.InvoiceSeller.SellerID)
                        {
                            applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.電子發票, Naming.InvoiceCenterBusinessType.銷項);
                        }
                        else
                        {
                            applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.電子發票, Naming.InvoiceCenterBusinessType.進項);
                        }

                        this.EntityList.InsertOnSubmit(newItem);

                        this.SubmitChanges();

                       //EIVOPlatformFactory.SendInvoiceB2B(this, new EventArgs<InvoiceItem> { Argument = newItem });
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

        public InvoiceItem ConvertToInvoiceItem(OrganizationToken owner, SellerInvoiceRootInvoice invItem, out Exception ex)
        {
            ex = null;
            String invNo, trackCode;
            getInvoiceNo(invItem.InvoiceNumber, out invNo, out trackCode);

            var seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();
            if (seller == null)
            {
                ex = new Exception(String.Format("發票開立人為非註冊會員,統一編號:{0}", invItem.SellerId));
                return null;
            }

            //if (String.IsNullOrEmpty(seller.InvoiceSignature))
            //{
            //    ex = new Exception(String.Format("發票開立人尚未匯入統一發票專用章,統一編號:{0}", invItem.SellerId));
            //    return null;
            //}


            var buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.BuyerId).FirstOrDefault();
            if (buyer == null)
            {
                ex = new Exception(String.Format("發票買受人為非註冊會員,統一編號:{0}", invItem.BuyerId));
                return null;
            }

            //if (owner.CompanyID != seller.CompanyID && owner.CompanyID != buyer.CompanyID)
            //{
            //    ex = new Exception(String.Format("發票資料傳輸人非發票之開立人或買受人,發票號碼:{0}", invItem.InvoiceNumber));
            //    return null;
            //}

            if (this.EntityList.Any(i => i.No == invNo && i.TrackCode == trackCode))
            {
                ex = new Exception(String.Format("發票號碼已存在:{0}", invItem.InvoiceNumber));
                return null;
            }

            var relation = this.GetTable<BusinessRelationship>().Where(b => b.MasterID == seller.CompanyID && b.RelativeID == buyer.CompanyID && b.BusinessID == (int)Naming.InvoiceCenterBusinessType.銷項).FirstOrDefault();
            if (relation == null)
            {
                ex = new Exception(String.Format("發票買受人非為開立人之銷項相對營業人:{0}", invItem.InvoiceNumber));
                return null;
            }
            else if (relation.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
            {
                ex = new Exception(String.Format("已停用發票買受人為銷項相對營業人之關係:{0}", invItem.InvoiceNumber));
                return null;
            }


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
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待接收
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invItem.BuyerName,
                    ReceiptNo = invItem.BuyerId,
                    Address = buyer.Addr,
                    ContactName = buyer.ContactName,
                    CustomerName = buyer.CompanyName,
                    EMail = buyer.ContactEmail,
                    Fax = buyer.Fax,
                    Phone = buyer.Phone,
                    PersonInCharge = buyer.UndertakerName,
                    BuyerID = buyer.CompanyID
                },
                InvoiceSeller = new InvoiceSeller
                {
                    Name = seller.CompanyName,
                    ReceiptNo = seller.ReceiptNo,
                    Address = seller.Addr,
                    ContactName = seller.ContactName,
                    CustomerName = seller.CompanyName,
                    EMail = seller.ContactEmail,
                    Fax = seller.Fax,
                    Phone = seller.Phone,
                    PersonInCharge = seller.UndertakerName,
                    SellerID = seller.CompanyID
                },
                InvoiceDate = String.IsNullOrEmpty(invItem.InvoiceTime) ? DateTime.ParseExact(invItem.InvoiceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture) : DateTime.ParseExact(String.Format("{0} {1}", invItem.InvoiceDate, invItem.InvoiceTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture),
                InvoiceType = byte.Parse(invItem.InvoiceType),
                No = invNo,
                TrackCode = trackCode,
                SellerID = seller.CompanyID,
                InvoiceAmountType = new InvoiceAmountType
                {
                    //DiscountAmount = invItem.DiscountAmount,
                    SalesAmount = invItem.SalesAmount,
                    TaxAmount = invItem.TaxAmount,
                    TaxType = invItem.TaxType,
                    TotalAmount = invItem.TotalAmount,
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(invItem.TotalAmount)
                }
            };

            if (invItem.ExtraRemark != null && invItem.ExtraRemark is XmlNode[])
            {
                XElement extra = new XElement("ExtraRemark");
                foreach (XmlNode n in (XmlNode[])invItem.ExtraRemark)
                {
                    extra.Add(XElement.Load(new XmlNodeReader(n)));
                }
                newItem.InvoiceItemExtension = new InvoiceItemExtension
                {
                    ExtraRemark = extra
                };
            }

            short seqNo = 0;

            var productItems = invItem.InvoiceItem.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                ItemNo = String.Format("{0}", i.SequenceNumber),
                Piece = i.Quantity,
                Piece2 = i.Quantity2,
                PieceUnit = i.Unit,
                PieceUnit2 = i.Unit2,
                UnitCost = i.UnitPrice,
                UnitCost2 = i.UnitPrice2,
                CostAmount2 = i.Amount2,
                Remark = i.Remark,
                TaxType = invItem.TaxType,
                No = (seqNo++)
            }).ToList();

            newItem.InvoiceDetails.AddRange(productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct
            }));

            if (_useSigner)
            {
                if (_signerCert != null)
                {
                    //if (!newItem.SignAndCheckToIssueInvoiceItem(_signerCert,null))
                    //{
                    //    ex = new Exception(String.Format("發票開立人已設定自動開立＼接收，簽章失敗:{0}", invItem.InvoiceNumber));
                    //    return null;
                    //}
                }
                else
                {
                    ex = new Exception(String.Format("發票開立人已設定自動開立＼接收，但尚未設定簽章憑證:{0}", invItem.InvoiceNumber));
                    return null;
                }
            }

            return newItem;
        }

        public Dictionary<int, Exception> SaveUploadInvoiceCancellation(CancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.CancelInvoice != null && item.CancelInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.CancelInvoice.Length; idx++)
                {
                    var cancelInvoice = item.CancelInvoice[idx];
                    try
                    {
                        Exception ex;
                        InvoiceItem invoice;
                        InvoiceCancellation cancelItem = ConvertToInvoiceCancellation(owner, cancelInvoice, out ex, out invoice);

                        if (cancelItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        var doc = new DerivedDocument
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                                DocDate = DateTime.Now,
                                CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                                DocumentOwner = new DocumentOwner
                                {
                                    OwnerID = owner.CompanyID
                                }
                            },
                            SourceID = cancelItem.InvoiceID
                        };

                        if (invoice.SellerID == owner.CompanyID)
                        {
                            applyProcessFlow(doc.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.作廢發票, Naming.InvoiceCenterBusinessType.銷項);
                        }
                        else if (invoice.InvoiceBuyer.BuyerID == owner.CompanyID)
                        {
                            applyProcessFlow(doc.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.作廢發票, Naming.InvoiceCenterBusinessType.進項);
                        }

                        this.GetTable<InvoiceCancellation>().InsertOnSubmit(cancelItem);
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

        public InvoiceCancellation ConvertToInvoiceCancellation(OrganizationToken owner, CancelInvoiceRootCancelInvoice cancellation, out Exception ex, out InvoiceItem invoice)
        {
            ex = null;
            invoice = null;

            String invNo, trackCode;
            if (cancellation.CancelInvoiceNumber.Length >= 10)
            {
                trackCode = cancellation.CancelInvoiceNumber.Substring(0, 2);
                invNo = cancellation.CancelInvoiceNumber.Substring(2);
            }
            else
            {
                trackCode = null;
                invNo = cancellation.CancelInvoiceNumber;
            }
            invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

            if (invoice == null)
            {
                ex = new Exception(String.Format("發票號碼不存在:{0}", cancellation.CancelInvoiceNumber));
                return null;
            }

            if (invoice.InvoiceCancellation != null)
            {
                ex = new Exception(String.Format("作廢發票已存在,發票號碼:{0}", cancellation.CancelInvoiceNumber));
                return null;
            }

            if (invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送 && invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已接收)
            {
                if (invoice.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                {
                    ex = new Exception(String.Format("發票申請退回,無法作廢發票,發票號碼:{0}", cancellation.CancelInvoiceNumber));
                }
                else if (invoice.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已註銷)
                {
                    ex = new Exception(String.Format("發票已註銷,無法作廢發票,發票號碼:{0}", cancellation.CancelInvoiceNumber));
                }
                else
                {
                    ex = new Exception(String.Format("發票未被接收或開立,無法作廢發票,發票號碼:{0}", cancellation.CancelInvoiceNumber));                   
                }
                return null;
            }


            InvoiceCancellation cancelItem = new InvoiceCancellation
            {
                InvoiceID = invoice.InvoiceID,
                CancellationNo = cancellation.CancelInvoiceNumber,
                Remark = String.Format("{0}{1}", cancellation.CancelReason, cancellation.Remark),
                ReturnTaxDocumentNo = cancellation.ReturnTaxDocumentNumber,
                CancelDate = DateTime.ParseExact(String.Format("{0} {1}", cancellation.CancelDate, cancellation.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
            };

            if (_useSigner && invoice.SellerID == owner.CompanyID)
            {
                if (_signerCert != null)
                {
                    //if (!cancelItem.SignAndCheckToIssueInvoiceCancellation(invoice, _signerCert, null))
                    //{
                    //    ex = new Exception(String.Format("作廢發票開立人已設定自動開立＼接收，簽章失敗:{0}", cancellation.CancelInvoiceNumber));
                    //    return null;
                    //}
                }
                else
                {
                    ex = new Exception(String.Format("作廢發票開立人已設定自動開立＼接收，但尚未設定簽章憑證:{0}", cancellation.CancelInvoiceNumber));
                    return null;
                }
            }

            return cancelItem;
        }

        public virtual Dictionary<int, Exception> SaveUploadInvoiceReturn(ReturnInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.ReturnInvoice != null && item.ReturnInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.ReturnInvoice.Length; idx++)
                {
                    var invItem = item.ReturnInvoice[idx];
                    try
                    {
                        Exception ex;
                        var invoiceItem = IsInvoiceReturnable(owner, invItem, out ex);
                        if (invoiceItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (invoiceItem.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收)
                        {
                            invoiceItem.CDS_Document.MoveToFifthBranchStep(this);
                        }
                        else
                        {
                            invoiceItem.CDS_Document.MoveToThirdBranchStep(this);
                        }

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

        public InvoiceItem IsInvoiceReturnable(OrganizationToken owner, ReturnInvoiceRootReturnInvoice invItem, out Exception ex)
        {
            ex = null;
            String invNo, trackCode;
            getInvoiceNo(invItem.Number, out invNo, out trackCode);

            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();
            if (invoice == null)
            {
                ex = new Exception(String.Format("發票號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != invoice.InvoiceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回發票之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != invoice.InvoiceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回發票之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            if (invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("發票不能退回,發票號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)invoice.CDS_Document.CurrentStep).ToString()));
                return null;
            }

            //判斷是否有開立折讓,且折讓未被註銷
            if (invoice.InvoiceAllowances.Any(a => !(a.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已作廢 || a.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已註銷)))
            {
                ex = new Exception(String.Format("折讓證明單已提出,發票不能退回,發票號碼:{0}", invItem.Number));
                return null;
            }

            //判斷是否有開立作廢發票,且作廢發票未被註銷
            if (this.GetTable<CDS_Document>().Any(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票
                    && d.DerivedDocument.SourceID == invoice.InvoiceID
                    && d.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已註銷))
            {
                ex = new Exception(String.Format("作廢發票已提出,發票不能退回,發票號碼:{0}", invItem.Number));
                return null;
            }

            return invoice;
        }

        public virtual Dictionary<int, Exception> SaveUploadInvoiceDelete(DeleteInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.DeleteInvoice != null && item.DeleteInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.DeleteInvoice.Length; idx++)
                {
                    var invItem = item.DeleteInvoice[idx];
                    try
                    {
                        Exception ex;
                        var invoiceItem = IsInvoiceRevocable(owner, invItem, out ex);
                        if (invoiceItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (invoiceItem.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                        {
                            invoiceItem.CDS_Document.MoveToFirstBranchStep(this);
                        }
                        else if (invoiceItem.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
                        {
                            invoiceItem.CDS_Document.MoveToSecondBranchStep(this);
                        }
                        else
                        {
                            invoiceItem.CDS_Document.MoveToForthBranchStep(this);
                        }

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

        public InvoiceItem IsInvoiceRevocable(OrganizationToken owner, DeleteInvoiceRootDeleteInvoice invItem, out Exception ex)
        {

            ex = null;
            String invNo, trackCode;
            getInvoiceNo(invItem.Number, out invNo, out trackCode);

            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();
            if (invoice == null)
            {
                ex = new Exception(String.Format("發票號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != invoice.InvoiceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷發票之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != invoice.InvoiceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷發票之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            //if (invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && invoice.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回)
            if (invoice.InvoiceCancellation != null &&
                (invoice.CDS_Document.ChildDocument.Any(r => r.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && r.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已註銷)
                && invoice.CDS_Document.ChildDocument.Any(r => r.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && r.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回)))
            {
                //ex = new Exception(String.Format("發票無法註銷,發票號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)invoice.CDS_Document.CurrentStep).ToString()));
                ex = new Exception(String.Format("發票無法註銷,發票號碼:{0},已作廢", invItem.Number));
                return null;
            }

            return invoice;
        }

        public virtual Dictionary<int, Exception> SaveUploadInvoiceCancellationDelete(DeleteCancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.DeleteCancelInvoice != null && item.DeleteCancelInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.DeleteCancelInvoice.Length; idx++)
                {
                    var invItem = item.DeleteCancelInvoice[idx];
                    try
                    {
                        Exception ex;
                        var docItem = IsInvoiceCancellationRevocable(owner, invItem, out ex);

                        if (docItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                        {
                            docItem.MoveToFirstBranchStep(this);
                        }
                        else if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
                        {
                            docItem.MoveToSecondBranchStep(this);
                        }
                        else
                        {
                            docItem.MoveToForthBranchStep(this);
                        }

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

        public CDS_Document IsInvoiceCancellationRevocable(OrganizationToken owner, DeleteCancelInvoiceRootDeleteCancelInvoice invItem, out Exception ex)
        {
            ex = null;
            String invNo, trackCode;
            getInvoiceNo(invItem.Number, out invNo, out trackCode);

            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();
            if (invoice == null)
            {
                ex = new Exception(String.Format("發票號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != invoice.InvoiceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷作廢發票之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != invoice.InvoiceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷作廢發票之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            if (invoice.InvoiceCancellation == null)
            {
                ex = new Exception(String.Format("作廢發票不存在,無法註銷,發票號碼:{0}", invItem.Number));
                return null;
            }

            CDS_Document docItem = this.GetTable<CDS_Document>().Where(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && d.DerivedDocument.SourceID == invoice.InvoiceID).FirstOrDefault();

            if (docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("作廢發票無法註銷,號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()));
                return null;
            }

            return docItem;
        }

        public virtual Dictionary<int, Exception> SaveUploadAllowanceDelete(DeleteAllowanceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.DeleteAllowance != null && item.DeleteAllowance.Length > 0)
            {
                for (int idx = 0; idx < item.DeleteAllowance.Length; idx++)
                {
                    var invItem = item.DeleteAllowance[idx];
                    try
                    {
                        Exception ex;
                        var allowanceItem = IsAllowanceRevocable(owner, invItem, out ex);
                        if (allowanceItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (allowanceItem.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                        {
                            allowanceItem.CDS_Document.MoveToFirstBranchStep(this);
                        }
                        else if (allowanceItem.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
                        {
                            allowanceItem.CDS_Document.MoveToSecondBranchStep(this);
                        }
                        else
                        {
                            allowanceItem.CDS_Document.MoveToForthBranchStep(this);
                        }

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

        public InvoiceAllowance IsAllowanceRevocable(OrganizationToken owner, DeleteAllowanceRootDeleteAllowance deleteItem, out Exception ex)
        {
            ex = null;
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == deleteItem.Number).FirstOrDefault();
            if (allowance == null)
            {
                ex = new Exception(String.Format("折讓單號碼不存在:{0}", deleteItem.Number));
                return null;
            }

            if (deleteItem.SellerId != allowance.InvoiceAllowanceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷折讓單之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", deleteItem.SellerId));
                return null;
            }

            if (deleteItem.BuyerId != allowance.InvoiceAllowanceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷折讓單之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", deleteItem.BuyerId));
                return null;
            }

            //if (allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回)
            if (allowance.InvoiceAllowanceCancellation != null &&
                (allowance.CDS_Document.ChildDocument.Any(r => r.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && r.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已註銷)
                && allowance.CDS_Document.ChildDocument.Any(r => r.CDS_Document.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && r.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回)))
            {
                //ex = new Exception(String.Format("折讓單不能註銷,折讓單號碼:{0},{1}", deleteItem.Number, ((Naming.B2BInvoiceStepDefinition)allowance.CDS_Document.CurrentStep).ToString()));
                ex = new Exception(String.Format("折讓單不能註銷,折讓單號碼:{0},已作廢", deleteItem.Number));
                return null;
            }

            return allowance;

        }

        public virtual Dictionary<int, Exception> SaveUploadInvoiceCancellationReturn(ReturnCancelInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.ReturnCancelInvoice != null && item.ReturnCancelInvoice.Length > 0)
            {
                for (int idx = 0; idx < item.ReturnCancelInvoice.Length; idx++)
                {
                    var invItem = item.ReturnCancelInvoice[idx];
                    try
                    {
                        Exception ex;
                        var docItem = IsInvoiceCancellationReturnable(owner, invItem, out ex);
                        if (docItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收)
                        {
                            docItem.MoveToFifthBranchStep(this);
                        }
                        else
                        {
                            docItem.MoveToThirdBranchStep(this);
                        }

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

        public CDS_Document IsInvoiceCancellationReturnable(OrganizationToken owner, ReturnCancelInvoiceRootReturnCancelInvoice invItem, out Exception ex)
        {
            ex = null;
            String invNo, trackCode;
            getInvoiceNo(invItem.Number, out invNo, out trackCode);

            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();
            if (invoice == null)
            {
                ex = new Exception(String.Format("發票號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != invoice.InvoiceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回作廢發票之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != invoice.InvoiceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回作廢發票之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            if (invoice.InvoiceCancellation == null)
            {
                ex = new Exception(String.Format("作廢發票不存在,無法退回,發票號碼:{0}", invItem.Number));
                return null;
            }

            CDS_Document docItem = this.GetTable<CDS_Document>().Where(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢發票 && d.DerivedDocument.SourceID == invoice.InvoiceID).FirstOrDefault();

            if (docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("作廢發票無法退回,號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()));
                return null;
            }

            return docItem;
        }

        public virtual Dictionary<int, Exception> SaveUploadAllowanceReturn(ReturnAllowanceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.ReturnAllowance != null && item.ReturnAllowance.Length > 0)
            {
                for (int idx = 0; idx < item.ReturnAllowance.Length; idx++)
                {
                    var invItem = item.ReturnAllowance[idx];
                    try
                    {
                        Exception ex;
                        InvoiceAllowance allowance = IsAllowanceReturnable(owner, invItem, out ex);

                        if (allowance == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (allowance.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收)
                        {
                            allowance.CDS_Document.MoveToFifthBranchStep(this);
                        }
                        else
                        {
                            allowance.CDS_Document.MoveToThirdBranchStep(this);
                        }

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

        public InvoiceAllowance IsAllowanceReturnable(OrganizationToken owner, ReturnAllowanceRootReturnAllowance returnItem, out Exception ex)
        {
            ex = null;
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == returnItem.Number).FirstOrDefault();
            if (allowance == null)
            {
                ex = new Exception(String.Format("折讓單號碼不存在:{0}", returnItem.Number));
                return null;
            }

            if (returnItem.SellerId != allowance.InvoiceAllowanceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回折讓單之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", returnItem.SellerId));
                return null;
            }

            if (returnItem.BuyerId != allowance.InvoiceAllowanceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回折讓單之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", returnItem.BuyerId));
                return null;
            }

            if (allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("折讓單不能退回,折讓單號碼:{0},{1}", returnItem.Number, ((Naming.B2BInvoiceStepDefinition)allowance.CDS_Document.CurrentStep).ToString()));
                return null;
            }

            //判斷是否有開立作廢發票,且作廢發票未被註銷
            if (this.GetTable<CDS_Document>().Any(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓
                    && d.DerivedDocument.SourceID == allowance.AllowanceID
                    && d.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已註銷))
            {
                ex = new Exception(String.Format("作廢折讓單已提出,折讓單不能退回,發票號碼:{0}", returnItem.Number));
                return null;
            }

            return allowance;
        }

        public virtual Dictionary<int, Exception> SaveUploadAllowanceCancellationReturn(ReturnCancelAllowanceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.ReturnCancelAllowance != null && item.ReturnCancelAllowance.Length > 0)
            {
                for (int idx = 0; idx < item.ReturnCancelAllowance.Length; idx++)
                {
                    var invItem = item.ReturnCancelAllowance[idx];
                    try
                    {
                        Exception ex;
                        var docItem = IsAllowanceCancellationReturnable(owner, invItem, out ex);
                        if (docItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收)
                        {
                            docItem.MoveToFifthBranchStep(this);
                        }
                        else
                        {
                            docItem.MoveToThirdBranchStep(this);
                        }

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

        public CDS_Document IsAllowanceCancellationReturnable(OrganizationToken owner, ReturnCancelAllowanceRootReturnCancelAllowance invItem, out Exception ex)
        {
            ex = null;
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == invItem.Number).FirstOrDefault();
            if (allowance == null)
            {
                ex = new Exception(String.Format("折讓單號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != allowance.InvoiceAllowanceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回作廢折讓單之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != allowance.InvoiceAllowanceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("退回作廢折讓單之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            if (allowance.InvoiceAllowanceCancellation == null)
            {
                ex = new Exception(String.Format("作廢折讓單不存在,無法退回,發票號碼:{0}", invItem.Number));
                return null;
            }

            CDS_Document docItem = this.GetTable<CDS_Document>().Where(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && d.DerivedDocument.SourceID == allowance.AllowanceID).FirstOrDefault();

            if (allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("作廢折讓單無法退回,號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()));
                return null;
            }

            return docItem;
        }

        public virtual Dictionary<int, Exception> SaveUploadAllowanceCancellationDelete(DeleteCancelAllowanceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.DeleteCancelAllowance != null && item.DeleteCancelAllowance.Length > 0)
            {
                for (int idx = 0; idx < item.DeleteCancelAllowance.Length; idx++)
                {
                    var invItem = item.DeleteCancelAllowance[idx];
                    try
                    {
                        Exception ex;
                        var docItem = IsAllowanceCancellationRevocable(owner, invItem, out ex);
                        if (docItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                        {
                            docItem.MoveToFirstBranchStep(this);
                        }
                        else if (docItem.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
                        {
                            docItem.MoveToSecondBranchStep(this);
                        }
                        else
                        {
                            docItem.MoveToForthBranchStep(this);
                        }
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

        public CDS_Document IsAllowanceCancellationRevocable(OrganizationToken owner, DeleteCancelAllowanceRootDeleteCancelAllowance invItem, out Exception ex)
        {

            ex = null;
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == invItem.Number).FirstOrDefault();
            if (allowance == null)
            {
                ex = new Exception(String.Format("折讓單號碼不存在:{0}", invItem.Number));
                return null;
            }

            if (invItem.SellerId != allowance.InvoiceAllowanceSeller.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷作廢折讓單之發票開立人非為原發票之發票開立人,統一編號錯誤:{0}", invItem.SellerId));
                return null;
            }

            if (invItem.BuyerId != allowance.InvoiceAllowanceBuyer.Organization.ReceiptNo)
            {
                ex = new Exception(String.Format("註銷作廢折讓單之發票買受人非為原發票之發票買受人,統一編號錯誤:{0}", invItem.BuyerId));
                return null;
            }

            if (allowance.InvoiceAllowanceCancellation == null)
            {
                ex = new Exception(String.Format("作廢折讓單不存在,無法註銷,發票號碼:{0}", invItem.Number));
                return null;
            }

            CDS_Document docItem = this.GetTable<CDS_Document>().Where(d => d.DocType == (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓 && d.DerivedDocument.SourceID == allowance.AllowanceID).FirstOrDefault();

            if (docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待接收 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.申請退回 && docItem.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送)
            {
                ex = new Exception(String.Format("作廢折讓單無法註銷,號碼:{0},{1}", invItem.Number, ((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()));
                return null;
            }

            return docItem;
        }


        public virtual Dictionary<int, Exception> SaveUploadInvoice(BuyerInvoiceRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();

            if (item != null && item.Invoice != null && item.Invoice.Length > 0)
            {
                for (int idx = 0; idx < item.Invoice.Length; idx++)
                {
                    var invItem = item.Invoice[idx];
                    try
                    {

                        var seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();
                        if (seller == null)
                        {
                            result.Add(idx, new Exception(String.Format("發票開立人為非註冊會員,統一編號:{0}", invItem.SellerId)));
                            continue;
                        }

                        var buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.BuyerId).FirstOrDefault();
                        if (buyer == null)
                        {
                            result.Add(idx, new Exception(String.Format("發票買受人為非註冊會員,統一編號:{0}", invItem.BuyerId)));
                            continue;
                        }

                        var relation = this.GetTable<BusinessRelationship>().Where(b => b.MasterID == buyer.CompanyID && b.RelativeID == seller.CompanyID && b.BusinessID == (int)Naming.InvoiceCenterBusinessType.進項).FirstOrDefault();
                        if (relation == null)
                        {
                            result.Add(idx, new Exception(String.Format("發票開立人非為買受人之進項相對營業人,單據編號:{0}", invItem.DataNumber)));
                            continue;
                        }
                        else if (relation.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
                        {
                            result.Add(idx, new Exception(String.Format("已停用發票開立人為買受人之進項相對營業人關係,單據編號:{0}", invItem.DataNumber)));
                            continue;
                        }



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
                                CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待開立
                            },
                            InvoiceBuyer = new InvoiceBuyer
                            {
                                Name = buyer.CompanyName,
                                ReceiptNo = buyer.ReceiptNo,
                                Address = buyer.Addr,
                                ContactName = buyer.ContactName,
                                CustomerName = buyer.CompanyName,
                                EMail = buyer.ContactEmail,
                                Fax = buyer.Fax,
                                Phone = buyer.Phone,
                                PersonInCharge = buyer.UndertakerName,
                                BuyerID = buyer.CompanyID
                            },
                            InvoiceSeller = new InvoiceSeller
                            {
                                Name = invItem.SellerName,
                                ReceiptNo = seller.ReceiptNo,
                                Address = seller.Addr,
                                ContactName = seller.ContactName,
                                CustomerName = seller.CompanyName,
                                EMail = seller.ContactEmail,
                                Fax = seller.Fax,
                                Phone = seller.Phone,
                                PersonInCharge = seller.UndertakerName,
                                SellerID = seller.CompanyID
                            },
                            InvoiceType = byte.Parse(invItem.InvoiceType),
                            SellerID = seller.CompanyID,
                            InvoiceAmountType = new InvoiceAmountType
                            {
                                //DiscountAmount = invItem.DiscountAmount,
                                SalesAmount = invItem.SalesAmount,
                                TaxAmount = invItem.TaxAmount,
                                TaxType = invItem.TaxType,
                                TotalAmount = invItem.TotalAmount,
                                TotalAmountInChinese = Utility.ValueValidity.MoneyShow(invItem.TotalAmount)
                            },
                            B2BBuyerInvoiceTag = new B2BBuyerInvoiceTag
                            {
                                DataNumber = invItem.DataNumber
                            }
                        };

                        short seqNo = 0;

                        var productItems = invItem.InvoiceItem.Select(i => new InvoiceProductItem
                        {
                            InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                            CostAmount = i.Amount,
                            CostAmount2 = i.Amount2,
                            ItemNo = String.Format("{0}", i.SequenceNumber),
                            Piece = (int?)i.Quantity,
                            Piece2 = i.Quantity2,
                            PieceUnit = i.Unit,
                            PieceUnit2 = i.Unit2,
                            UnitCost = i.UnitPrice,
                            UnitCost2 = i.UnitPrice2,
                            Remark = i.Remark,
                            TaxType = invItem.TaxType,
                            No = (seqNo++)
                        }).ToList();

                        newItem.InvoiceDetails.AddRange(productItems.Select(p => new InvoiceDetail
                        {
                            InvoiceProduct = p.InvoiceProduct
                        }));

                        using (var trackNoMgr = new TrackNoManager(this, seller.CompanyID))
                        {
                            if (trackNoMgr.CheckInvoiceNo(newItem))
                            {
                                applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.電子發票, Naming.InvoiceCenterBusinessType.進項);

                                this.EntityList.InsertOnSubmit(newItem);
                                this.SubmitChanges();
                            }
                            else
                            {
                                result.Add(idx, new Exception(String.Format("發票字軌號碼已用完或未設定!!,統一編號:{0}", invItem.SellerId)));
                                continue;
                            }
                        }
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


        public InvoiceAllowance ConvertToInvoiceAllowance(OrganizationToken owner, AllowanceRootAllowance item, out Exception ex)
        {
            ex = null; DateTime dt;

            if (item.AllowanceNumber.Trim().Length == 0)
            {
                ex = new Exception(String.Format("折讓證明單號碼不得為空白"));
                return null;
            }

            if (this.GetTable<InvoiceAllowance>().Any(i => i.AllowanceNumber == item.AllowanceNumber.Trim()))
            {
                ex = new Exception(String.Format("折讓證明單號碼已存在:{0}", item.AllowanceNumber));
                return null;
            }

            //折讓證明單日期
            if (item.AllowanceDate.Trim().Length == 0)
            {
                ex = new Exception(String.Format("折讓證明單日期不得為空白"));
                return null;
            }
            else
            {
                if (!DateTime.TryParse(item.AllowanceDate, out dt))
                {
                    ex = new Exception(String.Format("折讓證明單日期格式有錯:{0}", item.AllowanceDate));
                    return null;
                }
            }
            //發票開立人
            if (string.IsNullOrEmpty(item.SellerId))
            {
                ex = new Exception(String.Format("折讓證明單發票開立人不得為空白,號碼:{0}", item.AllowanceNumber));
                return null;
            }
            var seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == item.SellerId).FirstOrDefault();
            if (seller == null)
            {
                ex = new Exception(String.Format("折讓證明單發票開立人為非註冊會員,統一編號:{0}", item.SellerId));
                return null;
            }
            //發票開立人名稱
            if (string.IsNullOrEmpty(item.SellerName))
            {
                ex = new Exception(String.Format("折讓證明單發票開立人名稱不得為空白,號碼:{0}", item.AllowanceNumber));
                return null;
            }

            //發票買受人
            if (string.IsNullOrEmpty(item.BuyerId))
            {
                ex = new Exception(String.Format("折讓證明單發票買受人不得為空白,號碼:{0}", item.AllowanceNumber));
                return null;
            }
            var buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == item.BuyerId).FirstOrDefault();
            if (buyer == null)
            {
                ex = new Exception(String.Format("折讓證明單發票買受人為非註冊會員,統一編號:{0}", item.BuyerId));
                return null;
            }
            //發票買受人名稱
            if (string.IsNullOrEmpty(item.BuyerName))
            {
                ex = new Exception(String.Format("折讓證明單發票買受人名稱不得為空白,號碼:{0}", item.AllowanceNumber));
                return null;
            }

            if(owner.CompanyID!=seller.CompanyID && owner.CompanyID!=buyer.CompanyID)
            {
                ex = new Exception(String.Format("折讓證明單資料傳輸人非發票開立人或買受人,號碼:{0}", item.AllowanceNumber));
                return null;
            }
            //折讓種類
            if ((item.AllowanceType != 1) && (item.AllowanceType != 2))
            {
                ex = new Exception(String.Format("折讓證明單種類錯誤,號碼:{0}", item.AllowanceNumber));
                return null;
            }

            //折讓單營業稅額合計
            if (item.TaxAmount > 999999999999M || item.TaxAmount < 0 || Math.Floor(item.TaxAmount) != item.TaxAmount)
            {
                ex = new Exception(String.Format("折讓單營業稅額合計資料錯誤:{0}", item.AllowanceNumber));
                return null;
            }
            //折讓單金額合計
            if (item.TotalAmount > 999999999999M || item.TotalAmount < 0 || Math.Floor(item.TotalAmount) != item.TotalAmount)
            {
                ex = new Exception(String.Format("折讓單金額合計資料錯誤:{0}", item.AllowanceNumber));
                return null;
            }

            //折讓單明細發票日期
            InvoiceAllowance newItem = new InvoiceAllowance
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    CurrentStep = item.AllowanceType == 1 ? (int)Naming.B2BInvoiceStepDefinition.待傳送 : (int)Naming.B2BInvoiceStepDefinition.待開立
                },
                AllowanceDate = DateTime.ParseExact(item.AllowanceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                AllowanceNumber = item.AllowanceNumber,
                AllowanceType = item.AllowanceType,
                BuyerId = item.BuyerId,
                SellerId = item.SellerId,
                TaxAmount = item.TaxAmount,
                TotalAmount = item.TotalAmount,
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    SellerID = seller.CompanyID,
                    Address = seller.Addr,
                    ContactName = seller.ContactName,
                    CustomerID = seller.ReceiptNo,
                    CustomerName = seller.CompanyName,
                    EMail = seller.ContactEmail,
                    Fax = seller.Fax,
                    Name = seller.CompanyName,
                    PersonInCharge = seller.UndertakerName,
                    Phone = seller.Phone,
                    PostCode = "",
                    ReceiptNo = seller.ReceiptNo,
                    RoleRemark = ""
                },
                InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    BuyerID = buyer.CompanyID,
                    CustomerName = item.BuyerName,
                    Address = buyer.Addr,
                    ContactName = buyer.ContactName,
                    CustomerID = buyer.ReceiptNo,
                    EMail = buyer.ContactEmail,
                    Fax = buyer.Fax,
                    Name = buyer.CompanyName,
                    PersonInCharge = buyer.UndertakerName,
                    Phone = buyer.Phone,
                    PostCode = "",
                    ReceiptNo = buyer.ReceiptNo,
                    RoleRemark = ""
                }
            };

            List<InvoiceAllowanceItem> productItems = new List<InvoiceAllowanceItem>();
            foreach (var i in item.AllowanceItem)
            {
                String invNo, trackCode;
                getInvoiceNo(i.OriginalInvoiceNumber, out invNo, out trackCode);

                var originalInvoice = this.EntityList.Where(v => v.TrackCode == trackCode && v.No == invNo).FirstOrDefault();
                //if (originalInvoice == null)
                //{
                //    ex = new Exception(String.Format("折讓證明單明細之原始發票號碼不存在,折讓證明單號碼:{0}", item.AllowanceNumber));
                //    return null;
                //}

                if (originalInvoice != null)
                {

                    if (originalInvoice.InvoiceCancellation != null)
                    {
                        ex = new Exception(String.Format("折讓證明單明細之原始發票已作廢,無法再接受折讓,折讓證明單號碼:{0}", item.AllowanceNumber));
                        return null;
                    }

                    if (originalInvoice.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceQueryStepDefinition.待接收 || originalInvoice.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceQueryStepDefinition.待開立)
                    {
                        ex = new Exception(String.Format("折讓證明單明細之原始發票未被接收或開立,無法接受折讓,折讓證明單號碼:{0}", item.AllowanceNumber));
                        return null;
                    }
                }

                //折讓單明細發票號碼
                if (string.IsNullOrEmpty(i.OriginalInvoiceNumber))
                {
                    ex = new Exception(String.Format("折讓單明細發票號碼不得為空白:{0}", item.AllowanceNumber));
                    return null;
                }
                if (i.OriginalInvoiceNumber.Trim().Length != 10)
                {
                    ex = new Exception(String.Format("折讓單明細發票號碼長度有錯:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票原品名
                if (string.IsNullOrEmpty(i.OriginalDescription))
                {
                    ex = new Exception(String.Format("折讓單明細原品名不得為空白:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票數量
                if (i.Quantity > 999999999999.9999M || i.Quantity < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票數量超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票數量2
                if (i.Quantity2 > 999999999999.9999M || i.Quantity2 < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票數量2超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票單價
                if (i.UnitPrice > 999999999999.9999M || i.UnitPrice < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票單價超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票單價2
                if (i.UnitPrice2 > 999999999999.9999M || i.UnitPrice2 < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票單價2超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票金額
                if (i.Amount > 999999999999.9999M || i.Amount < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票金額超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票金額2
                if (i.Amount2 > 999999999999.9999M || i.Amount2 < -999999999999.9999M)
                {
                    ex = new Exception(String.Format("折讓單明細發票金額2超過核可範圍:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票稅額
                if (i.Tax > 999999999999M || i.Tax < 0 || Math.Floor(i.Tax) != i.Tax)
                {
                    ex = new Exception(String.Format("折讓單明細發票稅額資料錯誤:{0}", item.AllowanceNumber));
                    return null;
                }
                //折讓單明細發票課稅別
                if (i.TaxType != 1 && i.TaxType != 2 && i.TaxType != 3)
                {
                    ex = new Exception(String.Format("折讓單明細發票課稅別資料錯誤:{0}", item.AllowanceNumber));
                    return null;
                }

                if (string.IsNullOrEmpty(i.OriginalInvoiceDate))
                {
                    ex = new Exception(String.Format("折讓單明細發票日期不得為空白:{0}", item.AllowanceNumber));
                    return null;
                }
                else if (i.OriginalInvoiceDate.Trim().Length != 10)
                {
                    ex = new Exception(String.Format("折讓單明細發票日期長度有錯:{0}", item.AllowanceNumber));
                    return null;
                }

                if (!DateTime.TryParse(i.OriginalInvoiceDate, out dt))
                {
                    ex = new Exception(String.Format("折讓單明細發票日期格式有錯:{0}", item.AllowanceNumber));
                    return null;
                }
                else
                {
                    if (newItem.AllowanceDate < dt)
                    {
                        ex = new Exception(String.Format("折讓單證明日期早於原發票日期:{0}", item.AllowanceNumber));
                        return null;
                    }

                }

                var allowanceItem = new InvoiceAllowanceItem
                {
                    Amount = i.Amount,
                    Amount2 = i.Amount2,
                    InvoiceNo = i.OriginalInvoiceNumber,
                    InvoiceDate = DateTime.ParseExact(i.OriginalInvoiceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                    OriginalSequenceNo = !String.IsNullOrEmpty(i.OriginalSequenceNumber) ? short.Parse(i.OriginalSequenceNumber) : (short?)null,
                    Piece = i.Quantity,
                    Piece2 = i.Quantity2,
                    PieceUnit = i.Unit,
                    PieceUnit2 = i.Unit2,
                    OriginalDescription = i.OriginalDescription,
                    Tax = i.Tax,
                    TaxType = i.TaxType,
                    No = !String.IsNullOrEmpty(i.AllowanceSequenceNumber) ? short.Parse(i.AllowanceSequenceNumber) : (short?)null,
                    UnitCost = i.UnitPrice,
                    UnitCost2 = i.UnitPrice2
                };

                if (originalInvoice != null)
                {
                    InvoiceProductItem prodItem = null;
                    short originalNo;
                    if (!String.IsNullOrEmpty(i.OriginalSequenceNumber) && short.TryParse(i.OriginalSequenceNumber, out originalNo))
                    {
                        prodItem = this.GetTable<InvoiceProductItem>().Where(p => p.InvoiceProduct.InvoiceDetails.Count(t => t.InvoiceID == originalInvoice.InvoiceID) > 0
                            && p.No == originalNo).FirstOrDefault();
                    }
                    if (prodItem == null)
                    {
                        prodItem = this.GetTable<InvoiceProductItem>().Where(p => p.InvoiceProduct.InvoiceDetails.Count(t => t.InvoiceID == originalInvoice.InvoiceID) > 0)
                            .FirstOrDefault();
                    }
                    if (prodItem != null)
                        allowanceItem.ProductItemID = prodItem.ItemID;
                }

                productItems.Add(allowanceItem);
            }

            if (item.AllowanceItem.GroupBy(w => w.AllowanceSequenceNumber).Any(g => g.Count() > 1))
            {
                ex = new Exception(String.Format("折讓單證明單明細序號重覆:{0}", item.AllowanceNumber));
                return null;
            }

            newItem.InvoiceAllowanceDetails.AddRange(productItems.Select(p => new InvoiceAllowanceDetail
            {
                InvoiceAllowanceItem = p
            }));

            if (item.ExtraRemark != null && item.ExtraRemark is XmlNode[])
            {
                XElement extra = new XElement("ExtraRemark");
                foreach (XmlNode n in (XmlNode[])item.ExtraRemark)
                {
                    extra.Add(XElement.Load(new XmlNodeReader(n)));
                }
                newItem.InvoiceAllowanceItemExtension = new InvoiceAllowanceItemExtension
                {
                    ExtraRemark = extra
                };
            }

            return newItem;
        }

        public InvoiceAllowance ConvertToInvoiceAllowance(OrganizationToken owner, Schema.TurnKey.B1101.Allowance allowance)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = allowance.Main.Buyer.Address,
                    CompanyName = allowance.Main.Buyer.Name,
                    UndertakerName = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    ContactEmail = allowance.Main.Buyer.EmailAddress,
                    ReceiptNo = allowance.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Seller.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                seller = new Organization
                {
                    Addr = allowance.Main.Seller.Address,
                    CompanyName = allowance.Main.Seller.Name,
                    UndertakerName = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    ContactEmail = allowance.Main.Seller.EmailAddress,
                    ReceiptNo = allowance.Main.Seller.Identifier
                };
            }

            InvoiceAllowance newItem = new InvoiceAllowance
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                AllowanceDate = DateTime.ParseExact(allowance.Main.AllowanceDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                AllowanceNumber = allowance.Main.AllowanceNumber,
                AllowanceType = (byte)((int)allowance.Main.AllowanceType),
                BuyerId = allowance.Main.Buyer.Identifier,
                SellerId = allowance.Main.Seller.Identifier,
                TaxAmount = allowance.Amount.TaxAmount,
                TotalAmount = allowance.Amount.TotalAmount,
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    Name = allowance.Main.Seller.Name,
                    ReceiptNo = allowance.Main.Seller.Identifier,
                    ContactName = allowance.Main.Seller.PersonInCharge,
                    Address = allowance.Main.Seller.Address,
                    CustomerID = allowance.Main.Seller.CustomerNumber,
                    CustomerName = allowance.Main.Seller.Name,
                    EMail = allowance.Main.Seller.EmailAddress,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    PersonInCharge = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    RoleRemark = allowance.Main.Seller.RoleRemark,
                    Organization = seller
                },
                InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    Name = allowance.Main.Buyer.Name,
                    ReceiptNo = allowance.Main.Buyer.Identifier,
                    ContactName = allowance.Main.Buyer.PersonInCharge,
                    Address = allowance.Main.Buyer.Address,
                    CustomerID = allowance.Main.Buyer.CustomerNumber,
                    CustomerName = allowance.Main.Buyer.Name,
                    EMail = allowance.Main.Buyer.EmailAddress,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    PersonInCharge = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    RoleRemark = allowance.Main.Buyer.RoleRemark,
                    Organization = buyer
                }
            };

            //bool invalid = false;
            //foreach (var i in allowance.Details)
            //{
            //    String invNo, trackCode;
            //    getInvoiceNo(i.OriginalInvoiceNumber, out invNo, out trackCode);

            //    var originalInvoice = this.EntityList.Where(v => v.TrackCode == trackCode && v.No == invNo).FirstOrDefault();
            //    if (originalInvoice == null)
            //    {
            //        invalid = true;
            //        break;
            //    }
            //}

            //if (invalid)
            //{
            //    throw new Exception(String.Format("折讓證明單明細之原始發票號碼不存在,折讓證明單號碼:{0}", allowance.Main.AllowanceNumber));
            //}


            List<InvoiceAllowanceItem> productItems = new List<InvoiceAllowanceItem>();
            foreach (var i in allowance.Details)
            {
                var allowanceItem = new InvoiceAllowanceItem
                {
                    Amount = i.Amount,
                    Amount2 = i.Amount2,
                    InvoiceNo = i.OriginalInvoiceNumber,
                    InvoiceDate = DateTime.ParseExact(i.OriginalInvoiceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                    OriginalSequenceNo = !String.IsNullOrEmpty(i.OriginalSequenceNumber) ? short.Parse(i.OriginalSequenceNumber) : (short?)null,
                    Piece = i.Quantity,
                    Piece2 = i.Quantity2,
                    PieceUnit = i.Unit,
                    PieceUnit2 = i.Unit2,
                    OriginalDescription = i.OriginalDescription,
                    TaxType = (byte)i.TaxType,
                    No = !String.IsNullOrEmpty(i.AllowanceSequenceNumber) ? short.Parse(i.AllowanceSequenceNumber) : (short?)null,
                    UnitCost = i.UnitPrice,
                    UnitCost2 = i.UnitPrice2
                };

                productItems.Add(allowanceItem);
            }

            newItem.InvoiceAllowanceDetails.AddRange(productItems.Select(p => new InvoiceAllowanceDetail
            {
                InvoiceAllowanceItem = p
            }));

            return newItem;
        }

        public InvoiceAllowance ConvertToInvoiceAllowance(OrganizationToken owner, Schema.TurnKey.B1401.Allowance allowance)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = allowance.Main.Buyer.Address,
                    CompanyName = allowance.Main.Buyer.Name,
                    UndertakerName = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    ContactEmail = allowance.Main.Buyer.EmailAddress,
                    ReceiptNo = allowance.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Seller.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                seller = new Organization
                {
                    Addr = allowance.Main.Seller.Address,
                    CompanyName = allowance.Main.Seller.Name,
                    UndertakerName = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    ContactEmail = allowance.Main.Seller.EmailAddress,
                    ReceiptNo = allowance.Main.Seller.Identifier
                };
            }

            InvoiceAllowance newItem = new InvoiceAllowance
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                AllowanceDate = DateTime.ParseExact(allowance.Main.AllowanceDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                AllowanceNumber = allowance.Main.AllowanceNumber,
                AllowanceType = (byte)((int)allowance.Main.AllowanceType),
                BuyerId = allowance.Main.Buyer.Identifier,
                SellerId = allowance.Main.Seller.Identifier,
                TaxAmount = allowance.Amount.TaxAmount,
                TotalAmount = allowance.Amount.TotalAmount,
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    Name = allowance.Main.Seller.Name,
                    ReceiptNo = allowance.Main.Seller.Identifier,
                    ContactName = allowance.Main.Seller.PersonInCharge,
                    Address = allowance.Main.Seller.Address,
                    CustomerID = allowance.Main.Seller.CustomerNumber,
                    CustomerName = allowance.Main.Seller.Name,
                    EMail = allowance.Main.Seller.EmailAddress,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    PersonInCharge = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    RoleRemark = allowance.Main.Seller.RoleRemark,
                    Organization = seller
                },
                InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    Name = allowance.Main.Buyer.Name,
                    ReceiptNo = allowance.Main.Buyer.Identifier,
                    ContactName = allowance.Main.Buyer.PersonInCharge,
                    Address = allowance.Main.Buyer.Address,
                    CustomerID = allowance.Main.Buyer.CustomerNumber,
                    CustomerName = allowance.Main.Buyer.Name,
                    EMail = allowance.Main.Buyer.EmailAddress,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    PersonInCharge = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    RoleRemark = allowance.Main.Buyer.RoleRemark,
                    Organization = buyer
                }
            };

            //bool invalid = false;
            //foreach (var i in allowance.Details)
            //{
            //    String invNo, trackCode;
            //    getInvoiceNo(i.OriginalInvoiceNumber, out invNo, out trackCode);

            //    var originalInvoice = this.EntityList.Where(v => v.TrackCode == trackCode && v.No == invNo).FirstOrDefault();
            //    if (originalInvoice == null)
            //    {
            //        invalid = true;
            //        break;
            //    }
            //}

            //if (invalid)
            //{
            //    throw new Exception(String.Format("折讓證明單明細之原始發票號碼不存在,折讓證明單號碼:{0}", allowance.Main.AllowanceNumber));
            //}


            List<InvoiceAllowanceItem> productItems = new List<InvoiceAllowanceItem>();
            foreach (var i in allowance.Details)
            {
                var allowanceItem = new InvoiceAllowanceItem
                {
                    Amount = i.Amount,
                    Amount2 = i.Amount2,
                    InvoiceNo = i.OriginalInvoiceNumber,
                    InvoiceDate = DateTime.ParseExact(i.OriginalInvoiceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                    OriginalSequenceNo = !String.IsNullOrEmpty(i.OriginalSequenceNumber) ? short.Parse(i.OriginalSequenceNumber) : (short?)null,
                    Piece = i.Quantity,
                    Piece2 = i.Quantity2,
                    PieceUnit = i.Unit,
                    PieceUnit2 = i.Unit2,
                    OriginalDescription = i.OriginalDescription,
                    TaxType = (byte)i.TaxType,
                    No = !String.IsNullOrEmpty(i.AllowanceSequenceNumber) ? short.Parse(i.AllowanceSequenceNumber) : (short?)null,
                    UnitCost = i.UnitPrice,
                    UnitCost2 = i.UnitPrice2
                };

                productItems.Add(allowanceItem);
            }

            newItem.InvoiceAllowanceDetails.AddRange(productItems.Select(p => new InvoiceAllowanceDetail
            {
                InvoiceAllowanceItem = p
            }));

            return newItem;
        }

        public InvoiceAllowance ConvertToInvoiceAllowance(OrganizationToken owner, Schema.TurnKey.B0401.Allowance allowance)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = allowance.Main.Buyer.Address,
                    CompanyName = allowance.Main.Buyer.Name,
                    UndertakerName = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    ContactEmail = allowance.Main.Buyer.EmailAddress,
                    ReceiptNo = allowance.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == allowance.Main.Seller.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                seller = new Organization
                {
                    Addr = allowance.Main.Seller.Address,
                    CompanyName = allowance.Main.Seller.Name,
                    UndertakerName = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    ContactEmail = allowance.Main.Seller.EmailAddress,
                    ReceiptNo = allowance.Main.Seller.Identifier
                };
            }

            InvoiceAllowance newItem = new InvoiceAllowance
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                AllowanceDate = DateTime.ParseExact(allowance.Main.AllowanceDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                AllowanceNumber = allowance.Main.AllowanceNumber,
                AllowanceType = (byte)((int)allowance.Main.AllowanceType),
                BuyerId = allowance.Main.Buyer.Identifier,
                SellerId = allowance.Main.Seller.Identifier,
                TaxAmount = allowance.Amount.TaxAmount,
                TotalAmount = allowance.Amount.TotalAmount,
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    Name = allowance.Main.Seller.Name,
                    ReceiptNo = allowance.Main.Seller.Identifier,
                    ContactName = allowance.Main.Seller.PersonInCharge,
                    Address = allowance.Main.Seller.Address,
                    CustomerID = allowance.Main.Seller.CustomerNumber,
                    CustomerName = allowance.Main.Seller.Name,
                    EMail = allowance.Main.Seller.EmailAddress,
                    Fax = allowance.Main.Seller.FacsimileNumber,
                    PersonInCharge = allowance.Main.Seller.PersonInCharge,
                    Phone = allowance.Main.Seller.TelephoneNumber,
                    RoleRemark = allowance.Main.Seller.RoleRemark,
                    Organization = seller
                },
                InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    Name = allowance.Main.Buyer.Name,
                    ReceiptNo = allowance.Main.Buyer.Identifier,
                    ContactName = allowance.Main.Buyer.PersonInCharge,
                    Address = allowance.Main.Buyer.Address,
                    CustomerID = allowance.Main.Buyer.CustomerNumber,
                    CustomerName = allowance.Main.Buyer.Name,
                    EMail = allowance.Main.Buyer.EmailAddress,
                    Fax = allowance.Main.Buyer.FacsimileNumber,
                    PersonInCharge = allowance.Main.Buyer.PersonInCharge,
                    Phone = allowance.Main.Buyer.TelephoneNumber,
                    RoleRemark = allowance.Main.Buyer.RoleRemark,
                    Organization = buyer
                }
            };

            //bool invalid = false;
            //foreach (var i in allowance.Details)
            //{
            //    String invNo, trackCode;
            //    getInvoiceNo(i.OriginalInvoiceNumber, out invNo, out trackCode);

            //    var originalInvoice = this.EntityList.Where(v => v.TrackCode == trackCode && v.No == invNo).FirstOrDefault();
            //    if (originalInvoice == null)
            //    {
            //        invalid = true;
            //        break;
            //    }
            //}

            //if (invalid)
            //{
            //    throw new Exception(String.Format("折讓證明單明細之原始發票號碼不存在,折讓證明單號碼:{0}", allowance.Main.AllowanceNumber));
            //}


            List<InvoiceAllowanceItem> productItems = new List<InvoiceAllowanceItem>();
            foreach (var i in allowance.Details)
            {
                var allowanceItem = new InvoiceAllowanceItem
                {
                    Amount = i.Amount,
                    InvoiceNo = i.OriginalInvoiceNumber,
                    InvoiceDate = DateTime.ParseExact(i.OriginalInvoiceDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                    OriginalSequenceNo = !String.IsNullOrEmpty(i.OriginalSequenceNumber) ? short.Parse(i.OriginalSequenceNumber) : (short?)null,
                    Piece = i.Quantity,
                    PieceUnit = i.Unit,
                    OriginalDescription = i.OriginalDescription,
                    TaxType = (byte)i.TaxType,
                    No = !String.IsNullOrEmpty(i.AllowanceSequenceNumber) ? short.Parse(i.AllowanceSequenceNumber) : (short?)null,
                    UnitCost = i.UnitPrice
                };

                productItems.Add(allowanceItem);
            }

            newItem.InvoiceAllowanceDetails.AddRange(productItems.Select(p => new InvoiceAllowanceDetail
            {
                InvoiceAllowanceItem = p
            }));

            return newItem;
        }


        public Dictionary<int, Exception> SaveUploadAllowance(AllowanceRoot root, OrganizationToken owner)
        {

            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (root != null && root.Allowance != null && root.Allowance.Length > 0)
            {
                var table = this.GetTable<InvoiceAllowance>();
                var tblOrg = this.GetTable<Organization>();

                for (int idx = 0; idx < root.Allowance.Length; idx++)
                {
                    var item = root.Allowance[idx];
                    try
                    {
                        Exception ex;
                        InvoiceAllowance newItem = ConvertToInvoiceAllowance(owner, item, out ex);

                        if (newItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        if (newItem.InvoiceAllowanceBuyer.BuyerID == owner.CompanyID)
                        {
                            applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.發票折讓, Naming.InvoiceCenterBusinessType.進項);
                        }
                        else if (newItem.InvoiceAllowanceSeller.SellerID == owner.CompanyID)
                        {
                            applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.發票折讓, Naming.InvoiceCenterBusinessType.銷項);
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


        public Dictionary<int, Exception> SaveUploadAllowanceCancellation(CancelAllowanceRoot root, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (root != null && root.CancelAllowance != null && root.CancelAllowance.Length > 0)
            {
                var tblCancel = this.GetTable<InvoiceAllowanceCancellation>();

                for (int idx = 0; idx < root.CancelAllowance.Length; idx++)
                {
                    var item = root.CancelAllowance[idx];
                    try
                    {
                        Exception ex;
                        InvoiceAllowance allowance;
                        InvoiceAllowanceCancellation cancelItem = ConvertToAllowanceCancellation(owner, item, out ex, out allowance);

                        if (cancelItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        var doc = new DerivedDocument
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocType = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                                DocDate = DateTime.Now,
                                CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                                DocumentOwner = new DocumentOwner
                                {
                                    OwnerID = owner.CompanyID
                                }
                            },
                            SourceID = cancelItem.AllowanceID
                        };

                        if (allowance.InvoiceAllowanceBuyer.BuyerID == owner.CompanyID)
                        {
                            applyProcessFlow(doc.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓, Naming.InvoiceCenterBusinessType.進項);
                        }
                        else if (allowance.InvoiceAllowanceSeller.SellerID == owner.CompanyID)
                        {
                            applyProcessFlow(doc.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓, Naming.InvoiceCenterBusinessType.銷項);
                        }

                        this.GetTable<InvoiceAllowanceCancellation>().InsertOnSubmit(cancelItem);
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

        public InvoiceAllowanceCancellation ConvertToAllowanceCancellation(OrganizationToken owner, CancelAllowanceRootCancelAllowance item, out Exception ex, out InvoiceAllowance allowance)
        {
            ex = null;
            DateTime allowanceDate, cancelDate;

            allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();

            if (allowance == null)
            {
                ex = new Exception(String.Format("折讓證明單號碼不存在:{0}", item.CancelAllowanceNumber));
                return null;
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                ex = new Exception(String.Format("作廢折讓單已存在,折讓單號碼:{0}", item.CancelAllowanceNumber));
                return null;
            }

            if (allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.待批次傳送 && allowance.CDS_Document.CurrentStep != (int)Naming.B2BInvoiceStepDefinition.已接收)
            {
                if (allowance.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.申請退回)
                {
                    ex = new Exception(String.Format("折讓單申請退回,無法作廢折讓單,折讓單號碼:{0}", item.CancelAllowanceNumber));
                }
                else if (allowance.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.已註銷)
                {
                    ex = new Exception(String.Format("折讓單已註銷,無法作廢折讓單,折讓單號碼:{0}", item.CancelAllowanceNumber));
                }
                else
                {
                    ex = new Exception(String.Format("折讓證明單未被接收或開立,無法作廢折讓證明單,折讓單號碼:{0}", item.CancelAllowanceNumber));
                }
                return null;                
            }


            if (string.IsNullOrEmpty(item.AllowanceDate))
            {
                ex = new Exception(String.Format("折讓證明單日期不得為空白"));
                return null;
            }
            if (!DateTime.TryParse(item.AllowanceDate, out allowanceDate))
            {
                ex = new Exception(String.Format("折讓證明單日期格式有誤"));
                return null;
            }
            if (string.IsNullOrEmpty(item.CancelDate))
            {
                ex = new Exception(String.Format("作廢折讓單日期不得為空白"));
                return null;
            }

            if (string.IsNullOrEmpty(item.CancelTime))
            {
                ex = new Exception(String.Format("作廢折讓單時間不得為空白"));
                return null;
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.AssumeLocal, out cancelDate))
            {
                ex = new Exception(String.Format("作廢折讓單日期格式有誤"));
                return null;
            }
            if (cancelDate < allowanceDate)
            {
                ex = new Exception(String.Format("作廢折讓單日期不可小於折讓日期"));
                return null;
            }

            //if (allowance.AllowanceDate.Value != DateTime.Parse(item.AllowanceDate))
            //{
            //    ex = new Exception(String.Format("折讓證明單日期不合,折讓單日期:{0}", item.AllowanceDate));
            //    return null;
            //}

            if (string.IsNullOrEmpty(item.CancelReason))
            {
                ex = new Exception(String.Format("作廢折讓單原因不得為空白"));
                return null;
            }

            InvoiceAllowanceCancellation cancelItem = new InvoiceAllowanceCancellation
            {
                AllowanceID = allowance.AllowanceID,
                Remark = item.CancelReason + " " + item.Remark,
                CancelDate = cancelDate
            };

            return cancelItem;
        }

        public InvoiceItem ConvertToInvoiceItem(OrganizationToken owner, Model.Schema.TurnKey.A1101.Invoice invoice)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = invoice.Main.Buyer.Address,
                    CompanyName = invoice.Main.Buyer.Name,
                    UndertakerName = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    ContactEmail = invoice.Main.Buyer.EmailAddress,
                    ReceiptNo = invoice.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Seller.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                seller = new Organization
                {
                    Addr = invoice.Main.Seller.Address,
                    CompanyName = invoice.Main.Seller.Name,
                    UndertakerName = invoice.Main.Seller.PersonInCharge,
                    Phone = invoice.Main.Seller.TelephoneNumber,
                    Fax = invoice.Main.Seller.FacsimileNumber,
                    ContactEmail = invoice.Main.Seller.EmailAddress,
                    ReceiptNo = invoice.Main.Seller.Identifier
                };
            }

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
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invoice.Main.Buyer.Name,
                    ReceiptNo = invoice.Main.Buyer.Identifier,
                    ContactName = invoice.Main.Buyer.PersonInCharge,
                    Address = invoice.Main.Buyer.Address,
                    CustomerID = invoice.Main.Buyer.CustomerNumber,
                    CustomerName = invoice.Main.Buyer.Name,
                    EMail = invoice.Main.Buyer.EmailAddress,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    PersonInCharge = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    RoleRemark = invoice.Main.Buyer.RoleRemark,
                    Organization = buyer
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
                    Organization = seller
                },
                InvoiceDate = DateTime.ParseExact(String.Format("{0}", invoice.Main.InvoiceDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).Add(invoice.Main.InvoiceTimeSpecified ? invoice.Main.InvoiceTime.TimeOfDay : TimeSpan.Zero),
                InvoiceType = (byte)((int)invoice.Main.Invoice),
                No = invNo,
                TrackCode = trackCode,
                SellerID = seller.CompanyID,
                BuyerRemark = invoice.Main.BuyerRemarkSpecified ? String.Format("{0}", (int)invoice.Main.BuyerRemark) : null,
                Category = invoice.Main.Category,
                CheckNo = invoice.Main.CheckNumber,
                DonateMark = ((int)invoice.Main.DonateMark).ToString(),
                CustomsClearanceMark = invoice.Main.CustomsClearanceMarkSpecified ? ((int)invoice.Main.CustomsClearanceMark).ToString() : null,
                GroupMark = invoice.Main.GroupMark,
                PermitDate = String.IsNullOrEmpty(invoice.Main.PermitDate) ? (DateTime?)null : DateTime.ParseExact(invoice.Main.PermitDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                PermitNumber = invoice.Main.PermitNumber,
                PermitWord = invoice.Main.PermitWord,
                RelateNumber = invoice.Main.RelateNumber,
                Remark = invoice.Main.MainRemark,
                TaxCenter = invoice.Main.TaxCenter,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = invoice.Amount.DiscountAmountSpecified ? invoice.Amount.DiscountAmount : (decimal?)null,
                    SalesAmount = invoice.Amount.SalesAmount,
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
                CostAmount2 = i.Amount2,
                ItemNo = i.SequenceNumber,
                Piece = (int?)i.Quantity,
                Piece2 = (int?)i.Quantity2,
                PieceUnit = i.Unit,
                PieceUnit2 = i.Unit2,
                UnitCost = i.UnitPrice,
                UnitCost2 = i.UnitPrice2,
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

        public InvoiceItem ConvertToInvoiceItem(OrganizationToken owner, Model.Schema.TurnKey.A1401.Invoice invoice)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = invoice.Main.Buyer.Address,
                    CompanyName = invoice.Main.Buyer.Name,
                    UndertakerName = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    ContactEmail = invoice.Main.Buyer.EmailAddress,
                    ReceiptNo = invoice.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Seller.Identifier).FirstOrDefault();
            if (seller == null)
            {
                seller = new Organization
                {
                    Addr = invoice.Main.Seller.Address,
                    CompanyName = invoice.Main.Seller.Name,
                    UndertakerName = invoice.Main.Seller.PersonInCharge,
                    Phone = invoice.Main.Seller.TelephoneNumber,
                    Fax = invoice.Main.Seller.FacsimileNumber,
                    ContactEmail = invoice.Main.Seller.EmailAddress,
                    ReceiptNo = invoice.Main.Seller.Identifier,
                    OrganizationStatus = new OrganizationStatus
                    {
                        IronSteelIndustry = true
                    }
                };
            }
            else if (seller.OrganizationStatus == null)
            {
                seller.OrganizationStatus = new OrganizationStatus
                    {
                        IronSteelIndustry = true
                    };
            }

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
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invoice.Main.Buyer.Name,
                    ReceiptNo = invoice.Main.Buyer.Identifier,
                    ContactName = invoice.Main.Buyer.PersonInCharge,
                    Address = invoice.Main.Buyer.Address,
                    CustomerID = invoice.Main.Buyer.CustomerNumber,
                    CustomerName = invoice.Main.Buyer.Name,
                    EMail = invoice.Main.Buyer.EmailAddress,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    PersonInCharge = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    RoleRemark = invoice.Main.Buyer.RoleRemark,
                    Organization = buyer
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
                    Organization = seller
                },
                InvoiceDate = DateTime.ParseExact(String.Format("{0}", invoice.Main.InvoiceDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).Add(invoice.Main.InvoiceTimeSpecified ? invoice.Main.InvoiceTime.TimeOfDay : TimeSpan.Zero),
                InvoiceType = (byte)((int)invoice.Main.InvoiceType),
                No = invNo,
                TrackCode = trackCode,
                SellerID = seller.CompanyID,
                BuyerRemark = invoice.Main.BuyerRemarkSpecified ? String.Format("{0}", (int)invoice.Main.BuyerRemark) : null,
                Category = invoice.Main.Category,
                CheckNo = invoice.Main.CheckNumber,
                DonateMark = ((int)invoice.Main.DonateMark).ToString(),
                CustomsClearanceMark = invoice.Main.CustomsClearanceMarkSpecified ? ((int)invoice.Main.CustomsClearanceMark).ToString() : null,
                GroupMark = invoice.Main.GroupMark,
                PermitDate = String.IsNullOrEmpty(invoice.Main.PermitDate) ? (DateTime?)null : DateTime.ParseExact(invoice.Main.PermitDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                PermitNumber = invoice.Main.PermitNumber,
                PermitWord = invoice.Main.PermitWord,
                RelateNumber = invoice.Main.RelateNumber,
                Remark = invoice.Main.MainRemark,
                TaxCenter = invoice.Main.TaxCenter,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = invoice.Amount.DiscountAmountSpecified ? invoice.Amount.DiscountAmount : (decimal?)null,
                    SalesAmount = invoice.Amount.SalesAmount,
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
                CostAmount2 = i.Amount2,
                ItemNo = i.SequenceNumber,
                Piece = (int?)i.Quantity,
                Piece2 = (int?)i.Quantity2,
                PieceUnit = i.Unit,
                PieceUnit2 = i.Unit2,
                UnitCost = i.UnitPrice,
                UnitCost2 = i.UnitPrice2,
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

        public InvoiceItem ConvertToInvoiceItem(OrganizationToken owner, Model.Schema.TurnKey.A0401.Invoice invoice)
        {
            Organization buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Buyer.Identifier).FirstOrDefault();
            if (buyer == null)
            {
                buyer = new Organization
                {
                    Addr = invoice.Main.Buyer.Address,
                    CompanyName = invoice.Main.Buyer.Name,
                    UndertakerName = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    ContactEmail = invoice.Main.Buyer.EmailAddress,
                    ReceiptNo = invoice.Main.Buyer.Identifier
                };
            }

            Organization seller = this.GetTable<Organization>().Where(o => o.ReceiptNo == invoice.Main.Seller.Identifier).FirstOrDefault();
            if (seller == null)
            {
                seller = new Organization
                {
                    Addr = invoice.Main.Seller.Address,
                    CompanyName = invoice.Main.Seller.Name,
                    UndertakerName = invoice.Main.Seller.PersonInCharge,
                    Phone = invoice.Main.Seller.TelephoneNumber,
                    Fax = invoice.Main.Seller.FacsimileNumber,
                    ContactEmail = invoice.Main.Seller.EmailAddress,
                    ReceiptNo = invoice.Main.Seller.Identifier,
                    OrganizationStatus = new OrganizationStatus
                    {
                        IronSteelIndustry = false
                    }
                };
            }
            else if (seller.OrganizationStatus == null)
            {
                seller.OrganizationStatus = new OrganizationStatus
                {
                    IronSteelIndustry = false
                };
            }

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
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    Name = invoice.Main.Buyer.Name,
                    ReceiptNo = invoice.Main.Buyer.Identifier,
                    ContactName = invoice.Main.Buyer.PersonInCharge,
                    Address = invoice.Main.Buyer.Address,
                    CustomerID = invoice.Main.Buyer.CustomerNumber,
                    CustomerName = invoice.Main.Buyer.Name,
                    EMail = invoice.Main.Buyer.EmailAddress,
                    Fax = invoice.Main.Buyer.FacsimileNumber,
                    PersonInCharge = invoice.Main.Buyer.PersonInCharge,
                    Phone = invoice.Main.Buyer.TelephoneNumber,
                    RoleRemark = invoice.Main.Buyer.RoleRemark,
                    Organization = buyer
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
                    Organization = seller
                },
                InvoiceDate = DateTime.ParseExact(String.Format("{0}", invoice.Main.InvoiceDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).Add(invoice.Main.InvoiceTimeSpecified ? invoice.Main.InvoiceTime.TimeOfDay : TimeSpan.Zero),
                InvoiceType = (byte)((int)invoice.Main.InvoiceType),
                No = invNo,
                TrackCode = trackCode,
                Organization = seller,
                BuyerRemark = invoice.Main.BuyerRemarkSpecified ? String.Format("{0}", (int)invoice.Main.BuyerRemark) : null,
                Category = invoice.Main.Category,
                CheckNo = invoice.Main.CheckNumber,
                DonateMark = ((int)invoice.Main.DonateMark).ToString(),
                CustomsClearanceMark = invoice.Main.CustomsClearanceMarkSpecified ? ((int)invoice.Main.CustomsClearanceMark).ToString() : null,
                GroupMark = invoice.Main.GroupMark,
                PermitDate = String.IsNullOrEmpty(invoice.Main.PermitDate) ? (DateTime?)null : DateTime.ParseExact(invoice.Main.PermitDate, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture),
                PermitNumber = invoice.Main.PermitNumber,
                PermitWord = invoice.Main.PermitWord,
                RelateNumber = invoice.Main.RelateNumber,
                Remark = invoice.Main.MainRemark,
                TaxCenter = invoice.Main.TaxCenter,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = invoice.Amount.DiscountAmountSpecified ? invoice.Amount.DiscountAmount : (decimal?)null,
                    SalesAmount = invoice.Amount.SalesAmount,
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


        public void SaveA1101(Model.Schema.TurnKey.A1101.Invoice invoice, OrganizationToken owner)
        {
            InvoiceItem newItem = ConvertToInvoiceItem(owner, invoice);
            this.EntityList.InsertOnSubmit(newItem);
            this.SubmitChanges();
        }

        public void SaveA1401(Model.Schema.TurnKey.A1401.Invoice invoice, OrganizationToken owner)
        {
            InvoiceItem newItem = ConvertToInvoiceItem(owner, invoice);
            applyProcessFlow(newItem.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.電子發票);
            this.EntityList.InsertOnSubmit(newItem);
            this.SubmitChanges();
        }

        public void SaveA0401(Model.Schema.TurnKey.A0401.Invoice invoice, OrganizationToken owner)
        {
            InvoiceItem newItem = ConvertToInvoiceItem(owner, invoice);
            applyProcessFlow(newItem.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.電子發票);
            this.EntityList.InsertOnSubmit(newItem);
            this.SubmitChanges();
        }


        public void SaveB1101(Schema.TurnKey.B1101.Allowance allowance, OrganizationToken owner)
        {
            InvoiceAllowance newItem = ConvertToInvoiceAllowance(owner, allowance);
            this.GetTable<InvoiceAllowance>().InsertOnSubmit(newItem);
            this.SubmitChanges();
        }

        public void SaveB0401(Schema.TurnKey.B0401.Allowance allowance, OrganizationToken owner)
        {
            InvoiceAllowance newItem = ConvertToInvoiceAllowance(owner, allowance);
            applyProcessFlow(newItem.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.發票折讓);
            this.GetTable<InvoiceAllowance>().InsertOnSubmit(newItem);
            this.SubmitChanges();
        }


        public void SaveB1401(Schema.TurnKey.B1401.Allowance allowance, OrganizationToken owner)
        {
            InvoiceAllowance newItem = ConvertToInvoiceAllowance(owner, allowance);
            applyProcessFlow(newItem.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.發票折讓);
            this.GetTable<InvoiceAllowance>().InsertOnSubmit(newItem);
            this.SubmitChanges();
        }


        public InvoiceCancellation ConvertToInvoiceCancellation(OrganizationToken owner, Model.Schema.TurnKey.A0201.CancelInvoice cancellation)
        {
            String invNo, trackCode;
            if (cancellation.CancelInvoiceNumber.Length >= 10)
            {
                trackCode = cancellation.CancelInvoiceNumber.Substring(0, 2);
                invNo = cancellation.CancelInvoiceNumber.Substring(2);
            }
            else
            {
                trackCode = null;
                invNo = cancellation.CancelInvoiceNumber;
            }
            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

            if (invoice == null)
            {
                throw new Exception(String.Format("發票號碼不存在:{0}", cancellation.CancelInvoiceNumber));
            }

            if (invoice.InvoiceCancellation != null)
            {
                throw new Exception(String.Format("作廢發票已存在,發票號碼:{0}", cancellation.CancelInvoiceNumber));
            }

            InvoiceCancellation cancelItem = new InvoiceCancellation
            {
                InvoiceID = invoice.InvoiceID,
                CancellationNo = cancellation.CancelInvoiceNumber,
                Remark = String.Format("{0}{1}", cancellation.CancelReason, cancellation.Remark),
                ReturnTaxDocumentNo = cancellation.ReturnTaxDocumentNumber,
                CancelDate = DateTime.ParseExact(String.Format("{0}", cancellation.CancelDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).Add(cancellation.CancelTime.TimeOfDay)
            };

            return cancelItem;
        }

        public InvoiceCancellation ConvertToInvoiceCancellation(OrganizationToken owner, Model.Schema.TurnKey.A0501.CancelInvoice cancellation)
        {
            String invNo, trackCode;
            if (cancellation.CancelInvoiceNumber.Length >= 10)
            {
                trackCode = cancellation.CancelInvoiceNumber.Substring(0, 2);
                invNo = cancellation.CancelInvoiceNumber.Substring(2);
            }
            else
            {
                trackCode = null;
                invNo = cancellation.CancelInvoiceNumber;
            }
            var invoice = this.EntityList.Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

            if (invoice == null)
            {
                throw new Exception(String.Format("發票號碼不存在:{0}", cancellation.CancelInvoiceNumber));
            }

            if (invoice.InvoiceCancellation != null)
            {
                throw new Exception(String.Format("作廢發票已存在,發票號碼:{0}", cancellation.CancelInvoiceNumber));
            }

            InvoiceCancellation cancelItem = new InvoiceCancellation
            {
                InvoiceID = invoice.InvoiceID,
                CancellationNo = cancellation.CancelInvoiceNumber,
                Remark = String.Format("{0}{1}", cancellation.CancelReason, cancellation.Remark),
                ReturnTaxDocumentNo = cancellation.ReturnTaxDocumentNumber,
                CancelDate = DateTime.ParseExact(String.Format("{0}", cancellation.CancelDate), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).Add(cancellation.CancelTime.TimeOfDay)
            };

            return cancelItem;
        }

        public void SaveA0201(Schema.TurnKey.A0201.CancelInvoice item, OrganizationToken owner)
        {
            InvoiceCancellation cancelItem = ConvertToInvoiceCancellation(owner, item);
            this.GetTable<InvoiceCancellation>().InsertOnSubmit(cancelItem);

            var doc = new DerivedDocument
            {
                CDS_Document = new CDS_Document
                {
                    DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    DocDate = DateTime.Now,
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    }
                },
                SourceID = cancelItem.InvoiceID
            };

            this.GetTable<DerivedDocument>().InsertOnSubmit(doc);
            this.SubmitChanges();
        }

        public void SaveA0501(Schema.TurnKey.A0501.CancelInvoice item, OrganizationToken owner)
        {
            InvoiceCancellation cancelItem = ConvertToInvoiceCancellation(owner, item);
            this.GetTable<InvoiceCancellation>().InsertOnSubmit(cancelItem);

            var doc = new DerivedDocument
            {
                CDS_Document = new CDS_Document
                {
                    DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    DocDate = DateTime.Now,
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    }
                },
                SourceID = cancelItem.InvoiceID
            };

            applyProcessFlow(doc.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.作廢發票);

            this.GetTable<DerivedDocument>().InsertOnSubmit(doc);
            this.SubmitChanges();
        }

        public InvoiceAllowanceCancellation ConvertToAllowanceCancellation(OrganizationToken owner, Model.Schema.TurnKey.B0201.CancelAllowance item)
        {
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();

            if (allowance == null)
            {
                throw new Exception(String.Format("折讓證明單號碼不存在:{0}", item.CancelAllowanceNumber));
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                throw new Exception(String.Format("作廢折讓單已存在,折讓單號碼:{0}", item.CancelAllowanceNumber));
            }

            InvoiceAllowanceCancellation cancelItem = new InvoiceAllowanceCancellation
            {
                AllowanceID = allowance.AllowanceID,
                Remark = item.Remark,
                CancelDate = DateTime.ParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
            };

            return cancelItem;
        }

        public InvoiceAllowanceCancellation ConvertToAllowanceCancellation(OrganizationToken owner, Model.Schema.TurnKey.B0501.CancelAllowance item)
        {
            var allowance = this.GetTable<InvoiceAllowance>().Where(i => i.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();

            if (allowance == null)
            {
                throw new Exception(String.Format("折讓證明單號碼不存在:{0}", item.CancelAllowanceNumber));
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                throw new Exception(String.Format("作廢折讓單已存在,折讓單號碼:{0}", item.CancelAllowanceNumber));
            }

            InvoiceAllowanceCancellation cancelItem = new InvoiceAllowanceCancellation
            {
                AllowanceID = allowance.AllowanceID,
                Remark = item.Remark,
                CancelDate = DateTime.ParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
            };

            return cancelItem;
        }

        public void SaveB0201(Schema.TurnKey.B0201.CancelAllowance item, OrganizationToken owner)
        {

            InvoiceAllowanceCancellation cancelItem = ConvertToAllowanceCancellation(owner, item);
            this.GetTable<InvoiceAllowanceCancellation>().InsertOnSubmit(cancelItem);

            var doc = new DerivedDocument
            {
                CDS_Document = new CDS_Document
                {
                    DocType = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                    DocDate = DateTime.Now,
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    }
                },
                SourceID = cancelItem.AllowanceID
            };

            this.GetTable<DerivedDocument>().InsertOnSubmit(doc);

            this.SubmitChanges();
        }

        public void SaveB0501(Schema.TurnKey.B0501.CancelAllowance item, OrganizationToken owner)
        {

            InvoiceAllowanceCancellation cancelItem = ConvertToAllowanceCancellation(owner, item);
            this.GetTable<InvoiceAllowanceCancellation>().InsertOnSubmit(cancelItem);

            var doc = new DerivedDocument
            {
                CDS_Document = new CDS_Document
                {
                    DocType = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                    DocDate = DateTime.Now,
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待傳送,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    }
                },
                SourceID = cancelItem.AllowanceID
            };

            applyProcessFlow(doc.CDS_Document, Naming.B2BInvoiceDocumentTypeDefinition.作廢折讓);
            this.GetTable<DerivedDocument>().InsertOnSubmit(doc);

            this.SubmitChanges();
        }

        public void CheckInvoiceNo()
        {
            var items = this.EntityList.Where(i => i.InvoiceNoAssignment == null).GroupBy(i => i.SellerID);

            if (items.Count() > 0)
            {
                foreach (var item in items)
                {
                    try
                    {
                        using (TrackNoManager trackNoMgr = new TrackNoManager(this, item.Key.Value))
                        {
                            if (item.Select(i => trackNoMgr.CheckInvoiceNo(i)).Count(r => r) > 0)
                            {
                                trackNoMgr.SubmitChanges();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            }
        }

        public static void AsyncCheckInvoiceNo()
        {
            ThreadPool.QueueUserWorkItem(p =>
            {
                using (B2BInvoiceManager mgr = new B2BInvoiceManager())
                {
                    try
                    {
                        mgr.CheckInvoiceNo();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }
            });
        }

        //public void SaveE0401(Schema.EIVO.BranchTrack item, OrganizationToken owner)
        //{
        //    Settings.Default.E0401Outbound.CheckStoredPath();
        //    int invoiceCounter = Directory.GetFiles(Settings.Default.E0401Outbound).Length;
        //    String fileName = Path.Combine(Settings.Default.E0401Outbound, String.Format("E0401-{0:yyyyMMddHHmmssf}-{1:00000}.xml", DateTime.Now, invoiceCounter++));
        //    item.CreateE1401().ConvertToXml().Save(fileName);
        //}
    }
}
