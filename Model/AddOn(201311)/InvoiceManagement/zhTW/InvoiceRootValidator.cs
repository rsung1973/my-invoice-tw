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
using Model.Properties;

namespace Model.InvoiceManagement.zhTW
{
    public static partial class InvoiceRootValidator
    {
        public readonly static String[] __InvoiceTypeList = { "01", "02", "03", "04", "05", "06", "07", "08" };
        public static string RegularExpressions = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public static Exception CheckInvoice(this InvoiceRootInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, out String trackCode, out String invNo, out DateTime invoiceDate)
        {
            trackCode = null;
            invNo = null;
            invoiceDate = default(DateTime);

            if (invItem.InvoiceNumber == null || !Regex.IsMatch(invItem.InvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
            {
                return new Exception(String.Format("發票號碼，傳送資料:{0}，TAG：< InvoicNumber />", invItem.InvoiceNumber));
            }

            if (String.IsNullOrEmpty(invItem.InvoiceDate))
            {
                return new Exception("發票日期，TAG：< InvoiceDate />");
            }

            if (String.IsNullOrEmpty(invItem.InvoiceTime))
            {
                return new Exception("發票時間，TAG：< InvoiceTime />");
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", invItem.InvoiceDate, invItem.InvoiceTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                return new Exception(String.Format("發票日期、發票時間格式錯誤(YYYY/MM/DD HH:mm:ss)；上傳資料:{0} {1}", invItem.InvoiceDate, invItem.InvoiceTime));
            }

            trackCode = invItem.InvoiceNumber.Substring(0, 2);
            invNo = invItem.InvoiceNumber.Substring(2);

            String chkTrackCode = trackCode;
            String chkInvNo = invNo;

            if (mgr.GetTable<InvoiceItem>().Any(i => i.TrackCode == chkTrackCode && i.No == chkInvNo))
            {
                return new Exception("發票號碼重複，已存在的發票資料。");
            }

            return null;
        }

        public static Exception CheckDataNumber(this InvoiceRootInvoice invItem, Organization seller, GenericManager<EIVOEntityDataContext> mgr, out InvoicePurchaseOrder order)
        {
            order = null;
            if (String.IsNullOrEmpty(invItem.DataNumber))
            {
                return new Exception("單據號碼錯誤，TAG:< DataNumber />");
            }

            if (invItem.DataNumber.Length > 60)
            {
                return new Exception(String.Format("單據號碼資料長度最長為60碼；傳送資料:{0}，TAG:< DataNumber />", invItem.DataNumber));
            }

            bool InvOrder = mgr.GetTable<InvoicePurchaseOrder>().Any(d => d.OrderNo == invItem.DataNumber);
            if (InvOrder)
            {
                var InvOrderData = mgr.GetTable<InvoicePurchaseOrder>().Where(d => d.OrderNo == invItem.DataNumber);
                var InvData = mgr.GetTable<InvoiceItem>().Where(i => i.SellerID == seller.CompanyID).Join(InvOrderData, i => i.InvoiceID, o => o.InvoiceID, (i, o) => i);

                if (InvData != null && InvData.Count() > 0)
                {
                    return new Exception(String.Format("單據號碼不可重複；傳送資料:{0}，TAG:< DataNumber />", invItem.DataNumber));
                }
            }

            if (String.IsNullOrEmpty(invItem.DataDate))
            {
                return new Exception("單據日期錯誤，TAG:< DataDate />");
            }

            DateTime dataDate;
            if (!DateTime.TryParseExact(invItem.DataDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataDate))
            {
                return new Exception(String.Format("單據日期格式錯誤(YYYY/MM/DD)；上傳資料:{0}", invItem.DataDate));
            }

            order = new InvoicePurchaseOrder
            {
                OrderNo = invItem.DataNumber,
                PurchaseDate = dataDate
            };

            return null;
        }

        public static Exception CheckBusiness(this InvoiceRootInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out Organization seller)
        {
            seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< SellerId />", invItem.SellerId));
            }

            if (seller.CompanyID != owner.CompanyID)
            {
                return new Exception(String.Format("簽章設定人與發票開立人不符,開立人統一編號:{0}，TAG:< SellerId />", invItem.SellerId));
            }

            if (invItem.BuyerId == "0000000000")
            {
                //if (invItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(invItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format("B2C買方名稱格式錯誤，長度為ASCII字元4碼或中文全形字元2碼，傳送資料：{0}，TAG:< BuyerName />", invItem.BuyerName));
                //}
            }
            else if (invItem.BuyerId == null || !Regex.IsMatch(invItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< BuyerId />", invItem.BuyerId));
            }
            else if (invItem.BuyerName.Length > 60)
            {
                return new Exception(String.Format("買方名稱格式錯誤，長度最多60碼，傳送資料：{0}，TAG:< BuyerName />", invItem.BuyerName));
            }

            if (String.IsNullOrEmpty(invItem.RandomNumber))
            {
                invItem.RandomNumber = String.Format("{0:ffff}", DateTime.Now);
            }
            else if (!Regex.IsMatch(invItem.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format("交易隨機碼應由4位數值構成，上傳資料：{0}，TAG:< RandomNumber />", invItem.RandomNumber));
            }

            return null;
        }

        public static Exception CheckBusiness_Proxy(this InvoiceRootInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out Organization seller)
        {
            seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< SellerId />", invItem.SellerId));
            }

            //上傳店家
            var Org = seller;
            //代理店家
            var proxyOrg = mgr.GetTable<Organization>().Where(o => o.CompanyID == owner.CompanyID).FirstOrDefault();
            //上傳店家是否有註冊在代理店家底下
            var Inv_Org = mgr.GetTable<InvoiceIssuerAgent>().Where(i => i.AgentID == proxyOrg.CompanyID && i.IssuerID == Org.CompanyID).FirstOrDefault();
            if (Inv_Org == null && proxyOrg.ReceiptNo != Org.ReceiptNo)
            {
                return new Exception(String.Format("上傳店家並無登記在此發票開立代理店家底下:{0}，TAG:< SellerId />；代理發票開立店家:{1}", invItem.SellerId, proxyOrg.ReceiptNo));
            }

            if (invItem.BuyerId == "0000000000")
            {
                //if (invItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(invItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format("B2C買方名稱格式錯誤，長度為ASCII字元4碼或中文全形字元2碼，傳送資料：{0}，TAG:< BuyerName />", invItem.BuyerName));
                //}
            }
            else if (invItem.BuyerId == null || !Regex.IsMatch(invItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< BuyerId />", invItem.BuyerId));
            }
            else if (invItem.BuyerName.Length > 60)
            {
                return new Exception(String.Format("買方名稱格式錯誤，長度最多60碼，傳送資料：{0}，TAG:< BuyerName />", invItem.BuyerName));
            }

            if (String.IsNullOrEmpty(invItem.RandomNumber))
            {
                invItem.RandomNumber = String.Format("{0:ffff}", DateTime.Now);
            }
            else if (!Regex.IsMatch(invItem.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format("交易隨機碼應由4位數值構成，上傳資料：{0}，TAG:< RandomNumber />", invItem.RandomNumber));
            }

            return null;
        }


        public static Exception CheckAmount(this InvoiceRootInvoice invItem)
        {
            //應稅銷售額
            String strValue = String.Format("{0:0.}", invItem.SalesAmount);
            if (invItem.SalesAmount < 0 || strValue != invItem.SalesAmount.ToString())
            {
                return new Exception(String.Format("應稅銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< SalesAmount />", invItem.SalesAmount));
            }

            strValue = String.Format("{0:0.}", invItem.FreeTaxSalesAmount);
            if (invItem.FreeTaxSalesAmount < 0 || strValue != invItem.FreeTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("免稅銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< FreeTaxSalesAmount />", invItem.FreeTaxSalesAmount));
            }

            strValue = String.Format("{0:0.}", invItem.ZeroTaxSalesAmount);
            if (invItem.ZeroTaxSalesAmount < 0 || strValue != invItem.ZeroTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("零稅率銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< ZeroTaxSalesAmount />", invItem.ZeroTaxSalesAmount));
            }


            strValue = String.Format("{0:0.}", invItem.TaxAmount);
            if (invItem.TaxAmount < 0 || strValue != invItem.TaxAmount.ToString())
            {
                return new Exception(String.Format("營業稅額不可為負數且為整數，上傳資料：{0},TAG:< TaxAmount />", invItem.TaxAmount));
            }

            strValue = String.Format("{0:0.}", invItem.TotalAmount);
            if (invItem.TotalAmount < 0 || strValue != invItem.TotalAmount.ToString())
            {
                return new Exception(String.Format("總金額不可為負數且為整數，上傳資料：{0},TAG:< TaxAmount />", invItem.TotalAmount));
            }

            //課稅別
            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.TaxType))
            {
                return new Exception(String.Format("課稅別格式錯誤，上傳資料：{0},TAG:< TaxType />", invItem.TaxType));
            }

