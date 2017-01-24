using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eIVOGo.Models.ViewModel
{
    public class InputViewModel
    {
        public String Name { get; set; }
        public String Id { get; set; }
        public String Label { get; set; }
        public String Value { get; set; }
        public String PlaceHolder { get; set; }
        public String ErrorMessage { get; set; }
        public bool? IsValid { get; set; }
        public String InputType { get; set; }
        public Object DefaultValue { get; set; }
        public String ButtonStyle { get; set; }
        public String IconStyle { get; set; }
        public String Href { get; set; }
    }

    public partial class MailTrackingCsvViewMode
    {
        public int? PackageID { get; set; }
        public String MailNo1 { get; set; }
        public String MainNo2 { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int?[] InvoiceID { get; set; }
    }
}