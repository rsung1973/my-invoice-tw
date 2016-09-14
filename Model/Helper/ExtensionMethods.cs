using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;
using Uxnet.Com.Security.UseCrypto;
using System.Xml;

namespace Model.Helper
{
    public static class ExtensionMethods
    {
        public static Model.Schema.EIVO.A0101.Invoice BuildA0101(this InvoiceItem item)
        {
            Organization seller = item.Organization;
            InvoiceAmountType amount = item.InvoiceAmountType;

            var result = new Model.Schema.EIVO.A0101.Invoice
            {
                XSDVersion = "2.8",
                Main = new Model.Schema.EIVO.A0101.MainType
                {
                    InvoiceNumber = item.TrackCode + item.No,
                    BuyerRemark = item.BuyerRemark,
                    Category = item.Category,
                    CheckNumber = item.CheckNo,
                    CustomsClearanceMark = item.CustomsClearanceMark,
                    DonateMark = item.DonateMark,
                    GroupMark = item.GroupMark,
                    InvoiceDate = new Model.Schema.EIVO.A0101.DateType
                    {
                        Day = String.Format("{0:00}", item.InvoiceDate.Value.Day),
                        Month = String.Format("{0:00}", item.InvoiceDate.Value.Month),
                        Year = String.Format("{0:0000}", item.InvoiceDate.Value.Year - 1911)
                    },
                    InvoiceTime = item.InvoiceDate.Value.ToString("HH:mm:ss"),
//                    InvoiceTimeSpecified = true,
                    InvoiceType = "05",
                    MainRemark = item.Remark,
                    PermitDate = item.PermitDate.HasValue ? String.Format("{0:0000}{1:00}{2:00}", item.PermitDate.Value.Year - 1911, item.PermitDate.Value.Month, item.PermitDate.Value.Day) : null,
                    PermitNumber = item.PermitNumber,
                    PermitWord = item.PermitWord,
                    RelateNumber = item.RelateNumber,
                    TaxCenter = item.TaxCenter,
                    Seller = new Model.Schema.EIVO.A0101.MainTypeSeller
                    {
                        Address = seller.Addr,
                        //CustomerNumber = "",
                        EmailAddress = seller.ContactEmail,
                        FacsimileNumber = seller.Fax,
                        Identifier = seller.ReceiptNo,
                        Name = seller.CompanyName,
                        //PersonInCharge = seller.UndertakerName,
                        //RoleRemark = "",
                        TelephoneNumber = seller.Phone
                    }
                },
                Amount = new Model.Schema.EIVO.A0101.AmountType
                {
                    Currency = amount.CurrencyType != null ? amount.CurrencyType.AbbrevName : "NTD",
                    DiscountAmount = amount.DiscountAmount.HasValue ? (long)amount.DiscountAmount.Value : 0,
                    DiscountAmountSpecified = true,
                    ExchangeRate = 1,
                    ExchangeRateSpecified = false,
                    OriginalCurrencyAmount = amount.OriginalCurrencyAmount.HasValue ? amount.OriginalCurrencyAmount.Value : 0,
                    OriginalCurrencyAmountSpecified = false,
                    SalesAmount = (int)amount.SalesAmount.Value,
                    TaxAmount = amount.TaxAmount.HasValue ? (long)amount.TaxAmount.Value : 0,
                    TaxRate = amount.TaxRate.HasValue ? (float)amount.TaxRate.Value : 0.0000F,
                    TaxType = String.Format("{0}", amount.TaxType),
                    TotalAmount = (long)amount.TotalAmount.Value
                },
                Details = buildA0101Details(item)
            };

            if (item.InvoiceBuyer != null)
            {
                InvoiceBuyer buyer = item.InvoiceBuyer;
                if (buyer.Organization != null)
                {
                    Organization buyerOrg = buyer.Organization;
                    result.Main.Buyer = new Model.Schema.EIVO.A0101.MainTypeBuyer
                    {
                        Address = buyerOrg.Addr,
                        //CustomerNumber = "",
                        EmailAddress = buyerOrg.ContactEmail,
                        FacsimileNumber = buyerOrg.Fax,
                        Identifier = buyerOrg.ReceiptNo,
                        Name = buyerOrg.CompanyName,
                        //PersonInCharge = buyerOrg.UndertakerName,
                        //RoleRemark = "",
                        TelephoneNumber = buyerOrg.Phone
                    };
                }
                else
                {
                    result.Main.Buyer = new Model.Schema.EIVO.A0101.MainTypeBuyer
                    {
                        Address = buyer.Address,
                        //CustomerNumber = "",
                        //EmailAddress = "",
                        //FacsimileNumber = "",
                        Identifier = String.IsNullOrEmpty(buyer.ReceiptNo) ? "0000000000" : buyer.ReceiptNo,
                        Name = buyer.Name,
                        //PersonInCharge = buyer.Name,
                        //RoleRemark = "",
                        //TelephoneNumber = ""
                    };
                }
            }


            return result;
        }

