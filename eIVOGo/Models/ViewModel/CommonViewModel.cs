using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Models.ViewModel
{
    public partial class InquireInvoiceViewModel : CommonQueryViewModel
    {
        public int? CompanyID { get; set; }
        public int? AgentID { get; set; }
        public String DataNo { get; set; }
        public String Consumption { get; set; }
        public String BuyerReceiptNo { get; set; }
        public String BuyerName { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsWinning { get; set; }
        public bool? Cancelled { get; set; }
        public bool? IsAttached { get; set; }
        public bool? QueryAtStart { get; set; }

    }

    public partial class InquireNoIntervalViewModel
    {
        public int? Year { get; set; }
        public int? PeriodNo { get; set; }
        public int? SellerID { get; set; }
        public String SelectIndication { get; set; }
    }

    public partial class BusinessRelationshipViewModel
    {
        public int? CompanyID { get; set; }
        public String ReceiptNo { get; set; }
        public String CompanyName { get; set; }
        public String ContactEmail { get; set; }
        public String Phone { get; set; }
        public String CustomerNo { get; set; }
        public String Addr { get; set; }
        public int? CompanyStatus { get; set; }
        public int? BusinessType { get; set; }
        public bool? Entrusting { get; set; }
        public bool? EntrustToPrint { get; set; }
    }

    public partial class UserProfileViewModel
    {
        public String KeyID { get; set; }
        public int? SellerID { get; set; }
        public int? UID { get; set; }
        public String PID { get; set; }
        public String UserName { get; set; }
        public String Password { get; set; }
        public String Password1 {get;set;}
        public String EMail { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String MobilePhone { get; set; }
        public String Phone2 { get; set; }
        public bool? WaitForCheck { get; set; }
        public int? DefaultRoleID { get; set; }
    }

    public partial class InvoiceNoIntervalViewModel
    {
        public int? IntervalID { get; set; }
        public int? TrackID { get; set; }
        public int? SellerID { get; set; }
        public int? StartNo { get; set; }
        public int? EndNo { get; set; }
    }

    public partial class CommonQueryViewModel
    {
        public int? PageIndex { get; set; }
        public int?[] Sort { get; set; }
        public int? PageSize { get; set; }
        public String ResultAction { get; set; }
    }

    public partial class UserAccountQueryViewModel : CommonQueryViewModel
    {
        public int? SellerID { get; set; }
        public String PID { get; set; }
        public String UserName { get; set; }
        public int? RoleID { get; set; }
        public int? LevelID { get; set; }
    }

    public partial class TrackCodeQueryViewModel : CommonQueryViewModel
    {
        public int? Year { get; set; }
    }

    public partial class TrackCodeViewModel
    {
        public int? TrackID { get; set; }
        public short? Year { get; set; }
        public short? PeriodNo { get; set; }
        public String TrackCode { get; set; }

    }

    public partial class WinningNumberViewModel
    {
        public int? WinningID { get; set; }
        public int? Year { get; set; }
        public int? Period { get; set; }
        public int? Rank { get; set; }
        public String WinningNo { get; set; }
    }

    public partial class MailTrackingViewModel
    {
        public String StartNo { get; set; }
        public String EndNo { get; set; }
        public int? DeliveryStatus { get; set; }

    }


}