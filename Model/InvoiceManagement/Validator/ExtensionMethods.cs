using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model.DataEntity;

namespace Model.InvoiceManagement.Validator
{
    public static partial class ExtensionMethods
    {
        public static Exception OrganizationValueCheck(this Organization dataItem)
        {
            if (String.IsNullOrEmpty(dataItem.CompanyName))
            {
                //檢查名稱
                return new Exception("請輸入公司名稱!!");
            }
            if (String.IsNullOrEmpty(dataItem.ReceiptNo))
            {
                //檢查名稱
                return new Exception("請輸入公司統編!!");
                
            }
            if (String.IsNullOrEmpty(dataItem.Addr))
            {
                //檢查名稱
                return new Exception("請輸入公司地址!!");
                
            }
            if (String.IsNullOrEmpty(dataItem.Phone))
            {
                //檢查名稱
                return new Exception("請輸入公司電話!!");
                
            }

            Regex reg = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");

            if (String.IsNullOrEmpty(dataItem.ContactEmail) || !reg.IsMatch(dataItem.ContactEmail))
            {
                //檢查email
                return new Exception("電子信箱尚未輸入或輸入錯誤!!");
            }

            return null;
        }
    }
}
