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
using Model.UploadManagement;
using DataAccessLayer.basis;

namespace Model.InvoiceManagement
{
    public class CsvInvoiceCancellationUploadManager : CsvUploadManager<EIVOEntityDataContext, InvoiceItem, ItemUpload<InvoiceItem>>
    {
        private InvoiceCancellationUpload _uploadItem;
        private int _sellerID;

        public const String __Fields = "日期,發票號碼,作廢原因";

        public enum FieldIndex
        {
            日期 = 0,
            發票號碼 = 1,
            作廢原因 = 2
        }

        private DateTime _uploadCancellationDate;

        public CsvInvoiceCancellationUploadManager(GenericManager<EIVOEntityDataContext> manager,int sellerID)
            : base(manager)
        {
            _sellerID = sellerID;
        }

        public CsvInvoiceCancellationUploadManager(int sellerID)
            : this(null,sellerID)
        {

        }


        protected override void initialize()
        {
            __COLUMN_COUNT = 3; //日期,發票號碼,作廢原因
            _uploadCancellationDate = DateTime.Now;
        }

        public override void ParseData(UserProfileMember userProfile, string fileName, Encoding encoding)
        {
            _uploadItem = new InvoiceCancellationUpload
            {
                FilePath = fileName,
                UploadDate = DateTime.Now
            };

            base.ParseData(userProfile, fileName, encoding);
        }

        protected override void doSave()
        {
            this.SubmitChanges();
        }

        protected override bool validate(ItemUpload<InvoiceItem> item)
        {
            String[] column = item.Columns;

            if (column[(int)FieldIndex.發票號碼].Length != 10)
            {
                item.Status = String.Join("、", item.Status, "發票號碼錯誤");
                _bResult = false;
            }
            else
            {

                String trackCode = column[(int)FieldIndex.發票號碼].Substring(0, 2);
                String no = column[(int)FieldIndex.發票號碼].Substring(2);
                item.Entity = this.EntityList.Where(i => i.No == no && i.TrackCode == trackCode).FirstOrDefault();

                if (item.Entity == null)
                {
                    item.Status = String.Join("、", item.Status, "發票不存在");
                    _bResult = false;
                }
                else if (_items.Any(a => a.Columns[(int)FieldIndex.發票號碼] == column[(int)FieldIndex.發票號碼]))
                {
                    item.Status = String.Join("、", item.Status, "作廢發票號碼重複匯入");
                    _bResult = false;
                }
                else if (item.Entity.InvoiceCancellation != null)
                {
                    item.Status = String.Join("、", item.Status, "發票已作廢");
                    _bResult = false;
                }
                else if (item.Entity.SellerID != _sellerID)
                {
                    item.Status = String.Join("、", item.Status, "作廢發票開立人與原發票開立人不符");
                    _bResult = false;
                }
            }

            if (String.IsNullOrEmpty(column[(int)FieldIndex.作廢原因]))
            {
                item.Status = String.Join("、", item.Status, "未指定作廢原因");
                _bResult = false;
            }

            DateTime dateValue;
            if (!DateTime.TryParseExact(column[(int)FieldIndex.日期], "yyyy/M/d", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateValue))
            {
                item.Status = String.Join("、", item.Status, "日期錯誤");
                _bResult = false;
            }

            if (item.Entity != null && item.Entity.InvoiceCancellation == null)
            {
                item.Entity.InvoiceCancellation = new InvoiceCancellation
                {
                    CancelDate = _uploadCancellationDate,
                    //Remark = column[(int)FieldIndex.作廢原因],
                    CancellationNo = column[(int)FieldIndex.發票號碼],
                    CancelReason = column[(int)FieldIndex.作廢原因],
                };

                var doc = new DerivedDocument
                {
                    CDS_Document = new CDS_Document
                    {
                        DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                        DocDate = DateTime.Now,
                        DocumentOwner = new DocumentOwner
                        {
                            OwnerID = item.Entity.SellerID.Value
                        }
                    },
                    SourceID = item.Entity.InvoiceID
                };
                this.GetTable<DerivedDocument>().InsertOnSubmit(doc);

                new InvoiceCancellationUploadList
                {
                    InvoiceCancellation = item.Entity.InvoiceCancellation,
                    InvoiceCancellationUpload = _uploadItem
                };
            }

            return _bResult;
        }

    }
}
