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
    public class GoogleAllowanceUploadManager : GoogleUploadManager<InvoiceAllowance, GoogleInvoiceAllowance>
    {


        protected override void initialize()
        {
            __COLUMN_COUNT = 18;
        }

        protected override void doSave()
        {
            this.EntityList.InsertAllOnSubmit(_items.Select(i => i.Allowance));
            this.SubmitChanges();
        }

        protected override bool validate(GoogleInvoiceAllowance item)
        {
            string[] column = item.Columns;
            item.Allowance = new InvoiceAllowance
            {
                CDS_Document = new CDS_Document
                {
                    DocDate = DateTime.Now,
                    DocType = (int)Naming.DocumentTypeDefinition.E_Allowance,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                    }
                },
                AllowanceDate = DateTime.Now,
                InvoiceAllowanceSeller = new InvoiceAllowanceSeller
                {
                    SellerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID,
                    Name = _userProfile.CurrentUserRole.OrganizationCategory.Organization.CompanyName,
                    Address = _userProfile.CurrentUserRole.OrganizationCategory.Organization.Addr,
                    EMail = _userProfile.CurrentUserRole.OrganizationCategory.Organization.ContactEmail,
                    Fax = _userProfile.CurrentUserRole.OrganizationCategory.Organization.Fax,
                    ReceiptNo =_userProfile.CurrentUserRole.OrganizationCategory.Organization.ReceiptNo,
                    PersonInCharge=_userProfile.CurrentUserRole.OrganizationCategory.Organization.UndertakerName,
                    RoleRemark=_userProfile.CurrentUserRole.OrganizationCategory.Organization.ContactTitle,
                    Phone = _userProfile.CurrentUserRole.OrganizationCategory.Organization.Phone,
                    CustomerName = _userProfile.CurrentUserRole.OrganizationCategory.Organization.CompanyName,
                    ContactName = _userProfile.CurrentUserRole.OrganizationCategory.Organization.ContactName
                    
                },
                SellerId = _userProfile.CurrentUserRole.OrganizationCategory.Organization.ReceiptNo
            };

            item.Allowance.InvoiceAllowanceDetails.Add(new InvoiceAllowanceDetail
            {
                InvoiceAllowanceItem = new InvoiceAllowanceItem
                {
                    TaxType = 1,
                    No = 0
                }
            });

            checkInputFields(item);
            checkDateValue(item);
            var invItem = checkInvoiceNo(item);
            if (invItem != null)
                checkAmountValue(item, invItem);

            return _bResult;
        }

        private void checkAmountValue(GoogleInvoiceAllowance item, InvoiceItem invItem)
        {
            String[] column = item.Columns;
            bool isB2C = invItem.InvoiceBuyer.IsB2C();

            decimal totalAmt;
            decimal salesAmt;
            decimal taxAmt = 0;

            if (decimal.TryParse(column[12], out totalAmt))
            {
                if (isB2C)
                {
                    salesAmt = decimal.Round(totalAmt / 1.05m, 0, MidpointRounding.AwayFromZero);
                    taxAmt = totalAmt - salesAmt;
                    item.Allowance.TotalAmount = totalAmt;
                    item.Allowance.TaxAmount = taxAmt;
                }
                else
                {
                    if (decimal.TryParse(column[11], out taxAmt))
                    {
                        if (decimal.TryParse(column[9], out salesAmt))
                        {
                            item.Allowance.TotalAmount = totalAmt;
                            item.Allowance.TaxAmount = taxAmt;
                        }
                        else
                        {
                            item.Status = String.Join("、", item.Status, "未稅金額錯誤");
                            _bResult = false;
                        }
                    }
                    else
                    {
                        item.Status = String.Join("、", item.Status, "稅額錯誤");
                        _bResult = false;
                    }
                }
            }
            else
            {
                item.Status = String.Join("、", item.Status, "含稅金額錯誤");
                _bResult = false;
            }


            decimal costAmt;
            if (decimal.TryParse(column[7], out costAmt))
            {
                decimal piece;
                if (decimal.TryParse(column[8], out piece))
                {
                    item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.Amount = totalAmt;
                    item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.Piece = piece;
                    item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.Tax = taxAmt;
                    item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.UnitCost = costAmt;
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

        private void checkDateValue(GoogleInvoiceAllowance item)
        {
            string[] column = item.Columns;
            DateTime dateValue;
            if (DateTime.TryParseExact(column[1], "yyyy/M/d", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
            {
                item.Allowance.AllowanceDate = dateValue;
            }
            else
            {
                item.Status = String.Join("、", item.Status, "折讓單日期錯誤");
                _bResult = false;
            }

            DateTime invoiceDate;
            if (DateTime.TryParseExact(column[4], "yyyy/M/d", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.InvoiceDate = invoiceDate;
            }
            else
            {
                item.Status = String.Join("、", item.Status, "發票日期錯誤");
                _bResult = false;
            }
        }

        private InvoiceItem checkInvoiceNo(GoogleInvoiceAllowance item)
        {
            InvoiceItem invItem = null;

            if (item.Columns[5].Length != 10 || !ValueValidity.ValidateString(item.Columns[5], 14))
            {
                item.Status = String.Join("、", item.Status, "發票號碼格式錯誤");
                _bResult = false;
                return null;
            }
            else
            {
                String trackCode = item.Columns[5].Substring(0, 2);
                String no = item.Columns[5].Substring(2);

                invItem = this.GetTable<InvoiceItem>().Where(i => i.TrackCode == trackCode && i.No == no).FirstOrDefault();
            }

            if (invItem == null)
            {
                item.Status = String.Join("、", item.Status, "發票不存在");
                _bResult = false;
                return null;
            }

            if (invItem.InvoiceBuyer.CustomerID != item.Columns[2])
            {
                item.Status = String.Join("、", item.Status, "Google ID不存在");
                _bResult = false;
            }

            item.Allowance.InvoiceID = invItem.InvoiceID;
            item.Allowance.InvoiceItem = invItem;
            if ((invItem.InvoiceBuyer.IsB2C() && item.Columns[10] == String.Empty) || invItem.InvoiceBuyer.ReceiptNo == item.Columns[10])
            {
                item.Allowance.BuyerId = invItem.InvoiceBuyer.ReceiptNo;
                item.Allowance.InvoiceAllowanceBuyer = new InvoiceAllowanceBuyer
                {
                    Address = invItem.InvoiceBuyer.Address,
                    ContactName = invItem.InvoiceBuyer.ContactName,
                    CustomerID = invItem.InvoiceBuyer.CustomerID,
                    CustomerName = invItem.InvoiceBuyer.CustomerName,
                    Name = invItem.InvoiceBuyer.Name,
                    EMail = invItem.InvoiceBuyer.EMail,
                    Phone = invItem.InvoiceBuyer.Phone,
                    PostCode = invItem.InvoiceBuyer.PostCode,
                    ReceiptNo = invItem.InvoiceBuyer.ReceiptNo
                };
            }
            else
            {
                item.Status = String.Join("、", item.Status, "買受人統編錯誤");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(item.Columns[13]))
            {
                item.Status = String.Join("、", item.Status, "買受人名稱不得為空白");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(item.Columns[14]))
            {
                item.Status = String.Join("、", item.Status, "聯絡人不得為空白");
                _bResult = false;
            }
            if (String.IsNullOrEmpty(item.Columns[15]))
            {
                item.Status = String.Join("、", item.Status, "買受人地址不得為空白");
                _bResult = false;
            }
            if (String.IsNullOrEmpty(item.Columns[16]))
            {
                item.Status = String.Join("、", item.Status, "連絡電話不得為空白");
                _bResult = false;
            }
            if (String.IsNullOrEmpty(item.Columns[17]) && !ValueValidity.ValidateString(item.Columns[17],16))
            {
                item.Status = String.Join("、", item.Status, "EMail錯誤");
                _bResult = false;
            }

            item.Allowance.AllowanceType = invItem.InvoiceBuyer.IsB2C() ? (byte)2 : (byte)1;
            item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.TaxType = invItem.InvoiceDetails[0].InvoiceProduct.InvoiceProductItem[0].TaxType;
            item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.ProductItemID = invItem.InvoiceDetails[0].InvoiceProduct.InvoiceProductItem[0].ItemID;
            item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.InvoiceNo = invItem.TrackCode + invItem.No;
            if (item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.InvoiceDate.HasValue && invItem.InvoiceDate.Value.Date != item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.InvoiceDate.Value.Date)
            {
                item.Status = String.Join("、", item.Status, "發票日期錯誤");
                _bResult = false;
            }

            return invItem;
        }

        private void checkInputFields(GoogleInvoiceAllowance item)
        {
            String[] column = item.Columns;

            if (String.IsNullOrEmpty(column[3]))
            {
                item.Status = String.Join("、", item.Status, "未指定折讓號碼");
                _bResult = false;
            }
            else
            {
                item.Allowance.AllowanceNumber = column[3];
                if (EntityList.Any(a => a.AllowanceNumber == column[3]))
                {
                    item.Status = String.Join("、", item.Status, "該折讓單號碼資料庫已存在");
                    _bResult = false;
                }
            }

            if (String.IsNullOrEmpty(column[6]))
            {
                item.Status = String.Join("、", item.Status, "未指定品名");
                _bResult = false;
            }
            else
            {
                item.Allowance.InvoiceAllowanceDetails[0].InvoiceAllowanceItem.OriginalDescription = column[6];
            }

            if (String.IsNullOrEmpty(column[0]) || !ValueValidity.ValidateString(column[0], 20))
            {
                item.Status = String.Join("、", item.Status, "序號格式錯誤");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(column[2]) || !ValueValidity.ValidateString(column[2], 20))
            {
                item.Status = String.Join("、", item.Status, "GoogleID格式錯誤");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(column[3]) || !ValueValidity.ValidateString(column[3], 14))
            {
                item.Status = String.Join("、", item.Status, "折讓單號碼格式錯誤");
                _bResult = false;
            }

            if (_items.Any(a => a.Columns[3] == column[3]))
            {
                item.Status = String.Join("、", item.Status, "折讓單號碼重複匯入");
                _bResult = false;
            }

        }

    }

    public class GoogleInvoiceAllowance : IItemUpload
    {
        public String Status { get; set; }
        public InvoiceAllowance Allowance { get; set; }
        public String[] Columns { get; set; }
        public Naming.UploadStatusDefinition UploadStatus { get; set; }
    }
}
