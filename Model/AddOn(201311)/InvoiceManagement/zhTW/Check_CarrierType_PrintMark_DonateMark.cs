using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model.Schema.EIVO;
using Model.DataEntity;
using Model.Properties;
using Model.Locale;

namespace Model.InvoiceManagement.zhTW
{
    public static partial class Check_CarrierType_PrintMark_DonateMark
    {
        public readonly static String[] __InvoiceTypeList = { "01", "02", "03", "04", "05", "06", "07", "08" };

        #region 一般發票

        public static Exception CheckInvoiceDonation(this InvoiceRootInvoice invItem, Organization seller, bool all_printed, bool print_mark, out InvoiceDonation donation)
        {
            donation = null;
            all_printed = seller.OrganizationStatus.PrintAll == true;

            if (invItem.DonateMark == "1")
            {
                if ((all_printed && print_mark) || (!all_printed && print_mark))
                {
                    return new Exception(String.Format("標註列印或設定為全列印時，捐贈註記不得為'1'，傳送資料：{0}，TAG:< PrintMark />，全列印設定：{1}", invItem.PrintMark, all_printed));
                }

                if (String.IsNullOrEmpty(invItem.NPOBAN))
                {
                    return new Exception(String.Format("未指定發票捐贈對象統一編號，傳送資料：{0}，TAG:< NPOBAN />", invItem.NPOBAN));
                }
                donation = new InvoiceDonation
                {
                    AgencyCode = invItem.NPOBAN
                };
            }
            else if (invItem.DonateMark == "0")
            {

            }
            else
            {
                return new Exception(String.Format("捐贈註記錯誤，傳送資料：{0}，TAG:< DonateMark />", invItem.DonateMark));
            }
            return null;
        }