        private static Model.Schema.EIVO.A0101.DetailsTypeProductItem[] buildA0101Details(InvoiceItem item)
        {
            List<Model.Schema.EIVO.A0101.DetailsTypeProductItem> items = new List<Model.Schema.EIVO.A0101.DetailsTypeProductItem>();
            foreach (var detailItem in item.InvoiceDetails)
            {
                foreach (var productItem in detailItem.InvoiceProduct.InvoiceProductItem)
                {
                    items.Add(new Model.Schema.EIVO.A0101.DetailsTypeProductItem
                    {
                        Amount = Decimal.Parse(String.Format("{0:#,0.0000}",productItem.CostAmount.Value.ToString())),
                        Description = detailItem.InvoiceProduct.Brief,
                        Item = productItem.ItemNo,
                        Quantity = Decimal.Parse(String.Format("{0:#,0.0000}", productItem.Piece.Value.ToString())),
                        RelateNumber = productItem.RelateNumber,
                        Remark = productItem.Remark,
                        SequenceNumber = String.Format("{0:00}", productItem.No),
                        Unit = productItem.PieceUnit,
                        UnitPrice = productItem.UnitCost.HasValue ? Decimal.Parse(String.Format("{0:#,0.0000}",productItem.UnitCost.Value.ToString())) : 0
                    });
                }
            }
            return items.ToArray();
        }

