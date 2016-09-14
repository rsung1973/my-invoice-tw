using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Model.InvoiceManagement.zhTW;

namespace Model.InvoiceManagement.zhTW
{
    public static partial class InvoiceRootValidator_C0401
    {
        public static string RegularExpressions = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public static Exception CheckDataLength(this Model.Schema.MIG3_1.C0401.Invoice invItem)
        {
            #region 賣方

            if (!String.IsNullOrEmpty(invItem.Main.Seller.Address) && invItem.Main.Seller.Address.Length > 100)
            {
                return new Exception(String.Format("賣方地址資料格式錯誤，長度最長為100碼，傳送資料：{0}，TAG:< Address />", invItem.Main.Seller.Address));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.PersonInCharge) && invItem.Main.Seller.PersonInCharge.Length > 30)
            {
                return new Exception(String.Format("賣方負責人姓名資料格式錯誤，長度最長為30碼，傳送資料：{0}，TAG:< PersonInCharge />", invItem.Main.Seller.PersonInCharge));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.TelephoneNumber) && invItem.Main.Seller.TelephoneNumber.Length > 26)
            {
                return new Exception(String.Format("賣方電話號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< TelephoneNumber />", invItem.Main.Seller.TelephoneNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.FacsimileNumber) && invItem.Main.Seller.FacsimileNumber.Length > 26)
            {
                return new Exception(String.Format("賣方傳真號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< FacsimileNumber />", invItem.Main.Seller.FacsimileNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.EmailAddress) && invItem.Main.Seller.EmailAddress.Length > 80)
            {
                return new Exception(String.Format("賣方電子郵件地址資料格式錯誤，長度最長為80碼，傳送資料：{0}，TAG:<  EmailAddress />", invItem.Main.Seller.EmailAddress));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.CustomerNumber) && invItem.Main.Seller.CustomerNumber.Length > 20)
            {
                return new Exception(String.Format("賣方客戶編號資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< CustomerNumber />", invItem.Main.Seller.CustomerNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Seller.RoleRemark) && invItem.Main.Seller.RoleRemark.Length > 40)
            {
                return new Exception(String.Format("賣方營業人角色註記資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< RoleRemark />", invItem.Main.Seller.RoleRemark));
            }

            #endregion

            #region 買方

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.Address) && invItem.Main.Buyer.Address.Length > 100)
            {
                return new Exception(String.Format("買方地址資料格式錯誤，長度最長為100碼，傳送資料：{0}，TAG:< Address />", invItem.Main.Buyer.Address));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.PersonInCharge) && invItem.Main.Buyer.PersonInCharge.Length > 30)
            {
                return new Exception(String.Format("買方負責人姓名資料格式錯誤，長度最長為30碼，傳送資料：{0}，TAG:< PersonInCharge />", invItem.Main.Buyer.PersonInCharge));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.TelephoneNumber) && invItem.Main.Buyer.TelephoneNumber.Length > 26)
            {
                return new Exception(String.Format("買方電話號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< TelephoneNumber />", invItem.Main.Buyer.TelephoneNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.FacsimileNumber) && invItem.Main.Buyer.FacsimileNumber.Length > 26)
            {
                return new Exception(String.Format("買方傳真號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< FacsimileNumber />", invItem.Main.Buyer.FacsimileNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.EmailAddress) && invItem.Main.Buyer.EmailAddress.Length > 80)
            {
                return new Exception(String.Format("買方電子郵件地址資料格式錯誤，長度最長為80碼，傳送資料：{0}，TAG:<  EmailAddress />", invItem.Main.Buyer.EmailAddress));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.CustomerNumber) && invItem.Main.Buyer.CustomerNumber.Length > 20)
            {
                return new Exception(String.Format("買方客戶編號資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< CustomerNumber />", invItem.Main.Buyer.CustomerNumber));
            }

            if (!String.IsNullOrEmpty(invItem.Main.Buyer.RoleRemark) && invItem.Main.Buyer.RoleRemark.Length > 40)
            {
                return new Exception(String.Format("買方營業人角色註記資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< RoleRemark />", invItem.Main.Buyer.RoleRemark));
            }

            #endregion

            #region 發票主體(Main)

            if(!string.IsNullOrEmpty(invItem.Main.CheckNumber) && invItem.Main.CheckNumber.Length > 1)
            {
                return new Exception(String.Format("發票檢查碼資料格式錯誤，長度最長為1碼，傳送資料：{0}，TAG:< CheckNumber />", invItem.Main.CheckNumber));
            }

            if ((Enum.IsDefined(typeof(Model.Schema.MIG3_1.C0401.BuyerRemarkEnum), (int)invItem.Main.BuyerRemark)))
            {
                return new Exception(String.Format("買受人註記欄資料格式錯誤，傳送資料：{0}，TAG:< BuyerRemark />", invItem.Main.BuyerRemark));
            }

            if(!string.IsNullOrEmpty(invItem.Main.MainRemark) && invItem.Main.MainRemark.Length > 200)
            {
                return new Exception(String.Format("總備註資料格式錯誤，長度最長為200碼，傳送資料：{0}，TAG:< MainRemark />", invItem.Main.MainRemark));
            }

            if ((Enum.IsDefined(typeof(Model.Schema.MIG3_1.C0401.CustomsClearanceMarkEnum), (int)invItem.Main.CustomsClearanceMark)))
            {
                return new Exception(String.Format("通關方式註記資料格式錯誤，傳送資料：{0}，TAG:< CustomerClearanceMark />", (int) invItem.Main.CustomsClearanceMark));
            }

            if(!string.IsNullOrEmpty(invItem.Main.Category) && invItem.Main.Category.Length > 2)
            {
                return new Exception(String.Format("沖帳別資料格式錯誤，長度最長為2碼，傳送資料：{0}，TAG:< Category />", invItem.Main.Category));
            }

            if (!string.IsNullOrEmpty(invItem.Main.RelateNumber) && invItem.Main.RelateNumber.Length > 20)
            {
                return new Exception(String.Format("相關號碼資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< RelateNumber />", invItem.Main.RelateNumber));
            }

            if (!string.IsNullOrEmpty(invItem.Main.GroupMark) && invItem.Main.GroupMark.Length > 1)
            {
                return new Exception(String.Format("彙開註記資料格式錯誤，長度最長為1碼，傳送資料：{0}，TAG:< GroupMark />", invItem.Main.GroupMark));
            }

            if (!string.IsNullOrEmpty(invItem.Main.CarrierType) && invItem.Main.CarrierType.Length > 6)
            {
                return new Exception(String.Format("載具類別號碼資料格式錯誤，長度最長為6碼，傳送資料：{0}，TAG:< CarrierType />", invItem.Main.CarrierType));
            }

            if (!string.IsNullOrEmpty(invItem.Main.CarrierId1) && invItem.Main.CarrierId1.Length > 64)
            {
                return new Exception(String.Format("載具顯碼Id資料格式錯誤，長度最長為64碼，傳送資料：{0}，TAG:< CarrierId1 />", invItem.Main.CarrierId1));
            }

            if (!string.IsNullOrEmpty(invItem.Main.CarrierId2) && invItem.Main.CarrierId2.Length > 64)
            {
                return new Exception(String.Format("載具隱碼Id資料格式錯誤，長度最長為64碼，傳送資料：{0}，TAG:< CarrierId2 />", invItem.Main.CarrierId2));
            }

            if (!string.IsNullOrEmpty(invItem.Main.NPOBAN) && invItem.Main.NPOBAN.Length > 10)
            {
                return new Exception(String.Format("發票捐贈對象統一編號資料格式錯誤，長度最長為10碼，傳送資料：{0}，TAG:< NPOBAN />", invItem.Main.NPOBAN));
            }

            #endregion

            #region 發票品項(Detail)

            if (invItem.Details != null && invItem.Details.Count() > 0)
            {
                foreach (var item in invItem.Details)
                {
                    if (!string.IsNullOrEmpty(item.Unit) && item.Unit.Length > 6)
                    {
                        return new Exception(String.Format("單位資料格式錯誤，長度最長為6碼，傳送資料：{0}，TAG:< Unit />", item.Unit));
                    }

                    if(!string.IsNullOrEmpty(item.Remark) && item.Remark.Length > 40)
                    {
                        return new Exception(String.Format("單一欄位備註資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< Remark />", item.Remark));
                    }

                    if(!string.IsNullOrEmpty(item.RelateNumber) && item.RelateNumber.Length > 20)
                    {
                        return new Exception(String.Format("相關號碼資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< RelateNumber />", item.RelateNumber));
                    }
                }
            }
            else
            {
                return new Exception("發票品項不可空白。");
            }


            #endregion

            #region 發票金額(Amount)

            if (invItem.Amount.DiscountAmount.ToString().Length > 12)
            {
                return new Exception(String.Format("扣抵金額資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< DiscountAmount />", invItem.Amount.DiscountAmount));
            }

            if(invItem.Amount.OriginalCurrencyAmount.ToString().Length > 12)
            {
                return new Exception(String.Format("原幣金額資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< OriginalCurrencyAmount />", invItem.Amount.OriginalCurrencyAmount));
            }

            if (invItem.Amount.ExchangeRate.ToString().Length > 12)
            {
                return new Exception(String.Format("匯率資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< ExchangeRate />", invItem.Amount.ExchangeRate));
            }

            if(invItem.Amount.Currency.ToString().Length > 3)
            {
                return new Exception(String.Format("幣別資料格式錯誤，長度最長為3碼，傳送資料：{0}，TAG:< Currency />", invItem.Amount.Currency));
            }

            #endregion

            return null;
        }

        public static Exception CheckInvoice(this Model.Schema.MIG3_1.C0401.Invoice invItem, GenericManager<EIVOEntityDataContext> mgr, out String trackCode, out String invNo, out DateTime invoiceDate)
        {
            trackCode = null;
            invNo = null;
            invoiceDate = default(DateTime);

            if (invItem.Main.InvoiceNumber == null || !Regex.IsMatch(invItem.Main.InvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
            {
                return new Exception(String.Format("發票號碼，傳送資料:{0}，TAG：< InvoicNumber />", invItem.Main.InvoiceNumber));
            }

            if (String.IsNullOrEmpty(invItem.Main.InvoiceDate))
            {
                return new Exception("發票日期，TAG：< InvoiceDate />");
            }

            if (String.IsNullOrEmpty(invItem.Main.InvoiceTime.ToString()))
            {
                return new Exception("發票時間，TAG：< InvoiceTime />");
            }

            if (!DateTime.TryParseExact(invItem.Main.InvoiceDate, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                return new Exception(String.Format("發票日期格式錯誤；上傳資料:{0}", invItem.Main.InvoiceDate));
            }
            invoiceDate += invItem.Main.InvoiceTime.TimeOfDay;
            //if (!DateTime.TryParseExact(String.Format("{0} {1}", invItem.Main.InvoiceDate, invItem.Main.InvoiceTime.TimeOfDay.ToString()), "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            //{
            //    return new Exception(String.Format("發票日期、發票時間格式錯誤(YYYY/MM/DD HH:mm:ss)；上傳資料:{0} {1}", invItem.Main.InvoiceDate, invItem.Main.InvoiceTime));
            //}            

            trackCode = invItem.Main.InvoiceNumber.Substring(0, 2);
            invNo = invItem.Main.InvoiceNumber.Substring(2);

            String chkTrackCode = trackCode;
            String chkInvNo = invNo;

            if (mgr.GetTable<InvoiceItem>().Any(i => i.TrackCode == chkTrackCode && i.No == chkInvNo))
            {
                return new Exception("發票號碼重複，已存在的發票資料。");
            }

            return null;
        }

        public static Exception CheckBusiness(this Model.Schema.MIG3_1.C0401.Invoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out Organization seller)
        {
            seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.Main.Seller.Identifier).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< SellerId />", invItem.Main.Seller.Identifier));
            }

            if (seller.CompanyID != owner.CompanyID)
            {
                return new Exception(String.Format("簽章設定人與發票開立人不符,開立人統一編號:{0}，TAG:< SellerId />", invItem.Main.Seller.Identifier));
            }

            if (invItem.Main.Buyer.Identifier == "0000000000")
            {
                if (invItem.Main.Buyer.Name == null || Encoding.GetEncoding(950).GetBytes(invItem.Main.Buyer.Name).Length != 4)
                {
                    return new Exception(String.Format("B2C買方名稱格式錯誤，長度為ASCII字元4碼或中文全形字元2碼，傳送資料：{0}，TAG:< BuyerName />", invItem.Main.Buyer.Name));
                }
            }
            else if (invItem.Main.Buyer.Identifier == null || !Regex.IsMatch(invItem.Main.Buyer.Identifier, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< BuyerId />", invItem.Main.Buyer.Identifier));
            }
            else if (invItem.Main.Buyer.Name.Length > 60)
            {
                return new Exception(String.Format("買方名稱格式錯誤，長度最多60碼，傳送資料：{0}，TAG:< BuyerName />", invItem.Main.Buyer.Name));
            }

            if (String.IsNullOrEmpty(invItem.Main.RandomNumber))
            {
                invItem.Main.RandomNumber = String.Format("{0:ffff}", DateTime.Now);
            }
            else if (!Regex.IsMatch(invItem.Main.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format("交易隨機碼應由4位數值構成，上傳資料：{0}，TAG:< RandomNumber />", invItem.Main.RandomNumber));
            }


            return null;
        }

        public static Exception CheckAmount(this Model.Schema.MIG3_1.C0401.Invoice invItem)
        {
            //應稅銷售額
            String strValue = String.Format("{0:0.}", invItem.Amount.SalesAmount);
            if (invItem.Amount.SalesAmount < 0 || strValue != invItem.Amount.SalesAmount.ToString())
            {
                return new Exception(String.Format("應稅銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< SalesAmount />", invItem.Amount.SalesAmount));
            }


            strValue = String.Format("{0:0.}", invItem.Amount.FreeTaxSalesAmount);
            if (invItem.Amount.FreeTaxSalesAmount < 0 || strValue != invItem.Amount.FreeTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("免稅銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< FreeTaxSalesAmount />", invItem.Amount.FreeTaxSalesAmount));
            }

            strValue = String.Format("{0:0.}", invItem.Amount.ZeroTaxSalesAmount);
            if (invItem.Amount.ZeroTaxSalesAmount < 0 || strValue != invItem.Amount.ZeroTaxSalesAmount.ToString())
            {
                return new Exception(String.Format("零稅率銷售額合計(新台幣)不可為負數且為整數，上傳資料：{0},TAG:< ZeroTaxSalesAmount />", invItem.Amount.ZeroTaxSalesAmount));
            }


            strValue = String.Format("{0:0.}", invItem.Amount.TaxAmount);
            if (invItem.Amount.TaxAmount < 0 || strValue != invItem.Amount.TaxAmount.ToString())
            {
                return new Exception(String.Format("營業稅額不可為負數且為整數，上傳資料：{0},TAG:< TaxAmount />", invItem.Amount.TaxAmount));
            }

            strValue = String.Format("{0:0.}", invItem.Amount.TotalAmount);
            if (invItem.Amount.TotalAmount < 0 || strValue != invItem.Amount.TotalAmount.ToString())
            {
                return new Exception(String.Format("總金額不可為負數且為整數，上傳資料：{0},TAG:< TaxAmount />", invItem.Amount.TotalAmount));
            }

            //課稅別
            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition),(int)invItem.Amount.TaxType))
            {
                return new Exception(String.Format("課稅別格式錯誤，上傳資料：{0},TAG:< TaxType />", invItem.Amount.TaxType));
            }

            if (invItem.Amount.TaxRate != 0m && invItem.Amount.TaxRate != 0.05m)
            {
                return new Exception(String.Format("稅率格式錯誤，上傳資料：{0},TAG:< TaxRate />", invItem.Amount.TaxRate));
            }

            if ((int)invItem.Amount.TaxType == (byte)Naming.TaxTypeDefinition.零稅率)
            {
                if ((int)invItem.Main.CustomsClearanceMark==0)
                {
                    return new Exception(String.Format("若為零稅率發票，通關方式註記(CustomsClearanceMark)為必填欄位，上傳資料：{0},TAG:< CustomsClearanceMark />", (int)invItem.Main.CustomsClearanceMark));
                }
                else if ((int)invItem.Main.CustomsClearanceMark != 1 && (int)invItem.Main.CustomsClearanceMark != 2)
                {
                    return new Exception(String.Format("通關方式註記格式錯誤，限填非經海關出口：\"1\"或經海關出口：\"2\"，上傳資料：{0},TAG:< CustomsClearanceMark />", (int)invItem.Main.CustomsClearanceMark));
                }
            }
            else if ((int)invItem.Main.CustomsClearanceMark!= 0)
            {
                if ((int)invItem.Main.CustomsClearanceMark != 1 && (int)invItem.Main.CustomsClearanceMark != 2)
                {
                    return new Exception(String.Format("通關方式註記格式錯誤，限填非經海關出口：\"1\"或經海關出口：\"2\"，上傳資料：{0},TAG:< CustomsClearanceMark />", (int)invItem.Main.CustomsClearanceMark));
                }
            }

            return null;
        }

        

        public static Exception CheckInvoiceProductItems(this Model.Schema.MIG3_1.C0401.Invoice invItem, out IEnumerable<InvoiceProductItem> productItems)
        {
            short seqNo = 0;
            productItems = invItem.Details.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                Piece = i.Quantity,
                PieceUnit = i.Unit,
                UnitCost = i.UnitPrice,
                Remark = i.Remark,
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
                
               // String valueStr = product.UnitCost.ToString();
                if (!Regex.IsMatch(product.UnitCost.ToString(), RegularExpressions))
               // if (valueStr != String.Format("{0:.####}", product.UnitCost) || valueStr.Length > 17)
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
