using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Schema.EIVO;
using System.Globalization;
using DataAccessLayer.basis;
using Model.DataEntity;
using System.Text.RegularExpressions;
using Model.Locale;

namespace Model.InvoiceManagement.enUS
{
    public static partial class AllowanceRootValidator
    {
        #region 英文訊息專區

        public static Exception CheckBusiness(this AllowanceRootAllowance item, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out Organization seller)
        {
            seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == item.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("Seller ID does not exist, Seller ID: {0}, Incorrect TAG:< SellerId />", item.SellerId));
            }

            if (seller.CompanyID != owner.CompanyID)
            {
                return new Exception(String.Format("Cert ID and Seller ID does not match, Seller ID: {0}, Incorrect TAG:< SellerId />", item.SellerId));
            }

            if (item.SellerName == null || item.SellerName.Length > 60)
            {
                return new Exception(String.Format("Format of Seller Name error. The maximum length is 60 digitals, Incorrect Seller Name: {0}, Incorrect：{0}，TAG：< SellerName />", item.SellerName));
            }

            if (String.IsNullOrEmpty(item.GoogleId))
            {
                return new Exception(String.Format("GoogleId can not be blank，TAG:< GoogleId />"));
            }
            else if (item.GoogleId.Length > 64)
            {
                return new Exception(String.Format("GoogleId on the length up to 64 yards，TAG:< GoogleId />"));
            }

            if (item.BuyerId == "0000000000")
            {
                //if (item.BuyerName == null || Encoding.GetEncoding(950).GetBytes(item.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format("B2C buyer name format errors, four yards in length ASCII characters or Chinese full-width characters 2 yards，Incorrect：{0}，TAG:< BuyerName />", item.BuyerName));
                //}
            }
            else if (item.BuyerId == null || !Regex.IsMatch(item.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("Format of Buyer ID error, Incorrect Buyer ID: {0}, Incorrect TAG:< BuyerId />", item.BuyerId));
            }
            else if (item.BuyerName.Length > 60)
            {
                return new Exception(String.Format("Format of Buyer Name error. The maximum length is 60 digitals, Incorrect Buyer Name: {0}, Incorrect TAG:< BuyerName />", item.BuyerName));
            }

