using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Utility;

namespace eIVOGo.Models.ViewModel
{
    public class InvoiceViewModel
    {
        public InvoiceViewModel()
        {
            InvoiceType = 7;
            InvoiceDate = DateTime.Now;
            TaxAmount = 1;
            No = "00000000";
            RandomNo = String.Format("{0:0000}",(DateTime.Now.Ticks % 10000));
            TaxRate = 0.05m;
            DonateMark = "0";
        }
        public int? SellerID { get; set; }
        public String SellerName { get; set; }
        public String SellerReceiptNo { get; set; }
        public String BuyerReceiptNo { get; set; }
        public String No { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public String Remark { get; set; }
        public String BuyerRemark { get; set; }
        public String CustomsClearanceMark { get; set; }
        public byte? InvoiceType { get; set; }
        public byte? TaxType { get; set; }
        public decimal? TaxRate { get; set; }
        public String RandomNo { get; set; }
        public string DonateMark { get; set; }
        public string CarrierType { get; set; }
        public string CarrierId1 { get; set; }
        public string CarrierId2 { get; set; }
        public string NPOBAN { get; set; }
        public string CustomerID { get; set; }
        public string BuyerName { get; set; }
        public String TrackCode { get; set; }
        public String DataNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? SalesAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public String[] Brief { get; set; }
        public String[] ItemNo { get; set; }
        public String[] ItemRemark { get; set; }
        public decimal?[] UnitCost { get; set; }
        public decimal?[] CostAmount { get; set; }
        public int?[] Piece { get; set; }

    }
}