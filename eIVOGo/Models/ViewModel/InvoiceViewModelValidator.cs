using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DataAccessLayer.basis;
using Model.DataEntity;
using Model.InvoiceManagement;
using Model.Locale;
using Model.Resource;
using Model.Schema.EIVO;
using Utility;

namespace eIVOGo.Models.ViewModel
{
    public partial class InvoiceViewModelValidator<TEntity>
        where TEntity : class, new()
    {
        public readonly static String[] __InvoiceTypeList = { "01", "02", "03", "04", "05", "06", "07", "08" };
        public static string __DECIMAL_AMOUNT_PATTERN = "^-?\\d{1,12}(.[0-9]{0,4})?$";
        public static string __CELLPHONE_BARCODE = "3J0002";
        public static String __自然人憑證 = "CQ0001";

        protected ModelSource<TEntity> _mgr;
        protected Organization _owner;

        protected InvoiceViewModel _invItem;

        protected InvoiceItem _newItem;
        protected Organization _seller;
        protected InvoicePurchaseOrder _order;
        protected InvoiceBuyer _buyer;
        protected InvoiceCarrier _carrier;
        protected InvoiceDonation _donation;
        protected IEnumerable<InvoiceProductItem> _productItems;
      

        public InvoiceViewModelValidator(ModelSource<TEntity> mgr, Organization owner)
        {
            _mgr = mgr;
            _owner = owner;
        }

        public InvoiceItem InvoiceItem
        {
            get
            {
                return _newItem;
            }
        }


        public virtual Exception Validate(InvoiceViewModel dataItem)
        {
            _invItem = dataItem;

            Exception ex;

            _seller = null;
            _newItem = null;

            if ((ex = checkBusiness()) != null)
            {
                return ex;
            }

            if ((ex = checkDataNumber()) != null)
            {
                return ex;
            }

            if ((ex = checkAmount()) != null)
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
                InvoiceType = _invItem.InvoiceType,
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
                RandomNo = _invItem.RandomNo,
                InvoiceAmountType = new InvoiceAmountType
                {
                    DiscountAmount = _invItem.DiscountAmount,
                    SalesAmount = _invItem.SalesAmount,
                    TaxAmount = _invItem.TaxAmount,
                    TaxRate = _invItem.TaxRate,
                    TaxType = _invItem.TaxType,
                    TotalAmount = _invItem.TotalAmount,
                    TotalAmountInChinese = Utility.ValueValidity.MoneyShow(_invItem.TotalAmount),
                },
                InvoiceCarrier = _carrier,
                InvoiceDonation = _donation,
                PrintMark = _carrier == null ? "Y" : "N",
            };

            if (_order != null)
            {
                _newItem.InvoicePurchaseOrder = _order;
            }

            _newItem.InvoiceDetails.AddRange(_productItems.Select(p => new InvoiceDetail
            {
                InvoiceProduct = p.InvoiceProduct,
            }));

            try
            {
                using (TrackNoManager trackNoMgr = new TrackNoManager(_mgr, _seller.CompanyID))
                {
                    if (!trackNoMgr.ApplyInvoiceDate(_invItem.InvoiceDate.Value) || !trackNoMgr.CheckInvoiceNo(_newItem))
                    {
                        return new Exception(String.Format(MessageResources.AlertNullTrackNoInterval, _seller.ReceiptNo));
                    }
                    else
                    {
                        _newItem.InvoiceDate = _invItem.InvoiceDate;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex;
            }

            return null;
        }

        protected virtual Exception checkDataNumber()
        {
            _order = null;
            if (String.IsNullOrEmpty(_invItem.DataNumber))
            {
                return null;
            }


            if (_mgr.GetTable<InvoicePurchaseOrder>().Any(d => d.OrderNo == _invItem.DataNumber
                && d.InvoiceItem.SellerID==_seller.CompanyID))
            {
                return new Exception(String.Format(MessageResources.AlertDataNumberDuplicated, _invItem.DataNumber));
            }


            _order = new InvoicePurchaseOrder
            {
                OrderNo = _invItem.DataNumber
            };

            return null;
        }



        protected virtual Exception checkBusiness()
        {
            _seller = _mgr.GetTable<Organization>().Where(o => o.CompanyID == _invItem.SellerID).FirstOrDefault();
            if (_seller == null)
            {
                _seller = _mgr.GetTable<Organization>().Where(o => o.ReceiptNo == _invItem.SellerReceiptNo).FirstOrDefault();
            }
            if (_seller == null)
            {
                return new Exception(String.Format(MessageResources.AlertInvalidSeller, _invItem.SellerReceiptNo));
            }

            if (_seller.CompanyID != _owner.CompanyID && !_mgr.GetTable<InvoiceIssuerAgent>().Any(a=>a.AgentID==_owner.CompanyID && a.IssuerID==_seller.CompanyID))
            {
                return new Exception(String.Format(MessageResources.InvalidSellerOrAgent, _invItem.SellerReceiptNo, _owner.ReceiptNo));
            }

            if (!String.IsNullOrEmpty(_invItem.BuyerReceiptNo))
            {
                if (!Regex.IsMatch(_invItem.BuyerReceiptNo, "^[0-9]{8}$"))
                {
                    return new Exception(String.Format(MessageResources.InvalidBuyerId, _invItem.BuyerReceiptNo));
                }
                else if (!_invItem.BuyerReceiptNo.CheckRegno())
                {
                    return new Exception(String.Format(MessageResources.InvalidReceiptNo, _invItem.BuyerReceiptNo));
                }
            }

            if (String.IsNullOrEmpty(_invItem.RandomNo))
            {
                _invItem.RandomNo = String.Format("{0:ffff}", DateTime.Now); //ValueValidity.GenerateRandomCode(4)
            }
            else if (!Regex.IsMatch(_invItem.RandomNo, "^[0-9]{4}$"))
            {
                return new Exception(String.Format(MessageResources.InvalidRandomNumber, _invItem.RandomNo));
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

        protected virtual Exception checkBusinessDetails()
        {
            _buyer = new InvoiceBuyer
            {
                Name = ((int)4).CreateRandomStringCode(),
                ReceiptNo = String.IsNullOrEmpty(_invItem.BuyerReceiptNo) ? "0000000000" : _invItem.BuyerReceiptNo,
                CustomerID = _invItem.CustomerID,
                CustomerName = _invItem.BuyerName,
            };           

            return null;
        }



        protected virtual Exception checkAmount()
        {
            //應稅銷售額
            if (!_invItem.SalesAmount.HasValue || _invItem.SalesAmount < 0 || decimal.Floor(_invItem.SalesAmount.Value) != _invItem.SalesAmount.Value)
            {
                return new Exception(String.Format(MessageResources.InvalidSellingPrice, _invItem.SalesAmount));
            }


            if (!_invItem.TaxAmount.HasValue || _invItem.TaxAmount < 0 || decimal.Floor(_invItem.TaxAmount.Value) != _invItem.TaxAmount.Value)
            {
                return new Exception(String.Format(MessageResources.InvalidTaxAmount, _invItem.TaxAmount));
            }

            if (!_invItem.TotalAmount.HasValue || _invItem.TotalAmount < 0 || decimal.Floor(_invItem.TotalAmount.Value) != _invItem.TotalAmount.Value)
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
            if (_invItem.Brief == null || _invItem.Brief.Length == 0)
            {
                return new Exception(MessageResources.InvalidInvoiceDetails);
            }

            short seqNo = 0;
            _productItems = _invItem.Brief.Select(i => new InvoiceProductItem
            {
                InvoiceProduct = new InvoiceProduct { Brief = i },
                CostAmount = _invItem.CostAmount[seqNo],
                ItemNo = _invItem.ItemNo[seqNo],
                Piece = _invItem.Piece[seqNo],
                UnitCost = _invItem.UnitCost[seqNo],
                Remark = _invItem.ItemRemark[seqNo],
                TaxType = _invItem.TaxType,
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

            if (_invItem.DonateMark != "0" && _invItem.DonateMark != "1")
            {
                return new Exception(String.Format(MessageResources.InvalidDonationMark, _invItem.DonateMark));
            }


            if(!__InvoiceTypeList.Contains(String.Format("{0:00}",_invItem.InvoiceType)))
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
}