            return null;
        }

        public static Exception CheckMandatoryFields(this AllowanceRootAllowance item, GenericManager<EIVOEntityDataContext> mgr, out DateTime allowanceDate)
        {
            allowanceDate = default(DateTime);

            if (String.IsNullOrEmpty(item.AllowanceNumber))
            {
                return new Exception("Allowance Number can not be blank，TAG：< AllowanceNumber />");
            }

            //折讓證明單號碼
            if (item.AllowanceNumber.Length > 16)
            {
                return new Exception(String.Format("Maximum length of Allowance Number is 16 digitals; Incorrect Allowance Number:{0}, Incorrect：{0}，TAG：< AllowanceNumber />", item.AllowanceNumber));
            }

            var table = mgr.GetTable<InvoiceAllowance>();
            if (table.Any(i => i.AllowanceNumber == item.AllowanceNumber))
            {
                return new Exception(String.Format("Allowance Data to prove the existence of a single number has been:{0}", item.AllowanceNumber));
            }

            //折讓證明單日期
            if (String.IsNullOrEmpty(item.AllowanceDate))
            {
                return new Exception("Allowance Date can not be blank，TAG：< AllowanceDate />");
            }

            if (!DateTime.TryParseExact(item.AllowanceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out allowanceDate))
            {
                return new Exception(String.Format("Format of Allowance Date error(YYYY/MM/DD), Incorrect TAG:< AllowanceDate />:{0}", item.AllowanceDate));
            }

            //折讓種類
            if (item.AllowanceType != "1" && item.AllowanceType != "2")
            {
                return new Exception(String.Format("Just buy prescribing discount format types \"1\", selling prescribing \"2\"，Incorrect：{0}", item.AllowanceType));
            }

            return null;
        }

        public static Exception CheckAllowanceItem(this AllowanceRootAllowance item, GenericManager<EIVOEntityDataContext> mgr, out List<InvoiceAllowanceItem> productItems)
        {
            productItems = new List<InvoiceAllowanceItem>();
            var invTable = mgr.GetTable<InvoiceItem>();

            foreach (var i in item.AllowanceItem)
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
                    return new Exception(String.Format("Invoice data does not exist，Incorrect：{0}，TAG：< OriginalInvoiceNumber />", i.OriginalInvoiceNumber));
                }

                if (originalInvoice.InvoiceCancellation != null)
                {
                    return new Exception("Invoice has been voided, not a discount.");
                }

                var allowanceDate = String.Format("{0:yyyy/MM/dd}", i.OriginalInvoiceDate);
                var InvDate = String.Format("{0:yyyy/MM/dd}", originalInvoice.InvoiceDate);

                if (allowanceDate.ToString() != InvDate)
                {
                    return new Exception(String.Format("Discount invoice date is not the same as the original data；Invoice Date：{0} 、 Upload Data：{1} ", InvDate, allowanceDate));
                }

                if (originalInvoice.InvoiceBuyer.ReceiptNo != item.BuyerId && item.BuyerId != "0000000000")
                {
                    return new Exception(String.Format("BuyerId on the invoice does not comply，Incorrect：{0}", i.OriginalInvoiceNumber));
                }

                if (originalInvoice.InvoiceSeller.ReceiptNo != item.SellerId)
                {
                    return new Exception(String.Format("SellerId on the invoice does not comply，Incorrect：{0}", i.OriginalInvoiceNumber));
                }

                //原明細排列序號
                if (i.OriginalSequenceNumber > 1000 || i.OriginalSequenceNumber < 0)
                {
                    return new Exception(String.Format("Original Sequence Number on the length up to 3 yards，Incorrect：{0}，TAG：< OriginalInvoiceNumber />", i.OriginalSequenceNumber));
                }

                //折讓證明單明細排列序號
                if (i.AllowanceSequenceNumber > 1000 || i.AllowanceSequenceNumber < 0)
                {
                    return new Exception(String.Format("Allowance Sequence Number on the length up to 3 yards，Incorrect：{0}，TAG：< AllowanceSequenceNumber />", i.AllowanceSequenceNumber));
                }

                //原品名
                if (i.OriginalDescription == null || i.OriginalDescription.Length > 256)
                {
                    return new Exception(String.Format("Original Description can not be blank，Original Descriptionon the length up to 256 yards，Incorrect：{0}，TAG：< OriginalDescription />", i.OriginalDescription));
                }

                //單位
                if (i.Unit != null && i.Unit.Length > 6)
                {
                    return new Exception(String.Format("Unit on the length up to 6 yards，Incorrect：{0}，TAG：< Unit />", i.Unit));
                }

                //課稅別
                if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)i.TaxType))
                {
                    return new Exception(String.Format("Do not TaxType malformed，Incorrect：{0}，TAG：< TaxType />", i.TaxType));
                }

                DateTime invoiceDate;
                if (String.IsNullOrEmpty(i.OriginalInvoiceDate) || !DateTime.TryParseExact(String.Format("{0}", i.OriginalInvoiceDate), "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
                {
                    return new Exception(String.Format("Format of Original Invoice Date error(YYYY/MM/DD)；Incorrect:{0}", i.OriginalInvoiceDate));
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
                    var invProductItem = originalInvoice.InvoiceDetails.Join(mgr.GetTable<InvoiceProductItem>(), d => d.ProductID, p => p.ProductID, (d, p) => p)
                        .Where(p => p.No == i.OriginalSequenceNumber).FirstOrDefault();
                    if (invProductItem != null)
                    {
                        allowanceItem.ItemID = invProductItem.ItemID;
                    }
                }
                productItems.Add(allowanceItem);
            }

            return null;
        }

        #endregion
    }
}