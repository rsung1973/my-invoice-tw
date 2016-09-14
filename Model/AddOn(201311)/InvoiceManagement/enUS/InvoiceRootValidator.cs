using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Schema.EIVO;
using Utility;

namespace Model.InvoiceManagement.enUS
{
    public static partial class InvoiceRootValidator
    {
        public static string RegularExpressions = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public static Exception CheckBusiness(this InvoiceRootInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out Organization seller)
        {
            seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("Seller ID does not exist, Seller ID: {0}, Incorrect TAG:< SellerId />", invItem.SellerId));
            }

            if (String.IsNullOrEmpty(invItem.GoogleId))
            {
                return new Exception(String.Format("GoogleId can not be blank，TAG:< GoogleId />"));
            }
            else if (invItem.GoogleId.Length > 64)
            {
                return new Exception(String.Format("GoogleId can not be over 64 characters，TAG:< GoogleId />"));
            }

            if (seller.CompanyID != owner.CompanyID)
            {
                return new Exception(String.Format("Cert ID and Seller ID does not match, Seller ID: {0}, Incorrect TAG:< SellerId />", invItem.SellerId));
            }

            if (invItem.BuyerId == "0000000000")
            {
                //if (invItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(invItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format("B2C buyer name format errors, four yards in length ASCII characters or Chinese full-width characters 2 yards，Incorrect：{0}，TAG:< BuyerName />", invItem.BuyerName));
                //}
            }
            else if (invItem.BuyerId == null || !Regex.IsMatch(invItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("Format of Buyer ID error, Incorrect Buyer ID: {0}, Incorrect TAG:< BuyerId />", invItem.BuyerId));
            }
            else if (invItem.BuyerName.Length > 60)
            {
                return new Exception(String.Format("Format of Buyer Name error. The maximum length is 60 digitals, Incorrect Buyer Name: {0}, Incorrect TAG:< BuyerName />", invItem.BuyerName));
            }

            if (String.IsNullOrEmpty(invItem.RandomNumber))
            {
                invItem.RandomNumber = String.Format("{0:ffff}", DateTime.Now);
            }
            else if (!Regex.IsMatch(invItem.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format("Random should be 4 arabic numbers, Incorrect Random Number: {0}, Incorrect TAG:< RandomNumber />", invItem.RandomNumber));
            }