        public static Model.Schema.EIVO.B0101.Allowance BuildB0101(this InvoiceAllowance item)
        {
            var result = new Model.Schema.EIVO.B0101.Allowance
            {
                XSDVersion = "2.8",
                Main = new Model.Schema.EIVO.B0101.MainType
                {
                    AllowanceDate = new Model.Schema.EIVO.B0101.DateType
                    {
                        Day = String.Format("{0:00}", item.AllowanceDate.Value.Day),
                        Month = String.Format("{0:00}", item.AllowanceDate.Value.Month),
                        Year = String.Format("{0:0000}", item.AllowanceDate.Value.Year - 1911)
                    },
                    AllowanceNumber = item.AllowanceNumber,
                    AllowanceType = item.AllowanceType.ToString()
                },
                Amount = new Model.Schema.EIVO.B0101.AmountType
                {
                    TaxAmount = item.TaxAmount.HasValue ? (long)item.TaxAmount.Value : 0,
                    TotalAmount = item.TotalAmount.HasValue ? (long)item.TotalAmount.Value : 0
                }
            };

            result.Details = item.InvoiceAllowanceDetails.Select(d => new Schema.EIVO.B0101.DetailsTypeProductItem
            {
                AllowanceSequenceNumber = d.InvoiceAllowanceItem.No.ToString(),
                Amount = d.InvoiceAllowanceItem.Amount.Value,
                InvoiceDate = new Schema.EIVO.B0101.DateType
                {
                    Day = String.Format("{0:00}", item.InvoiceItem.InvoiceDate.Value.Day),
                    Month = String.Format("{0:00}", item.InvoiceItem.InvoiceDate.Value.Month),
                    Year = String.Format("{0:0000}", item.InvoiceItem.InvoiceDate.Value.Year - 1911)
                },
                OriginalDescription = d.InvoiceAllowanceItem.OriginalDescription,
                InvoiceNumber = String.Format("{0}{1}", item.InvoiceItem.TrackCode, item.InvoiceItem.No),
                Quantity = d.InvoiceAllowanceItem.Piece.HasValue ? d.InvoiceAllowanceItem.Piece.Value : 0.00000M,
                Tax = (long)d.InvoiceAllowanceItem.Tax.Value,
                TaxType = String.Format("{0}", d.InvoiceAllowanceItem.TaxType),
                Unit = d.InvoiceAllowanceItem.PieceUnit,
                UnitPrice = d.InvoiceAllowanceItem.UnitCost.HasValue ? d.InvoiceAllowanceItem.UnitCost.Value : 0
            }).ToArray();

            if (item.InvoiceAllowanceSeller != null)
            {
                Organization seller = item.InvoiceAllowanceSeller.Organization;
                result.Main.Seller = new Model.Schema.EIVO.B0101.MainTypeSeller
                {
                    Address = seller.Addr,
                    CustomerNumber = "",
                    EmailAddress = seller.ContactEmail,
                    FacsimileNumber = seller.Fax,
                    Identifier = seller.ReceiptNo,
                    Name = seller.CompanyName,
                    //PersonInCharge = seller.UndertakerName,
                    //RoleRemark = "",
                    TelephoneNumber = seller.Phone
                };
            }
            else
            {
                result.Main.Seller = new Model.Schema.EIVO.B0101.MainTypeSeller
                {
                    Identifier = item.SellerId
                };
            }

            if (item.InvoiceAllowanceBuyer != null)
            {
                Organization buyerOrg = item.InvoiceAllowanceBuyer.Organization;
                if (buyerOrg != null)
                {
                    result.Main.Buyer = new Model.Schema.EIVO.B0101.MainTypeBuyer
                    {
                        Address = buyerOrg.Addr,
                        CustomerNumber = "",
                        EmailAddress = buyerOrg.ContactEmail,
                        FacsimileNumber = buyerOrg.Fax,
                        Identifier = buyerOrg.ReceiptNo,
                        Name = buyerOrg.CompanyName,
                        //PersonInCharge = buyerOrg.UndertakerName,
                        //RoleRemark = "",
                        TelephoneNumber = buyerOrg.Phone
                    };
                }
                else
                {
                    result.Main.Buyer = new Model.Schema.EIVO.B0101.MainTypeBuyer
                    {
                        Address = item.InvoiceAllowanceBuyer.Address,
                        CustomerNumber = "",
                        EmailAddress = item.InvoiceAllowanceBuyer.EMail,
                        FacsimileNumber = "",
                        Identifier = item.InvoiceAllowanceBuyer.ReceiptNo,
                        Name = item.InvoiceAllowanceBuyer.CustomerName,
                        //PersonInCharge = buyerOrg.UndertakerName,
                        //RoleRemark = "",
                        TelephoneNumber = item.InvoiceAllowanceBuyer.Phone
                    };
                }
            }
            else
            {
                result.Main.Buyer = new Model.Schema.EIVO.B0101.MainTypeBuyer
                {
                    Identifier = item.BuyerId
                };
            }
            return result;
        }

        public static Model.Schema.EIVO.A0201.CancelInvoice BuildA0201(this InvoiceItem item)
        {
            Organization seller = item.Organization;
            InvoiceCancellation cancelItem = item.InvoiceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.EIVO.A0201.CancelInvoice
            {
                XSDVersion = "2.8",
                CancelDate = new Model.Schema.EIVO.A0201.DateType
                {
                    Day = String.Format("{0:00}", cancelItem.CancelDate.Value.Day),
                    Month = String.Format("{0:00}", cancelItem.CancelDate.Value.Month),
                    Year = String.Format("{0:0000}", cancelItem.CancelDate.Value.Year - 1911)
                },
                BuyerId =  item.InvoiceBuyer.BuyerID.HasValue ? item.InvoiceBuyer.Organization.ReceiptNo : item.InvoiceBuyer.ReceiptNo,
                CancelInvoiceNumber = cancelItem.CancellationNo,
                CancelTime = cancelItem.CancelDate.Value.ToString("HH:mm:ss"),
                InvoiceDate = new Model.Schema.EIVO.A0201.DateType
                {
                    Day = String.Format("{0:00}", item.InvoiceDate.Value.Day),
                    Month = String.Format("{0:00}", item.InvoiceDate.Value.Month),
                    Year = String.Format("{0:0000}", item.InvoiceDate.Value.Year - 1911)
                },
                Remark = cancelItem.Remark,
                ReturnTaxDocumentNumber = cancelItem.ReturnTaxDocumentNo,
                SellerId = seller.ReceiptNo
            };

            return result;
        }