        public static Exception CheckInvoiceCarrier(this InvoiceRootInvoice invItem, Organization seller, InvoiceDonation donation, bool all_printed, bool print_mark, out InvoiceCarrier carrier)
        {
            carrier = null;
            print_mark = invItem.PrintMark == "y" || invItem.PrintMark == "Y";

            bool carrierTypeIsNullOrEnpty;
            bool carrier_phone = check_carrier_phone(invItem, out carrierTypeIsNullOrEnpty);

            #region 無論B2B、B2C、列印、全列印，如有載具必須為公載具(手機條碼)

            //不為公載具
            if (!carrierTypeIsNullOrEnpty && !carrier_phone)
            {
                return new Exception("上傳載具資料，僅接受公載具(手機條碼)格式資料。");
            }

            #endregion

            #region B2C(列印N才有載具)

            if (invItem.BuyerId == "0000000000")
            {
                //列印N才有載具
                if ((all_printed && print_mark) || (!all_printed && print_mark))
                {
                    if (!String.IsNullOrEmpty(invItem.CarrierType))
                    {
                        return new Exception(String.Format("註記列印時載具類別請留空白，傳送資料：{0}，TAG:< CarrierType />", invItem.CarrierType));
                    }
                    if (!String.IsNullOrEmpty(invItem.CarrierId1))
                    {
                        return new Exception(String.Format("註記列印時載具顯碼請留空白，傳送資料：{0}，TAG:< CarrierId1 />", invItem.CarrierId1));
                    }
                    if (!String.IsNullOrEmpty(invItem.CarrierId2))
                    {
                        return new Exception(String.Format("註記列印時載具隱碼請留空白，傳送資料：{0}，TAG:< CarrierId2 />", invItem.CarrierId2));
                    }
                }
                else
                {
                    //捐贈與否
                    if (donation == null)
                    {
                        //自動配發
                        carrier = new InvoiceCarrier
                        {
                            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? EIVOPlatformFactory.DefaultUserCarrierType : invItem.CarrierType,
                            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? System.Guid.NewGuid().ToString() : invItem.CarrierId1
                        };

                        if (String.IsNullOrEmpty(invItem.CarrierId2))
                        {
                            carrier.CarrierNo2 = carrier.CarrierNo;
                        }
                        else
                        {
                            carrier.CarrierNo2 = invItem.CarrierId2;
                        }
                    }
                    else
                    {
                        //無自動配發
                        carrier = new InvoiceCarrier
                        {
                            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                        };

                        if (String.IsNullOrEmpty(invItem.CarrierId2))
                        {
                            carrier.CarrierNo2 = carrier.CarrierNo;
                        }
                        else
                        {
                            carrier.CarrierNo2 = invItem.CarrierId2;
                        }
                    }
                }
            }
            #endregion

            #region B2B

            else
            {
                #region 舊邏輯 - 註解 2014-12-18  註：B2B無須配發網優載具

                //#region 1.全列印:Y、列印:Y、2.全列印:N、列印Y

                ////公載具，不配發
                //if ((all_printed && print_mark) || (!all_printed && print_mark))
                //{
                //    if (carrier_phone)
                //    {
                //        carrier = new InvoiceCarrier
                //        {
                //            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                //            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                //        };

                //        if (String.IsNullOrEmpty(invItem.CarrierId2))
                //        {
                //            carrier.CarrierNo2 = carrier.CarrierNo;
                //        }
                //        else
                //        {
                //            carrier.CarrierNo2 = invItem.CarrierId2;
                //        }
                //    }
                //}

                //#endregion

                //#region 1.全列印:Y、列印:N 2.全列印:N、列印N

                ////無帶載具上來，自動配發
                //if ((all_printed && !print_mark) || (!all_printed && !print_mark))
                //{
                //    //if (carrierTypeIsNullOrEnpty)
                //    //{
                //    //    carrier = new InvoiceCarrier
                //    //    {
                //    //        CarrierType = Settings.Default.DefaultUserCarrierType,
                //    //        CarrierNo = System.Guid.NewGuid().ToString(),
                //    //    };

                //    //    if (String.IsNullOrEmpty(invItem.CarrierId2))
                //    //    {
                //    //        carrier.CarrierNo2 = carrier.CarrierNo;
                //    //    }
                //    //    else
                //    //    {
                //    //        carrier.CarrierNo2 = invItem.CarrierId2;
                //    //    }
                //    //}

                //    if (carrier_phone)
                //    {
                //        carrier = new InvoiceCarrier
                //        {
                //            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                //            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                //        };

                //        if (String.IsNullOrEmpty(invItem.CarrierId2))
                //        {
                //            carrier.CarrierNo2 = carrier.CarrierNo;
                //        }
                //        else
                //        {
                //            carrier.CarrierNo2 = invItem.CarrierId2;
                //        }
                //    }
                //}

                //#endregion

                #endregion


                #region B2B 發票系統會夾帶發票PDF寄送Email，視同列印。
               
                //有載具就塞
                if (carrier_phone)
                {
                    carrier = new InvoiceCarrier
                    {
                        CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                        CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                    };

                    if (String.IsNullOrEmpty(invItem.CarrierId2))
                    {
                        carrier.CarrierNo2 = carrier.CarrierNo;
                    }
                    else
                    {
                        carrier.CarrierNo2 = invItem.CarrierId2;
                    }
                }

                #endregion
            }

            #endregion

            return null;
        }

        public static Exception CheckMandatoryFields(this InvoiceRootInvoice invItem, Organization seller, bool all_printed, bool print_mark)
        {
            if (String.IsNullOrEmpty(invItem.SellerId))
            {
                return new Exception("賣方-營業人統一編號錯誤，TAG:< SellerId />");
            }

            //if (String.IsNullOrEmpty(invItem.BuyerName))
            //{
            //    return new Exception("買方-名稱錯誤，TAG:< BuyerName />");
            //}

            if (String.IsNullOrEmpty(invItem.BuyerId))
            {
                return new Exception("買方-營業人統一編號錯誤，TAG:< BuyerId />");
            }

            if (String.IsNullOrEmpty(invItem.InvoiceType))
            {
                return new Exception("發票類別錯誤，TAG:< InvoiceType />");
            }

            if (String.IsNullOrEmpty(invItem.DonateMark))
            {
                return new Exception("捐贈註記錯誤，TAG:< DonateMark />");
            }

            if (String.IsNullOrEmpty(invItem.PrintMark))
            {
                return new Exception("紙本電子發票已列印註記錯誤，TAG:< PrintMark />");
            }

            if (!__InvoiceTypeList.Contains(invItem.InvoiceType))
            {
                return new Exception(String.Format("發票類別格式錯誤，請依發票種類填寫相應代號\"01\"-\"06\"，上傳資料：{0}，TAG:< InvoiceType />", invItem.InvoiceType));
            }

            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.TaxType))
            {
                return new Exception("課稅別錯誤，TAG:< TaxType />");
            }

