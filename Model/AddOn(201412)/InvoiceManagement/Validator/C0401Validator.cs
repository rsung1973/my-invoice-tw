using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.Locale;
using Model.Resource;
using Model.Schema.EIVO;
using Model.Schema.MIG3_1.C0401;
using Utility;

namespace Model.InvoiceManagement.Validator
{
    public partial class C0401Validator : InvoiceRootInvoiceValidator
    {

        protected Invoice _c0401Item;

        public C0401Validator(GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner) : base(mgr,owner)
        {

        }

        public override Exception Validate(InvoiceRootInvoice dataItem)
        {
            return new Exception(String.Format("不支援此格式的資料驗證:{0}", typeof(InvoiceRootInvoice).ToString()));
        }

        public override void StartAutoTrackNo()
        {

        }
        
        public Exception Validate(Invoice dataItem)
        {
            _c0401Item = dataItem;

            Exception ex;

            _seller = null;
            _newItem = null;

            if ((ex = checkBusiness()) != null)
            {
                return ex;
            }


            if ((ex = checkAmount()) != null)
            {
                return ex;
            }

            if ((ex = checkInvoiceDelivery()) != null)
            {
                return ex;
            }

            if ((ex = checkMandatoryFields()) != null)
            {
                return ex;
            }

            if ((ex = checkInvoiceProductItems()) != null)
            {
                return ex;
            }

            if ((ex = checkInvoice()) != null)
            {
                return ex;
            }


            return null;
        }


