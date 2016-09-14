using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;

namespace Model.InvoiceManagement
{
    public class GoogleInvoiceUploadManager : GoogleUploadManager<InvoiceItem,GoogleInvoiceItem>
    {
        private InvoicePurchaseOrderUpload _uploadItem;
        private InvoiceNoInterval _currentInterval;
        private int _currentNo;
        private DateTime _uploadInvoiceDate;
        private Organization _seller;

        protected override void initialize()
        {
            __COLUMN_COUNT = 15;   //若CSV檔要增加備註欄位,則此數值要調成16   2013/01/07 Howard
            _uploadInvoiceDate = DateTime.Now;
        }

        public DateTime UploadInvoiceDate
        {
            get
            {
                return _uploadInvoiceDate;
            }
            set
            {
                _uploadInvoiceDate = value;
            }
        }

        public override void ParseData(UserProfileMember userProfile, string fileName, Encoding encoding)
        {
            _uploadItem = new InvoicePurchaseOrderUpload
            {
                FilePath = fileName,
                UID = userProfile.UID,
                UploadDate = DateTime.Now
            };

            _userProfile = userProfile;
            _currentInterval = getCurrentInterval();
            _seller = this.GetTable<Organization>().Where(o => o.CompanyID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID).First();

            base.ParseData(userProfile, fileName, encoding);

        }

        protected override void doSave()
        {
                this.EntityList.InsertAllOnSubmit(_items.Select(i => i.Invoice));
                this.SubmitChanges();
        }

        protected override bool validate(GoogleInvoiceItem item)
        {
            String[] column = item.Columns;

            bool isB2C = false;
            if (String.IsNullOrEmpty(column[7]))
            {
                isB2C = true;
            }
            else  if (column[7].Length!=8 || !ValueValidity.ValidateString(column[7], 20))
            {
                item.Status = String.Join("、", item.Status, "對方統編格式錯誤");
                _bResult = false;
            }

            item.Invoice = new InvoiceItem
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Invoice,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID,
                        ClientID = "GoogleGUI"
                    },
                    ChannelID = (int)Naming.ChannelIDType.FromWeb
                },
                DonateMark = "0",
                InvoiceDate = _uploadInvoiceDate,
                InvoiceType = _seller.OrganizationStatus.SettingInvoiceType.HasValue ? (byte)_seller.OrganizationStatus.SettingInvoiceType.Value : (byte)Naming.InvoiceTypeDefinition.電子計算機,
                SellerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID,
                RandomNo = ValueValidity.GenerateRandomCode(4),
                //Remark = column[15],   "尚未確認暫時註銷 2012/01/07  Howard"

