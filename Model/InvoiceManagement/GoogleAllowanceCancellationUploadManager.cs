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
    public class GoogleAllowanceCancellationUploadManager : GoogleUploadManager<InvoiceAllowance,GoogleInvoiceAllowance>
    {

        protected override void initialize()
        {
            __COLUMN_COUNT = 4;
        }

        protected override void doSave()
        {
            this.SubmitChanges();
        }

        protected override bool validate(GoogleInvoiceAllowance item)
        {

            string[] column = item.Columns;

            String allowanceNo = column[1];
            item.Allowance = this.EntityList.Where(i => i.AllowanceNumber == allowanceNo).FirstOrDefault();

            if (item.Allowance == null)
            {
                item.Status = String.Join("、", item.Status, "折讓證明單不存在");
                _bResult = false;
            }
            else if (_items.Any(a => a.Columns[1] == column[1]))
            {
                item.Status = String.Join("、", item.Status, "作廢折讓證明單號碼重複匯入");
                _bResult = false;
            }
            else if (item.Allowance.InvoiceAllowanceCancellation != null)
            {
                item.Status = String.Join("、", item.Status, "折讓證明單已作廢");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(column[3]))
            {
                item.Status = String.Join("、", item.Status, "未指定作廢原因");
                _bResult = false;
            }

            DateTime dateValue;
            if (!DateTime.TryParseExact(column[0], "yyyy/M/d", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
            {
                item.Status = String.Join("、", item.Status, "日期錯誤");
                _bResult = false;
            }

            if (item.Allowance != null)
            {
                if (String.IsNullOrEmpty(column[2]))
                {
                    if (!item.Allowance.InvoiceItem.InvoiceBuyer.IsB2C())
                    {
                        item.Status = String.Join("、", item.Status, "對方統編錯誤");
                        _bResult = false;
                    }
                }
                else
                {
                    if (column[2] != item.Allowance.InvoiceItem.InvoiceBuyer.ReceiptNo)
                    {
                        item.Status = String.Join("、", item.Status, "對方統編錯誤");
                        _bResult = false;
                    }
                }
            }

            if (item.Allowance != null && item.Allowance.InvoiceAllowanceCancellation == null)
            {
                item.Allowance.InvoiceAllowanceCancellation = new InvoiceAllowanceCancellation
                {
                    CancelDate = dateValue,
                    //Remark = column[3]
                    CancelReason = column[3],
                };

                var doc = new DerivedDocument
                {
                    CDS_Document = new CDS_Document
                    {
                        DocType = (int)Naming.DocumentTypeDefinition.E_AllowanceCancellation,
                        DocDate = DateTime.Now,
                        DocumentOwner = new DocumentOwner
                        {
                            OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                        }
                    },
                    SourceID = item.Allowance.AllowanceID
                };
                this.GetTable<DerivedDocument>().InsertOnSubmit(doc);
            }


            return _bResult;
        }

    }
}
