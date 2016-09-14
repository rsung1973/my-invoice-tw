using System;

namespace Model.Locale
{
    /// <summary>
    /// Naming 的摘要描述。
    /// </summary>
    public class Naming
    {
        private Naming()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        public const int INSURER = 5;

        public enum CategoryID
        {
            COMP_SYS = 1,	                            //	1	系統公司	 sketch_admin.gif
            COMP_WELFARE = 14,
            COMP_E_INVOICE_B2C_SELLER = 15,	            //	2	賣方	     sketch_seller.gif
            COMP_E_INVOICE_B2C_BUYER = 16,	            //	3	買方	     sketch_buyer.gif
            COMP_E_INVOICE_GOOGLE_TW = 17,              //  4   Google台灣
            COMP_ENTERPRISE_GROUP   =   18,
            COMP_VIRTUAL_CHANNEL = 19,
            COMP_INVOICE_AGENT = 20,                    //  5   發票開立代理
        }

        public enum B2CCategoryID
        {
            店家 = 15,	                                //	2	賣方	     sketch_seller.gif
            Google台灣 = 17,                            //  4  
            店家發票自動配號 = 19,
            開立發票店家代理 = 20,
        }

        public enum RoleID
        {
            ROLE_SYS = 1,	                            //	1	系統公司	 sketch_admin.gif
            ROLE_SELLER = 51,	                        //	2	賣方	     sketch_seller.gif
            ROLE_BUYER = 52,	                        //	3	買方	     sketch_buyer.gif
            ROLE_GUEST = 53,
            ROLE_NETWORKSELLER = 54,                    //  4   網購業者
            ROLE_GOOGLETW = 55,                         //  5   GOOGLE台灣
            集團成員 = 61,
            相對營業人 = 62,
            集團成員_自動開立接收 = 63
        }

        public enum RoleQueryDefinition
        {
            平台管理員 = 1,	                            //	1	系統公司	 sketch_admin.gif
            賣方 = 51,	                                //	2	賣方	     sketch_seller.gif
            買方 = 52,	                                //	3	買方	     sketch_buyer.gif
            訪客 = 53,
            網購業者 = 54,                              //  4   網購業者
            Google台灣 = 55,                            //  5   GOOGLE台灣
            集團成員 = 61,
            相對營業人 = 62,
            集團成員_自動開立接收 = 63
        }

        public enum DocumentTypeDefinition
        {
            E_Invoice = 10,                             //	電子發票
            E_Allowance = 11,                           //	電子發票折讓證明
            E_InvoiceCancellation = 12,                 //  作廢電子發票
            E_InvoiceReturn = 13,                       //	電子發票退回
            E_AllowanceCancellation = 14,               //	作廢電子發票折讓證明
            E_InvoiceVoid = 15,	                        //  電子發票註銷
            E_Receipt = 16,                             //  收據
            E_ReceiptCancellation = 17,                 //  作廢收據
            PurchaseOrder = 100,                        //  採購單
            WarehouseWarrant = 101,                     //  入庫單
            PurchaseOrderReturned = 102,                //	採購退貨單
            BuyerOrder = 103,                           //	訂單
            OrderExchangeGoods = 104,                   //	換貨單
            OrderGoodsReturned = 105                    //	退貨單
        }

        public enum B2BInvoiceDocumentTypeDefinition
        {
            電子發票 = 10,                              //	電子發票
            發票折讓 = 11,                              //	電子發票折讓證明
            作廢發票 = 12,                              //  作廢電子發票
            作廢折讓 = 14,
            收據 = 16,
            作廢收據 = 17
        }

        public enum MemberStatusDefinition
        {
            Mark_To_Delete = 1101,                      //1101	註記停用	    註記停用
            Wait_For_Check = 1102,                      //1102	等待回覆確認	等待回覆確認
            Checked = 1103                              //1103	人員已確認	    人員已確認
        }