                //DonationID = donatory != null ? donatory.CompanyID : (int?)null,
                InvoicePurchaseOrder = new InvoicePurchaseOrder
                {
                    InvoicePurchaseOrderUpload = _uploadItem,
                    OrderNo = column[0] + column[2]
                },
                InvoiceSeller = new InvoiceSeller
                {
                    Name = _seller.CompanyName,
                    ReceiptNo = _seller.ReceiptNo,
                    Address = _seller.Addr,
                    ContactName = _seller.ContactName,
                    CustomerName = _seller.CompanyName,
                    EMail = _seller.ContactEmail,
                    Fax = _seller.Fax,
                    Phone = _seller.Phone,
                    PersonInCharge = _seller.UndertakerName,
                    SellerID = _seller.CompanyID
                },
                InvoiceBuyer = new InvoiceBuyer
                {
                    CustomerID = column[2],
                    ReceiptNo = isB2C ? "0000000000" : column[7],
                    Name = column[10],
                    CustomerName = column[10],
                    ContactName = column[11],
                    EMail = column[14],
                    Address = column[12],
                    Phone = column[13]
                },
            };

            checkPrintMarkAndCarrier(item, isB2C);
            checkInputFields(item);
            checkInvoiceNo(item);
            checkOrderNo(item);
            checkAmountValue(item,isB2C);
            checkDateValue(item);

            return _bResult;
        }

        private void checkPrintMarkAndCarrier(GoogleInvoiceItem item,bool isB2C)
        {
            ///B2C的發票=>列印或歸戶
            ///
            if (isB2C)
            {
                if (_seller.OrganizationStatus.PrintAll == true)
                {
                    item.Invoice.PrintMark = "Y";
                }
                else
                {
                    item.Invoice.PrintMark = "N";
                    String carrierNo = Guid.NewGuid().ToString();
                    item.Invoice.InvoiceCarrier = new InvoiceCarrier
                    {
                        CarrierType = EIVOPlatformFactory.DefaultUserCarrierType,
                        CarrierNo = carrierNo,
                        CarrierNo2 = carrierNo
                    };
                }
            }
            ///B2B的發票=>全列印
            ///
            else
            {
                item.Invoice.PrintMark = "Y";
            }
        }


        private void checkDateValue(GoogleInvoiceItem item)
        {
            String[] column = item.Columns;
            DateTime dateValue;
            if (DateTime.TryParseExact(column[1], "yyyy/M/d", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
            {
                item.Invoice.InvoicePurchaseOrder.PurchaseDate = dateValue;
            }
            else
            {
                item.Status = String.Join("、", item.Status, "日期錯誤");
                _bResult = false;
            }

            decimal costAmt;
            if (decimal.TryParse(column[4], out costAmt))
            {
                int piece;
                if (int.TryParse(column[5], out piece))
                {
                    InvoiceProductItem productItem = new InvoiceProductItem
                    {
                        InvoiceProduct = new InvoiceProduct { Brief = column[3] },
                        CostAmount = costAmt,
                        Piece = piece,
                        UnitCost = costAmt,
                        TaxType = 1,
                        No = 0,
                        //Remark=column[15]   "尚未確認暫時註銷 2012/01/07  Howard"
                    };
                    item.Invoice.InvoiceDetails.Add(new InvoiceDetail
                    {
                        InvoiceProduct = productItem.InvoiceProduct
                    });
                }
                else
                {
                    item.Status = String.Join("、", item.Status, "數量錯誤");
                    _bResult = false;
                }
            }
            else
            {
                item.Status = String.Join("、", item.Status, "單價(未稅)錯誤");
                _bResult = false;
            }
        }

        private void checkAmountValue(GoogleInvoiceItem item,  bool isB2C)
        {
            String[] column = item.Columns;
            decimal totalAmt;
            if (decimal.TryParse(column[9], out totalAmt))
            {
                if (!isB2C)
                {
                    decimal taxAmt, salesAmt;
                    if (decimal.TryParse(column[8], out taxAmt))
                    {
                        if (decimal.TryParse(column[6], out salesAmt))
                        {
                            item.Invoice.InvoiceAmountType = new InvoiceAmountType
                            {
                                TotalAmount = totalAmt,
                                SalesAmount = salesAmt,
                                TaxAmount = taxAmt,
                                TaxRate = 0.05m,
                                TaxType = 1,
                                TotalAmountInChinese = Utility.ValueValidity.MoneyShow(totalAmt)
                            };
                        }
                        else
                        {
                            item.Status = String.Join("、", item.Status, "金額錯誤");
                            _bResult = false;
                        }
                    }
                    else
                    {
                        item.Status = String.Join("、", item.Status, "稅額錯誤");
                        _bResult = false;
                    }
                }
                else
                {
                    item.Invoice.InvoiceAmountType = new InvoiceAmountType
                    {
                        TotalAmount = totalAmt,
                        SalesAmount = decimal.Round(totalAmt / 1.05m, 0, MidpointRounding.AwayFromZero),
                        TaxRate = 0.05m,
                        TaxType = 1,
                        TotalAmountInChinese = Utility.ValueValidity.MoneyShow(totalAmt)
                    };
                    item.Invoice.InvoiceAmountType.TaxAmount = item.Invoice.InvoiceAmountType.TotalAmount - item.Invoice.InvoiceAmountType.SalesAmount;
                }
            }
            else
            {
                item.Status = String.Join("、", item.Status, "含稅金額錯誤");
                _bResult = false;
            }
        }

        private void checkInputFields(GoogleInvoiceItem item)
        {
            String[] column = item.Columns;
            if (column[0].Length != 11)
            {
                item.Status = String.Join("、", item.Status, "序號非11位數");
                _bResult = false;
            }
            else if (!ValueValidity.ValidateString(column[0], 20))
            {
                item.Status = String.Join("、", item.Status, "序號格式錯誤");
                _bResult = false;
            }

            if(String.IsNullOrEmpty(column[2]) || !ValueValidity.ValidateString(column[2],20))
            {
                item.Status = String.Join("、", item.Status, "GoogleID格式錯誤");
                _bResult = false;
            }

            if(String.IsNullOrEmpty(column[3]))
            {
                item.Status = String.Join("、", item.Status, "品項不得為空白");
                _bResult = false;
            }


            if(String.IsNullOrEmpty(column[10]))
            {
                item.Status = String.Join("、", item.Status, "名稱不得為空白");
                _bResult = false;
            }

            if(String.IsNullOrEmpty(column[11]))
            {
                item.Status = String.Join("、", item.Status, "聯絡人不得為空白");
                _bResult = false;
            }

            if(String.IsNullOrEmpty(column[12]))
            {
                item.Status = String.Join("、", item.Status, "地址不得為空白");
                _bResult = false;
            }

                        if(String.IsNullOrEmpty(column[13]))
            {
                item.Status = String.Join("、", item.Status, "連絡電話不得為空白");
                _bResult = false;
            }

            if(String.IsNullOrEmpty(column[14]) || !ValueValidity.ValidateString(column[14],16))
            {
                item.Status = String.Join("、", item.Status, "Email格式不正確");
                _bResult = false;
            }

            if (_items.Any(g => g.Columns[0] == column[0] && g.Columns[2] == column[2]))
            {
                item.Status = String.Join("、", item.Status, "Key(序號+GoogleID)重複");
                _bResult = false;
            }
        }

        private void checkInvoiceNo(GoogleInvoiceItem item)
        {
            if (_currentInterval == null)
            {
                item.Status = String.Join("、", item.Status, "未設定發票字軌或發票號碼已用完");
                _bResult = false;
                _breakParsing = true;
                return;
            }
            _currentNo = _currentInterval.StartNo + _currentInterval.InvoiceNoAssignments.Count;
            item.Invoice.InvoiceNoAssignment = new InvoiceNoAssignment
            {
                InvoiceNoInterval = _currentInterval,
                InvoiceNo = _currentNo
            };

            item.Invoice.TrackCode = _currentInterval.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode;
            item.Invoice.No = String.Format("{0:00000000}", _currentNo);

            _currentNo++;
            if (_currentNo > _currentInterval.EndNo)
            {
                _currentInterval = getNextInterval(_currentInterval.IntervalID);
            }
        }

        private void checkOrderNo(GoogleInvoiceItem item)
        {
            if (this.GetTable<InvoicePurchaseOrder>().Any(p => p.OrderNo == item.Invoice.InvoicePurchaseOrder.OrderNo
                && p.InvoiceItem.SellerID == item.Invoice.SellerID))
            {
                item.Status = String.Join("、", item.Status, "匯入資料重複");
                _bResult = false;
            }

        }


        private InvoiceNoInterval getCurrentInterval()
        {
            int currentYear = _uploadInvoiceDate.Year;
            int currentPeriodNo = (_uploadInvoiceDate.Month + 1) / 2;
            var intervalItems = this.GetTable<InvoiceNoInterval>().Where(n => n.InvoiceTrackCodeAssignment.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == currentYear
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == currentPeriodNo);
            return intervalItems.Where(n => n.InvoiceNoAssignments.Count == 0 || n.StartNo + n.InvoiceNoAssignments.Count < n.EndNo).OrderBy(n => n.StartNo).FirstOrDefault();
        }

        private InvoiceNoInterval getNextInterval(int intervalID)
        {
            int currentYear = _uploadInvoiceDate.Year;
            int currentPeriodNo = (_uploadInvoiceDate.Month + 1) / 2;
            var intervalItems = this.GetTable<InvoiceNoInterval>().Where(n => n.InvoiceTrackCodeAssignment.SellerID == _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year == currentYear
                && n.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo == currentPeriodNo);
            return intervalItems.Where(n => (n.InvoiceNoAssignments.Count == 0 || n.StartNo + n.InvoiceNoAssignments.Count < n.EndNo) && n.IntervalID > intervalID).OrderBy(n => n.StartNo).FirstOrDefault();
        }
    }

    public class GoogleInvoiceItem : IItemUpload
    {
        public String Status { get; set; }
        public InvoiceItem Invoice { get; set; }
        public String[] Columns { get; set; }
        public Naming.UploadStatusDefinition UploadStatus { get; set; }
    }

}
