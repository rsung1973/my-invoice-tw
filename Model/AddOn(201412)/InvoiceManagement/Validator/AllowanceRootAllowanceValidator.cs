using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Schema.EIVO;
using System.Text.RegularExpressions;
using Model.Resource;
using System.Globalization;
using Model.Locale;

namespace Model.InvoiceManagement.Validator
{
    public partial class AllowanceRootAllowanceValidator
    {
        protected GenericManager<EIVOEntityDataContext> _mgr;
        protected OrganizationToken _owner;

        protected AllowanceRootAllowance _allowanceItem;

        protected InvoiceAllowance _newItem;
        protected Organization _seller;
        protected List<InvoiceAllowanceItem> _productItems;
        protected DateTime _allowanceDate;


        public AllowanceRootAllowanceValidator(GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner)
        {
            _mgr = mgr;
            _owner = owner;
        }

        public InvoiceAllowance Allowance
        {
            get 
            {
                return _newItem;
            }
        }

        public virtual Exception Validate(AllowanceRootAllowance dataItem)
        {
            _allowanceItem = dataItem;

            Exception ex;

            _seller = null;
            _newItem = null;

            bool IsProxy = _owner.Organization.OrganizationCategory.Any(c => c.CategoryID == (int)Naming.B2CCategoryID.開立發票店家代理);
            
            if((ex = CheckBusiness()) != null && !IsProxy)
            {
                return ex;
            }

            if((ex = CheckBusiness_Proxy()) != null && IsProxy)
            {
                return ex;
            }

            if((ex = CheckMandatoryFields()) != null)
            {
                return ex;
            }

            if((ex = CheckAllowanceItem()) != null)
            {
                return ex;
            }

            return null;
        }

        protected virtual Exception CheckBusiness()
        {
            _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _allowanceItem.SellerId).FirstOrDefault();

            if (_seller == null)
            {
                return new Exception(String.Format(MessageResources.AlertInvalidSeller, _allowanceItem.SellerId));
            }

            if (_seller.CompanyID != _owner.CompanyID)
            {
                return new Exception(String.Format(MessageResources.AlertSellerSignature, _allowanceItem.SellerId));
            }

            if (_allowanceItem.SellerName == null || _allowanceItem.SellerName.Length > 60)
            {
                return new Exception(String.Format(MessageResources.AlertSellerNameLength, _allowanceItem.SellerName));
            }

            if (_allowanceItem.BuyerId == "0000000000")
            {
                //if (_allowanceItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(_allowanceItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format(MessageResources.InvalidBuyerName, _allowanceItem.BuyerName));
                //}
            }
            else if (_allowanceItem.BuyerId == null || !Regex.IsMatch(_allowanceItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format(MessageResources.InvalidBuyerId, _allowanceItem.BuyerId));
            }
            else if (_allowanceItem.BuyerName.Length > 60)
            {
                return new Exception(String.Format(MessageResources.AlertBuyerNameLength, _allowanceItem.BuyerName));
            }

            return null;
        }

        protected virtual Exception CheckBusiness_Proxy()
        {
            _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _allowanceItem.SellerId).FirstOrDefault();

            if (_seller == null)
            {
                return new Exception(String.Format(MessageResources.AlertInvalidSeller, _allowanceItem.SellerId));
            }

            //代理店家
            var proxyOrg = _mgr.GetTable<Organization>().Where(i => i.CompanyID == _owner.CompanyID).FirstOrDefault();            
            //上傳店家資料是否註冊在代理店家下
            var proxy = _mgr.GetTable<InvoiceIssuerAgent>().Where(i => i.AgentID == proxyOrg.CompanyID && i.IssuerID == _seller.CompanyID).FirstOrDefault();
            if (proxy == null && proxyOrg.ReceiptNo != _allowanceItem.SellerId)
            {
                return new Exception(String.Format(MessageResources.InvalidSellerNotAttributionTheAgent, _seller.ReceiptNo, proxyOrg.ReceiptNo));
            }

            if (_allowanceItem.SellerName == null || _allowanceItem.SellerName.Length > 60)
            {
                return new Exception(String.Format(MessageResources.AlertSellerNameLength, _allowanceItem.SellerName));
            }

