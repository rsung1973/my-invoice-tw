using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Schema.EIVO;
using DataAccessLayer.basis;
using Model.DataEntity;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Model.InvoiceManagement.enUS
{
    public static partial class CancelAllowanceRootValidator
    {
        #region 英文訊息專區

        public static Exception CheckMandatoryFields(this CancelAllowanceRootCancelAllowance item, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out InvoiceAllowance allowance, out DateTime cancelDate)
        {
            cancelDate = default(DateTime);

            allowance = mgr.GetTable<InvoiceAllowance>().Where(a => a.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();
            if (allowance == null)
            {
                return new Exception(String.Format("Allowance Number does not exist，Allowance Number:{0}，TAG：< CancelAllowanceNumber />", item.CancelAllowanceNumber));
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                return new Exception(String.Format("Cancel Allowance Number already exists,Allowance Number:{0}", item.CancelAllowanceNumber));
            }


            DateTime allowanceDate;
            if (String.IsNullOrEmpty(item.AllowanceDate))
            {
                return new Exception("Allowance Date，TAG：< AllowanceDate />");
            }
            if (!DateTime.TryParseExact(item.AllowanceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out allowanceDate))
            {
                return new Exception(String.Format("Format of Original Allowance Date error(YYYY/MM/DD)；Incorrect:{0}", item.AllowanceDate));
            }


            if (item.BuyerId != "0000000000" && !Regex.IsMatch(item.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("BuyerId can not be blank，Incorrect：{0}，TAG:< BuyerId />", item.BuyerId));
            }

            Organization seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == item.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("Seller ID does not exist, Seller ID: {0}, Incorrect TAG:< SellerId />", item.SellerId));
            }

            if (seller.CompanyID != owner.CompanyID)
            {
                return new Exception(String.Format("Cert ID and Seller ID does not match, Seller ID: {0}, Incorrect TAG:< SellerId />", item.SellerId));
            }

            if (String.IsNullOrEmpty(item.CancelDate))
            {
                return new Exception("Cancel Date can not be blank，TAG：< CancelDate />");
            }

            if (String.IsNullOrEmpty(item.CancelTime))
            {
                return new Exception("Cancel Time can not be blank，TAG：< CancelTime />");
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            {
                return new Exception(String.Format("Format of Cancel Date or Cancel Time error(YYYY/MM/DD HH:mm:ss), Incorrect:{0} {1}", item.CancelDate, item.CancelTime));
            }

            if (String.IsNullOrEmpty(item.CancelReason))
            {
                return new Exception("Cancel Reason can not be blank，TAG：< CancelReason />");
            }

            if (item.CancelReason.Length > 20)
            {
                return new Exception(String.Format("At least one yard length data format, up to 20 yards，Incorrect：{0}，Incorrect：{0}，TAG：< CancelReason />", item.CancelReason));
            }

            if (item.Remark != null && item.Remark.Length > 200)
            {
                return new Exception(String.Format("Note length can not be more than 200 data，Incorrect：{0}，TAG：< Remark />", item.Remark));
            }

            return null;
        }

        #endregion
    }
}