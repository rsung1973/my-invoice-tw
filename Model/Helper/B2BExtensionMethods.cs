using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Schema.EIVO;
using Model.DataEntity;
using System.Xml;

namespace Model.Helper
{
    public static class B2BExtensionMethods
    {

        public static Model.Schema.TurnKey.A1401.Invoice CreateA1401(this InvoiceItem item)
        {
            var result = new Model.Schema.TurnKey.A1401.Invoice
            {
                Main = new Schema.TurnKey.A1401.Main
                {
                    Buyer = new Schema.TurnKey.A1401.MainBuyer
                    {
                        Address = item.InvoiceBuyer.Address,
                        CustomerNumber = item.InvoiceBuyer.CustomerID,
                        EmailAddress = item.InvoiceBuyer.EMail,
                        FacsimileNumber = item.InvoiceBuyer.Fax,
                        Identifier = item.InvoiceBuyer.ReceiptNo,
                        Name = item.InvoiceBuyer.Name,
                        PersonInCharge = item.InvoiceBuyer.PersonInCharge,
                        RoleRemark = item.InvoiceBuyer.RoleRemark,
                        TelephoneNumber = item.InvoiceBuyer.Phone
                    },
                    BuyerRemark = String.IsNullOrEmpty(item.BuyerRemark) ? Model.Schema.TurnKey.A1401.BuyerRemarkEnum.Item1 : (Model.Schema.TurnKey.A1401.BuyerRemarkEnum)int.Parse(item.BuyerRemark),
                    BuyerRemarkSpecified = !String.IsNullOrEmpty(item.BuyerRemark),
                    Category = item.Category,
                    CheckNumber = item.CheckNo,
                    CustomsClearanceMark = String.IsNullOrEmpty(item.CustomsClearanceMark) ? Model.Schema.TurnKey.A1401.CustomsClearanceMarkEnum.Item1 : (Model.Schema.TurnKey.A1401.CustomsClearanceMarkEnum)int.Parse(item.CustomsClearanceMark),
                    CustomsClearanceMarkSpecified = !String.IsNullOrEmpty(item.CustomsClearanceMark),
                    InvoiceType = (Schema.TurnKey.A1401.InvoiceTypeEnum)((int)item.InvoiceType),
                    //DonateMark = (Schema.TurnKey.A1401.DonateMarkEnum)(int.Parse(item.DonateMark)),
                    DonateMark = string.IsNullOrEmpty(item.DonateMark) ? Model.Schema.TurnKey.A1401.DonateMarkEnum.Item0 : (Schema.TurnKey.A1401.DonateMarkEnum)(int.Parse(item.DonateMark)),
                    GroupMark = item.GroupMark,
                    InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
                    InvoiceTimeSpecified = false,
                    InvoiceNumber = String.Format("{0}{1}", item.TrackCode, item.No),
                    MainRemark = item.Remark,
                    PermitNumber = item.PermitNumber,
                    PermitDate = item.PermitDate.HasValue ? String.Format("{0:yyyyMMdd}", item.PermitDate.Value) : null,
                    PermitWord = item.PermitWord,
                    RelateNumber = item.RelateNumber,
                    TaxCenter = item.TaxCenter,
                    Seller = new Schema.TurnKey.A1401.MainSeller
                    {
                        Address = item.InvoiceSeller.Address,
                        CustomerNumber = item.InvoiceSeller.CustomerID,
                        EmailAddress = item.InvoiceSeller.EMail,
                        FacsimileNumber = item.InvoiceSeller.Fax,
                        Identifier = item.InvoiceSeller.ReceiptNo,
                        Name = item.InvoiceSeller.Name,
                        PersonInCharge = item.InvoiceSeller.PersonInCharge,
                        RoleRemark = item.InvoiceSeller.RoleRemark,
                        TelephoneNumber = item.InvoiceSeller.Phone
                    }
                },
                Details = buildA1401Details(item),
                Amount = new Schema.TurnKey.A1401.Amount
                {
                    CurrencySpecified = false,
                    DiscountAmount = item.InvoiceAmountType.DiscountAmount.HasValue ? (long)item.InvoiceAmountType.DiscountAmount.Value : 0,
                    DiscountAmountSpecified = item.InvoiceAmountType.DiscountAmount.HasValue,
                    ExchangeRateSpecified = false,
                    OriginalCurrencyAmountSpecified = false,
                    SalesAmount = item.InvoiceAmountType.SalesAmount.HasValue ? (long)item.InvoiceAmountType.SalesAmount.Value : 0,
                    TaxAmount = item.InvoiceAmountType.TaxAmount.HasValue ? (long)item.InvoiceAmountType.TaxAmount.Value : 0,
                    TaxRate = item.InvoiceAmountType.TaxRate.HasValue ? item.InvoiceAmountType.TaxRate.Value : 0.05m,
                    TaxType = (Schema.TurnKey.A1401.TaxTypeEnum)((int)item.InvoiceAmountType.TaxType.Value),
                    TotalAmount = item.InvoiceAmountType.TotalAmount.HasValue ? (long)item.InvoiceAmountType.TotalAmount : 0
                }
            };

            return result;
        }

        private static Schema.TurnKey.A1401.DetailsProductItem[] buildA1401Details(InvoiceItem item)
        {
            List<Model.Schema.TurnKey.A1401.DetailsProductItem> items = new List<Schema.TurnKey.A1401.DetailsProductItem>();
            foreach (var detailItem in item.InvoiceDetails)
            {
                foreach (var productItem in detailItem.InvoiceProduct.InvoiceProductItem)
                {
                    items.Add(new Model.Schema.TurnKey.A1401.DetailsProductItem
                    {
                        Amount = productItem.CostAmount.HasValue ? productItem.CostAmount.Value : 0m,
                        Amount2 = productItem.CostAmount2.HasValue ? productItem.CostAmount2.Value : 0m,
                        Description = detailItem.InvoiceProduct.Brief,
                        Quantity = productItem.Piece.HasValue ? productItem.Piece.Value : 0,
                        Quantity2 = productItem.Piece2.HasValue ? productItem.Piece2.Value : 0,
                        RelateNumber = productItem.RelateNumber,
                        Remark = productItem.Remark,
                        SequenceNumber = String.Format("{0:00}", productItem.No),
                        Unit = productItem.PieceUnit,
                        Unit2 = productItem.PieceUnit2,
                        UnitPrice = productItem.UnitCost.HasValue ? productItem.UnitCost.Value : 0,
                        UnitPrice2 = productItem.UnitCost2.HasValue ? productItem.UnitCost2.Value : 0
                    });
                }
            }
            return items.ToArray();
        }