            if (invItem.TaxRate != 0m && invItem.TaxRate != 0.05m)
            {
                return new Exception(String.Format("稅率格式錯誤，上傳資料：{0},TAG:< TaxRate />", invItem.TaxRate));
            }

            if (invItem.TaxType == (byte)Naming.TaxTypeDefinition.零稅率)
            {
                if (String.IsNullOrEmpty(invItem.CustomsClearanceMark))
                {
                    return new Exception(String.Format("若為零稅率發票，通關方式註記(CustomsClearanceMark)為必填欄位，上傳資料：{0},TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
                else if (invItem.CustomsClearanceMark != "1" && invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format("通關方式註記格式錯誤，限填非經海關出口：\"1\"或經海關出口：\"2\"，上傳資料：{0},TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
            }
            else if (!String.IsNullOrEmpty(invItem.CustomsClearanceMark))
            {
                if (invItem.CustomsClearanceMark != "1" && invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format("通關方式註記格式錯誤，限填非經海關出口：\"1\"或經海關出口：\"2\"，上傳資料：{0},TAG:< CustomsClearanceMark />", invItem.CustomsClearanceMark));
                }
            }

            return null;
        }

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
                    return new Exception(String.Format("品項名稱不可空白長度不得大於256，傳送資料：{0}，TAG:< Description />", product.InvoiceProduct.Brief));
                }

                if (!String.IsNullOrEmpty(product.PieceUnit) && product.PieceUnit.Length > 6)
                {
                    return new Exception(String.Format("單位格式錯誤，傳送資料：{0}，TAG:< Unit />", product.PieceUnit));
                }

                if (!Regex.IsMatch(product.UnitCost.ToString(), RegularExpressions))
                {
                    return new Exception(String.Format("單價資料格式錯誤，傳送資料：{0}，TAG:< UnitPrice />", product.UnitCost));
                }

                if (!Regex.IsMatch(product.CostAmount.ToString(), RegularExpressions))
                {
                    return new Exception(String.Format("金額格式錯誤，傳送資料：{0}，TAG:< Amount />", product.CostAmount));
                }
            }
            return null;
        }
    }
}