            //if (String.IsNullOrEmpty(invItem.SalesAmount.ToString()))
            //{
            //    return  new Exception("應稅銷售額合計(新台幣)錯誤，TAG:< SalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.FreeTaxSalesAmount.ToString()))
            //{
            //    return  new Exception("免稅銷售額合計(新台幣)錯誤，TAG:< FreeTaxSalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.ZeroTaxSalesAmount.ToString()))
            //{
            //    return  new Exception("零稅率銷售額合計(新台幣)錯誤，TAG:< ZeroTaxSalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.TaxRate.ToString()))
            //{
            //    return new Exception("稅率錯誤，TAG:< TaxRate />");
            //}

            //if (String.IsNullOrEmpty(invItem.TaxAmount.ToString()))
            //{
            //    return new Exception("營業稅額錯誤，TAG:< TaxAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.TotalAmount.ToString()))
            //{
            //    return new Exception("總計錯誤，TAG:< TotalAmount />";
            //}

            //printed = seller.OrganizationStatus.PrintAll == true;

            //全列印
            if (all_printed)
            {
                if (invItem.Contact == null)
                {
                    return new Exception("公司帳號設定為全列印，請填寫聯絡人資訊；TAG:< Contact />");
                }

                if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
                {
                    return new Exception("聯絡人名稱錯誤，TAG:< Name />");
                }

                if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
                {
                    return new Exception("聯絡人地址錯誤，TAG:< Address />");
                }

                if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
                {
                    return new Exception("聯絡人Email錯誤，TAG:< Email />");
                }

                if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
                {
                    return new Exception("聯絡人電話號碼錯誤，TAG:< TEL />");
                }
            }
            else
            {
                //沒全列印、判斷 上傳PrintMark。
                if (print_mark && invItem.Contact != null)
                {
                    bool contact = !String.IsNullOrEmpty(invItem.Contact.Name) || String.IsNullOrEmpty(invItem.Contact.Address) || !String.IsNullOrEmpty(invItem.Contact.Email) || !String.IsNullOrEmpty(invItem.Contact.TEL);

                    //聯絡人資訊有任一值
                    if (contact == true)
                    {
                        if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
                        {
                            return new Exception("聯絡人名稱錯誤，TAG:< Name />");
                        }

                        if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
                        {
                            return new Exception("聯絡人地址錯誤，TAG:< Address />");
                        }

                        if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
                        {
                            return new Exception("聯絡人Email錯誤，TAG:< Email />");
                        }

                        if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
                        {
                            return new Exception("聯絡人電話號碼錯誤，TAG:< TEL />");
                        }
                    }
                }
            }