        public enum CACatalogDefinition
        {
            簽章測試 = 0,                               //簽章測試
            字軌維護 = 1,                               //字軌維護
            開立發票 = 2,
            開立作廢發票 = 3,
            開立折讓單 = 4,
            開立作廢折讓單 = 5,
            下載大平台發票 = 6,
            下載大平台作廢發票 = 7,
            下載大平台折讓 = 8,
            下載大平台作廢折讓 = 9,
            UXGW上傳資料 = 10,
            授權資料下載 = 11,
            接收發票=12,
            接收折讓單 = 13,
            接收作廢發票 = 14,
            接收作廢折讓單 = 15,
            列印發票 = 16,
            列印折讓單 = 17,
            列印作廢發票 = 18,
            列印作廢折讓單 = 19,
            設定簽章憑證 = 20,
            UXGW自動接收 = 21,
            平台自動接收 = 22,
            平台自動開立 = 23,
            UXGW上傳附件檔 = 24,
            開立收據 = 25,
            開立作廢收據 = 26,
            接收收據 = 27,
            接收作廢收據 = 28,
            退回發票 = 29,
            退回作廢發票 = 30,
            退回折讓單 = 31,
            退回作廢折讓單 = 32,
            退回收據 = 33,
            註銷發票 = 34,
            註銷作廢發票 = 35,
            註銷折讓單 = 36,
            註銷作廢折讓單 = 37,
            註銷收據 = 38,
            UXGW自動開立 = 39,
            UXGW下載資料    = 40,
            UXGW上傳發票退回 = 41,
            UXGW上傳發票註銷 = 42,
            UXGW上傳折讓單退回 = 43,
            UXGW上傳折讓單註銷 = 44,
            UXGW上傳作廢發票退回 = 45,
            UXGW上傳作廢發票註銷 = 46,
            UXGW上傳作廢折讓單退回 = 47,
            UXGW上傳作廢折讓單註銷 = 48,
            UXGW收回相對營業人未接收 = 49,
            UXGW接收相對營業人退回申請 = 50
        }

        public enum B2BCACatalogQueryDefinition
        {
            字軌維護 = 1,                                //字軌維護
            開立發票 = 2,
            開立作廢發票 = 3,
            開立折讓單 = 4,
            開立作廢折讓單 = 5,
            接收發票 = 12,
            接收折讓單 = 13,
            接收作廢發票 = 14,
            接收作廢折讓單 = 15,
            列印發票 = 16,
            列印折讓單 = 17,
            列印作廢發票 = 18,
            列印作廢折讓單 = 19,
            設定簽章憑證 = 20,
            平台自動接收 = 22,
            平台自動開立 = 23,
            退回發票 = 29,
            退回作廢發票 = 30,
            退回折讓單 = 31,
            退回作廢折讓單 = 32,
            退回收據 = 33,
            註銷發票 = 34,
            註銷作廢發票 = 35,
            註銷折讓單 = 36,
            註銷作廢折讓單 = 37,
            註銷收據 = 38,
            UXGW上傳資料 = 10,
            UXGW自動接收 = 21,
            UXGW上傳附件檔 = 24,
            UXGW下載PDF = 40,
            UXGW上傳發票退回 = 41,
            UXGW上傳發票註銷 = 42,
            UXGW上傳折讓單退回 = 43,
            UXGW上傳折讓單註銷 = 44,
            UXGW上傳作廢發票退回 = 45,
            UXGW上傳作廢發票註銷 = 46,
            UXGW上傳作廢折讓單退回 = 47,
            UXGW上傳作廢折讓單註銷 = 48,
            UXGW接收發票 = 49,
            UXGW接收作廢發票 = 50,
            UXGW接收折讓單 = 51,
            UXGW接收作廢折讓單 = 52,
            UXGW相對營業人未接收發票 = 53,
            UXGW相對營業人未接收作廢發票 = 54,
            UXGW相對營業人未接收折讓單 = 55,
            UXGW相對營業人未接收作廢折讓單 = 56
        }

        public enum eIVOUXCACatalogQueryDefinition
        {
            UXGW上傳資料 = 10,
            設定簽章憑證 = 20,
        }

