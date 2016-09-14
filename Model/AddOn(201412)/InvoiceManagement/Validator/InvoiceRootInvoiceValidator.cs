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
using Utility;

namespace Model.InvoiceManagement.Validator
{
    public partial class InvoiceRootInvoiceValidator
    {
        public readonly static String[] __InvoiceTypeList = { "01", "02", "03", "04", "05", "06", "07", "08" };
        public static string __DECIMAL_AMOUNT_PATTERN = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public static string __CELLPHONE_BARCODE = "3J0002";
        public static String __自然人憑證 = "CQ0001";

        protected GenericManager<EIVOEntityDataContext> _mgr;
        protected OrganizationToken _owner;

        protected InvoiceRootInvoice _invItem;

        protected InvoiceItem _newItem;
        protected Organization _seller;
        protected InvoicePurchaseOrder _order;
        protected InvoiceBuyer _buyer;
        protected InvoiceCarrier _carrier;
        protected InvoiceDonation _donation;
        protected IEnumerable<InvoiceProductItem> _productItems;

        protected bool _isAutoTrackNo;
        protected Dictionary<int, TrackNoManager> _trackNoManagerList;
        protected Func<Exception>[, , ,] _deliveryCheck;
      

        public InvoiceRootInvoiceValidator(GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner)
        {
            _mgr = mgr;
            _owner = owner;

            initializeDeliveryCheck();
        }

