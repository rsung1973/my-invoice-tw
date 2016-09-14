using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Models
{
    public class WinningInvoiceReportItem
    {
        public String SellerReceiptNo { get; set; }
        public String SellerName { get; set; }
        public String Addr { get; set; }
        public int WinningCount { get; set; }
        public int DonationCount { get; set; }
    }
}