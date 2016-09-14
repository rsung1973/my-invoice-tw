using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccessLayer.basis;
using Model.Locale;
using Model.Helper;

namespace ModelExtension.MessageManagement
{
    public static partial class ExtensionMethods
    {
        public static String GetCounterpartMobile(this CDS_Document item)
        {
            switch ((Naming.DocumentTypeDefinition)item.DocType)
            {
                case Naming.DocumentTypeDefinition.E_Invoice:
                    return item.InvoiceItem.InvoiceBuyer.Phone;
                case Naming.DocumentTypeDefinition.E_InvoiceCancellation:
                    return item.DerivedDocument.ParentDocument.InvoiceItem.InvoiceBuyer.Phone;
                case Naming.DocumentTypeDefinition.E_Allowance:
                    return item.InvoiceAllowance.InvoiceAllowanceSeller.Phone;
                case Naming.DocumentTypeDefinition.E_AllowanceCancellation:
                    return item.DerivedDocument.ParentDocument.InvoiceAllowance.InvoiceAllowanceSeller.Phone;
            }
            return null;
        }

        public static bool IsB2C(this InvoiceBuyer buyer)
        {
            return buyer.ReceiptNo == "0000000000";
        }
    }

    public enum SMSStatus
    {
        已發送 = 0,
        發送成功 = 100,
        手機端因素未能送達 = 101,
        電信終端設備異常_102 = 102,
        無此手機號碼 = 103,
        電信終端設備異常_104 = 104,
        電信終端設備異常_105 = 105,
        電信終端設備異常_106 = 106,
        逾時收訊 = 107,
        語音簡訊發送失敗 = 108,
        預約簡訊 = 300,
        無額度_或額度不足 = 301,
        取消簡訊 = 303,
        未開通國際簡訊 = 500,
        代表此呼叫為回覆簡訊之內容 = 999,
        無效門號 = -3,
        DT格式錯誤或預計發送時間已過去24小時以上 = -4
    }

    public partial class DocumentReplicationDataSource : LinqToSqlDataSource<MessageEntityDataContext, DocumentReplication> { }

}