        private void initializeDeliveryCheck()
        {
            _deliveryCheck = new Func<Exception>[2, 2, 2, 2];

            #region 列印Y

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.Yes, (int)IsB2C.Yes, (int)DonationIntent.Yes] = () =>
                {
                    return new Exception(String.Format(MessageResources.AlertPrintedInvoiceCarrierType, _invItem.CarrierType));
                };

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.Yes, (int)IsB2C.Yes, (int)DonationIntent.No] = 
                _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.Yes, (int)IsB2C.Yes, (int)DonationIntent.Yes];

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.Yes, (int)IsB2C.No, (int)DonationIntent.Yes] = () =>
            {
                return new Exception(String.Format(MessageResources.AlertDoationInvoiceCarryType, _invItem.CarrierType));
            };

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.Yes, (int)IsB2C.No, (int)DonationIntent.No] = checkPublicCarrier;

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.No, (int)IsB2C.Yes, (int)DonationIntent.Yes] = () =>
            {
                return new Exception(String.Format(MessageResources.AlertPrintedInvoiceDonation, _invItem.PrintMark));
            };

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.No, (int)IsB2C.No, (int)DonationIntent.Yes] = () => 
            {
                return new Exception(String.Format(MessageResources.AlertDoationInvoiceCarryType, _invItem.CarrierType));
            };

            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.No, (int)IsB2C.Yes, (int)DonationIntent.No] = 
            
            _deliveryCheck[(int)PrintedMark.Yes, (int)CarrierIntent.No, (int)IsB2C.No, (int)DonationIntent.No] = () => { return null; };

            #endregion 

            #region 列印N

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.Yes, (int)IsB2C.Yes, (int)DonationIntent.Yes] = () =>
                {
                    Exception ex = checkCarrierDataIsComplete();
                    if (ex != null)
                        return ex;

                    if (String.IsNullOrEmpty(_invItem.NPOBAN))
                    {
                        return new Exception(String.Format(MessageResources.InvalidDonationTaker, _invItem.NPOBAN));
                    }
                    else
                    {
                        if(_invItem.NPOBAN.Length<3||_invItem.NPOBAN.Length>7)
                            return new Exception(String.Format(MessageResources.InvalidDonationTaker, _invItem.NPOBAN));
                    

                    }

                    _donation = new InvoiceDonation
                    {
                        AgencyCode = _invItem.NPOBAN
                    };

                    return null;
                };

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.Yes, (int)IsB2C.Yes, (int)DonationIntent.No] = () =>
                {
                    Exception ex = checkCarrierDataIsComplete();
                    if (ex != null)
                        return ex;

                    //if (_invItem.CarrierType == __CELLPHONE_BARCODE)
                    //{
                    //    ex = checkPublicCarrier();
                    //    if (ex != null)
                    //        return ex;
                    //}

                    //_carrier = new InvoiceCarrier
                    //{
                    //    CarrierType = _invItem.CarrierType,
                    //    CarrierNo = String.IsNullOrEmpty(_invItem.CarrierId1) ? _invItem.CarrierId2 : _invItem.CarrierId1
                    //};

                    //if (String.IsNullOrEmpty(_invItem.CarrierId2))
                    //{
                    //    _carrier.CarrierNo2 = _carrier.CarrierNo;
                    //}
                    //else
                    //{
                    //    _carrier.CarrierNo2 = _invItem.CarrierId2;
                    //}

                    return null;
                };

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.Yes, (int)IsB2C.No, (int)DonationIntent.Yes] = checkPublicCarrier;
            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.Yes, (int)IsB2C.No, (int)DonationIntent.No] = checkPublicCarrier;

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.No, (int)IsB2C.Yes, (int)DonationIntent.Yes] = () =>
                {
                    if (String.IsNullOrEmpty(_invItem.NPOBAN))
                    {
                        return new Exception(String.Format(MessageResources.InvalidDonationTaker, _invItem.NPOBAN));
                    }
                    else
                    {
                        if (_invItem.NPOBAN.Length < 3 || _invItem.NPOBAN.Length > 7)
                            return new Exception(String.Format(MessageResources.InvalidDonationTaker, _invItem.NPOBAN));


                    }
                    _donation = new InvoiceDonation
                    {
                        AgencyCode = _invItem.NPOBAN
                    };

                    return null;

                };

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.No, (int)IsB2C.Yes, (int)DonationIntent.No] = checkPrintAll;

            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.No, (int)IsB2C.No, (int)DonationIntent.Yes] =() => { return null; };
            _deliveryCheck[(int)PrintedMark.No, (int)CarrierIntent.No, (int)IsB2C.No, (int)DonationIntent.No] = checkPrintAll;

            #endregion

        }

        public InvoiceItem InvoiceItem
        {
            get
            {
                return _newItem;
            }
        }

        public virtual void StartAutoTrackNo()
        {
            _trackNoManagerList = new Dictionary<int, TrackNoManager>();
            _isAutoTrackNo = true;
        }

        public virtual void EndAutoTrackNo()
        {
            _isAutoTrackNo = false;
            if (_trackNoManagerList != null)
            {
                foreach (var item in _trackNoManagerList)
                {
                    item.Value.Dispose();
                }
                _trackNoManagerList.Clear();
            }
        }


        public virtual Exception Validate(InvoiceRootInvoice dataItem)
        {
            _invItem = dataItem;

            Exception ex;

            _seller = null;
            _newItem = null;

            if ((ex = checkBusiness()) != null)
            {
                return ex;
            }

            if (_isAutoTrackNo)
            {
                if ((ex = checkDataNumber()) != null)
                {
                    return ex;
                }
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


        protected virtual Exception checkInvoice()
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
                InvoiceType = byte.Parse(_invItem.InvoiceType),
                SellerID = _seller.CompanyID,
                CustomsClearanceMark = _invItem.CustomsClearanceMark,
                InvoiceSeller = new InvoiceSeller
                {
                    Name = _seller.CompanyName,
                    ReceiptNo = _seller.ReceiptNo,
                    Address = _seller.Addr,
                    ContactName = _seller.ContactName,
                    //CustomerID = String.IsNullOrEmpty(_invItem.GoogleId) ? "" : _invItem.GoogleId,
                    CustomerName = _seller.CompanyName,
                    EMail = _seller.ContactEmail,
                    Fax = _seller.Fax,
                    Phone = _seller.Phone,
                    PersonInCharge = _seller.UndertakerName,
                    SellerID = _seller.CompanyID,
                },
                InvoiceBuyer = _buyer,
                RandomNo = _invItem.RandomNumber,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = _invItem.DiscountAmount,
                    SalesAmount = (_invItem.TaxType == 1) ? _invItem.SalesAmount :
                                  (_invItem.TaxType == 2) ? _invItem.ZeroTaxSalesAmount :
                                  (_invItem.TaxType == 3) ? _invItem.FreeTaxSalesAmount : _invItem.SalesAmount,
                    TaxAmount = _invItem.TaxAmount,
                    TaxRate = _invItem.TaxRate,
                    TaxType = _invItem.TaxType,
                    TotalAmount = _invItem.TotalAmount,
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(_invItem.TotalAmount),
                },
                InvoiceCarrier = _carrier,
                InvoiceDonation = _donation,
                PrintMark = _invItem.PrintMark,
            };

            if (_order != null)
            {
                _newItem.InvoicePurchaseOrder = _order;
            }

            _newItem.InvoiceDetails.AddRange(_productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct,
            }));

            if (_isAutoTrackNo)
            {
                try
                {
                    TrackNoManager trackNoMgr;
                    if (_trackNoManagerList.ContainsKey(_seller.CompanyID))
                    {
                        trackNoMgr = _trackNoManagerList[_seller.CompanyID];
                    }
                    else
                    {
                        trackNoMgr = new TrackNoManager(_mgr, _seller.CompanyID);
                        _trackNoManagerList[_seller.CompanyID] = trackNoMgr;
                    }

                    if (!trackNoMgr.CheckInvoiceNo(_newItem))
                    {
                        return new Exception(String.Format(MessageResources.AlertNullTrackNoInterval, _seller.ReceiptNo));
                    }
                    else
                    {
                        _newItem.InvoiceDate = DateTime.Now;
                    }

                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
            {
                DateTime invoiceDate = DateTime.Now;

                if (String.IsNullOrEmpty(_invItem.InvoiceDate))
                {
                    return new Exception(MessageResources.AlertInvoiceDate);
                }

                if (String.IsNullOrEmpty(_invItem.InvoiceTime))
                {
                    return new Exception(MessageResources.AlertInvoiceTime);
                }

                if (!DateTime.TryParseExact(String.Format("{0} {1}", _invItem.InvoiceDate, _invItem.InvoiceTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
                {
                    return new Exception(String.Format(MessageResources.AlertInvoiceDateTime, _invItem.InvoiceDate, _invItem.InvoiceTime));
                }

                _newItem.InvoiceDate = invoiceDate;

                if (_invItem.InvoiceNumber == null || !Regex.IsMatch(_invItem.InvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
                {
                    return new Exception(String.Format(MessageResources.AlertInvoiceNumber, _invItem.InvoiceNumber));
                }

                _newItem.TrackCode = _invItem.InvoiceNumber.Substring(0, 2);
                _newItem.No = _invItem.InvoiceNumber.Substring(2);
                int periodNo = (invoiceDate.Month + 1) / 2;

                if (!_mgr.GetTable<InvoiceTrackCode>().Any(t => t.Year == invoiceDate.Year && t.PeriodNo == periodNo && t.TrackCode == _newItem.TrackCode))
                {
                    return new Exception(String.Format(MessageResources.InvalidTrackCode, _invItem.InvoiceNumber));
                }

                if (_mgr.GetTable<InvoiceItem>().Any(i => i.TrackCode == _newItem.TrackCode && i.No == _newItem.No))
                {
                    return new Exception(MessageResources.AlertInvoiceDuplicated);
                }
            }

            if (_invItem.CustomerDefined != null)
            {
                _newItem.InvoiceItemExtension = new InvoiceItemExtension 
                {
                    ProjectNo = _invItem.CustomerDefined.ProjectNo,
                    PurchaseNo = _invItem.CustomerDefined.PurchaseNo
                };

                if (_invItem.CustomerDefined.StampDutyFlagSpecified)
                {
                    _newItem.InvoiceItemExtension.StampDutyFlag = (byte)_invItem.CustomerDefined.StampDutyFlag;
                }
            }

            return null;
        }

        protected virtual Exception checkDataNumber()
        {
            _order = null;
            if (String.IsNullOrEmpty(_invItem.DataNumber))
            {
                return new Exception(MessageResources.AlertDataNumber);
            }

            if (_invItem.DataNumber.Length > 60)
            {
                return new Exception(String.Format(MessageResources.AlertDataNumberLimitedLength, _invItem.DataNumber));
            }

            if (_mgr.GetTable<InvoicePurchaseOrder>().Any(d => d.OrderNo == _invItem.DataNumber
                && d.InvoiceItem.SellerID==_seller.CompanyID))
            {
                return new Exception(String.Format(MessageResources.AlertDataNumberDuplicated, _invItem.DataNumber));
            }

            if (String.IsNullOrEmpty(_invItem.DataDate))
            {
                return new Exception(MessageResources.AlertDataDate);
            }

            DateTime dataDate;
            if (!DateTime.TryParseExact(_invItem.DataDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out dataDate))
            {
                return new Exception(String.Format(MessageResources.AlertDataDateFormat, _invItem.DataDate));
            }

            _order = new InvoicePurchaseOrder
            {
                OrderNo = _invItem.DataNumber,
                PurchaseDate = dataDate
            };

            return null;
        }



        protected virtual Exception checkBusiness()
        {
            _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _invItem.SellerId).FirstOrDefault();
            if (_seller == null)
            {
                return new Exception(String.Format(MessageResources.AlertInvalidSeller, _invItem.SellerId));
            }

            if (_seller.CompanyID != _owner.CompanyID && !_mgr.GetTable<InvoiceIssuerAgent>().Any(a=>a.AgentID==_owner.CompanyID && a.IssuerID==_seller.CompanyID))
            {
                //return new Exception(String.Format(MessageResources.AlertSellerSignature, _invItem.SellerId));
                return new Exception(String.Format(MessageResources.InvalidSellerOrAgent, _invItem.SellerId, _owner.Organization.ReceiptNo));
            }

            if (_invItem.BuyerId == "0000000000")
            {
                //if (_invItem.BuyerName == null || Encoding.GetEncoding(950).GetBytes(_invItem.BuyerName).Length != 4)
                //{
                //    return new Exception(String.Format(MessageResources.InvalidBuyerName, _invItem.BuyerName));
                //}
            }
            else if (_invItem.BuyerId == null || !Regex.IsMatch(_invItem.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format(MessageResources.InvalidBuyerId, _invItem.BuyerId));
            }
            else if(!_invItem.BuyerId.CheckRegno())
            {
                return new Exception(String.Format(MessageResources.InvalidReceiptNo, _invItem.BuyerId));
            }
            else if (_invItem.PrintMark != "Y" && (String.IsNullOrEmpty(_invItem.BuyerName) || _invItem.BuyerName.Length > 60))
            {
                return new Exception(String.Format(MessageResources.InvalidBuyerNameLengthLimit, _invItem.BuyerName));
            }

            if (String.IsNullOrEmpty(_invItem.RandomNumber))
            {
                _invItem.RandomNumber = String.Format("{0:ffff}", DateTime.Now); //ValueValidity.GenerateRandomCode(4)
            }
            else if (!Regex.IsMatch(_invItem.RandomNumber, "^[0-9]{4}$"))
            {
                return new Exception(String.Format(MessageResources.InvalidRandomNumber, _invItem.RandomNumber));
            }

            return checkBusinessDetails();
        }

        protected bool checkPublicCarrierId(String carrierId)
        {
            return carrierId != null && carrierId.Length == 8 && carrierId.StartsWith("/");
        }

        protected virtual Exception checkPublicCarrier()
        {
            if (_invItem.CarrierType == __CELLPHONE_BARCODE)
            {
                if (checkPublicCarrierId(_invItem.CarrierId1))
                {
                    _carrier = new InvoiceCarrier
                        {
                            CarrierType = _invItem.CarrierType,
                            CarrierNo = _invItem.CarrierId1,
                            CarrierNo2 = _invItem.CarrierId1
                        };

                    return null;
                }
                else if (checkPublicCarrierId(_invItem.CarrierId2))
                {
                    _carrier = new InvoiceCarrier
                        {
                            CarrierType = _invItem.CarrierType,
                            CarrierNo = _invItem.CarrierId2,
                            CarrierNo2 = _invItem.CarrierId2
                        };

                    return null;
                }
            }
            else if (_invItem.CarrierType == __自然人憑證)
            {
                if (_invItem.CarrierId1 != null && Regex.IsMatch(_invItem.CarrierId1, "^[A-Z]{2}[0-9]{14}$"))
                {
                    _carrier = new InvoiceCarrier
                    {
                        CarrierType = _invItem.CarrierType,
                        CarrierNo = _invItem.CarrierId1,
                        CarrierNo2 = _invItem.CarrierId1
                    };

                    return null;
                }
                else if (_invItem.CarrierId2 != null && Regex.IsMatch(_invItem.CarrierId2, "^[A-Z]{2}[0-9]{14}$"))
                {
                    _carrier = new InvoiceCarrier
                    {
                        CarrierType = _invItem.CarrierType,
                        CarrierNo = _invItem.CarrierId2,
                        CarrierNo2 = _invItem.CarrierId2
                    };

                    return null;
                }
            }

            return new Exception(String.Format(MessageResources.InvalidPublicCarrierType, _invItem.CarrierType, _invItem.CarrierId1, _invItem.CarrierId2));
        }

        protected virtual Exception checkPrintAll()
        {

            if (_seller.OrganizationStatus.PrintAll == true)
            {
                //全列印
                //if (_invItem.Contact == null)
                //{
                //    return new Exception(MessageResources.AlertContactToPrintAll);
                //}

                _invItem.PrintMark = "Y";

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

            return null;

        }

        protected virtual Exception checkBusinessDetails()
        {
            _buyer = new InvoiceBuyer
            {
                Name = _invItem.BuyerName,
                ReceiptNo = _invItem.BuyerId,
                CustomerID = String.IsNullOrEmpty(_invItem.GoogleId) ? "" : _invItem.GoogleId,
                CustomerName = _invItem.BuyerName,
            };           

            if (_invItem.Contact != null)
            {

                if (!String.IsNullOrEmpty(_invItem.Contact.Name) && _invItem.Contact.Name.Length > 64)
                {
                    return new Exception(String.Format(MessageResources.InvalidContactName, _invItem.Contact.Name));
                }

                if (!String.IsNullOrEmpty(_invItem.Contact.Address) && _invItem.Contact.Address.Length > 128)
                {
                    return new Exception(String.Format(MessageResources.InvalidContactAddress, _invItem.Contact.Address));
                }

                if (!String.IsNullOrEmpty(_invItem.Contact.Email) && _invItem.Contact.Email.Length > 512)
                {
                    return new Exception(String.Format(MessageResources.InvalidContactEMail, _invItem.Contact.Email));
                }

                if (!String.IsNullOrEmpty(_invItem.Contact.TEL) && _invItem.Contact.TEL.Length > 64)
                {
                    return new Exception(String.Format(MessageResources.InvalidContactPhone, _invItem.Contact.TEL));
                }

                _buyer.ContactName =  _invItem.Contact.Name;
                _buyer.Address = _invItem.Contact.Address;
                _buyer.Phone = _invItem.Contact.TEL;
                _buyer.EMail = _invItem.Contact.Email != null ? _invItem.Contact.Email.Replace(';', ',').Replace('、', ',').Replace(' ', ',') : null;
            }

            return null;
        }

        protected virtual Exception checkInvoiceDelivery()
        {

            _carrier = null;
            _donation = null;

            var checkFunc = _deliveryCheck[Convert.ToInt32(_invItem.PrintMark == "Y"),
                Convert.ToInt32(!String.IsNullOrEmpty(_invItem.CarrierType) 
                    && !(String.IsNullOrEmpty(_invItem.CarrierId1) && String.IsNullOrEmpty(_invItem.CarrierId2))),
                Convert.ToInt32(_invItem.BuyerId == "0000000000"),
                Convert.ToInt32(_invItem.DonateMark == "1")];

            return checkFunc();

        }


        protected virtual Exception checkAmount()
        {
            //應稅銷售額
            if (_invItem.SalesAmount < 0 || decimal.Floor(_invItem.SalesAmount) != _invItem.SalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidSellingPrice, _invItem.SalesAmount));
            }

            if (_invItem.FreeTaxSalesAmount < 0 || decimal.Floor(_invItem.FreeTaxSalesAmount) != _invItem.FreeTaxSalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidFreeTaxAmount, _invItem.FreeTaxSalesAmount));
            }

            if (_invItem.ZeroTaxSalesAmount < 0 || decimal.Floor(_invItem.ZeroTaxSalesAmount) != _invItem.ZeroTaxSalesAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidZeroTaxAmount, _invItem.ZeroTaxSalesAmount));
            }


            if (_invItem.TaxAmount < 0 || decimal.Floor(_invItem.TaxAmount) != _invItem.TaxAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidTaxAmount, _invItem.TaxAmount));
            }

            if (_invItem.TotalAmount < 0 || decimal.Floor(_invItem.TotalAmount) != _invItem.TotalAmount)
            {
                return new Exception(String.Format(MessageResources.InvalidTotalAmount, _invItem.TotalAmount));
            }

            //課稅別
            if (!Enum.IsDefined(typeof(Naming.TaxTypeDefinition), (int)_invItem.TaxType))
            {
                return new Exception(String.Format(MessageResources.InvalidTaxType, _invItem.TaxType));
            }

            if (_invItem.TaxRate < 0m)
            {
                return new Exception(String.Format(MessageResources.InvalidTaxRate, _invItem.TaxRate));
            }

            if (_invItem.TaxType == (byte)Naming.TaxTypeDefinition.零稅率)
            {
                if (String.IsNullOrEmpty(_invItem.CustomsClearanceMark))
                {
                    return new Exception(String.Format(MessageResources.AlertClearanceMarkZeroTax, _invItem.CustomsClearanceMark));
                }
                else if (_invItem.CustomsClearanceMark != "1" && _invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format(MessageResources.AlertClearanceMarkExport, _invItem.CustomsClearanceMark));
                }
            }
            else if (!String.IsNullOrEmpty(_invItem.CustomsClearanceMark))
            {
                if (_invItem.CustomsClearanceMark != "1" && _invItem.CustomsClearanceMark != "2")
                {
                    return new Exception(String.Format(MessageResources.AlertClearanceMarkExport, _invItem.CustomsClearanceMark));
                }
            }

            return null;
        }


        protected virtual Exception checkInvoiceProductItems()
        {
            if (_invItem.InvoiceItem == null || _invItem.InvoiceItem.Length == 0)
            {
                return new Exception(MessageResources.InvalidInvoiceDetails);
            }

            short seqNo = 0;
            _productItems = _invItem.InvoiceItem.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i.Description },
                CostAmount = i.Amount,
                ItemNo = i.Item,
                Piece = i.Quantity,
                PieceUnit = i.Unit,
                UnitCost = i.UnitPrice,
                Remark = i.Remark,
                TaxType = i.TaxType,
                No = (seqNo++)
            }).ToList();


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

        protected virtual Exception checkMandatoryFields()
        {

            if (_invItem.BuyerId == "0000000000" && _invItem.DonateMark != "0" && _invItem.DonateMark != "1")
            {
                return new Exception(String.Format(MessageResources.InvalidDonationMark, _invItem.DonateMark));
            }

            if (String.IsNullOrEmpty(_invItem.PrintMark))
            {
                //return new Exception(MessageResources.InvalidPrintMark);
                _invItem.PrintMark = "N";
            }
            else
            {
                _invItem.PrintMark = _invItem.PrintMark.ToUpper();
                if (_invItem.PrintMark != "Y" && _invItem.PrintMark != "N")
                {
                    return new Exception(MessageResources.InvalidPrintMark);
                }
            }

            if(!__InvoiceTypeList.Contains(_invItem.InvoiceType))
            {
                return new Exception(String.Format(MessageResources.InvalidInvoiceType, _invItem.InvoiceType));
            }


            return null;
        }

        protected virtual Exception checkCarrierDataIsComplete()
        {

            if (String.IsNullOrEmpty(_invItem.CarrierType))
            {
                return new Exception(MessageResources.AlertInvoiceCarrierComplete);
            }
            else
            {
                if (_invItem.CarrierType.Length > 6 || (_invItem.CarrierId1 != null && _invItem.CarrierId1.Length > 64) || (_invItem.CarrierId2 != null && _invItem.CarrierId2.Length > 64))
                    return new Exception(String.Format(MessageResources.AlertInvoiceCarrierLength, _invItem.CarrierType, _invItem.CarrierId1, _invItem.CarrierId2));

                _carrier = new InvoiceCarrier
                {
                    CarrierType = _invItem.CarrierType
                };

                if (!String.IsNullOrEmpty(_invItem.CarrierId1))
                {
                    if (_invItem.CarrierId1.Length > 64)
                        return new Exception(String.Format(MessageResources.AlertInvoiceCarrierLength, _invItem.CarrierType, _invItem.CarrierId1, _invItem.CarrierId2));

                    _carrier.CarrierNo = _invItem.CarrierId1;
                }

                if (!String.IsNullOrEmpty(_invItem.CarrierId2))
                {
                    if (_invItem.CarrierId2.Length > 64)
                        return new Exception(String.Format(MessageResources.AlertInvoiceCarrierLength, _invItem.CarrierType, _invItem.CarrierId1, _invItem.CarrierId2));

                    _carrier.CarrierNo2 = _invItem.CarrierId2;
                }

                if (_carrier.CarrierNo == null)
                {
                    if (_carrier.CarrierNo2 == null)
                    {
                        return new Exception(MessageResources.AlertInvoiceCarrierComplete);
                    }
                    else
                    {
                        _carrier.CarrierNo = _carrier.CarrierNo2;
                    }
                }
                else
                {
                    if (_carrier.CarrierNo2 == null)
                        _carrier.CarrierNo2 = _carrier.CarrierNo;
                }
            }

            return null;
        }

    }

    public enum IsB2C
    {
        No = 0,
        Yes = 1
    }
    public enum PrintedMark
    {
        No = 0,
        Yes = 1
    }
    public enum CarrierIntent
    {
        No = 0,
        Yes = 1
    }
    public enum DonationIntent
    {
        No = 0,
        Yes = 1
    }

}
