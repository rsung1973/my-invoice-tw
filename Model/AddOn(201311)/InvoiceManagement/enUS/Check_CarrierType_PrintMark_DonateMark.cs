using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Model.Schema.EIVO;
using Model.DataEntity;
using Model.Properties;
using Model.Locale;
using Model.InvoiceManagement;

namespace Model.InvoiceManagement.enUS
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
                    return new Exception(String.Format("Donated e-GUI can not be marked to be printed, Incorrect Print Mark: {0}, Incorrect TAG:< PrintMark />", invItem.PrintMark));
                }

                if (String.IsNullOrEmpty(invItem.NPOBAN))
                {
                    return new Exception(String.Format("Unassigned donate ID, Incorrect NPOBAN: {0}, Incorrect TAG:< NPOBAN />", invItem.NPOBAN));
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
                return new Exception(String.Format("Donate Mark error, Incorrect Donate Mark: {0}, Incorrect TAG:< DonateMark />", invItem.DonateMark));
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
                return new Exception("Upload carrier type, accept only public carrier (mobile barcode) format data。");
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
                        return new Exception(String.Format("Carrier Type should be blank for the printed e-GUI, Incorrect Carrier Type: {0}，Incorrect TAG:< CarrierType />", invItem.CarrierType));
                    }
                    if (!String.IsNullOrEmpty(invItem.CarrierId1))
                    {
                        return new Exception(String.Format("Carrier ID1 should be blank for the printed e-GUI, Incorrect Carrier Id1: {0}，Incorrect TAG:< CarrierId1 />", invItem.CarrierId1));
                    }
                    if (!String.IsNullOrEmpty(invItem.CarrierId2))
                    {
                        return new Exception(String.Format("Carrier ID2 should be blank for the printed e-GUI, Incorrect Carrier Id2: {0}，Incorrect TAG:< CarrierId2 />", invItem.CarrierId2));
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
                return new Exception("SellerId can not be blank，TAG:< SellerId />");
            }

            if (String.IsNullOrEmpty(invItem.BuyerId))
            {
                return new Exception("BuyerId can not be blank，TAG:< BuyerId />");
            }

            if (String.IsNullOrEmpty(invItem.InvoiceType))
            {
                return new Exception("InvoiceType can not be blank，TAG:< InvoiceType />");
            }

            if (String.IsNullOrEmpty(invItem.DonateMark))
            {
                return new Exception("DonateMark can not be blank，TAG:< DonateMark />");
            }

            if (String.IsNullOrEmpty(invItem.PrintMark))
            {
                return new Exception("PrintMark can not be blank，TAG:< PrintMark />");
            }

            if (!__InvoiceTypeList.Contains(invItem.InvoiceType))
            {
                return new Exception(String.Format("Format of Invoice Type error, please fill in correct code 01-06, Incorrect Invoice Type: {0}, Incorrect TAG:< InvoiceType />", invItem.InvoiceType));
            }

            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)invItem.TaxType))
            {
                return new Exception("Tax Type error, Incorrect TAG:< TaxType />");
            }

            //全列印
            if (all_printed)
            {
                if (invItem.Contact == null)
                {
                    return new Exception("If the account is set to print, fill out the contact information；TAG:< Contact />");
                }

                if (String.IsNullOrEmpty(invItem.Contact.Name) || invItem.Contact.Name.Length > 64)
                {
                    return new Exception("Contact Name error, Incorrect TAG:< Name />");
                }

                if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
                {
                    return new Exception("Contact Address error, Incorrect TAG:< Address />");
                }

                if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
                {
                    return new Exception("Contact Email error, Incorrect TAG:< Email />");
                }

                if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
                {
                    return new Exception("Contact telephone error, Incorrect TAG:< TEL />");
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
                            return new Exception("Contact Name error, Incorrect TAG:< Name />");
                        }

                        if (String.IsNullOrEmpty(invItem.Contact.Address) || invItem.Contact.Address.Length > 128)
                        {
                            return new Exception("Contact Address error, Incorrect TAG:< Name />");
                        }

                        if (!String.IsNullOrEmpty(invItem.Contact.Email) && invItem.Contact.Email.Length > 512)
                        {
                            return new Exception("Contact Email error, Incorrect TAG:< Name />");
                        }

                        if (!String.IsNullOrEmpty(invItem.Contact.TEL) && invItem.Contact.TEL.Length > 64)
                        {
                            return new Exception("Contact TEL error, Incorrect TAG:< Name />");
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
    }
}