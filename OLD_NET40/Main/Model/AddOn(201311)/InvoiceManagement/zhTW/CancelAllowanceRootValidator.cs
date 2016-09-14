using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Schema.EIVO;
using DataAccessLayer.basis;
using Model.DataEntity;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Model.InvoiceManagement.zhTW
{
    public static partial class CancelAllowanceRootValidator
    {
        //檢查基本必填項目(作廢折讓單)
        public static Exception CheckMandatoryFields(this CancelAllowanceRootCancelAllowance item, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out InvoiceAllowance allowance, out DateTime cancelDate)
        {
            cancelDate = default(DateTime);

            allowance = mgr.GetTable<InvoiceAllowance>().Where(a => a.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();
            if (allowance == null)
            {
                return new Exception(String.Format("折讓證明單不存在，折讓證明單號碼:{0}，TAG：< CancelAllowanceNumber />", item.CancelAllowanceNumber));
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                return new Exception(String.Format("作廢折讓單已存在,折讓單號碼:{0}", item.CancelAllowanceNumber));
            }


            DateTime allowanceDate;
            if (String.IsNullOrEmpty(item.AllowanceDate))
            {
                return new Exception("折讓證明單日期，TAG：< AllowanceDate />");
            }
            if (!DateTime.TryParseExact(item.AllowanceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out allowanceDate))
            {
                return new Exception(String.Format("折讓證明單日期錯誤(YYYY/MM/DD)；傳送資料:{0}", item.AllowanceDate));
            }


            if (item.BuyerId != "0000000000" && !Regex.IsMatch(item.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< BuyerId />", item.BuyerId));
            }

            Organization seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == item.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< SellerId />", item.SellerId));
            }

            if (seller.CompanyID != owner.CompanyID && !mgr.GetTable<InvoiceIssuerAgent>().Any(a => a.AgentID == owner.CompanyID && a.IssuerID == seller.CompanyID))
            {
                return new Exception(String.Format("簽章設定人與發票開立人不符,開立人統一編號:{0}，TAG:< SellerId />", item.SellerId));
            }

            if (String.IsNullOrEmpty(item.CancelDate))
            {
                return new Exception("作廢日期，TAG：< CancelDate />");
            }

            if (String.IsNullOrEmpty(item.CancelTime))
            {
                return new Exception("作廢時間，TAG：< CancelTime />");
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            {
                return new Exception(String.Format("作廢折讓證明單作廢日期、作廢時間格式錯誤(YYYY/MM/DD HH:mm:ss)；傳送資料:{0} {1}", item.CancelDate, item.CancelTime));
            }

            if (String.IsNullOrEmpty(item.CancelReason))
            {
                return new Exception("作廢原因不可空白，TAG：< CancelReason />");
            }

            if (item.CancelReason.Length > 20)
            {
                return new Exception(String.Format("資料格式長度最少1碼，最多20碼，傳送資料：{0}，TAG：< CancelReason />", item.CancelReason));
            }

            if (item.Remark != null && item.Remark.Length > 200)
            {
                return new Exception(String.Format("備註資料長度不可超過200，傳送資料：{0}，TAG：< Remark />", item.Remark));
            }

            return null;

        }

        public static Exception CheckMandatoryFields_Proxy(this CancelAllowanceRootCancelAllowance item, GenericManager<EIVOEntityDataContext> mgr, OrganizationToken owner, out InvoiceAllowance allowance, out DateTime cancelDate)
        {
            cancelDate = default(DateTime);

            allowance = mgr.GetTable<InvoiceAllowance>().Where(a => a.AllowanceNumber == item.CancelAllowanceNumber).FirstOrDefault();
            if (allowance == null)
            {
                return new Exception(String.Format("折讓證明單不存在，折讓證明單號碼:{0}，TAG：< CancelAllowanceNumber />", item.CancelAllowanceNumber));
            }

            if (allowance.InvoiceAllowanceCancellation != null)
            {
                return new Exception(String.Format("作廢折讓單已存在,折讓單號碼:{0}", item.CancelAllowanceNumber));
            }


            DateTime allowanceDate;
            if (String.IsNullOrEmpty(item.AllowanceDate))
            {
                return new Exception("折讓證明單日期，TAG：< AllowanceDate />");
            }
            if (!DateTime.TryParseExact(item.AllowanceDate, "yyyy/MM/dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out allowanceDate))
            {
                return new Exception(String.Format("折讓證明單日期錯誤(YYYY/MM/DD)；傳送資料:{0}", item.AllowanceDate));
            }


            if (item.BuyerId != "0000000000" && !Regex.IsMatch(item.BuyerId, "^[0-9]{8}$"))
            {
                return new Exception(String.Format("買方識別碼錯誤，傳送資料：{0}，TAG:< BuyerId />", item.BuyerId));
            }

            Organization seller = mgr.GetTable<Organization>().Where(o => o.ReceiptNo == item.SellerId).FirstOrDefault();

            if (seller == null)
            {
                return new Exception(String.Format("賣方為非註冊店家,開立人統一編號:{0}，TAG:< SellerId />", item.SellerId));
            }

            //代理店家
            var proxyOrg = mgr.GetTable<Organization>().Where(i => i.CompanyID == owner.CompanyID).FirstOrDefault();
            //上傳資料店家
            var Org = seller;
            //上傳店家資料是否註冊在代理店家下
            var proxy = mgr.GetTable<InvoiceIssuerAgent>().Where(i => i.AgentID == proxyOrg.CompanyID && i.IssuerID == Org.CompanyID).FirstOrDefault();
            if (proxy == null && proxyOrg.ReceiptNo != item.SellerId)
            {
                return new Exception(String.Format("上傳店家並無登記在此發票開立代理店家底下:{0}，TAG:< SellerId />；代理發票開立店家:{1}", Org.ReceiptNo, proxyOrg.ReceiptNo));
            }

            if (String.IsNullOrEmpty(item.CancelDate))
            {
                return new Exception("作廢日期，TAG：< CancelDate />");
            }

            if (String.IsNullOrEmpty(item.CancelTime))
            {
                return new Exception("作廢時間，TAG：< CancelTime />");
            }

            if (!DateTime.TryParseExact(String.Format("{0} {1}", item.CancelDate, item.CancelTime), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out cancelDate))
            {
                return new Exception(String.Format("作廢折讓證明單作廢日期、作廢時間格式錯誤(YYYY/MM/DD HH:mm:ss)；傳送資料:{0} {1}", item.CancelDate, item.CancelTime));
            }

            if (String.IsNullOrEmpty(item.CancelReason))
            {
                return new Exception("作廢原因不可空白，TAG：< CancelReason />");
            }

            if (item.CancelReason.Length > 20)
            {
                return new Exception(String.Format("資料格式長度最少1碼，最多20碼，傳送資料：{0}，TAG：< CancelReason />", item.CancelReason));
            }

            if (item.Remark != null && item.Remark.Length > 200)
            {
                return new Exception(String.Format("備註資料長度不可超過200，傳送資料：{0}，TAG：< Remark />", item.Remark));
            }

            return null;

        }

    }
}