        public static Model.Schema.EIVO.B0201.CancelAllowance BuildB0201(this InvoiceAllowance item)
        {
            InvoiceAllowanceCancellation cancelItem = item.InvoiceAllowanceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.EIVO.B0201.CancelAllowance
            {
                XSDVersion = "2.8",
                AllowanceDate = new Model.Schema.EIVO.B0201.DateType
                {
                    Day = String.Format("{0:00}", item.AllowanceDate.Value.Day),
                    Month = String.Format("{0:00}", item.AllowanceDate.Value.Month),
                    Year = String.Format("{0:0000}", item.AllowanceDate.Value.Year - 1911)
                },
                CancelDate = new Model.Schema.EIVO.B0201.DateType
                {
                    Day = String.Format("{0:00}", cancelItem.CancelDate.Value.Day),
                    Month = String.Format("{0:00}", cancelItem.CancelDate.Value.Month),
                    Year = String.Format("{0:0000}", cancelItem.CancelDate.Value.Year - 1911)
                },
                CancelTime = cancelItem.CancelDate.Value.ToString("HH:mm:ss"),
                CancelAllowanceNumber = item.AllowanceNumber,
                Remark = cancelItem.Remark,
                BuyerId = item.BuyerId,
                SellerId = item.InvoiceAllowanceSeller.Organization.ReceiptNo
            };

            return result;
        }

        public static bool SignAndCheck(X509Certificate2 certificate, StringBuilder sb,Naming.CACatalogDefinition catalog,int docID,Naming.DocumentTypeDefinition typeID)
        {
            SignedCms signedCms;
            if (certificate != null)
            {
                ContentInfo content = new ContentInfo(Encoding.Default.GetBytes(sb.ToString()));
                signedCms = new SignedCms(content, false);
                CmsSigner signer = new CmsSigner(certificate);

                signedCms.ComputeSignature(signer, true);
            }
            else
            {
                signedCms = EIVOPlatformFactory.SignCms(sb.ToString());
                if (signedCms == null)
                    return false;
            }

            CryptoUtility crypto = new CryptoUtility();
            byte[] dataToSign;
            PKCS7Log log = crypto.CA_Log.Table.DataSet as PKCS7Log;
            if (log != null)
            {
                log.Crypto = crypto;
                log.Catalog = catalog;
                log.DocID = docID;
                log.TypeID = typeID;
            }

            return crypto.VerifyEnvelopedPKCS7(signedCms.Encode(), out dataToSign);
        }

