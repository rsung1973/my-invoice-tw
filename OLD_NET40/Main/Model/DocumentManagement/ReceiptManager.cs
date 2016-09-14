using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Schema.EIVO.B2B;
using Utility;
using Model.Helper;

namespace Model.DocumentManagement
{
    public class ReceiptManager : EIVOEntityManager<ReceiptItem>
    {
        private X509Certificate2 _signerCert;
        private bool _useSigner;

        public ReceiptManager() : base() { }
        public ReceiptManager(GenericManager<EIVOEntityDataContext> mgr) : base(mgr) { }

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

        public virtual Dictionary<int, Exception> SaveUploadReceipt(ReceiptRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.Receipt != null && item.Receipt.Length > 0)
            {
                for (int idx = 0; idx < item.Receipt.Length; idx++)
                {
                    var receipt = item.Receipt[idx];
                    try
                    {
                        Exception ex;
                        ReceiptItem newItem = ConvertToReceiptItem(owner, receipt, out ex);

                        if (newItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        applyProcessFlow(newItem.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.收據, Naming.InvoiceCenterBusinessType.銷項);

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

        public ReceiptItem ConvertToReceiptItem(OrganizationToken owner, ReceiptRootReceipt receipt, out Exception ex)
        {
            ex = null;

            var buyer = this.GetTable<Organization>().Where(o => o.ReceiptNo == receipt.BuyerId).FirstOrDefault();
            if (buyer == null)
            {
                ex = new Exception(String.Format("買受人為非註冊會員,統一編號:{0}", receipt.BuyerId));
                return null;
            }

            String no;
            if (receipt.ReceiptNumber==null || (no=receipt.ReceiptNumber.Trim()).Length==0)
            {
                ex = new Exception("單據號碼為空白");
                return null;
            }

            if (this.EntityList.Any(i => i.No == no))
            {
                ex = new Exception(String.Format("收據號碼已存在:{0}", receipt.ReceiptNumber));
                return null;
            }

            var relation = this.GetTable<BusinessRelationship>().Where(b => b.MasterID == owner.CompanyID && b.RelativeID == buyer.CompanyID && b.BusinessID == (int)Naming.InvoiceCenterBusinessType.銷項).FirstOrDefault();
            if (relation == null)
            {
                ex = new Exception(String.Format("買受人非為開立人之銷項相對營業人:{0}", receipt.ReceiptNumber));
                return null;
            }
            else if (relation.CurrentLevel == (int)Naming.MemberStatusDefinition.Mark_To_Delete)
            {
                ex = new Exception(String.Format("已停用買受人為銷項相對營業人之關係:{0}", receipt.ReceiptNumber));
                return null;
            }


            ReceiptItem newItem = new ReceiptItem
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.B2BInvoiceDocumentTypeDefinition.收據,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = owner.CompanyID
                    },
                    CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待接收
                },
                BuyerID = buyer.CompanyID,
                ReceiptDate = DateTime.ParseExact(receipt.ReceiptDate, "yyyy/MM/dd", System.Globalization.CultureInfo.CurrentCulture),
                No = no,
                SellerID = owner.CompanyID,
                TotalAmount = receipt.TotalAmount
            };

            short seqNo = 0;

            var productItems = receipt.ReceiptItem.Select(i => new ReceiptDetail
            {
                Description = i.Description ,
                Amount =  i.Amount,
                SequenceNO = (seqNo++),
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                Remark = i.Remark                
            });

            newItem.ReceiptDetail.AddRange(productItems);

            if (_useSigner)
            {
                if (_signerCert != null)
                {
                    //if (!newItem.SignAndCheckToReceiveReceipt(_signerCert, null))
                    //{
                    //    ex = new Exception(String.Format("收據開立人已設定自動開立＼接收，簽章失敗:{0}", receipt.ReceiptNumber));
                    //    return null;
                    //}
                }
                else
                {
                    ex = new Exception(String.Format("收據開立人已設定自動開立＼接收，但尚未設定簽章憑證:{0}", receipt.ReceiptNumber));
                    return null;
                }
            }

            return newItem;
        }



        public Dictionary<int, Exception> SaveUploadReceiptCancellation(CancelReceiptRoot item, OrganizationToken owner)
        {
            Dictionary<int, Exception> result = new Dictionary<int, Exception>();
            if (item != null && item.CancelReceipt != null && item.CancelReceipt.Length > 0)
            {
                for (int idx = 0; idx < item.CancelReceipt.Length; idx++)
                {
                    var cancelInvoice = item.CancelReceipt[idx];
                    try
                    {
                        Exception ex;
                        ReceiptItem receipt;
                        ReceiptCancellation cancelItem = ConvertToReceiptCancellation(owner, cancelInvoice, out ex, out receipt);

                        if (cancelItem == null)
                        {
                            result.Add(idx, ex);
                            continue;
                        }

                        var doc = new DerivedDocument
                        {
                            CDS_Document = new CDS_Document
                            {
                                DocType = (int)Naming.B2BInvoiceDocumentTypeDefinition.作廢收據,
                                DocDate = DateTime.Now,
                                CurrentStep = (int)Naming.B2BInvoiceStepDefinition.待接收,
                                DocumentOwner = new DocumentOwner
                                {
                                    OwnerID = owner.CompanyID
                                }
                            },
                            SourceID = cancelItem.ReceiptID
                        };

                        applyProcessFlow(doc.CDS_Document, owner.CompanyID, Naming.B2BInvoiceDocumentTypeDefinition.作廢收據, Naming.InvoiceCenterBusinessType.銷項);

                        this.GetTable<ReceiptCancellation>().InsertOnSubmit(cancelItem);
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

        public ReceiptCancellation ConvertToReceiptCancellation(OrganizationToken owner, CancelReceiptRootCancelReceipt cancellation, out Exception ex, out ReceiptItem receipt)
        {
            ex = null;
            receipt = this.EntityList.Where(i => i.No == cancellation.CancelReceiptNumber).FirstOrDefault();

            if (receipt == null)
            {
                ex = new Exception(String.Format("收據號碼不存在:{0}", cancellation.CancelReceiptNumber));
                return null;
            }

            if (receipt.ReceiptCancellation != null)
            {
                ex = new Exception(String.Format("作廢收據已存在,收據號碼:{0}", cancellation.CancelReceiptNumber));
                return null;
            }

            if (receipt.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待接收 || receipt.CDS_Document.CurrentStep == (int)Naming.B2BInvoiceStepDefinition.待開立)
            {
                ex = new Exception(String.Format("原收據未被接收,無法作廢收據,號碼:{0}", cancellation.CancelReceiptNumber));
                return null;
            }


            ReceiptCancellation cancelItem = new ReceiptCancellation
            {
                ReceiptID = receipt.ReceiptID,
                CancellationNo = cancellation.CancelReceiptNumber,
                Remark = cancellation.Remark,
                CancelDate = DateTime.ParseExact(String.Format("{0} {1}", cancellation.CancelDate, cancellation.CancelTime), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture)
            };

            if (_useSigner)
            {
                if (_signerCert != null)
                {
                    //if (!cancelItem.SignAndCheckToIssueReceiptCancellation(receipt, _signerCert, null))
                    //{
                    //    ex = new Exception(String.Format("作廢收據開立人已設定自動開立＼接收，簽章失敗:{0}", cancellation.CancelReceiptNumber));
                    //    return null;
                    //}
                }
                else
                {
                    ex = new Exception(String.Format("作廢收據開立人已設定自動開立＼接收，但尚未設定簽章憑證:{0}", cancellation.CancelReceiptNumber));
                    return null;
                }
            }

            return cancelItem;
        }

    }
}