        public static Model.Schema.TurnKey.A1101.Invoice CreateA1101(this InvoiceItem item)
        {
            var result = new Model.Schema.TurnKey.A1101.Invoice
            {
                Main = new Schema.TurnKey.A1101.Main
                {
                    Buyer = new Schema.TurnKey.A1101.MainBuyer
                    {
                        Address = item.InvoiceBuyer.Address,
                        CustomerNumber = item.InvoiceBuyer.CustomerID,
                        EmailAddress = item.InvoiceBuyer.EMail,
                        FacsimileNumber = item.InvoiceBuyer.Fax,
                        Identifier = item.InvoiceBuyer.ReceiptNo,
                        Name = item.InvoiceBuyer.Name,
                        PersonInCharge = item.InvoiceBuyer.PersonInCharge,
                        RoleRemark = item.InvoiceBuyer.RoleRemark,
                        TelephoneNumber = item.InvoiceBuyer.Phone
                    },
                    BuyerRemark = String.IsNullOrEmpty(item.BuyerRemark) ? Model.Schema.TurnKey.A1101.BuyerRemarkEnum.Item1 : (Model.Schema.TurnKey.A1101.BuyerRemarkEnum)int.Parse(item.BuyerRemark),
                    BuyerRemarkSpecified = !String.IsNullOrEmpty(item.BuyerRemark),
                    Category = item.Category,
                    CheckNumber = item.CheckNo,
                    CustomsClearanceMark = String.IsNullOrEmpty(item.CustomsClearanceMark) ? Model.Schema.TurnKey.A1101.CustomsClearanceMarkEnum.Item1 : (Model.Schema.TurnKey.A1101.CustomsClearanceMarkEnum)int.Parse(item.CustomsClearanceMark),
                    CustomsClearanceMarkSpecified = !String.IsNullOrEmpty(item.CustomsClearanceMark),
                    DonateMark = (Schema.TurnKey.A1101.DonateMarkEnum)(int.Parse(item.DonateMark)),
                    GroupMark = item.GroupMark,
                    InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
                    InvoiceTimeSpecified = false,
                    InvoiceNumber = String.Format("{0}{1}", item.TrackCode, item.No),
                    MainRemark = item.Remark,
                    PermitNumber = item.PermitNumber,
                    PermitDate = item.PermitDate.HasValue ? String.Format("{0:yyyyMMdd}", item.PermitDate.Value) : null,
                    PermitWord = item.PermitWord,
                    RelateNumber = item.RelateNumber,
                    TaxCenter = item.TaxCenter,
                    Seller = new Schema.TurnKey.A1101.MainSeller
                    {
                        Address = item.InvoiceSeller.Address,
                        CustomerNumber = item.InvoiceSeller.CustomerID,
                        EmailAddress = item.InvoiceSeller.EMail,
                        FacsimileNumber = item.InvoiceSeller.Fax,
                        Identifier = item.InvoiceSeller.ReceiptNo,
                        Name = item.InvoiceSeller.Name,
                        PersonInCharge = item.InvoiceSeller.PersonInCharge,
                        RoleRemark = item.InvoiceSeller.RoleRemark,
                        TelephoneNumber = item.InvoiceSeller.Phone
                    }
                },
                Details = buildA1101Details(item),
                Amount = new Schema.TurnKey.A1101.Amount {
                    CurrencySpecified = false,
                    DiscountAmount= item.InvoiceAmountType.DiscountAmount.HasValue?(long)item.InvoiceAmountType.DiscountAmount.Value:0,
                    DiscountAmountSpecified = item.InvoiceAmountType.DiscountAmount.HasValue,
                    ExchangeRateSpecified = false,
                    OriginalCurrencyAmountSpecified = false,
                    SalesAmount = item.InvoiceAmountType.SalesAmount.HasValue?(long)item.InvoiceAmountType.SalesAmount.Value:0,
                    TaxAmount = item.InvoiceAmountType.TaxAmount.HasValue?(long)item.InvoiceAmountType.TaxAmount.Value:0,
                    TaxRate = item.InvoiceAmountType.TaxRate.HasValue?item.InvoiceAmountType.TaxRate.Value:0m,
                    TaxType = (Schema.TurnKey.A1101.TaxTypeEnum)item.InvoiceAmountType.TaxType.Value
                }
            };

            return result;
        }

        private static Schema.TurnKey.A1101.DetailsProductItem[] buildA1101Details(InvoiceItem item)
        {
            List<Model.Schema.TurnKey.A1101.DetailsProductItem> items = new List<Schema.TurnKey.A1101.DetailsProductItem>();
            foreach (var detailItem in item.InvoiceDetails)
            {
                foreach (var productItem in detailItem.InvoiceProduct.InvoiceProductItem)
                {
                    items.Add(new Model.Schema.TurnKey.A1101.DetailsProductItem
                    {
                        Amount = productItem.CostAmount.HasValue ? productItem.CostAmount.Value : 0m,
                        Amount2 = productItem.CostAmount2.HasValue ? productItem.CostAmount2.Value : 0m,
                        Description = detailItem.InvoiceProduct.Brief,
                        Quantity = productItem.Piece.HasValue ? productItem.Piece.Value : 0,
                        Quantity2 = productItem.Piece2.HasValue ? productItem.Piece2.Value : 0,
                        RelateNumber = productItem.RelateNumber,
                        Remark = productItem.Remark,
                        SequenceNumber = String.Format("{0:00}", productItem.No),
                        Unit = productItem.PieceUnit,
                        Unit2 = productItem.PieceUnit2,
                        UnitPrice = productItem.UnitCost.HasValue ? productItem.UnitCost.Value : 0,
                        UnitPrice2 = productItem.UnitCost2.HasValue ? productItem.UnitCost2.Value : 0
                    });
                }
            }
            return items.ToArray();
        }

