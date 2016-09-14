using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.JsGrid.DataField
{
    public partial class JsGridField : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        [Bindable(true)]
        public String FieldVariable
        { get; set; }

        [Bindable(true)]
        public String type { get; set; }
        [Bindable(true)]
        public String name { get; set; }
        [Bindable(true)]
        public String title { get; set; }
        [Bindable(true)]
        public String align { get; set; }
        [Bindable(true)]
        public int width { get; set; }
        [Bindable(true)]
        public String css { get; set; }
        [Bindable(true)]
        public String headercss { get; set; }
        [Bindable(true)]
        public String filtercss { get; set; }
        [Bindable(true)]
        public String insertcss { get; set; }
        [Bindable(true)]
        public String editcss { get; set; }
        [Bindable(true)]
        public String itemTemplate { get; set; }
        [Bindable(true)]
        public String footerTemplate { get; set; }
        [Bindable(true)]
        public String headerTemplate { get; set; }

    }
}