using DataAccessLayer.basis;
using Model.DataEntity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Model.InvoiceManagement.zhTW
{
    public static partial class CancelInvoiceRootValidator_C0501
    {
        //檢查基本必填項目(作廢發票)
        public static Exception CheckMandatoryFields(this Model.Schema.MIG3_1.C0501.CancelInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out InvoiceItem invoice, out DateTime cancelDate)
        {
            invoice = null;
            cancelDate = default(DateTime);

            if (String.IsNullOrEmpty(invItem.CancelInvoiceNumber) || !Regex.IsMatch(invItem.CancelInvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
            {
                return new Exception(String.Format("作廢發票號碼錯誤，作廢發票號碼長度應為10碼(含字軌)，傳送資料：{0}，TAG：< CancelInvoiceNumber />", invItem.CancelInvoiceNumber));
            }
            String invNo, trackCode;
            trackCode = invItem.CancelInvoiceNumber.Substring(0, 2);
            invNo = invItem.CancelInvoiceNumber.Substring(2);

            DateTime invoiceDate;
            if (String.IsNullOrEmpty(invItem.InvoiceDate))
            {
                return new Exception("發票日期錯誤，TAG：< InvoiceDate/>");
            }

            if (!DateTime.TryParseExact(invItem.InvoiceDate, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                return new Exception(String.Format("發票日期格式錯誤(YYYY/MM/DD)，傳送資料：{0}，TAG：< InvoiceDate/>", invItem.InvoiceDate));
            }

            invoice = mgr.GetTable<InvoiceItem>().Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

            if (invoice == null)
            {
                return new Exception(String.Format("發票號碼不存在:{0}", invItem.CancelInvoiceNumber));
            }

            if (invItem.SellerId != invoice.InvoiceSeller.ReceiptNo)
            {
                return new Exception(String.Format("上傳作廢資料賣方統編不符；發票：{0},上傳資料{1}", invoice.InvoiceSeller.ReceiptNo, invItem.SellerId));
            }

            if (invoice.InvoiceCancellation != null)
            {
                return new Exception(String.Format("作廢發票已存在,發票號碼:{0}", invItem.CancelInvoiceNumber));
            }


            if (String.IsNullOrEmpty(invItem.SellerId))
            {
                return new Exception("賣方統一編號不可為空白，TAG：< SellerId />");
            }

            if (string.IsNullOrEmpty(invItem.BuyerId))
            {
                return new Exception("買方統一編號不可為空白，TAG:< BuyerId />");
            }

            var Buyer = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == invItem.BuyerId).FirstOrDefault();

            if(invItem.BuyerId != invoice.InvoiceBuyer.ReceiptNo)
            {
                return new Exception(String.Format("上傳作廢資料買方統編不符；發票：{0},上傳資料{1}", invoice.InvoiceBuyer.ReceiptNo, invItem.BuyerId));
            }

            if (String.IsNullOrEmpty(invItem.CancelDate))
            {
                return new Exception("作廢日期，TAG：< CancelDate />");
            }

            if (String.IsNullOrEmpty(invItem.CancelTime.ToString()))
            {
                return new Exception("作廢時間，TAG：< CancelTime />");
            }
            if (!DateTime.TryParseExact(invItem.CancelDate, "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            {
                return new Exception(String.Format("作廢發票日期格式錯誤；上傳資料:{0}", invItem.CancelDate));
            }
            //if (!DateTime.TryParseExact(String.Format("{0} {1}", invItem.CancelDate, invItem.CancelTime.TimeOfDay), "yyyyMMdd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            //{
            //    return new Exception(String.Format("作廢發票日期、發票時間格式錯誤(YYYY/MM/DD HH:mm:ss)；傳送資料:{0} {1}", invItem.CancelDate, invItem.CancelTime));
            //}
            cancelDate += invItem.CancelTime.TimeOfDay;
            if (String.IsNullOrEmpty(invItem.CancelReason))
            {
                return new Exception("作廢原因不可空白，TAG：< CancelReason />");
            }

            if (invItem.CancelReason.Length > 256)
            {
                return new Exception(String.Format("資料格式長度最少1碼，最多20碼，傳送資料：{0}，TAG：< CancelReason />", invItem.CancelReason));
            }

            //備註
            if (invItem.Remark != null && invItem.Remark.Length > 200)
            {
                return new Exception(String.Format("備註資料長度不可超過200，傳送資料：{0}，TAG：< Remark />", invItem.Remark));
            }

            return null;
        }
    }
}