        public enum UploadStatusDefinition
        {
            等待匯入 = 0,
            資料錯誤 = 1,
            匯入成功 = 2,
            匯入失敗 = 3
        }

        public enum DocumentStepDefinition
        {
            預覽 = 1200,
            待審核 = 1201,
            已開立 = 1202,
            已退回 = 1203,
            已刪除 = 1204
        }

        public enum BuyerOrderTypeDefinition
        {
            一般消費者 = 1,
            供應商 = 2,
            公關 = 3
        }

        public enum DataItemSource
        {
            FromViewState = 0,
            FromDB = 1,
            FromPreviousPage = 2
        }

        public enum DataItemStatus
        {
            Unchanged = 0,
            Modified = 1
        }

        public enum B2BInvoiceStepDefinition
        {
            待接收 = 1301,                               //	待接收           
            待開立 = 1302,                               //	待開立
            待傳送 = 1303,                               //	待傳送至IFS-EIVO
            已傳送 = 1304,	                             //	已傳送至IFS-EIVO
            已接收 = 1305,
            已開立 = 1306,
            已註銷 = 1307,
            待批次傳送 = 1308,
            申請退回 = 1309,
            已作廢 = 1310,
            Xml待傳輸 = 1311,
            PDF待傳輸 = 1312,
            未接收資料待通知 = 1313,
            未接收資料待傳輸 = 1314,
            註銷申請待開立 = 1315,
            退回申請待開立 = 1316
        }

        public enum B2BInvoiceQueryStepDefinition
        { 
            待接收 = 1301,                               //	待接收
            待開立 = 1302,                               //	待開立
            已開立 = 1306,
            已註銷 = 1307,
            已接收 = 1308,                               //原名稱 "待批次傳送",因顯示需要改 "已接收"
            申請退回 = 1309
        }

        public enum CenterInvoiceReturnStepDefinition
        {
            待接收 = 1301,                                //	待接收
            已接收 = 1308,                                //原名稱 "待批次傳送",因顯示需要改 "已接收"
            申請退回 = 1309
        }

        public enum InvoiceCenterBusinessType
        { 
            銷項  =   1,
            進項  =   2
        }

        public enum CounterpartBusinessQueryType
        {
            //銷項 = 1,
            進項 = 2
        }        

        public enum InvoiceCenterBusinessQueryType
        {
            銷項 = 1,
            進項 = 2
        }

        public enum MessageTypeDefinition
        {
            發票開立通知 = 1,
            發票中獎通知 = 2,
            系統訊息通知 = 3,
            客服訊息通知 = 4
        }

        public enum B2CInvoiceLevelDefinition
        {
            已開立 = 0,
            待傳送 = 1,
            已註銷 = 6
        }

        public enum InvoiceTypeDefinition
        {
            三聯式 = 1,
            二聯式 = 2,
            二聯式收銀機 = 3,
            特種稅額 = 4,
            電子計算機 = 5,
            三聯式收銀機 = 6,
            一般稅額計算之電子發票 = 7,
            特種稅額計算之電子發票 = 8,
        }

        public enum InvoiceTypeFormat
        {
            三聯式, 電子計算機 = 21,
            二聯式 = 32,
            二聯式收銀機 = 22,
            三聯式收銀機 = 25,
            特種稅額 = 37,
            一般稅額計算之電子發票 = 25,
        }

        public enum TaxTypeDefinition
        {
            應稅 = 1,
            零稅率 = 2,
            免稅 = 3,
            特種稅率 = 4,
            混合稅率 = 9
        }

        public enum InvoiceDeliveryStatus
        {
            待傳送 = 1303,                               //	待傳送至IFS-EIVO
            已傳送 = 1304,	                             //	已傳送至IFS-EIVO
            申請退回 = 1309
        }
        public enum ChannelIDType
        {
            FromWeb = 0,
            FromGW=1
        }

        public enum DataResultMode
        {
            Display = 0,
            Print = 1,
            Download = 2
        }

    }
}