        protected override Exception checkInvoice()
        {
            _newItem = new InvoiceItem
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = _owner.CompanyID
                    }
                },
                DonateMark = _donation == null ? "0" : "1",
                InvoiceType = (byte)_c0401Item.Main.InvoiceType,
                SellerID = _seller.CompanyID,
                CustomsClearanceMark = _c0401Item.Main.CustomsClearanceMarkSpecified ? ((int)_c0401Item.Main.CustomsClearanceMark).ToString() : null,
                InvoiceSeller = new InvoiceSeller
                {
                    Name = _seller.CompanyName,
                    ReceiptNo = _seller.ReceiptNo,
                    Address = _seller.Addr,
                    ContactName = _seller.ContactName,
                    CustomerID = _c0401Item.Main.Seller.Identifier,
                    CustomerName = _seller.CompanyName,
                    EMail = _seller.ContactEmail,
                    Fax = _seller.Fax,
                    Phone = _seller.Phone,
                    PersonInCharge = _seller.UndertakerName,
                    SellerID = _seller.CompanyID,
                },
                InvoiceBuyer = _buyer,
                RandomNo = _c0401Item.Main.RandomNumber,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = _c0401Item.Amount.DiscountAmount,
                    SalesAmount = (_c0401Item.Amount.TaxType == TaxTypeEnum.Item1) ? _c0401Item.Amount.SalesAmount :
                                  (_c0401Item.Amount.TaxType == TaxTypeEnum.Item2) ? _c0401Item.Amount.ZeroTaxSalesAmount :
                                  (_c0401Item.Amount.TaxType == TaxTypeEnum.Item3) ? _c0401Item.Amount.FreeTaxSalesAmount : _c0401Item.Amount.SalesAmount,
                    TaxAmount = _c0401Item.Amount.TaxAmount,
                    TaxRate = _c0401Item.Amount.TaxRate,
                    TaxType = (byte)_c0401Item.Amount.TaxType,
                    TotalAmount = _c0401Item.Amount.TotalAmount,
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(_c0401Item.Amount.TotalAmount),
                },
                InvoiceCarrier = _carrier,
                InvoiceDonation = _donation,
                PrintMark = _c0401Item.Main.PrintMark,
            };

            if (_order != null)
            {
                _newItem.InvoicePurchaseOrder = _order;
            }

            _newItem.InvoiceDetails.AddRange(_productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct,
            }));



            DateTime invoiceDate = DateTime.Now;

            if (String.IsNullOrEmpty(_c0401Item.Main.InvoiceDate))
            {
                return new Exception(MessageResources.AlertInvoiceDate);
            }
            if (!DateTime.TryParseExact(_c0401Item.Main.InvoiceDate, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                return new Exception(String.Format(MessageResources.AlertInvoiceDateTime, _c0401Item.Main.InvoiceDate, _c0401Item.Main.InvoiceTime.TimeOfDay.ToString()));
            }
            invoiceDate += _c0401Item.Main.InvoiceTime.TimeOfDay;
            //if (!DateTime.TryParseExact(String.Format("{0} {1}", _c0401Item.Main.InvoiceDate, _c0401Item.Main.InvoiceTime.TimeOfDay.ToString()), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            //{
            //    return new Exception(String.Format(MessageResources.AlertInvoiceDateTime, _c0401Item.Main.InvoiceDate, _c0401Item.Main.InvoiceTime.TimeOfDay.ToString()));
            //}

            _newItem.InvoiceDate = invoiceDate;

            if (_c0401Item.Main.InvoiceNumber == null || !Regex.IsMatch(_c0401Item.Main.InvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
            {
                return new Exception(String.Format(MessageResources.AlertInvoiceNumber, _c0401Item.Main.InvoiceNumber));
            }

            _newItem.TrackCode = _c0401Item.Main.InvoiceNumber.Substring(0, 2);
            _newItem.No = _c0401Item.Main.InvoiceNumber.Substring(2);

            if (_mgr.GetTable<InvoiceItem>().Any(i => i.TrackCode == _newItem.TrackCode && i.No == _newItem.No))
            {
                return new Exception(MessageResources.AlertInvoiceDuplicated);
            }

            return null;
        }


        protected override Exception checkBusiness()
        {

            if (_c0401Item.Main == null || _c0401Item.Main.Seller == null || _c0401Item.Main.Buyer==null)
            {
                return new Exception("C0401資料格式錯誤");
            }

            _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _c0401Item.Main.Seller.Identifier).FirstOrDefault();

            if (_seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< Identifier />", _c0401Item.Main.Seller.Identifier));
            }

            if (_seller.CompanyID != _owner.CompanyID && !_mgr.GetTable<InvoiceIssuerAgent>().Any(a => a.AgentID == _owner.CompanyID && a.IssuerID == _seller.CompanyID))
            {
                return new Exception(String.Format("簽章設定人與發票開立人不符,開立人統一編號:{0}，TAG:< Identifier />", _c0401Item.Main.Seller.Identifier));
            }

            if (_c0401Item.Main.Buyer.Identifier == "0000000000")
            {
                if (_c0401Item.Main.Buyer.Name == null || Encoding.GetEncoding(950).GetBytes(_c0401Item.Main.Buyer.Name).Length != 4)
                {
                    return new Exception(String.Format("B2C買方名稱格式錯誤，長度為ASCII字元4碼或中文全形字元2碼，傳送資料：{0}，TAG:< Name />", _c0401Item.Main.Buyer.Name));
                }
            }
            else if (_c0401Item.Main.Buyer.Identifier == null || !Regex.IsMatch(_c0401Item.Main.Buyer.Identifier, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< Identifier />", _c0401Item.Main.Buyer.Identifier));
            }
            else if (_c0401Item.Main.Buyer.Name.Length > 60)
            {
                return new Exception(String.Format("買方名稱格式錯誤，長度最多60碼，傳送資料：{0}，TAG:< Name />", _c0401Item.Main.Buyer.Name));
            }

            if (String.IsNullOrEmpty(_c0401Item.Main.RandomNumber))
            {
                _c0401Item.Main.RandomNumber = String.Format("{0:ffff}", DateTime.Now);
            }
            else if (!Regex.IsMatch(_c0401Item.Main.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format("交易隨機碼應由4位數值構成，上傳資料：{0}，TAG:< RandomNumber />", _c0401Item.Main.RandomNumber));
            }

            return checkBusinessDetails();
        }

        protected override Exception checkPublicCarrier()
        {
            if (_c0401Item.Main.CarrierType != __CELLPHONE_BARCODE
                || (!checkPublicCarrierId(_c0401Item.Main.CarrierId1) && !checkPublicCarrierId(_c0401Item.Main.CarrierId2))                )
            {
                return new Exception(String.Format(MessageResources.InvalidPublicCarrierType, _c0401Item.Main.CarrierType, _c0401Item.Main.CarrierId1, _c0401Item.Main.CarrierId2));
            }

            return null;
        }

        protected override Exception checkPrintAll()
        {
            //全列印
            //if (String.IsNullOrEmpty(_buyer.ContactName) || String.IsNullOrEmpty(_buyer.Address))
            //{
            //    return new Exception(MessageResources.AlertContactToPrintAll);
            //}

            return null;
        }

        protected override Exception checkBusinessDetails()
        {

            #region 賣方

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.Address) && _c0401Item.Main.Seller.Address.Length > 100)
            {
                return new Exception(String.Format("賣方地址資料格式錯誤，長度最長為100碼，傳送資料：{0}，TAG:< Address />", _c0401Item.Main.Seller.Address));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.PersonInCharge) && _c0401Item.Main.Seller.PersonInCharge.Length > 30)
            {
                return new Exception(String.Format("賣方負責人姓名資料格式錯誤，長度最長為30碼，傳送資料：{0}，TAG:< PersonInCharge />", _c0401Item.Main.Seller.PersonInCharge));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.TelephoneNumber) && _c0401Item.Main.Seller.TelephoneNumber.Length > 26)
            {
                return new Exception(String.Format("賣方電話號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< TelephoneNumber />", _c0401Item.Main.Seller.TelephoneNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.FacsimileNumber) && _c0401Item.Main.Seller.FacsimileNumber.Length > 26)
            {
                return new Exception(String.Format("賣方傳真號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< FacsimileNumber />", _c0401Item.Main.Seller.FacsimileNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.EmailAddress) && _c0401Item.Main.Seller.EmailAddress.Length > 80)
            {
                return new Exception(String.Format("賣方電子郵件地址資料格式錯誤，長度最長為80碼，傳送資料：{0}，TAG:<  EmailAddress />", _c0401Item.Main.Seller.EmailAddress));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.CustomerNumber) && _c0401Item.Main.Seller.CustomerNumber.Length > 20)
            {
                return new Exception(String.Format("賣方客戶編號資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< CustomerNumber />", _c0401Item.Main.Seller.CustomerNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Seller.RoleRemark) && _c0401Item.Main.Seller.RoleRemark.Length > 40)
            {
                return new Exception(String.Format("賣方營業人角色註記資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< RoleRemark />", _c0401Item.Main.Seller.RoleRemark));
            }

            #endregion

            #region 買方

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.Address) && _c0401Item.Main.Buyer.Address.Length > 100)
            {
                return new Exception(String.Format("買方地址資料格式錯誤，長度最長為100碼，傳送資料：{0}，TAG:< Address />", _c0401Item.Main.Buyer.Address));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.PersonInCharge) && _c0401Item.Main.Buyer.PersonInCharge.Length > 30)
            {
                return new Exception(String.Format("買方負責人姓名資料格式錯誤，長度最長為30碼，傳送資料：{0}，TAG:< PersonInCharge />", _c0401Item.Main.Buyer.PersonInCharge));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.TelephoneNumber) && _c0401Item.Main.Buyer.TelephoneNumber.Length > 26)
            {
                return new Exception(String.Format("買方電話號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< TelephoneNumber />", _c0401Item.Main.Buyer.TelephoneNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.FacsimileNumber) && _c0401Item.Main.Buyer.FacsimileNumber.Length > 26)
            {
                return new Exception(String.Format("買方傳真號碼資料格式錯誤，長度最長為26碼，傳送資料：{0}，TAG:< FacsimileNumber />", _c0401Item.Main.Buyer.FacsimileNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.EmailAddress) && _c0401Item.Main.Buyer.EmailAddress.Length > 80)
            {
                return new Exception(String.Format("買方電子郵件地址資料格式錯誤，長度最長為80碼，傳送資料：{0}，TAG:<  EmailAddress />", _c0401Item.Main.Buyer.EmailAddress));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.CustomerNumber) && _c0401Item.Main.Buyer.CustomerNumber.Length > 20)
            {
                return new Exception(String.Format("買方客戶編號資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< CustomerNumber />", _c0401Item.Main.Buyer.CustomerNumber));
            }

            if (!String.IsNullOrEmpty(_c0401Item.Main.Buyer.RoleRemark) && _c0401Item.Main.Buyer.RoleRemark.Length > 40)
            {
                return new Exception(String.Format("買方營業人角色註記資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< RoleRemark />", _c0401Item.Main.Buyer.RoleRemark));
            }

            #endregion

            _buyer = new InvoiceBuyer
            {
                Name = _c0401Item.Main.Buyer.Name,
                ReceiptNo = _c0401Item.Main.Buyer.Identifier,
                CustomerID = _c0401Item.Main.Buyer.Identifier,
                CustomerName = _c0401Item.Main.Buyer.Name,
            };

            _buyer.ContactName = _c0401Item.Main.Buyer.Name;
            _buyer.Address = _c0401Item.Main.Buyer.Address;
            _buyer.Phone = _c0401Item.Main.Buyer.TelephoneNumber;
            _buyer.EMail = _c0401Item.Main.Buyer.EmailAddress != null ? _c0401Item.Main.Buyer.EmailAddress.Replace(' ', ',').Replace(';', ',').Replace('、', ',') : null;

            return null;
        }

        protected override Exception checkInvoiceDelivery()
        {

            _carrier = null;
            _donation = null;

            if (_c0401Item.Main.PrintMark=="Y")
            {
                if (String.IsNullOrEmpty(_c0401Item.Main.CarrierType))
                {
                    if (_c0401Item.Main.DonateMark == DonateMarkEnum.Item1)
                    {
                        if (_c0401Item.Main.Buyer.Identifier == "0000000000")
                        {
                            return new Exception(String.Format(MessageResources.AlertPrintedInvoiceDonation, _c0401Item.Main.PrintMark));
                        }

                        if (String.IsNullOrEmpty(_c0401Item.Main.NPOBAN))
                        {
                            return new Exception(String.Format(MessageResources.InvalidDonationTaker, _c0401Item.Main.NPOBAN));
                        }
                        _donation = new InvoiceDonation
                        {
                            AgencyCode = _c0401Item.Main.NPOBAN
                        };
                    }                    
                }
                else
                {
                    if (_c0401Item.Main.Buyer.Identifier == "0000000000")
                    {
                        return new Exception(String.Format(MessageResources.AlertPrintedInvoiceCarrierType, _c0401Item.Main.CarrierType));
                    }
                    else
                    {
                        Exception ex = checkPublicCarrier();
                        if (ex != null)
                            return ex;
                    }
                }
            }
            else
            {
                if (String.IsNullOrEmpty(_c0401Item.Main.CarrierType))
                {
                    if (_c0401Item.Main.Buyer.Identifier == "0000000000")
                    {
                        if (_c0401Item.Main.DonateMark == DonateMarkEnum.Item1)
                        {

                            if (String.IsNullOrEmpty(_c0401Item.Main.NPOBAN))
                            {
                                return new Exception(String.Format(MessageResources.InvalidDonationTaker, _c0401Item.Main.NPOBAN));
                            }
                            _donation = new InvoiceDonation
                            {
                                AgencyCode = _c0401Item.Main.NPOBAN
                            };
                        }
                        else
                        {
                            _carrier = new InvoiceCarrier
                            {
                                CarrierType = EIVOPlatformFactory.DefaultUserCarrierType,
                                CarrierNo = Guid.NewGuid().ToString()
                            };

                            _carrier.CarrierNo2 = _carrier.CarrierNo;

                        }
                    }
                    else
                    {
                        if (_seller.OrganizationStatus.PrintAll == true)
                        {
                            Exception ex = checkPrintAll();
                            if (ex != null)
                                return ex;

                            _c0401Item.Main.PrintMark = "Y";

                        }
                        else
                        {
                            return new Exception(String.Format(MessageResources.AlertInvoiceDelivery, _c0401Item.Main.InvoiceNumber));
                        }
                    }
                }
                else
                {
                    Exception ex = checkPublicCarrier();
                    if (ex != null)
                        return ex;

                    _carrier = new InvoiceCarrier
                    {
                        CarrierType = _c0401Item.Main.CarrierType,
                        CarrierNo = String.IsNullOrEmpty(_c0401Item.Main.CarrierId1) ? _c0401Item.Main.CarrierId2 : _c0401Item.Main.CarrierId1
                    };

                    if (String.IsNullOrEmpty(_c0401Item.Main.CarrierId2))
                    {
                        _carrier.CarrierNo2 = _carrier.CarrierNo;
                    }
                    else
                    {
                        _carrier.CarrierNo2 = _c0401Item.Main.CarrierId2;
                    }
                }
            }

            return null;

        }


        protected override Exception checkAmount()
        {
            #region 發票金額(Amount)

            if (_c0401Item.Amount.DiscountAmount.ToString().Length > 12)
            {
                return new Exception(String.Format("扣抵金額資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< DiscountAmount />", _c0401Item.Amount.DiscountAmount));
            }

            if (_c0401Item.Amount.OriginalCurrencyAmount.ToString().Length > 12)
            {
                return new Exception(String.Format("原幣金額資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< OriginalCurrencyAmount />", _c0401Item.Amount.OriginalCurrencyAmount));
            }

            if (_c0401Item.Amount.ExchangeRate.ToString().Length > 12)
            {
                return new Exception(String.Format("匯率資料格式錯誤，長度最長為12碼，傳送資料：{0}，TAG:< ExchangeRate />", _c0401Item.Amount.ExchangeRate));
            }

            if (_c0401Item.Amount.Currency.ToString().Length > 3)
            {
                return new Exception(String.Format("幣別資料格式錯誤，長度最長為3碼，傳送資料：{0}，TAG:< Currency />", _c0401Item.Amount.Currency));
            }

            #endregion

            //應稅銷售額
            if (_c0401Item.Amount.SalesAmount < 0 || decimal.Floor(_c0401Item.Amount.SalesAmount) != _c0401Item.Amount.SalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidSellingPrice, _c0401Item.Amount.SalesAmount));
            }

            if (_c0401Item.Amount.FreeTaxSalesAmount < 0 || decimal.Floor(_c0401Item.Amount.FreeTaxSalesAmount) != _c0401Item.Amount.FreeTaxSalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidFreeTaxAmount, _c0401Item.Amount.FreeTaxSalesAmount));
            }

            if (_c0401Item.Amount.ZeroTaxSalesAmount < 0 || decimal.Floor(_c0401Item.Amount.ZeroTaxSalesAmount) != _c0401Item.Amount.ZeroTaxSalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidZeroTaxAmount, _c0401Item.Amount.ZeroTaxSalesAmount));
            }


            if (_c0401Item.Amount.TaxAmount < 0 || decimal.Floor(_c0401Item.Amount.TaxAmount) != _c0401Item.Amount.TaxAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidTaxAmount, _c0401Item.Amount.TaxAmount));
            }

            if (_c0401Item.Amount.TotalAmount < 0 || decimal.Floor(_c0401Item.Amount.TotalAmount) != _c0401Item.Amount.TotalAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidTotalAmount, _c0401Item.Amount.TotalAmount));
            }

            //課稅別
            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)_c0401Item.Amount.TaxType))
            {
                return new Exception(String.Format(MessageResources.InvalidTaxType, _c0401Item.Amount.TaxType));
            }

            if (_c0401Item.Amount.TaxRate != 0m && _c0401Item.Amount.TaxRate != 0.05m)
            {
                return new Exception(String.Format(MessageResources.InvalidTaxRate, _c0401Item.Amount.TaxRate));
            }

            if ((int)_c0401Item.Amount.TaxType == (int)Naming.TaxTypeDefinition.零稅率)
            {
                if (!_c0401Item.Main.CustomsClearanceMarkSpecified)
                {
                    return new Exception(String.Format(MessageResources.AlertClearanceMarkZeroTax, null));
                }
            }

            return null;
        }


        protected override Exception checkInvoiceProductItems()
        {

            #region 發票品項(Detail)

            if (_c0401Item.Details != null && _c0401Item.Details.Count() > 0)
            {
                foreach (var item in _c0401Item.Details)
                {
                    if (!string.IsNullOrEmpty(item.Unit) && item.Unit.Length > 6)
                    {
                        return new Exception(String.Format("單位資料格式錯誤，長度最長為6碼，傳送資料：{0}，TAG:< Unit />", item.Unit));
                    }

                    if (!string.IsNullOrEmpty(item.Remark) && item.Remark.Length > 40)
                    {
                        return new Exception(String.Format("單一欄位備註資料格式錯誤，長度最長為40碼，傳送資料：{0}，TAG:< Remark />", item.Remark));
                    }

                    if (!string.IsNullOrEmpty(item.RelateNumber) && item.RelateNumber.Length > 20)
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

            _productItems = _c0401Item.Details.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                ItemNo = i.SequenceNumber,
                Piece = i.Quantity,
                PieceUnit = i.Unit,
                UnitCost = i.UnitPrice,
                Remark = i.Remark,
                TaxType = (byte)_c0401Item.Amount.TaxType,
                No = short.Parse(i.SequenceNumber)
            });


            foreach (var product in _productItems)
            {
                if (String.IsNullOrEmpty(product.InvoiceProduct.Brief) || product.InvoiceProduct.Brief.Length > 256)
                {
                    return new Exception(String.Format(MessageResources.InvalidProductDescription, product.InvoiceProduct.Brief));
                }


                if (!String.IsNullOrEmpty(product.PieceUnit) && product.PieceUnit.Length > 6)
                {
                    return new Exception(String.Format(MessageResources.InvalidPieceUnit, product.PieceUnit));
                }


                if (!Regex.IsMatch(product.UnitCost.ToString(), __DECIMAL_AMOUNT_PATTERN))
                {
                    return new Exception(String.Format(MessageResources.InvalidUnitPrice, product.UnitCost));
                }

                if (!Regex.IsMatch(product.CostAmount.ToString(), __DECIMAL_AMOUNT_PATTERN))
                {
                    return new Exception(String.Format(MessageResources.InvalidCostAmount, product.CostAmount));
                }

            }
            return null;
        }

        protected override Exception checkMandatoryFields()
        {

            if (String.IsNullOrEmpty(_c0401Item.Main.Seller.Identifier))
            {
                return new Exception("賣方-營業人統一編號錯誤，TAG:< Identifier />");
            }

            if (String.IsNullOrEmpty(_c0401Item.Main.Buyer.Identifier))
            {
                return new Exception("買方-營業人統一編號錯誤，TAG:< Identifier />");
            }

            if (String.IsNullOrEmpty(_c0401Item.Main.PrintMark))
            {
                return new Exception(MessageResources.InvalidPrintMark);
            }

            _c0401Item.Main.PrintMark = _c0401Item.Main.PrintMark.ToUpper();
            if (_c0401Item.Main.PrintMark != "Y" && _c0401Item.Main.PrintMark != "N")
            {
                return new Exception(MessageResources.InvalidPrintMark);
            }

            #region 發票主體(Main)

            if (!string.IsNullOrEmpty(_c0401Item.Main.CheckNumber) && _c0401Item.Main.CheckNumber.Length > 1)
            {
                return new Exception(String.Format("發票檢查碼資料格式錯誤，長度最長為1碼，傳送資料：{0}，TAG:< CheckNumber />", _c0401Item.Main.CheckNumber));
            }

            if ((Enum.IsDefined(typeof(Model.Schema.MIG3_1.C0401.BuyerRemarkEnum), (int)_c0401Item.Main.BuyerRemark)))
            {
                return new Exception(String.Format("買受人註記欄資料格式錯誤，傳送資料：{0}，TAG:< BuyerRemark />", _c0401Item.Main.BuyerRemark));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.MainRemark) && _c0401Item.Main.MainRemark.Length > 200)
            {
                return new Exception(String.Format("總備註資料格式錯誤，長度最長為200碼，傳送資料：{0}，TAG:< MainRemark />", _c0401Item.Main.MainRemark));
            }

            if ((Enum.IsDefined(typeof(Model.Schema.MIG3_1.C0401.CustomsClearanceMarkEnum), ((int)_c0401Item.Main.CustomsClearanceMark).ToString())))
            {
                return new Exception(String.Format("通關方式註記資料格式錯誤，傳送資料：{0}，TAG:< CustomerClearanceMark />", (int)_c0401Item.Main.CustomsClearanceMark));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.Category) && _c0401Item.Main.Category.Length > 2)
            {
                return new Exception(String.Format("沖帳別資料格式錯誤，長度最長為2碼，傳送資料：{0}，TAG:< Category />", _c0401Item.Main.Category));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.RelateNumber) && _c0401Item.Main.RelateNumber.Length > 20)
            {
                return new Exception(String.Format("相關號碼資料格式錯誤，長度最長為20碼，傳送資料：{0}，TAG:< RelateNumber />", _c0401Item.Main.RelateNumber));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.GroupMark) && _c0401Item.Main.GroupMark.Length > 1)
            {
                return new Exception(String.Format("彙開註記資料格式錯誤，長度最長為1碼，傳送資料：{0}，TAG:< GroupMark />", _c0401Item.Main.GroupMark));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.CarrierType) && _c0401Item.Main.CarrierType.Length > 6)
            {
                return new Exception(String.Format("載具類別號碼資料格式錯誤，長度最長為6碼，傳送資料：{0}，TAG:< CarrierType />", _c0401Item.Main.CarrierType));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.CarrierId1) && _c0401Item.Main.CarrierId1.Length > 64)
            {
                return new Exception(String.Format("載具顯碼Id資料格式錯誤，長度最長為64碼，傳送資料：{0}，TAG:< CarrierId1 />", _c0401Item.Main.CarrierId1));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.CarrierId2) && _c0401Item.Main.CarrierId2.Length > 64)
            {
                return new Exception(String.Format("載具隱碼Id資料格式錯誤，長度最長為64碼，傳送資料：{0}，TAG:< CarrierId2 />", _c0401Item.Main.CarrierId2));
            }

            if (!string.IsNullOrEmpty(_c0401Item.Main.NPOBAN) && _c0401Item.Main.NPOBAN.Length > 10)
            {
                return new Exception(String.Format("發票捐贈對象統一編號資料格式錯誤，長度最長為10碼，傳送資料：{0}，TAG:< NPOBAN />", _c0401Item.Main.NPOBAN));
            }

            #endregion


            return null;
        }

    }
}