        public static Model.Schema.TurnKey.B1101.Allowance CreateB1101(this InvoiceAllowance item)
        {
            var result = new Model.Schema.TurnKey.B1101.Allowance
            {
                Main = new Schema.TurnKey.B1101.Main
                {
                    AllowanceDate = String.Format("{0:yyyyMMdd}", item.AllowanceDate),
                    AllowanceNumber = item.AllowanceNumber,
                    AllowanceType = (Model.Schema.TurnKey.B1101.AllowanceTypeEnum)item.AllowanceType,
                    Buyer = new Schema.TurnKey.B1101.MainBuyer {
                        Address = item.InvoiceAllowanceBuyer.Address,
                        CustomerNumber = item.InvoiceAllowanceBuyer.CustomerID,
                        EmailAddress = item.InvoiceAllowanceBuyer.EMail,
                        FacsimileNumber = item.InvoiceAllowanceBuyer.Fax,
                        Identifier = item.InvoiceAllowanceBuyer.ReceiptNo,
                        Name = item.InvoiceAllowanceBuyer.Name,
                        PersonInCharge = item.InvoiceAllowanceBuyer.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceBuyer.Phone,
                        RoleRemark = item.InvoiceAllowanceBuyer.RoleRemark
                    },
                    Seller = new Schema.TurnKey.B1101.MainSeller {
                        Address = item.InvoiceAllowanceSeller.Address,
                        CustomerNumber = item.InvoiceAllowanceSeller.CustomerID,
                        EmailAddress = item.InvoiceAllowanceSeller.EMail,
                        FacsimileNumber = item.InvoiceAllowanceSeller.Fax,
                        Identifier = item.InvoiceAllowanceSeller.ReceiptNo,
                        Name = item.InvoiceAllowanceSeller.Name,
                        PersonInCharge = item.InvoiceAllowanceSeller.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceSeller.Phone,
                        RoleRemark = item.InvoiceAllowanceSeller.RoleRemark
                    }
                },
                Amount = new Model.Schema.TurnKey.B1101.Amount
                {
                    TaxAmount = item.TaxAmount.HasValue ? (long)item.TaxAmount.Value : 0,
                    TotalAmount = item.TotalAmount.HasValue ? (long)item.TotalAmount.Value : 0
                }
            };

            result.Details = item.InvoiceAllowanceDetails.Select(d => new Schema.TurnKey.B1101.DetailsProductItem
            {
                AllowanceSequenceNumber = d.InvoiceAllowanceItem.No.ToString(),
                Amount = d.InvoiceAllowanceItem.Amount.HasValue? d.InvoiceAllowanceItem.Amount.Value :0m,
                Amount2 = d.InvoiceAllowanceItem.Amount2.HasValue?d.InvoiceAllowanceItem.Amount2.Value:0m,
                OriginalInvoiceDate = String.Format("{0:yyyyMMdd}",item.InvoiceItem.InvoiceDate),
                OriginalDescription = d.InvoiceAllowanceItem.OriginalDescription,
                OriginalInvoiceNumber = String.Format("{0}{1}", item.InvoiceItem.TrackCode, item.InvoiceItem.No),
                Quantity = d.InvoiceAllowanceItem.Piece.HasValue ? d.InvoiceAllowanceItem.Piece.Value : 0.00000M,
                Quantity2 = d.InvoiceAllowanceItem.Piece2.HasValue?d.InvoiceAllowanceItem.Piece2.Value:0.00000M,
                Tax = (long)d.InvoiceAllowanceItem.Tax.Value,
                TaxType = (Schema.TurnKey.B1101.DetailsProductItemTaxType)d.InvoiceAllowanceItem.TaxType,
                Unit = d.InvoiceAllowanceItem.PieceUnit,
                UnitPrice = d.InvoiceAllowanceItem.UnitCost.HasValue ? d.InvoiceAllowanceItem.UnitCost.Value : 0
            }).ToArray();

            return result;
        }

        public static Model.Schema.TurnKey.B1401.Allowance CreateB1401(this InvoiceAllowance item)
        {
            var result = new Model.Schema.TurnKey.B1401.Allowance
            {
                Main = new Schema.TurnKey.B1401.Main
                {
                    AllowanceDate = String.Format("{0:yyyyMMdd}", item.AllowanceDate),
                    AllowanceNumber = item.AllowanceNumber,
                    AllowanceType = (Model.Schema.TurnKey.B1401.AllowanceTypeEnum)item.AllowanceType,
                    Buyer = new Schema.TurnKey.B1401.MainBuyer
                    {
                        Address = item.InvoiceAllowanceBuyer.Address,
                        CustomerNumber = item.InvoiceAllowanceBuyer.CustomerID,
                        EmailAddress = item.InvoiceAllowanceBuyer.EMail,
                        FacsimileNumber = item.InvoiceAllowanceBuyer.Fax,
                        Identifier = item.InvoiceAllowanceBuyer.ReceiptNo,
                        Name = item.InvoiceAllowanceBuyer.Name,
                        PersonInCharge = item.InvoiceAllowanceBuyer.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceBuyer.Phone,
                        RoleRemark = item.InvoiceAllowanceBuyer.RoleRemark
                    },
                    Seller = new Schema.TurnKey.B1401.MainSeller
                    {
                        Address = item.InvoiceAllowanceSeller.Address,
                        CustomerNumber = item.InvoiceAllowanceSeller.CustomerID,
                        EmailAddress = item.InvoiceAllowanceSeller.EMail,
                        FacsimileNumber = item.InvoiceAllowanceSeller.Fax,
                        Identifier = item.InvoiceAllowanceSeller.ReceiptNo,
                        Name = item.InvoiceAllowanceSeller.Name,
                        PersonInCharge = item.InvoiceAllowanceSeller.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceSeller.Phone,
                        RoleRemark = item.InvoiceAllowanceSeller.RoleRemark
                    }
                },
                Amount = new Model.Schema.TurnKey.B1401.Amount
                {
                    TaxAmount = item.TaxAmount.HasValue ? (long)item.TaxAmount.Value : 0,
                    TotalAmount = item.TotalAmount.HasValue ? (long)item.TotalAmount.Value : 0
                }
            };

            result.Details = item.InvoiceAllowanceDetails.Select(d => new Schema.TurnKey.B1401.DetailsProductItem
            {
                AllowanceSequenceNumber = d.InvoiceAllowanceItem.No.ToString(),
                Amount = d.InvoiceAllowanceItem.Amount.HasValue ? d.InvoiceAllowanceItem.Amount.Value : 0m,
                Amount2 = d.InvoiceAllowanceItem.Amount2.HasValue ? d.InvoiceAllowanceItem.Amount2.Value : 0m,
                OriginalSequenceNumber  = d.InvoiceAllowanceItem.OriginalSequenceNo.HasValue ?  d.InvoiceAllowanceItem.OriginalSequenceNo.Value .ToString () :"1",
                OriginalInvoiceDate = String.Format("{0:yyyyMMdd}", d.InvoiceAllowanceItem.InvoiceDate),
                OriginalDescription = d.InvoiceAllowanceItem.OriginalDescription,
                OriginalInvoiceNumber = d.InvoiceAllowanceItem.InvoiceNo,
                Quantity = d.InvoiceAllowanceItem.Piece.HasValue ? d.InvoiceAllowanceItem.Piece.Value : 0.00000M,
                Quantity2 = d.InvoiceAllowanceItem.Piece2.HasValue ? d.InvoiceAllowanceItem.Piece2.Value : 0.00000M,
                Tax = (long)d.InvoiceAllowanceItem.Tax.Value,
                TaxType = (Schema.TurnKey.B1401.DetailsProductItemTaxType)d.InvoiceAllowanceItem.TaxType,
                Unit = d.InvoiceAllowanceItem.PieceUnit,
                Unit2 = d.InvoiceAllowanceItem.PieceUnit2,
                UnitPrice2 = d.InvoiceAllowanceItem .UnitCost2 .HasValue ?d.InvoiceAllowanceItem.UnitCost2.Value : 0,
                UnitPrice = d.InvoiceAllowanceItem.UnitCost.HasValue ? d.InvoiceAllowanceItem.UnitCost.Value : 0
            }).ToArray();

            return result;
        }

