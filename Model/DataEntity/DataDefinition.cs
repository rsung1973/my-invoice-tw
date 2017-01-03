using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.DataEntity
{
    public partial class DataDefinition
    {
    }

    public partial class InvoiceDeliveryTracking
    {
        public int? DuplicateCount { get; set; }
        public bool MergedItem { get; set; }
    }

}
