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
    public class GoogleInvoiceCancellationUploadManager : GoogleUploadManager<InvoiceItem,GoogleInvoiceItem>
    {
        private InvoiceCancellationUpload _uploadItem;
        protected override void initialize()
        {
            __COLUMN_COUNT = 3;
        }

        public override void ParseData(UserProfileMember userProfile, string fileName, Encoding encoding)
        {
            _uploadItem = new InvoiceCancellationUpload
            {
                FilePath = fileName,
                UID = userProfile.UID,
                UploadDate = DateTime.Now
            };

            base.ParseData(userProfile, fileName, encoding);
        }

        protected override void doSave()
        {
            this.SubmitChanges();
        }

        protected override bool validate(GoogleInvoiceItem item)
        {
            String[] column = item.Columns;

            if (column[1].Length != 10)
            {
                item.Status = String.Join("、", item.Status, "發票號碼錯誤");
                _bResult = false;
            }
            else
            {

                String trackCode = column[1].Substring(0, 2);
                String no = column[1].Substring(2);
                item.Invoice = this.EntityList.Where(i => i.No == no && i.TrackCode == trackCode).FirstOrDefault();

                if (item.Invoice == null)
                {
                    item.Status = String.Join("、", item.Status, "發票不存在");
                    _bResult = false;
                }
                else if (_items.Any(a => a.Columns[1] == column[1]))
                {
                    item.Status = String.Join("、", item.Status, "作廢發票號碼重複匯入");
                    _bResult = false;
                }
                else if (item.Invoice.InvoiceCancellation != null)
                {
                    item.Status = String.Join("、", item.Status, "發票已作廢");
                    _bResult = false;
                }
            }

            if (String.IsNullOrEmpty(column[2]))
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

            if (item.Invoice != null && item.Invoice.InvoiceCancellation == null)
            {
                item.Invoice.InvoiceCancellation = new InvoiceCancellation
                {
                    CancelDate = dateValue,
                    //Remark = column[2],
                    CancellationNo = column[1],
                    CancelReason = column[2],
                };

                new InvoiceCancellationUploadList
                {
                    InvoiceCancellation = item.Invoice.InvoiceCancellation,
                    InvoiceCancellationUpload = _uploadItem
                };

                var doc = new DerivedDocument
                {
                    CDS_Document = new CDS_Document
                    {
                        DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                        DocDate = DateTime.Now,
                        DocumentOwner = new DocumentOwner
                        {
                            OwnerID = _userProfile.CurrentUserRole.OrganizationCategory.CompanyID
                        }
                    },
                    SourceID = item.Invoice.InvoiceID
                };
                this.GetTable<DerivedDocument>().InsertOnSubmit(doc);
            }

            return _bResult;
        }

    }
}