        public static Model.Schema.TurnKey.A0201.CancelInvoice CreateA0201(this InvoiceItem item)
        {
            InvoiceCancellation cancelItem = item.InvoiceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.TurnKey.A0201.CancelInvoice
            {
                CancelDate = String.Format("{0:yyyyMMdd}", cancelItem.CancelDate.Value),
                BuyerId = item.InvoiceBuyer.ReceiptNo,
                CancelInvoiceNumber = cancelItem.CancellationNo,
                CancelTime = cancelItem.CancelDate.Value,
                InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate.Value),
                Remark = cancelItem.Remark,
                ReturnTaxDocumentNumber = cancelItem.ReturnTaxDocumentNo,
                SellerId = item.InvoiceSeller.ReceiptNo,
                CancelReason = cancelItem.Remark
            };

            return result;
        }

        public static Model.Schema.TurnKey.A0501.CancelInvoice CreateA0501(this InvoiceItem item)
        {
            InvoiceCancellation cancelItem = item.InvoiceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.TurnKey.A0501.CancelInvoice
            {
                CancelDate = String.Format("{0:yyyyMMdd}", cancelItem.CancelDate.Value),
                BuyerId = item.InvoiceBuyer.ReceiptNo,
                CancelInvoiceNumber = cancelItem.CancellationNo,
                CancelTime = cancelItem.CancelDate.Value,
                InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate.Value),
                Remark = cancelItem.Remark,
                ReturnTaxDocumentNumber = cancelItem.ReturnTaxDocumentNo,
                SellerId = item.InvoiceSeller.ReceiptNo,
                CancelReason = cancelItem.Remark
            };