            return null;
        }

        private static bool check_carrier_phone(InvoiceRootInvoice invItem, out bool carrierIsNullOrEmpty)
        {
            carrierIsNullOrEmpty = false;
            if (String.IsNullOrEmpty(invItem.CarrierType) && String.IsNullOrEmpty(invItem.CarrierId1) && String.IsNullOrEmpty(invItem.CarrierId2))
            {
                carrierIsNullOrEmpty = true;
                return false;
            }

            if (!String.IsNullOrEmpty(invItem.CarrierType))
            {
                if (invItem.CarrierType == "3J0002")
                {
                    if (String.IsNullOrEmpty(invItem.CarrierId1) && String.IsNullOrEmpty(invItem.CarrierId2))
                        return false;
                    if (!String.IsNullOrEmpty(invItem.CarrierId1) && ((invItem.CarrierId1.Length != 8 || !invItem.CarrierId1.StartsWith("/"))))
                        return false;
                    if (!String.IsNullOrEmpty(invItem.CarrierId2) && ((invItem.CarrierId2.Length != 8 || !invItem.CarrierId2.StartsWith("/"))))
                        return false;

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion

        #region C0401格式上傳

        public static Exception CheckInvoiceDonation_C0401(this Schema.MIG3_1.C0401.Invoice invItem, Organization seller, bool all_printed, bool print_mark, out InvoiceDonation donation)
        {
            donation = null;
            all_printed = seller.OrganizationStatus.PrintAll == true;

            if (((int)invItem.Main.DonateMark).Equals(1))
            {
                if ((all_printed && print_mark) || (!all_printed && print_mark))
                {
                    return new Exception(String.Format("標註列印或設定為全列印時，捐贈註記不得為'1'，傳送資料：{0}，TAG:< PrintMark />，全列印設定：{1}", invItem.Main.PrintMark, all_printed));
                }

                if (String.IsNullOrEmpty(invItem.Main.NPOBAN))
                {
                    return new Exception(String.Format("未指定發票捐贈對象統一編號，傳送資料：{0}，TAG:< NPOBAN />", invItem.Main.NPOBAN));
                }
                donation = new InvoiceDonation
                {
                    AgencyCode = invItem.Main.NPOBAN
                };
            }
            else if (((int)invItem.Main.DonateMark).Equals(0))
            {

            }
            else
            {
                return new Exception(String.Format("捐贈註記錯誤，傳送資料：{0}，TAG:< DonateMark />", invItem.Main.DonateMark));
            }
            return null;
        }

        public static Exception CheckInvoiceCarrier_C0401(this Schema.MIG3_1.C0401.Invoice invItem, Organization seller, InvoiceDonation donation, bool all_printed, bool print_mark, out InvoiceCarrier carrier)
        {
            carrier = null;
            print_mark = invItem.Main.PrintMark == "y" || invItem.Main.PrintMark == "Y";

            bool carrierTypeIsNullOrEnpty;
            bool carrier_phone = check_carrier_phone_C0401(invItem, out carrierTypeIsNullOrEnpty);

            #region 無論B2B、B2C、列印、全列印，如有載具必須為公載具(手機條碼)

            //不為公載具
            if (!carrierTypeIsNullOrEnpty && !carrier_phone)
            {
                return new Exception("上傳載具資料，僅接受公載具(手機條碼)格式資料。");
            }

            #endregion

            #region B2C(列印N才有載具)

            if (invItem.Main.Buyer.Identifier == "0000000000")
            {
                //列印N才有載具
                if ((all_printed && print_mark) || (!all_printed && print_mark))
                {
                    if (!String.IsNullOrEmpty(invItem.Main.CarrierType))
                    {
                        return new Exception(String.Format("註記列印時載具類別請留空白，傳送資料：{0}，TAG:< CarrierType />", invItem.Main.CarrierType));
                    }
                    if (!String.IsNullOrEmpty(invItem.Main.CarrierId1))
                    {
                        return new Exception(String.Format("註記列印時載具顯碼請留空白，傳送資料：{0}，TAG:< CarrierId1 />", invItem.Main.CarrierId1));
                    }
                    if (!String.IsNullOrEmpty(invItem.Main.CarrierId2))
                    {
                        return new Exception(String.Format("註記列印時載具隱碼請留空白，傳送資料：{0}，TAG:< CarrierId2 />", invItem.Main.CarrierId2));
                    }
                }
                else
                {
                    //捐贈與否
                    if (donation == null)
                    {
                        //自動配發
                        carrier = new InvoiceCarrier
                        {
                            CarrierType = String.IsNullOrEmpty(invItem.Main.CarrierType) ? EIVOPlatformFactory.DefaultUserCarrierType : invItem.Main.CarrierType,
                            CarrierNo = String.IsNullOrEmpty(invItem.Main.CarrierId1) ? System.Guid.NewGuid().ToString() : invItem.Main.CarrierId1
                        };

                        if (String.IsNullOrEmpty(invItem.Main.CarrierId2))
                        {
                            carrier.CarrierNo2 = carrier.CarrierNo;
                        }
                        else
                        {
                            carrier.CarrierNo2 = invItem.Main.CarrierId2;
                        }
                    }
                    else
                    {
                        //無自動配發
                        carrier = new InvoiceCarrier
                        {
                            CarrierType = String.IsNullOrEmpty(invItem.Main.CarrierType) ? "" : invItem.Main.CarrierType,
                            CarrierNo = String.IsNullOrEmpty(invItem.Main.CarrierId1) ? "" : invItem.Main.CarrierId1
                        };

                        if (String.IsNullOrEmpty(invItem.Main.CarrierId2))
                        {
                            carrier.CarrierNo2 = carrier.CarrierNo;
                        }
                        else
                        {
                            carrier.CarrierNo2 = invItem.Main.CarrierId2;
                        }
                    }
                }
            }
            #endregion

            #region B2B

            else
            {
                #region 舊邏輯 - 註解 2014-12-18  註：B2B無須配發網優載具

                //#region 1.全列印:Y、列印:Y、2.全列印:N、列印Y

                ////公載具，不配發
                //if ((all_printed && print_mark) || (!all_printed && print_mark))
                //{
                //    if (carrier_phone)
                //    {
                //        carrier = new InvoiceCarrier
                //        {
                //            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                //            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                //        };

                //        if (String.IsNullOrEmpty(invItem.CarrierId2))
                //        {
                //            carrier.CarrierNo2 = carrier.CarrierNo;
                //        }
                //        else
                //        {
                //            carrier.CarrierNo2 = invItem.CarrierId2;
                //        }
                //    }
                //}

                //#endregion

                //#region 1.全列印:Y、列印:N 2.全列印:N、列印N

                ////無帶載具上來，自動配發
                //if ((all_printed && !print_mark) || (!all_printed && !print_mark))
                //{
                //    //if (carrierTypeIsNullOrEnpty)
                //    //{
                //    //    carrier = new InvoiceCarrier
                //    //    {
                //    //        CarrierType = Settings.Default.DefaultUserCarrierType,
                //    //        CarrierNo = System.Guid.NewGuid().ToString(),
                //    //    };

                //    //    if (String.IsNullOrEmpty(invItem.CarrierId2))
                //    //    {
                //    //        carrier.CarrierNo2 = carrier.CarrierNo;
                //    //    }
                //    //    else
                //    //    {
                //    //        carrier.CarrierNo2 = invItem.CarrierId2;
                //    //    }
                //    //}

                //    if (carrier_phone)
                //    {
                //        carrier = new InvoiceCarrier
                //        {
                //            CarrierType = String.IsNullOrEmpty(invItem.CarrierType) ? "" : invItem.CarrierType,
                //            CarrierNo = String.IsNullOrEmpty(invItem.CarrierId1) ? "" : invItem.CarrierId1
                //        };

                //        if (String.IsNullOrEmpty(invItem.CarrierId2))
                //        {
                //            carrier.CarrierNo2 = carrier.CarrierNo;
                //        }
                //        else
                //        {
                //            carrier.CarrierNo2 = invItem.CarrierId2;
                //        }
                //    }
                //}

                //#endregion

                #endregion


                #region B2B 發票系統會夾帶發票PDF寄送Email，視同列印。

                //有載具就塞
                if (carrier_phone)
                {
                    carrier = new InvoiceCarrier
                    {
                        CarrierType = String.IsNullOrEmpty(invItem.Main.CarrierType) ? "" : invItem.Main.CarrierType,
                        CarrierNo = String.IsNullOrEmpty(invItem.Main.CarrierId1) ? "" : invItem.Main.CarrierId1
                    };

                    if (String.IsNullOrEmpty(invItem.Main.CarrierId2))
                    {
                        carrier.CarrierNo2 = carrier.CarrierNo;
                    }
                    else
                    {
                        carrier.CarrierNo2 = invItem.Main.CarrierId2;
                    }
                }

                #endregion
            }

            #endregion

            return null;
        }

        public static Exception CheckMandatoryFields_C0401(this Schema.MIG3_1.C0401.Invoice invItem, Organization seller, bool all_printed, bool print_mark)
        {
            if (String.IsNullOrEmpty(invItem.Main.Seller.Identifier))
            {
                return new Exception("賣方-營業人統一編號錯誤，TAG:< SellerId />");
            }

            //if (String.IsNullOrEmpty(invItem.BuyerName))
            //{
            //    return new Exception("買方-名稱錯誤，TAG:< BuyerName />");
            //}

            if (String.IsNullOrEmpty(invItem.Main.Buyer.Identifier))
            {
                return new Exception("買方-營業人統一編號錯誤，TAG:< BuyerId />");
            }

            //if (invItem.Main.InvoiceType == null)
            //{
            //    return new Exception("發票類別錯誤，TAG:< InvoiceType />");
            //}

            //if (invItem.Main.DonateMark == null)
            //{
            //    return new Exception("捐贈註記錯誤，TAG:< DonateMark />");
            //}

            if (String.IsNullOrEmpty(invItem.Main.PrintMark))
            {
                return new Exception("紙本電子發票已列印註記錯誤，TAG:< PrintMark />");
            }

            String InvType = String.Format("{0}{1}", "0", ((int)invItem.Main.InvoiceType).ToString());

            if (!__InvoiceTypeList.Contains(InvType))
            {
                return new Exception(String.Format("發票類別格式錯誤，請依發票種類填寫相應代號\"01\"-\"06\"，上傳資料：{0}，TAG:< InvoiceType />", invItem.Main.InvoiceType));
            }

            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.Amount.TaxType))
            {
                return new Exception("課稅別錯誤，TAG:< TaxType />");
            }

            //if (String.IsNullOrEmpty(invItem.SalesAmount.ToString()))
            //{
            //    return  new Exception("應稅銷售額合計(新台幣)錯誤，TAG:< SalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.FreeTaxSalesAmount.ToString()))
            //{
            //    return  new Exception("免稅銷售額合計(新台幣)錯誤，TAG:< FreeTaxSalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.ZeroTaxSalesAmount.ToString()))
            //{
            //    return  new Exception("零稅率銷售額合計(新台幣)錯誤，TAG:< ZeroTaxSalesAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.TaxRate.ToString()))
            //{
            //    return new Exception("稅率錯誤，TAG:< TaxRate />");
            //}

            //if (String.IsNullOrEmpty(invItem.TaxAmount.ToString()))
            //{
            //    return new Exception("營業稅額錯誤，TAG:< TaxAmount />");
            //}

            //if (String.IsNullOrEmpty(invItem.TotalAmount.ToString()))
            //{
            //    return new Exception("總計錯誤，TAG:< TotalAmount />";
            //}

            //printed = seller.OrganizationStatus.PrintAll == true;

            //全列印
            //if (all_printed)
            //{
            //    if (invItem.Contact == null)
            //    {
            //        return new Exception("公司帳號設定為全列印，請填寫聯絡人資訊；TAG:< Contact />");
            //    }

            //    if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
            //    {
            //        return new Exception("聯絡人名稱錯誤，TAG:< Name />");
            //    }

            //    if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
            //    {
            //        return new Exception("聯絡人地址錯誤，TAG:< Address />");
            //    }

            //    if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
            //    {
            //        return new Exception("聯絡人Email錯誤，TAG:< Email />");
            //    }

            //    if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
            //    {
            //        return new Exception("聯絡人電話號碼錯誤，TAG:< TEL />");
            //    }
            //}
            //else
            //{
            //    //沒全列印、判斷 上傳PrintMark。
            //    if (print_mark && invItem.Contact != null)
            //    {
            //        bool contact = !String.IsNullOrEmpty(invItem.Contact.Name) || String.IsNullOrEmpty(invItem.Contact.Address) || !String.IsNullOrEmpty(invItem.Contact.Email) || !String.IsNullOrEmpty(invItem.Contact.TEL);

            //        //聯絡人資訊有任一值
            //        if (contact == true)
            //        {
            //            if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
            //            {
            //                return new Exception("聯絡人名稱錯誤，TAG:< Name />");
            //            }

            //            if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
            //            {
            //                return new Exception("聯絡人地址錯誤，TAG:< Address />");
            //            }

            //            if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
            //            {
            //                return new Exception("聯絡人Email錯誤，TAG:< Email />");
            //            }

            //            if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
            //            {
            //                return new Exception("聯絡人電話號碼錯誤，TAG:< TEL />");
            //            }
            //        }
            //    }
            //}

            return null;
        }

        private static bool check_carrier_phone_C0401(Schema.MIG3_1.C0401.Invoice invItem, out bool carrierIsNullOrEmpty)
        {
            carrierIsNullOrEmpty = false;
            if (String.IsNullOrEmpty(invItem.Main.CarrierType) && String.IsNullOrEmpty(invItem.Main.CarrierId1) && String.IsNullOrEmpty(invItem.Main.CarrierId2))
            {
                carrierIsNullOrEmpty = true;
                return false;
            }

            if (!String.IsNullOrEmpty(invItem.Main.CarrierType))
            {
                if (invItem.Main.CarrierType == "3J0002")
                {
                    if (String.IsNullOrEmpty(invItem.Main.CarrierId1) && String.IsNullOrEmpty(invItem.Main.CarrierId2))
                        return false;
                    if (!String.IsNullOrEmpty(invItem.Main.CarrierId1) && ((invItem.Main.CarrierId1.Length != 8 || !invItem.Main.CarrierId1.StartsWith("/"))))
                        return false;
                    if (!String.IsNullOrEmpty(invItem.Main.CarrierId2) && ((invItem.Main.CarrierId2.Length != 8 || !invItem.Main.CarrierId2.StartsWith("/"))))
                        return false;

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion
    }
}