            return null;
        }

        public static Exception CheckDataNumber(this InvoiceRootInvoice invItem, Organization seller, GenericManager<EIVOEntityDataContext> mgr, out InvoicePurchaseOrder order)
        {
            order = null;
            if (String.IsNullOrEmpty(invItem.DataNumber))
            {
                return new Exception(String.Format("Number can not be blank，TAG:< DataNumber />"));
            }

            if (invItem.DataNumber.Length > 60)
            {
                return new Exception(String.Format("Maximum length of DataNumber is 60 digitals; Incorrect Data Number:{0}, Incorrect TAG: < DataNumber />", invItem.DataNumber));
            }

             bool InvOrder = mgr.GetTable<InvoicePurchaseOrder>().Any(d => d.OrderNo == invItem.DataNumber);
             if (InvOrder)
             {
                 var InvOrderData = mgr.GetTable<InvoicePurchaseOrder>().Where(d => d.OrderNo == invItem.DataNumber);
                 var InvData = mgr.GetTable<InvoiceItem>().Where(i => i.SellerID == seller.CompanyID).Join(InvOrderData, i => i.InvoiceID, o => o.InvoiceID, (i, o) => i);

                 if (InvData != null && InvData.Count() > 0)
                 {
                     return new Exception(String.Format("Duplicated DataNumber; Incorrect Data Number:{0}, Incorrect TAG: < DataNumber />", invItem.DataNumber));
                 }
             }

            if (String.IsNullOrEmpty(invItem.DataDate))
            {
                return new Exception("Data Date can not be blank，TAG:< DataDate />");
            }

            DateTime dataDate;
            if (!DateTime.TryParseExact(invItem.DataDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataDate))
            {
                return new Exception(String.Format("Format of Data Date error(YYYY/MM/DD), Incorrect TAG:< DataDate />", invItem.DataDate));
            }

            order = new InvoicePurchaseOrder
            {
                OrderNo = invItem.DataNumber,
                PurchaseDate = dataDate
            };

            return null;
        }

        //public static Exception CheckMandatoryFields(this InvoiceRootInvoice invItem)
        //{
        //    if (String.IsNullOrEmpty(invItem.SellerId))
        //    {
        //        return new Exception("SellerId can not be blank，TAG:< SellerId />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.BuyerName))
        //    {
        //        return new Exception("BuyerName can not be blank，TAG:< BuyerName />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.BuyerId))
        //    {
        //        return new Exception("BuyerId can not be blank，TAG:< BuyerId />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.InvoiceType))
        //    {
        //        return new Exception("InvoiceType can not be blank，TAG:< InvoiceType />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.DonateMark))
        //    {
        //        return new Exception("DonateMark can not be blank，TAG:< DonateMark />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.PrintMark))
        //    {
        //        return new Exception("PrintMark can not be blank，TAG:< PrintMark />");
        //    }

        //    if (!Model.InvoiceManagement.zhTW.InvoiceRootValidator.__InvoiceTypeList.Contains(invItem.InvoiceType))
        //    {
        //        return new Exception(String.Format("Format of Invoice Type error, please fill in correct code 01-06, Incorrect Invoice Type: {0}, Incorrect TAG:< InvoiceType />", invItem.InvoiceType));
        //    }

        //    if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.TaxType))
        //    {
        //        return new Exception("Tax Type error, Incorrect TAG:< TaxType />");
        //    }

        //    if (invItem.Contact == null)
        //    {
        //        return new Exception("Contact error, Incorrect TAG:< Contact />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
        //    {
        //        return new Exception("Contact Name error, Incorrect TAG:< Name />");
        //    }

        //    if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
        //    {
        //        return new Exception("Contact Address error, Incorrect TAG:< Address />");
        //    }

        //    if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
        //    {
        //        return new Exception("Contact Email error, Incorrect TAG:< Email />");
        //    }

        //    if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
        //    {
        //        return new Exception("Contact telephone error, Incorrect TAG:< TEL />");
        //    }

        //    return null;
        //}

        public static Exception CheckAmount(this InvoiceRootInvoice invItem)
        {
            //應稅銷售額
            String strValue = String.Format("{0:0.}", invItem.SalesAmount);
            if (invItem.SalesAmount < 0 || strValue != invItem.SalesAmount.ToString())
            {
                return new Exception(String.Format("Sales Amount (NTD) should be positive and integer, Incorrect Sales Amount: {0}, Incorrect TAG:< SalesAmount />", invItem.SalesAmount));
            }


            strValue = String.Format("{0:0.}", invItem.FreeTaxSalesAmount);
            if (invItem.FreeTaxSalesAmount < 0 || strValue != invItem.FreeTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("Free Tax Sales Amount (NTD) should be positive and integer, Incorrect Free Tax Sales Amount: {0}, Incorrect TAG:< FreeTaxSalesAmount />", invItem.FreeTaxSalesAmount));
            }

            strValue = String.Format("{0:0.}", invItem.ZeroTaxSalesAmount);
            if (invItem.ZeroTaxSalesAmount < 0 || strValue != invItem.ZeroTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("Zero Tax Sales Amount (NTD) should be positive and integer, Incorrect Zero Tax Sales Amount: {0}, Incorrect TAG:< ZeroTaxSalesAmount />", invItem.ZeroTaxSalesAmount));
            }


            strValue = String.Format("{0:0.}", invItem.TaxAmount);
            if (invItem.TaxAmount < 0 || strValue != invItem.TaxAmount.ToString())
            {
                return new Exception(String.Format("Tax Amount should be positive and integer, Incorrect Tax Amount: {0}, Incorrect TAG:< TaxAmount />", invItem.TaxAmount));
            }

            strValue = String.Format("{0:0.}", invItem.TotalAmount);
            if (invItem.TotalAmount < 0 || strValue != invItem.TotalAmount.ToString())
            {
                return new Exception(String.Format("Total Amount should be positive and integer, Incorrect Total Amount: {0}, Incorrect TAG:< TotalAmount />", invItem.TotalAmount));
            }

            //課稅別
            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.TaxType))
            {
                return new Exception(String.Format("Tax Type error, Incorrect Tax Type: {0}, Incorrect TAG:< TaxType />", invItem.TaxType));
            }

            if (invItem.TaxRate != 0m && invItem.TaxRate != 0.05m)
            {
                return new Exception(String.Format("Tax Rate error, Incorrect Tax Rate: {0}, Incorrect TAG:< TaxRate />", invItem.TaxRate));
            }

            if (invItem.TaxType == (byte)Naming.TaxTypeDefinition.零稅率)
            {
                if (String.IsNullOrEmpty(invItem.CustomsClearanceMark))
                {
                    return new Exception(String.Format("Customs Clearance Mark should be filled for the Zero Tax e-GUI, Incorrect Customs Clearance Mark: {0}, Incorrect TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
                else if (invItem.CustomsClearanceMark != "1" && invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format("Customs Clearance Mark error, export not via Custom: 1, export via Custom: 2, Incorrect Customs Clearance Mark: {0}, Incorrect TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
            }
            else if (!String.IsNullOrEmpty(invItem.CustomsClearanceMark))
            {
                if (invItem.CustomsClearanceMark != "1" && invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format("Customs Clearance Mark error, export not via Custom: 1, export via Custom: 2, Incorrect Customs Clearance Mark: {0}, Incorrect TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
            }

            return null;
        }

        //public static Exception CheckInvoiceCarrier(this InvoiceRootInvoice invItem, Organization seller, out bool printed, out InvoiceCarrier carrier)
        //{
        //    carrier = null;
        //    printed = seller.OrganizationStatus.PrintAll == true || invItem.PrintMark == "Y" || invItem.PrintMark == "y";

        //    if (invItem.BuyerId == "0000000000")
        //    {
        //        if (printed)
        //        {
        //            if (!String.IsNullOrEmpty(invItem.CarrierType))
        //            {
        //                return new Exception(String.Format("Carrier Type should be blank for the printed e-GUI, Incorrect Carrier Type: {0}，Incorrect TAG:< CarrierType />", invItem.CarrierType));
        //            }
        //            if (!String.IsNullOrEmpty(invItem.CarrierId1))
        //            {
        //                return new Exception(String.Format("Carrier ID1 should be blank for the printed e-GUI, Incorrect Carrier Id1: {0}，Incorrect TAG:< CarrierId1 />", invItem.CarrierId1));
        //            }
        //            if (!String.IsNullOrEmpty(invItem.CarrierId2))
        //            {
        //                return new Exception(String.Format("Carrier ID2 should be blank for the printed e-GUI, Incorrect Carrier Id2: {0}，Incorrect TAG:< CarrierId2 />", invItem.CarrierId2));
        //            }
        //        }
        //        else
        //        {
        //            carrier = new InvoiceCarrier
        //            {
        //                CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "3J0001" : invItem.CarrierType,
        //                CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? System.Guid.NewGuid().ToString() : invItem.CarrierId1
        //            };

        //            if (String.IsNullOrEmpty(invItem.CarrierId2))
        //            {
        //                carrier.CarrierNo2 = carrier.CarrierNo;
        //            }
        //            else
        //            {
        //                carrier.CarrierNo2 = invItem.CarrierId2;
        //            }
        //        }
        //    }
        //    return null;
        //}

        //public static Exception CheckInvoiceDonation(this InvoiceRootInvoice invItem, bool printed, out InvoiceDonation donation)
        //{
        //    donation = null;
        //    if (invItem.DonateMark == "1")
        //    {
        //        if (printed)
        //        {
        //            return new Exception(String.Format("Donated e-GUI can not be marked to be printed, Incorrect Print Mark: {0}, Incorrect TAG:< PrintMark />", invItem.PrintMark));
        //        }

        //        if (String.IsNullOrEmpty(invItem.NPOBAN))
        //        {
        //            return new Exception(String.Format("Unassigned donate ID, Incorrect NPOBAN: {0}, Incorrect TAG:< NPOBAN />", invItem.NPOBAN));
        //        }
        //        donation = new InvoiceDonation
        //        {
        //            AgencyCode = invItem.NPOBAN
        //        };
        //    }
        //    else if (invItem.DonateMark == "0")
        //    {

        //    }
        //    else
        //    {
        //        return new Exception(String.Format("Donate Mark error, Incorrect Donate Mark: {0}, Incorrect TAG:< DonateMark />", invItem.DonateMark));
        //    }
        //    return null;
        //}

        public static Exception CheckInvoiceProductItems(this InvoiceRootInvoice invItem, out IEnumerable<InvoiceProductItem> productItems)
        {
            short seqNo = 0;
            productItems = invItem.InvoiceItem.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                ItemNo = i.Item,
                Piece = i.Quantity,
                PieceUnit = i.Unit,
                UnitCost = i.UnitPrice,
                Remark = i.Remark,
                TaxType = 1,
                No = (seqNo++)
            }).ToList();


            foreach (var product in productItems)
            {
                if (String.IsNullOrEmpty(product.InvoiceProduct.Brief) || product.InvoiceProduct.Brief.Length > 256)
                {
                    return new Exception(String.Format("Maximum length of Description should not exceed 256 digitals, Incorrect Description: {0}，Incorrect TAG:< Description />", product.InvoiceProduct.Brief));
                }


                if (!String.IsNullOrEmpty(product.PieceUnit) && product.PieceUnit.Length > 6)
                {
                    return new Exception(String.Format("Format of Unit error, Incorrect Unit: {0}，Incorrect TAG:< Unit />", product.PieceUnit));
                }


                if (!Regex.IsMatch(product.UnitCost.ToString(), RegularExpressions))
                {
                    return new Exception(String.Format("Format of Unit Price error, Incorrect Unit Price: {0}，Incorrect TAG:< UnitPrice />", product.UnitCost));
                }

               
                if (!Regex.IsMatch(product.CostAmount.ToString(), RegularExpressions))
                {
                    return new Exception(String.Format("Format of Amount error, Incorrect Amount: {0}, Incorrect TAG:< Amount />", product.CostAmount));
                }

            }
            return null;
        }
    }
}
