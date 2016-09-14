using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Schema.EIVO;
using System.Text.RegularExpressions;
using System.Globalization;
using Model.DataEntity;
using DataAccessLayer.basis;

namespace Model.InvoiceManagement.enUS
{
    public static partial class CancelInvoiceRootValidator
    {
        #region 英文訊息專區

        public static Exception CheckMandatoryFields(this CancelInvoiceRootCancelInvoice invItem, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out InvoiceItem invoice, out DateTime cancelDate)
        {
            invoice = null;
            cancelDate = default(DateTime);

            if (String.IsNullOrEmpty(invItem.CancelInvoiceNumber) || !Regex.IsMatch(invItem.CancelInvoiceNumber, "^[a-zA-Z]{2}[0-9]{8}$"))
            {
                return new Exception(String.Format("Error void CancelInvoiceNumber, CancelInvoiceNumber length should be set aside for the 10 yards (including track code)，傳送資料：{0}，TAG：< CancelInvoiceNumber />", invItem.CancelInvoiceNumber));
            }
            String invNo, trackCode;
            trackCode = invItem.CancelInvoiceNumber.Substring(0, 2);
            invNo = invItem.CancelInvoiceNumber.Substring(2);

            DateTime invoiceDate;
            if (String.IsNullOrEmpty(invItem.InvoiceDate))
            {
                return new Exception("InvoiceDate error, Incorrect TAG:< InvoiceDate />");
            }

            if (!DateTime.TryParseExact(invItem.InvoiceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out invoiceDate))
            {
                return new Exception(String.Format("Format of InvoiceDate error(YYYY/MM/DD), Incorrect TAG:< InvoiceDate />", invItem.InvoiceDate));
            }

            invoice = mgr.GetTable<InvoiceItem>().Where(i => i.No == invNo && i.TrackCode == trackCode).FirstOrDefault();

            if (invoice == null)
            {
                return new Exception(String.Format("Invoice No. does not exist:{0}", invItem.CancelInvoiceNumber));
            }

            if (invoice.SellerID != owner.CompanyID)
            {
                return new Exception(String.Format("Non-original invoice voided invoice Liren,Cancel Invoice Number:{0}", invItem.CancelInvoiceNumber));
            }

            if (invoice.InvoiceCancellation != null)
            {
                return new Exception(String.Format("Cancel Invoice already exists,Cancel Invoice Number:{0}", invItem.CancelInvoiceNumber));
            }


            if (String.IsNullOrEmpty(invItem.SellerId))
            {
                return new Exception("SellerId can not be blank，TAG:< SellerId />");
            }

            if (String.IsNullOrEmpty(invItem.CancelDate))
            {
                return new Exception("CancelDate can not be blank，TAG：< CancelDate />");
            }

            if (String.IsNullOrEmpty(invItem.CancelTime))
            {
                return new Exception("CancelTime can not be blank，TAG：< CancelTime />");
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", invItem.CancelDate, invItem.CancelTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            {
                return new Exception(String.Format("Format of Cancel Date or Cancel Time error(YYYY/MM/DD HH:mm:ss), Incorrect:{0} {1}", invItem.CancelDate, invItem.CancelTime));
            }

            if (String.IsNullOrEmpty(invItem.CancelReason))
            {
                return new Exception("CancelReason can not be blank，TAG：< CancelReason />");
            }

            if (invItem.CancelReason.Length > 256)
            {
                return new Exception(String.Format("At least one yard length data format, up to 20 yards，Incorrect：{0}，TAG：< CancelReason />", invItem.CancelReason));
            }

            //備註
            if (invItem.Remark != null && invItem.Remark.Length > 200)
            {
                return new Exception(String.Format("Note length can not be more than 200 data，Incorrect：{0}，TAG：< Remark />", invItem.Remark));
            }

            return null;
        }

        #endregion
    }
}