        public static bool SignAndCheckToReceiveInvoiceItem(this InvoiceItem item, X509Certificate2 certificate, StringBuilder sb,Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.CustomerName : item.InvoiceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.ReceiptNo : item.InvoiceSeller.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("發票號碼:").Append(item.TrackCode).Append(item.No).Append("\r\n")
                .Append("發票日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceDate.Value)).Append("\r\n")
                .Append("發票開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.CustomerName : item.InvoiceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb,Naming.CACatalogDefinition.平台自動接收,item.InvoiceID,Naming.DocumentTypeDefinition.E_Invoice);
        }

        public static bool SignAndCheckInvoiceItemByCounterpart(this InvoiceItem item, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動簽章確認\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.CustomerName : item.InvoiceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.ReceiptNo : item.InvoiceSeller.ReceiptNo).Append("\r\n")
                .Append("簽章確認時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("發票號碼:").Append(item.TrackCode).Append(item.No).Append("\r\n")
                .Append("發票日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceDate.Value)).Append("\r\n")
                .Append("原發票開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.CustomerName : item.InvoiceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, item.InvoiceID, Naming.DocumentTypeDefinition.E_Invoice);
        }

        public static bool SignAndCheckToIssueInvoiceItem(this InvoiceItem item, X509Certificate2 certificate, StringBuilder sb,Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.CustomerName : item.InvoiceBuyer.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.ReceiptNo : item.InvoiceBuyer.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("發票號碼:").Append(item.TrackCode).Append(item.No).Append("\r\n")
                .Append("發票日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceDate.Value)).Append("\r\n")
                .Append("發票接收營業人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.CustomerName : item.InvoiceSeller.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, item.InvoiceID, Naming.DocumentTypeDefinition.E_Invoice);

        }

        public static bool SignAndCheckToReceiveInvoiceAllowance(this InvoiceAllowance item, X509Certificate2 certificate, StringBuilder sb,Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票折讓證明單由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.ReceiptNo : item.InvoiceAllowanceSeller.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("折讓單開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動接收, item.AllowanceID, Naming.DocumentTypeDefinition.E_Allowance);

        }

        public static bool SignAndCheckInvoiceAllowanceByCounterpart(this InvoiceAllowance item, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票折讓證明單(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動簽章確認\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.ReceiptNo : item.InvoiceAllowanceSeller.ReceiptNo).Append("\r\n")
                .Append("簽章確認時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("原折讓單開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, item.AllowanceID, Naming.DocumentTypeDefinition.E_Allowance);

        }

        public static bool SignAndCheckToIssueInvoiceAllowance(this InvoiceAllowance item, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本發票折讓證明單(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.ReceiptNo : item.InvoiceAllowanceBuyer.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("折讓單接收營業人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, item.AllowanceID, Naming.DocumentTypeDefinition.E_Allowance);
        }

        public static bool SignAndCheckToReceiveInvoiceCancellation(this InvoiceItem item, X509Certificate2 certificate, StringBuilder sb,int docID,Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本作廢發票由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(type==Naming.InvoiceCenterBusinessType.銷項 ?item.InvoiceBuyer.CustomerName:item.InvoiceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type==Naming.InvoiceCenterBusinessType.銷項 ?item.InvoiceBuyer.ReceiptNo:item.InvoiceSeller.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("作廢發票號碼:").Append(item.InvoiceCancellation.CancellationNo).Append("\r\n")
                .Append("作廢發票日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceCancellation.CancelDate.Value)).Append("\r\n")
                .Append("發票開立人:").Append(type==Naming.InvoiceCenterBusinessType.銷項 ?item.InvoiceSeller.CustomerName:item.InvoiceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動接收, docID, Naming.DocumentTypeDefinition.E_InvoiceCancellation);
        }

        public static bool SignAndCheckInvoiceCancellationByCounterpart(this CDS_Document docItem, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            var item = docItem.DerivedDocument.ParentDocument.InvoiceItem;
            var cancelItem = item.InvoiceCancellation;

            sb.Append("本作廢發票(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動簽章確認\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.CustomerName : item.InvoiceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.ReceiptNo : item.InvoiceSeller.ReceiptNo).Append("\r\n")
                .Append("簽章確認時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("作廢發票號碼:").Append(item.InvoiceCancellation.CancellationNo).Append("\r\n")
                .Append("作廢發票日期:").Append(ValueValidity.ConvertChineseDateString(item.InvoiceCancellation.CancelDate.Value)).Append("\r\n")
                .Append("原發票開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.CustomerName : item.InvoiceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, docItem.DocID, Naming.DocumentTypeDefinition.E_InvoiceCancellation);
        }

        public static bool SignAndCheckToIssueInvoiceCancellation(this CDS_Document docItem, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            var item = docItem.DerivedDocument.ParentDocument.InvoiceItem;
            var cancelItem = item.InvoiceCancellation;

            sb.Append("本作廢發票(").Append(((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()).Append(")由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.CustomerName : item.InvoiceBuyer.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceSeller.ReceiptNo : item.InvoiceBuyer.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("作廢發票號碼:").Append(cancelItem.CancellationNo).Append("\r\n")
                .Append("作廢發票日期:").Append(ValueValidity.ConvertChineseDateString(cancelItem.CancelDate.Value)).Append("\r\n")
                .Append("發票接收營業人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceBuyer.CustomerName : item.InvoiceSeller.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, docItem.DocID, Naming.DocumentTypeDefinition.E_InvoiceCancellation);

        }

        public static bool SignAndCheckToReceiveAllowanceCancellation(this InvoiceAllowance item, X509Certificate2 certificate, StringBuilder sb,int docID,Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本作廢發票折讓證明單由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.ReceiptNo : item.InvoiceAllowanceSeller.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("折讓單開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動接收, docID, Naming.DocumentTypeDefinition.E_AllowanceCancellation);

        }

        public static bool SignAndCheckAllowanceCancellationByCounterpart(this CDS_Document docItem, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {

            if (sb == null)
                sb = new StringBuilder();

            var item = docItem.DerivedDocument.ParentDocument.InvoiceAllowance;
            var cancelItem = item.InvoiceAllowanceCancellation;

            sb.Append("本作廢發票折讓證明單(").Append(((Naming.B2BInvoiceStepDefinition)item.CDS_Document.CurrentStep).ToString()).Append(")由營業人指定由系統主動簽章確認\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.ReceiptNo : item.InvoiceAllowanceSeller.ReceiptNo).Append("\r\n")
                .Append("簽章確認時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("原折讓單開立人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, docItem.DocID, Naming.DocumentTypeDefinition.E_AllowanceCancellation);
        }

        public static bool SignAndCheckToIssueAllowanceCancellation(this CDS_Document docItem, X509Certificate2 certificate, StringBuilder sb, Naming.InvoiceCenterBusinessType type)
        {
            if (sb == null)
                sb = new StringBuilder();

            var item = docItem.DerivedDocument.ParentDocument.InvoiceAllowance;
            var cancelItem = item.InvoiceAllowanceCancellation;

            sb.Append("本作廢發票折讓證明單(").Append(((Naming.B2BInvoiceStepDefinition)docItem.CurrentStep).ToString()).Append(")由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.CustomerName : item.InvoiceAllowanceBuyer.CustomerName).Append("\r\n")
                .Append("營業人統編:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceSeller.ReceiptNo : item.InvoiceAllowanceBuyer.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("折讓單號碼:").Append(item.AllowanceNumber).Append("\r\n")
                .Append("折讓單日期:").Append(ValueValidity.ConvertChineseDateString(item.AllowanceDate.Value)).Append("\r\n")
                .Append("折讓單接收營業人:").Append(type == Naming.InvoiceCenterBusinessType.銷項 ? item.InvoiceAllowanceBuyer.CustomerName : item.InvoiceAllowanceSeller.CustomerName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, docItem.DocID, Naming.DocumentTypeDefinition.E_AllowanceCancellation);
        }

        public static bool SignAndCheckToReceiveReceipt(this ReceiptItem item, X509Certificate2 certificate, StringBuilder sb)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本收據由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(item.Buyer.CompanyName).Append("\r\n")
                .Append("營業人統編:").Append(item.Buyer.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("收據號碼:").Append(item.No).Append("\r\n")
                .Append("收據開立日期:").Append(ValueValidity.ConvertChineseDateString(item.ReceiptDate)).Append("\r\n")
                .Append("收據開立人:").Append(item.Seller.CompanyName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動接收, item.ReceiptID, Naming.DocumentTypeDefinition.E_Receipt);

        }

        public static bool SignAndCheckToIssueReceipt(this ReceiptItem item, X509Certificate2 certificate, StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本收據由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(item.Seller.CompanyName).Append("\r\n")
                .Append("營業人統編:").Append(item.Seller.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("收據號碼:").Append(item.No).Append("\r\n")
                .Append("收據開立日期:").Append(ValueValidity.ConvertChineseDateString(item.ReceiptDate)).Append("\r\n")
                .Append("收據接收營業人:").Append(item.Buyer.CompanyName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, item.ReceiptID, Naming.DocumentTypeDefinition.E_Receipt);
        }

        public static bool SignAndCheckToReceiveReceiptCancellation(this ReceiptItem item, X509Certificate2 certificate, StringBuilder sb,int docID)
        {

            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本作廢收據由營業人指定由系統主動接收\r\n")
                .Append("營業人名稱:").Append(item.Buyer.CompanyName).Append("\r\n")
                .Append("營業人統編:").Append(item.Buyer.ReceiptNo).Append("\r\n")
                .Append("接收時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("作廢收據號碼:").Append(item.ReceiptCancellation.CancellationNo).Append("\r\n")
                .Append("作廢收據開立日期:").Append(ValueValidity.ConvertChineseDateString(item.ReceiptCancellation.CancelDate)).Append("\r\n")
                .Append("作廢收據開立人:").Append(item.Seller.CompanyName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動接收, docID, Naming.DocumentTypeDefinition.E_ReceiptCancellation);

        }


        public static bool SignAndCheckToIssueReceiptCancellation(this ReceiptCancellation cancelItem, ReceiptItem item, X509Certificate2 certificate, StringBuilder sb,int docID)
        {
            if (sb == null)
                sb = new StringBuilder();

            sb.Append("本作廢收據由營業人指定由系統主動開立\r\n")
                .Append("營業人名稱:").Append(item.Seller.CompanyName).Append("\r\n")
                .Append("營業人統編:").Append(item.Seller.ReceiptNo).Append("\r\n")
                .Append("開立時間:").Append(DateTime.Now.ToString()).Append("\r\n")
                .Append("作廢收據號碼:").Append(cancelItem.CancellationNo).Append("\r\n")
                .Append("作廢收據開立日期:").Append(ValueValidity.ConvertChineseDateString(cancelItem.CancelDate)).Append("\r\n")
                .Append("作廢收據接收營業人:").Append(item.Buyer.CompanyName).Append("\r\n");

            return SignAndCheck(certificate, sb, Naming.CACatalogDefinition.平台自動開立, docID, Naming.DocumentTypeDefinition.E_ReceiptCancellation);
        }

        public static Exception UploadInvoiceSpecializedCheck(this Model.Schema.EIVO.B2B.SellerInvoiceRootInvoice item)
        {
            if (item.BuyerId == "84613756")
            {
                XmlNode[] extraRemark = item.ExtraRemark as XmlNode[];
                XmlNode node;
                if (extraRemark == null)
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵時,必須有ExtraRemark的Tag內容,部門、姓名、職編、合約/訂單/工令/採購案號四欄位"));
                }
                if ((node = extraRemark.Where(n => n.Name == "RepDept").FirstOrDefault()) == null)
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,ExtraRemark的Tag內容缺少RepDept的Tag"));
                }
                else if (node["Description"] == null || String.IsNullOrEmpty(node["Description"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepDept的Description標籤或其標籤內容為空白"));
                }
                else if (node["Value"] == null || String.IsNullOrEmpty(node["Value"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepDept的Value標籤或其標籤內容為空白"));
                }

                if ((node = extraRemark.Where(n => n.Name == "RepName").FirstOrDefault()) == null)
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,ExtraRemark的Tag內容缺少RepName的Tag"));
                }
                else if (node["Description"] == null || String.IsNullOrEmpty(node["Description"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepName的Description標籤或其標籤內容為空白"));
                }
                else if (node["Value"] == null || String.IsNullOrEmpty(node["Value"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepName的Value標籤或其標籤內容為空白"));
                }

                if ((node = extraRemark.Where(n => n.Name == "RepEmployeeId").FirstOrDefault()) == null)
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,ExtraRemark的Tag內容缺少RepEmployeeId的Tag"));
                }
                else if (node["Description"] == null || String.IsNullOrEmpty(node["Description"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepEmployeeId的Description標籤或其標籤內容為空白"));
                }
                else if (node["Value"] == null || String.IsNullOrEmpty(node["Value"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepEmployeeId的Value標籤或其標籤內容為空白"));
                }

                if ((node = extraRemark.Where(n => n.Name == "RepNumber").FirstOrDefault()) == null)
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,ExtraRemark的Tag內容缺少RepNumber的Tag"));
                }
                else if (node["Description"] == null || String.IsNullOrEmpty(node["Description"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepNumber的Description標籤或其標籤內容為空白"));
                }
                else if (node["Value"] == null || String.IsNullOrEmpty(node["Value"].InnerText))
                {
                    return new Exception(String.Format("發票買受人為中龍鋼鐵,缺少ExtraRemark的RepNumber的Value標籤或其標籤內容為空白"));
                }
            }
            return null;
        }

        //public static Model.Schema.MIG3_1.E0401.BranchTrack CreateE1401(this Model.Schema.EIVO.BranchTrack item)
        //{
        //    var result = new Model.Schema.MIG3_1.E0401.BranchTrack
        //    {
        //        Main = new Schema.MIG3_1.E0401.Main
        //        {
        //            HeadBan = item.Main.HeadBan,
        //            BranchBan = item.Main.BranchBan,
        //            InvoiceType = (Schema.MIG3_1.E0401.InvoiceTypeEnum)(int.Parse(item.Main.InvoiceType)),
        //            YearMonth = item.Main.YearMonth,
        //            InvoiceTrack = item.Main.InvoiceTrack,
        //            InvoiceBeginNo = item.Main.InvoiceBeginNo,
        //            InvoiceEndNo = item.Main.InvoiceEndNo
        //        },
        //        Details = item.Details.Select(d => new Schema.MIG3_1.E0401.DetailsBranchTrackItem
        //            {
        //                InvoiceBeginNo = d.InvoiceBeginNo,
        //                InvoiceEndNo = d.InvoiceEndNo,
        //                InvoiceBooklet = long.Parse(d.InvoiceBooklet)
        //            }
        //        ).ToArray()
        //    };
        //    return result;
        //}       
    }
}