            return result;
        }


        public static Model.Schema.TurnKey.B0201.CancelAllowance CreateB0201(this InvoiceAllowance item)
        {
            InvoiceAllowanceCancellation cancelItem = item.InvoiceAllowanceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.TurnKey.B0201.CancelAllowance
            {
                AllowanceDate = String.Format("{0:yyyyMMdd}", item.AllowanceDate),
                CancelDate = String.Format("{0:yyyyMMdd}", cancelItem.CancelDate),
                CancelTime = cancelItem.CancelDate.Value,
                CancelAllowanceNumber = item.AllowanceNumber,
                Remark = cancelItem.Remark,
                BuyerId = item.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.InvoiceAllowanceSeller.ReceiptNo,
                CancelReason = cancelItem.Remark
            };

            return result;
        }

        public static Model.Schema.TurnKey.B0501.CancelAllowance CreateB0501(this InvoiceAllowance item)
        {
            InvoiceAllowanceCancellation cancelItem = item.InvoiceAllowanceCancellation;

            if (cancelItem == null)
                return null;

            var result = new Model.Schema.TurnKey.B0501.CancelAllowance
            {
                AllowanceDate = String.Format("{0:yyyyMMdd}", item.AllowanceDate),
                CancelDate = String.Format("{0:yyyyMMdd}", cancelItem.CancelDate),
                CancelTime = cancelItem.CancelDate.Value,
                CancelAllowanceNumber = item.AllowanceNumber,
                Remark = cancelItem.Remark,
                BuyerId = item.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.InvoiceAllowanceSeller.ReceiptNo,
                CancelReason = cancelItem.Remark
            };

            return result;
        }

        public static Model.Schema.TurnKey.A0401.Invoice CreateA0401(this InvoiceItem item)
        {
            var result = new Model.Schema.TurnKey.A0401.Invoice
            {
                Main = new Schema.TurnKey.A0401.Main
                {
                    Buyer = new Schema.TurnKey.A0401.MainBuyer
                    {
                        Address = item.InvoiceBuyer.Address,
                        CustomerNumber = item.InvoiceBuyer.CustomerID,
                        EmailAddress = item.InvoiceBuyer.EMail,
                        FacsimileNumber = item.InvoiceBuyer.Fax,
                        Identifier = item.InvoiceBuyer.ReceiptNo,
                        Name = item.InvoiceBuyer.Name,
                        PersonInCharge = item.InvoiceBuyer.PersonInCharge,
                        RoleRemark = item.InvoiceBuyer.RoleRemark,
                        TelephoneNumber = item.InvoiceBuyer.Phone
                    },
                    BuyerRemark = String.IsNullOrEmpty(item.BuyerRemark) ? Model.Schema.TurnKey.A0401.MainBuyerRemark.Item1 : (Model.Schema.TurnKey.A0401.MainBuyerRemark)int.Parse(item.BuyerRemark),
                    BuyerRemarkSpecified = !String.IsNullOrEmpty(item.BuyerRemark),
                    Category = item.Category,
                    CheckNumber = item.CheckNo,
                    CustomsClearanceMark = String.IsNullOrEmpty(item.CustomsClearanceMark) ? Model.Schema.TurnKey.A0401.CustomsClearanceMarkEnum.Item1 : (Model.Schema.TurnKey.A0401.CustomsClearanceMarkEnum)int.Parse(item.CustomsClearanceMark),
                    CustomsClearanceMarkSpecified = !String.IsNullOrEmpty(item.CustomsClearanceMark),
                    InvoiceType = (Schema.TurnKey.A0401.InvoiceTypeEnum)((int)item.InvoiceType),
                    //DonateMark = (Schema.TurnKey.A1401.DonateMarkEnum)(int.Parse(item.DonateMark)),
                    DonateMark = string.IsNullOrEmpty(item.DonateMark) ? Model.Schema.TurnKey.A0401.DonateMarkEnum.Item0 : (Schema.TurnKey.A0401.DonateMarkEnum)(int.Parse(item.DonateMark)),
                    GroupMark = item.GroupMark,
                    InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
                    InvoiceTimeSpecified = false,
                    InvoiceNumber = String.Format("{0}{1}", item.TrackCode, item.No),
                    MainRemark = item.Remark,
                    PermitNumber = item.PermitNumber,
                    PermitDate = item.PermitDate.HasValue ? String.Format("{0:yyyyMMdd}", item.PermitDate.Value) : null,
                    PermitWord = item.PermitWord,
                    RelateNumber = item.RelateNumber,
                    TaxCenter = item.TaxCenter,
                    Seller = new Schema.TurnKey.A0401.MainSeller
                    {
                        Address = item.InvoiceSeller.Address,
                        CustomerNumber = item.InvoiceSeller.CustomerID,
                        EmailAddress = item.InvoiceSeller.EMail,
                        FacsimileNumber = item.InvoiceSeller.Fax,
                        Identifier = item.InvoiceSeller.ReceiptNo,
                        Name = item.InvoiceSeller.Name,
                        PersonInCharge = item.InvoiceSeller.PersonInCharge,
                        RoleRemark = item.InvoiceSeller.RoleRemark,
                        TelephoneNumber = item.InvoiceSeller.Phone
                    }
                },
                Details = buildA0401Details(item),
                Amount = new Schema.TurnKey.A0401.Amount
                {
                    CurrencySpecified = false,
                    DiscountAmount = item.InvoiceAmountType.DiscountAmount.HasValue ? (long)item.InvoiceAmountType.DiscountAmount.Value : 0,
                    DiscountAmountSpecified = item.InvoiceAmountType.DiscountAmount.HasValue,
                    ExchangeRateSpecified = false,
                    OriginalCurrencyAmountSpecified = false,
                    SalesAmount = item.InvoiceAmountType.SalesAmount.HasValue ? (long)item.InvoiceAmountType.SalesAmount.Value : 0,
                    TaxAmount = item.InvoiceAmountType.TaxAmount.HasValue ? (long)item.InvoiceAmountType.TaxAmount.Value : 0,
                    TaxRate = item.InvoiceAmountType.TaxRate.HasValue ? item.InvoiceAmountType.TaxRate.Value : 0.05m,
                    TaxType = (Schema.TurnKey.A0401.TaxTypeEnum)((int)item.InvoiceAmountType.TaxType.Value),
                    TotalAmount = item.InvoiceAmountType.TotalAmount.HasValue ? (long)item.InvoiceAmountType.TotalAmount : 0
                }
            };

            return result;
        }

        private static Schema.TurnKey.A0401.DetailsProductItem[] buildA0401Details(InvoiceItem item)
        {
            List<Model.Schema.TurnKey.A0401.DetailsProductItem> items = new List<Schema.TurnKey.A0401.DetailsProductItem>();
            foreach (var detailItem in item.InvoiceDetails)
            {
                foreach (var productItem in detailItem.InvoiceProduct.InvoiceProductItem)
                {
                    items.Add(new Model.Schema.TurnKey.A0401.DetailsProductItem
                    {
                        Amount = productItem.CostAmount.HasValue ? productItem.CostAmount.Value : 0m,
                        Description = detailItem.InvoiceProduct.Brief,
                        Quantity = productItem.Piece.HasValue ? productItem.Piece.Value : 0,
                        RelateNumber = productItem.RelateNumber,
                        Remark = productItem.Remark,
                        SequenceNumber = String.Format("{0:00}", productItem.No),
                        Unit = productItem.PieceUnit,
                        UnitPrice = productItem.UnitCost.HasValue ? productItem.UnitCost.Value : 0,
                    });
                }
            }
            return items.ToArray();
        }

        public static Model.Schema.TurnKey.B0401.Allowance CreateB0401(this InvoiceAllowance item)
        {
            var result = new Model.Schema.TurnKey.B0401.Allowance
            {
                Main = new Schema.TurnKey.B0401.Main
                {
                    AllowanceDate = String.Format("{0:yyyyMMdd}", item.AllowanceDate),
                    AllowanceNumber = item.AllowanceNumber,
                    AllowanceType = (Model.Schema.TurnKey.B0401.AllowanceTypeEnum)item.AllowanceType,
                    Buyer = new Schema.TurnKey.B0401.MainBuyer
                    {
                        Address = item.InvoiceAllowanceBuyer.Address,
                        CustomerNumber = item.InvoiceAllowanceBuyer.CustomerID,
                        EmailAddress = item.InvoiceAllowanceBuyer.EMail,
                        FacsimileNumber = item.InvoiceAllowanceBuyer.Fax,
                        Identifier = item.InvoiceAllowanceBuyer.ReceiptNo,
                        Name = item.InvoiceAllowanceBuyer.Name,
                        PersonInCharge = item.InvoiceAllowanceBuyer.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceBuyer.Phone,
                        RoleRemark = item.InvoiceAllowanceBuyer.RoleRemark
                    },
                    Seller = new Schema.TurnKey.B0401.MainSeller
                    {
                        Address = item.InvoiceAllowanceSeller.Address,
                        CustomerNumber = item.InvoiceAllowanceSeller.CustomerID,
                        EmailAddress = item.InvoiceAllowanceSeller.EMail,
                        FacsimileNumber = item.InvoiceAllowanceSeller.Fax,
                        Identifier = item.InvoiceAllowanceSeller.ReceiptNo,
                        Name = item.InvoiceAllowanceSeller.Name,
                        PersonInCharge = item.InvoiceAllowanceSeller.PersonInCharge,
                        TelephoneNumber = item.InvoiceAllowanceSeller.Phone,
                        RoleRemark = item.InvoiceAllowanceSeller.RoleRemark
                    }
                },
                Amount = new Model.Schema.TurnKey.B0401.Amount
                {
                    TaxAmount = item.TaxAmount.HasValue ? (long)item.TaxAmount.Value : 0,
                    TotalAmount = item.TotalAmount.HasValue ? (long)item.TotalAmount.Value : 0
                }
            };

            result.Details = item.InvoiceAllowanceDetails.Select(d => new Schema.TurnKey.B0401.DetailsProductItem
            {
                AllowanceSequenceNumber = d.InvoiceAllowanceItem.No.ToString(),
                Amount = d.InvoiceAllowanceItem.Amount.HasValue ? d.InvoiceAllowanceItem.Amount.Value : 0m,
                OriginalSequenceNumber = d.InvoiceAllowanceItem.OriginalSequenceNo.HasValue ? d.InvoiceAllowanceItem.OriginalSequenceNo.Value.ToString() : "1",
                OriginalInvoiceDate = String.Format("{0:yyyyMMdd}", d.InvoiceAllowanceItem.InvoiceDate),
                OriginalDescription = d.InvoiceAllowanceItem.OriginalDescription,
                OriginalInvoiceNumber = d.InvoiceAllowanceItem.InvoiceNo,
                Quantity = d.InvoiceAllowanceItem.Piece.HasValue ? d.InvoiceAllowanceItem.Piece.Value : 0.00000M,
                Tax = (long)d.InvoiceAllowanceItem.Tax.Value,
                TaxType = (Schema.TurnKey.B0401.DetailsProductItemTaxType)d.InvoiceAllowanceItem.TaxType,
                Unit = d.InvoiceAllowanceItem.PieceUnit,
                UnitPrice = d.InvoiceAllowanceItem.UnitCost.HasValue ? d.InvoiceAllowanceItem.UnitCost.Value : 0
            }).ToArray();

            return result;
        }

        public static Model.Schema.EIVO.B2B.NotReceivedInvoiceRoot CreateNotReceivedInvoiceRoot(this IEnumerable<InvoiceItem> items)
        {
            var result = new Model.Schema.EIVO.B2B.NotReceivedInvoiceRoot
            {
                NotReceivedInvoice = buildNotReceivedInvoice(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.NotReceivedInvoiceRootNotReceivedInvoice[] buildNotReceivedInvoice(IEnumerable<InvoiceItem> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.NotReceivedInvoiceRootNotReceivedInvoice
            {
                Number = String.Format("{0}{1}", item.TrackCode, item.No),
                Date = String.Format("{0:yyyy/MM/dd}", item.InvoiceDate),
                BuyerId = item.InvoiceBuyer.ReceiptNo,
                SellerId = item.InvoiceSeller.ReceiptNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.NotReceivedCancelInvoiceRoot CreateNotReceivedCancelInvoiceRoot(this IEnumerable<CDS_Document> items)
        {
            var result = new Model.Schema.EIVO.B2B.NotReceivedCancelInvoiceRoot
            {
                NotReceivedCancelInvoice = buildNotReceivedInvoiceCancellation(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.NotReceivedCancelInvoiceRootNotReceivedCancelInvoice[] buildNotReceivedInvoiceCancellation(IEnumerable<CDS_Document> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.NotReceivedCancelInvoiceRootNotReceivedCancelInvoice
            {
                Number = String.Format("{0}{1}", item.DerivedDocument.ParentDocument.InvoiceItem.TrackCode, item.DerivedDocument.ParentDocument.InvoiceItem.No),
                Date = String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceCancellation.CancelDate),
                BuyerId = item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.ReceiptNo,
                SellerId = item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.ReceiptNo
            }).ToArray();
        }


        public static Model.Schema.EIVO.B2B.NotReceivedAllowanceRoot CreateNotReceivedAllowanceRoot(this IEnumerable<InvoiceAllowance> items)
        {
            var result = new Model.Schema.EIVO.B2B.NotReceivedAllowanceRoot
            {
                NotReceivedAllowance = buildNotReceivedAllowance(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.NotReceivedAllowanceRootNotReceivedAllowance[] buildNotReceivedAllowance(IEnumerable<InvoiceAllowance> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.NotReceivedAllowanceRootNotReceivedAllowance
            {
                Number = item.AllowanceNumber,
                Date = String.Format("{0:yyyy/MM/dd}", item.AllowanceDate),
                BuyerId = item.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.InvoiceAllowanceSeller.ReceiptNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.NotReceivedCancelAllowanceRoot CreateNotReceivedCancelAllowanceRoot(this IEnumerable<CDS_Document> items)
        {
            var result = new Model.Schema.EIVO.B2B.NotReceivedCancelAllowanceRoot
            {
                NotReceivedCancelAllowance = buildNotReceivedAllowanceCancellation(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.NotReceivedCancelAllowanceRootNotReceivedCancelAllowance[] buildNotReceivedAllowanceCancellation(IEnumerable<CDS_Document> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.NotReceivedCancelAllowanceRootNotReceivedCancelAllowance
            {
                Number = item.DerivedDocument.ParentDocument.InvoiceAllowance.AllowanceNumber,
                Date = String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate),
                BuyerId = item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
            }).ToArray();
        }


        public static Model.Schema.EIVO.B2B.ReturnInvoiceRoot CreateReturnInvoiceRoot(this IEnumerable<InvoiceItem> items)
        {
            var result = new Model.Schema.EIVO.B2B.ReturnInvoiceRoot
            {
                ReturnInvoice = buildReturnInvoiceRoot(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.ReturnInvoiceRootReturnInvoice[] buildReturnInvoiceRoot(IEnumerable<InvoiceItem> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.ReturnInvoiceRootReturnInvoice
            {
                Number = String.Format("{0}{1}", item.TrackCode, item.No),
                Date = String.Format("{0:yyyy/MM/dd}", item.InvoiceDate),
                BuyerId = item.InvoiceBuyer.ReceiptNo,
                SellerId = item.InvoiceSeller.ReceiptNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.ReturnCancelInvoiceRoot CreateReturnCancelInvoiceRoot(this IEnumerable<CDS_Document> items)
        {
            var result = new Model.Schema.EIVO.B2B.ReturnCancelInvoiceRoot
            {
                ReturnCancelInvoice = buildReturnInvoiceCancellation(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.ReturnCancelInvoiceRootReturnCancelInvoice[] buildReturnInvoiceCancellation(IEnumerable<CDS_Document> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.ReturnCancelInvoiceRootReturnCancelInvoice
            {
                Number = String.Format("{0}{1}", item.DerivedDocument.ParentDocument.InvoiceItem.TrackCode, item.DerivedDocument.ParentDocument.InvoiceItem.No),
                Date = String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceCancellation.CancelDate),
                BuyerId = item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.ReceiptNo,
                SellerId = item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceSeller.ReceiptNo
            }).ToArray();
        }


        public static Model.Schema.EIVO.B2B.ReturnAllowanceRoot CreateReturnAllowanceRoot(this IEnumerable<InvoiceAllowance> items)
        {
            var result = new Model.Schema.EIVO.B2B.ReturnAllowanceRoot
            {
                ReturnAllowance = buildReturnAllowance(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.ReturnAllowanceRootReturnAllowance[] buildReturnAllowance(IEnumerable<InvoiceAllowance> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.ReturnAllowanceRootReturnAllowance
            {
                Number = item.AllowanceNumber,
                Date = String.Format("{0:yyyy/MM/dd}", item.AllowanceDate),
                BuyerId = item.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.InvoiceAllowanceSeller.ReceiptNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.ReturnCancelAllowanceRoot CreateReturnCancelAllowanceRoot(this IEnumerable<CDS_Document> items)
        {
            var result = new Model.Schema.EIVO.B2B.ReturnCancelAllowanceRoot
            {
                ReturnCancelAllowance = buildReturnAllowanceCancellation(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.ReturnCancelAllowanceRootReturnCancelAllowance[] buildReturnAllowanceCancellation(IEnumerable<CDS_Document> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.ReturnCancelAllowanceRootReturnCancelAllowance
            {
                Number = item.DerivedDocument.ParentDocument.InvoiceAllowance.AllowanceNumber,
                Date = String.Format("{0:yyyy/MM/dd}", item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceCancellation.CancelDate),
                BuyerId = item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.SellerInvoiceRoot CreateSellerInvoiceRoot(this IEnumerable<InvoiceItem> items)
        {
            var result = new Model.Schema.EIVO.B2B.SellerInvoiceRoot
            {
                Invoice = buildSellerInvoiceRootInvoices(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.SellerInvoiceRootInvoice[] buildSellerInvoiceRootInvoices(IEnumerable<InvoiceItem> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.SellerInvoiceRootInvoice
                {
                    BuyerId = item.InvoiceBuyer.ReceiptNo,
                    BuyerName = item.InvoiceBuyer.Name,
                    SellerId = item.InvoiceSeller.ReceiptNo,
                    InvoiceTime = "",
                    InvoiceType = String.Format("{0:00}", item.InvoiceType.Value),
                    InvoiceDate = String.Format("{0:yyyy/MM/dd}", item.InvoiceDate),
                    InvoiceNumber = String.Format("{0}{1}", item.TrackCode, item.No),
                    InvoiceItem = buildSellerInvoiceRootDetails(item),
                    //DiscountAmount = item.InvoiceAmountType.DiscountAmount.HasValue ? item.InvoiceAmountType.DiscountAmount.Value : 0,
                    SalesAmount = item.InvoiceAmountType.SalesAmount.HasValue ? Decimal.Parse(String.Format("{0:0}", item.InvoiceAmountType.SalesAmount.Value)) : 0,
                    TaxAmount = item.InvoiceAmountType.TaxAmount.HasValue ? Decimal.Parse(String.Format("{0:0}", item.InvoiceAmountType.TaxAmount.Value)) : 0,
                    TaxType = item.InvoiceAmountType.TaxType.Value,
                    TotalAmount = item.InvoiceAmountType.TotalAmount.HasValue ? Decimal.Parse(String.Format("{0:0}", item.InvoiceAmountType.TotalAmount.Value)) : 0,
                    ExtraRemark = buildSellerInvoiceExtraRemark(item)
                }).ToArray();
        }

        private static XmlNode[] buildSellerInvoiceExtraRemark(InvoiceItem item)
        { 
            if(item.InvoiceItemExtension != null )
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(item.InvoiceItemExtension.ExtraRemark.ToString());
                return doc.DocumentElement.ChildNodes.Cast<XmlNode>().ToArray();
            }
            return null;
        }

        private static Schema.EIVO.B2B.SellerInvoiceRootInvoiceInvoiceItem[] buildSellerInvoiceRootDetails(InvoiceItem item)
        {
            List<Model.Schema.EIVO.B2B.SellerInvoiceRootInvoiceInvoiceItem> items = new List<Schema.EIVO.B2B.SellerInvoiceRootInvoiceInvoiceItem>();
            foreach (var detailItem in item.InvoiceDetails)
            {
                foreach (var productItem in detailItem.InvoiceProduct.InvoiceProductItem)
                {
                    items.Add(new Model.Schema.EIVO.B2B.SellerInvoiceRootInvoiceInvoiceItem
                    {
                        Amount = productItem.CostAmount.HasValue ?  Decimal.Parse( String.Format("{0:0.0000}", productItem.CostAmount.Value)) : 0m,
                        Amount2 = productItem.CostAmount2.HasValue ?  Decimal.Parse( String.Format("{0:0.0000}", productItem.CostAmount2.Value)) : 0m,
                        Description = detailItem.InvoiceProduct.Brief,
                        Quantity = productItem.Piece.HasValue ? Decimal.Parse( String.Format("{0:0.0000}", productItem.Piece.Value)) : 0,
                        Quantity2 = productItem.Piece2.HasValue ? Decimal.Parse( String.Format("{0:0.0000}", productItem.Piece2.Value)) : 0,
                        Remark = productItem.Remark,
                        SequenceNumber = decimal .Parse ( String.Format("{0:00}", productItem.No))+1,
                        Unit = productItem.PieceUnit,
                        Unit2 = productItem.PieceUnit2,
                        UnitPrice = productItem.UnitCost.HasValue ?  Decimal.Parse( String.Format("{0:0.0000}", productItem.UnitCost.Value)) : 0,
                        UnitPrice2 = productItem.UnitCost2.HasValue ?  Decimal.Parse( String.Format("{0:0.0000}", productItem.UnitCost2.Value)) : 0
                    });
                }
            }
            return items.ToArray();
        }

        public static Model.Schema.EIVO.B2B.CancelInvoiceRoot CreateCancelInvoiceRoot(this IEnumerable<CDS_Document> items)
        {
            return new Model.Schema.EIVO.B2B.CancelInvoiceRoot
            {
                CancelInvoice = buildCancelInvoiceRootCancelInvoices(items.Select(d=>d.DerivedDocument.ParentDocument.InvoiceItem))
            };
        }

        private static Schema.EIVO.B2B.CancelInvoiceRootCancelInvoice[] buildCancelInvoiceRootCancelInvoices(IEnumerable<InvoiceItem> items)
        {
            return items.Select(i => new Model.Schema.EIVO.B2B.CancelInvoiceRootCancelInvoice
            {

                BuyerId = i.InvoiceBuyer.ReceiptNo,
                SellerId = i.InvoiceSeller.ReceiptNo,
                InvoiceDate = String.Format("{0:yyyy/MM/dd}", i.InvoiceDate),
                CancelDate = String.Format("{0:yyyy/MM/dd}", i.InvoiceCancellation.CancelDate),
                CancelInvoiceNumber = i.InvoiceCancellation.CancellationNo,
                CancelTime = String.Format("{0:HH:mm:ss}", i.InvoiceCancellation.CancelDate),
                CancelReason = String.Concat(i.InvoiceCancellation.CancelReason, i.InvoiceCancellation.Remark),
                //Remark = i.InvoiceCancellation.Remark,
                Remark = "",
                ReturnTaxDocumentNumber = i.InvoiceCancellation.ReturnTaxDocumentNo
            }).ToArray();
        }

        public static Model.Schema.EIVO.B2B.AllowanceRoot CreateAllowanceRoot(this IEnumerable<InvoiceAllowance> items)
        {
            var result = new Model.Schema.EIVO.B2B.AllowanceRoot
            {
                Allowance = buildAllowanceRootAllowance(items)
            };

            return result;
        }

        private static Schema.EIVO.B2B.AllowanceRootAllowance[] buildAllowanceRootAllowance(IEnumerable<InvoiceAllowance> items)
        {
            return items.Select(a => new Model.Schema.EIVO.B2B.AllowanceRootAllowance
            {
                BuyerId = a.InvoiceAllowanceBuyer.ReceiptNo,
                BuyerName = a.InvoiceAllowanceBuyer.CustomerName,
                SellerId = a.InvoiceAllowanceSeller.ReceiptNo,
                SellerName = a.InvoiceAllowanceSeller.CustomerName,
                AllowanceItem = a.InvoiceAllowanceDetails.Select(d => new Schema.EIVO.B2B.AllowanceRootAllowanceAllowanceItem
                {
                    AllowanceSequenceNumber = d.InvoiceAllowanceItem.No.ToString(),
                    Amount = d.InvoiceAllowanceItem.Amount.HasValue ? d.InvoiceAllowanceItem.Amount.Value : 0m,
                    Amount2 = d.InvoiceAllowanceItem.Amount2.HasValue ? d.InvoiceAllowanceItem.Amount2.Value : 0m,
                    OriginalInvoiceDate = a.InvoiceItem == null ? String.Format("{0:yyyy/MM/dd}", d.InvoiceAllowanceItem.InvoiceDate) : String.Format("{0:yyyy/MM/dd}", a.InvoiceItem.InvoiceDate),
                    OriginalDescription = d.InvoiceAllowanceItem.OriginalDescription,
                    OriginalInvoiceNumber = a.InvoiceItem == null ? d.InvoiceAllowanceItem.InvoiceNo : String.Format("{0}{1}", a.InvoiceItem.TrackCode, a.InvoiceItem.No),
                    Quantity = d.InvoiceAllowanceItem.Piece.HasValue ? d.InvoiceAllowanceItem.Piece.Value : 0.00000M,
                    Quantity2 = d.InvoiceAllowanceItem.Piece2.HasValue ? d.InvoiceAllowanceItem.Piece2.Value : 0.00000M,
                    Tax = (long)d.InvoiceAllowanceItem.Tax.Value,
                    TaxType = d.InvoiceAllowanceItem.TaxType.Value,
                    Unit = d.InvoiceAllowanceItem.PieceUnit,
                    UnitPrice = d.InvoiceAllowanceItem.UnitCost.HasValue ? d.InvoiceAllowanceItem.UnitCost.Value : 0,
                    Unit2 = d.InvoiceAllowanceItem.PieceUnit2,
                    UnitPrice2 = d.InvoiceAllowanceItem.UnitCost2.HasValue ? d.InvoiceAllowanceItem.UnitCost2.Value : 0,
                    Remark = d.InvoiceAllowanceItem.Remark,
                    OriginalSequenceNumber = d.InvoiceAllowanceItem.OriginalSequenceNo.HasValue ? d.InvoiceAllowanceItem.OriginalSequenceNo.Value.ToString() : "",
                }).ToArray(),
                AllowanceDate = String.Format("{0:yyyy/MM/dd}", a.AllowanceDate),
                TaxAmount = a.TaxAmount.HasValue ? a.TaxAmount.Value : 0,
                AllowanceNumber = a.AllowanceNumber,
                TotalAmount = a.TotalAmount.HasValue ? a.TotalAmount.Value : 0,
                AllowanceType = a.AllowanceType.Value,
                ExtraRemark = buildAllowanceExtraRemark(a)
            }).ToArray();
        }

        private static XmlNode[] buildAllowanceExtraRemark(InvoiceAllowance item)
        {
            if (item.InvoiceAllowanceItemExtension != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(item.InvoiceAllowanceItemExtension.ExtraRemark.ToString());
                return doc.DocumentElement.ChildNodes.Cast<XmlNode>().ToArray();
            }
            return null;
        }

        public static Model.Schema.MIG3_1.E0402.BranchTrackBlank BuildE0402(this InvoiceTrackCodeAssignment item, IQueryable<UnassignedInvoiceNoSummary> Summary)
        {
            var result = new Model.Schema.MIG3_1.E0402.BranchTrackBlank
            {
                Main = new Schema.MIG3_1.E0402.Main
                {
                    HeadBan = item.Organization.ReceiptNo,
                    BranchBan = item.Organization.ReceiptNo,
                    InvoiceType = (Schema.MIG3_1.E0402.InvoiceTypeEnum)(5),
                    YearMonth = String.Format("{0:000}{1:00}", item.InvoiceTrackCode.Year - 1911, item.InvoiceTrackCode.PeriodNo * 2),
                    InvoiceTrack = item.InvoiceTrackCode.TrackCode
                },
                Details = buildE0402Details(item, Summary)
            };
            return result;
        }

        private static Schema.MIG3_1.E0402.DetailsBranchTrackBlankItem[] buildE0402Details(InvoiceTrackCodeAssignment item, IQueryable<UnassignedInvoiceNoSummary> Summary)
        {
            List<Model.Schema.MIG3_1.E0402.DetailsBranchTrackBlankItem> items = new List<Schema.MIG3_1.E0402.DetailsBranchTrackBlankItem>();

            foreach (var detail in Summary.ToList())
            {
                items.Add(new Model.Schema.MIG3_1.E0402.DetailsBranchTrackBlankItem
                {
                    InvoiceBeginNo = String.Format("{0:00000000}", detail.StartNo),
                    InvoiceEndNo = String.Format("{0:00000000}", detail.EndNo)
                }
                );
            }
            return items.ToArray();
        }


        public static Model.Schema.EIVO.B2B.CancelAllowanceRoot CreateCancelAllowanceRoot(this IEnumerable<CDS_Document> items)
        {
            return new Model.Schema.EIVO.B2B.CancelAllowanceRoot
            {
                 CancelAllowance = buildCancelAllowanceRoot(items.Select(d=>d.DerivedDocument.ParentDocument.InvoiceAllowance))
            };
        }

        private static Schema.EIVO.B2B.CancelAllowanceRootCancelAllowance[] buildCancelAllowanceRoot(IEnumerable<InvoiceAllowance> items)
        {
            return items.Select(item => new Model.Schema.EIVO.B2B.CancelAllowanceRootCancelAllowance
            {
                BuyerId = item.InvoiceAllowanceBuyer.ReceiptNo,
                SellerId = item.InvoiceAllowanceSeller.ReceiptNo,
                AllowanceDate = String.Format("{0:yyyy/MM/dd}", item.AllowanceDate),
                CancelDate = String.Format("{0:yyyy/MM/dd}", item.InvoiceAllowanceCancellation.CancelDate),
                CancelAllowanceNumber = item.AllowanceNumber,
                CancelTime = String.Format("{0:HH:mm:ss}", item.InvoiceAllowanceCancellation.CancelDate),
                CancelReason = item.InvoiceAllowanceCancellation.Remark,
                //Remark = item.InvoiceAllowanceCancellation.Remark
                Remark = ""
            }).ToArray();
        }

    }
}