            if (_allowanceItem.BuyerId == "0000000000")
            {
                //if (_allowanceItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(_allowanceItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format(MessageResources.InvalidBuyerName, _allowanceItem.BuyerName));
                //}
            }
            else if (_allowanceItem.BuyerId == null || !Regex.IsMatch(_allowanceItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format(MessageResources.InvalidBuyerId, _allowanceItem.BuyerId));
            }
            else if (_allowanceItem.BuyerName.Length > 60)
            {
                return new Exception(String.Format(MessageResources.AlertBuyerNameLength, _allowanceItem.BuyerName));
            }

            return null;
        }

        protected virtual Exception CheckMandatoryFields()
        {
            _allowanceDate = default(DateTime);

            if (String.IsNullOrEmpty(_allowanceItem.AllowanceNumber))
            {
                return new Exception(MessageResources.InvalidAllowanceNo);
            }

            //折讓證明單號碼
            if (_allowanceItem.AllowanceNumber.Length > 16)
            {
                return new Exception(String.Format(MessageResources.AlertAllowanceNoLength, _allowanceItem.AllowanceNumber));
            }

            var table = _mgr.GetTable<InvoiceAllowance>();
            if (table.Any(i => i.AllowanceNumber == _allowanceItem.AllowanceNumber))
            {
                return new Exception(String.Format(MessageResources.AlertAllowanceDuplicated, _allowanceItem.AllowanceNumber));
            }

            //折讓證明單日期
            if (String.IsNullOrEmpty(_allowanceItem.AllowanceDate))
            {
                return new Exception(MessageResources.InvalidAllowanceDate);
            }

            if (!DateTime.TryParseExact(_allowanceItem.AllowanceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out _allowanceDate))
            {
                return new Exception(String.Format(MessageResources.AlertAllowanceDateFormat, _allowanceItem.AllowanceDate));
            }

            //折讓種類
            if (_allowanceItem.AllowanceType != "1" && _allowanceItem.AllowanceType != "2")
            {
                return new Exception(String.Format(MessageResources.AlertAllowanceType, _allowanceItem.AllowanceType));
            }

            return null;
        }

        protected virtual Exception CheckAllowanceItem()
        {
            _productItems = new List<InvoiceAllowanceItem>();
            var invTable = _mgr.GetTable<InvoiceItem>();

            foreach (var i in _allowanceItem.AllowanceItem)
            {
                InvoiceItem originalInvoice = null;

                if (i.OriginalInvoiceNumber != null && i.OriginalInvoiceNumber.Length == 10)
                {
                    String invNo, trackCode;
                    trackCode = i.OriginalInvoiceNumber.Substring(0, 2);
                    invNo = i.OriginalInvoiceNumber.Substring(2);
                    originalInvoice = invTable.Where(n => n.TrackCode == trackCode && n.No == invNo).FirstOrDefault();
                }

                if (originalInvoice == null)
                {
                    return new Exception(String.Format(MessageResources.InvalidAllowance_NoInvoiceData, i.OriginalInvoiceNumber));
                }

                if (originalInvoice.InvoiceCancellation != null)
                {
                    return new Exception(MessageResources.InvalidAllowance_InvoiceHasBeenCanceled);
                }


                var allowanceDate = String.Format("{0:yyyy/MM/dd}", i.OriginalInvoiceDate);
                var InvDate = String.Format("{0:yyyy/MM/dd}", originalInvoice.InvoiceDate);

                if (allowanceDate.ToString() != InvDate)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_InvoiceDate, InvDate, allowanceDate));
                }


                if (originalInvoice.InvoiceBuyer.ReceiptNo != _allowanceItem.BuyerId && _allowanceItem.BuyerId != "0000000000")
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_InvoiceBuyerError, i.OriginalInvoiceNumber));
                }

                if (originalInvoice.InvoiceSeller.ReceiptNo != _allowanceItem.SellerId)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_InvoiceSellerIsDifferent, i.OriginalInvoiceNumber));
                }

                //原明細排列序號
                if (i.OriginalSequenceNumber > 1000 || i.OriginalSequenceNumber < 0)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_OriginalSequenceNumber, i.OriginalSequenceNumber));
                }

                //折讓證明單明細排列序號
                if (i.AllowanceSequenceNumber > 1000 || i.AllowanceSequenceNumber < 0)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_AllowanceSequenceNumber, i.AllowanceSequenceNumber));
                }

                //原品名
                if (i.OriginalDescription == null || i.OriginalDescription.Length > 256)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_OriginalDescription, i.OriginalDescription));
                }

                //單位
                if (i.Unit != null && i.Unit.Length > 6)
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_Unit, i.Unit));
                }

                //課稅別
                if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)i.TaxType))
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_TaxType, i.TaxType));
                }

                DateTime invoiceDate;
                if (String.IsNullOrEmpty(i.OriginalInvoiceDate) || !DateTime.TryParseExact(String.Format("{0}", i.OriginalInvoiceDate), "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
                {
                    return new Exception(String.Format(MessageResources.AlertAllowance_InvoiceDateFormat, i.OriginalInvoiceDate));
                }

                var allowanceItem = new InvoiceAllowanceItem
                {
                    Amount = i.Amount,
                    InvoiceNo = i.OriginalInvoiceNumber,
                    InvoiceDate = invoiceDate,
                    ItemNo = i.Item,
                    OriginalSequenceNo = (short)i.OriginalSequenceNumber,
                    Piece = i.Quantity,
                    PieceUnit = i.Unit,
                    OriginalDescription = i.OriginalDescription,
                    TaxType = i.TaxType,
                    No = (short)i.AllowanceSequenceNumber,
                    UnitCost = i.UnitPrice,
                    Tax = i.Tax,
                };

                if (originalInvoice != null)
                {
                    var invProductItem = _mgr.GetTable<InvoiceItem>().Where(v => v.InvoiceID == originalInvoice.InvoiceID)
                        .Join(_mgr.GetTable<InvoiceDetail>(), v => v.InvoiceID, d => d.InvoiceID, (v, d) => d)
                        .Join(_mgr.GetTable<InvoiceProduct>(), d => d.ProductID, p => p.ProductID, (d, p) => p)
                        .Join(_mgr.GetTable<InvoiceProductItem>(), p => p.ProductID, t => t.ProductID, (p, t) => t)
                        .Where(t => t.No == i.OriginalSequenceNumber).FirstOrDefault();
                    //var invProductItem = originalInvoice.InvoiceDetails.Join(_mgr.GetTable<InvoiceProductItem>(), d => d.ProductID, p => p.ProductID, (d, p) => p)
                    //    .Where(p => p.No == i.OriginalSequenceNumber).FirstOrDefault();
                    if (invProductItem != null)
                    {
                        allowanceItem.ItemID = invProductItem.ItemID;
                    }
                }
                _productItems.Add(allowanceItem);
            }

            _newItem = new InvoiceAllowance() 
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance
                },
                AllowanceDate = _allowanceDate,
                AllowanceNumber = _allowanceItem.AllowanceNumber,
                AllowanceType = byte.Parse(_allowanceItem.AllowanceType),
                BuyerId = _allowanceItem.BuyerId,
                SellerId = _allowanceItem.SellerId,
                TaxAmount = _allowanceItem.TaxAmount,
                TotalAmount = _allowanceItem.TotalAmount,
                //InvoiceID =  invTable.Where(i=>i.TrackCode + i.No == item.AllowanceItem.Select(a=>a.OriginalInvoiceNumber).FirstOrDefault()).Select(i=>i.InvoiceID).FirstOrDefault(),
                InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    Name = _allowanceItem.BuyerName,
                    ReceiptNo = _allowanceItem.BuyerId,
                    CustomerID = String.IsNullOrEmpty(_allowanceItem.GoogleId) ? "" : _allowanceItem.GoogleId,
                    ContactName = _allowanceItem.BuyerName,
                    CustomerName = _allowanceItem.BuyerName
                },
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    Name = _seller.CompanyName,
                    ReceiptNo = _seller.ReceiptNo,
                    Address = _seller.Addr,
                    ContactName = _seller.ContactName,
                    CustomerID = String.IsNullOrEmpty(_allowanceItem.GoogleId) ? "" : _allowanceItem.GoogleId,
                    CustomerName = _seller.CompanyName,
                    EMail = _seller.ContactEmail,
                    Fax = _seller.Fax,
                    Phone = _seller.Phone,
                    PersonInCharge = _seller.UndertakerName,
                    SellerID = _seller.CompanyID,
                }
            };

            _newItem.InvoiceAllowanceDetails.AddRange(_productItems.Select(p => new InvoiceAllowanceDetail 
            {
                InvoiceAllowanceItem = p,
            }));

            if (_owner != null)
            {
                _newItem.CDS_Document.DocumentOwner = new DocumentOwner
                {
                    OwnerID = _owner.CompanyID,
                };
            }

            return null;
        }
    }
}
