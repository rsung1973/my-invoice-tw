using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.DataModel;
using Utility;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class TrackCodePeriodSelector : ItemSelector
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            selector.Load += new EventHandler(selector_Load);
        }

        void selector_Load(object sender, EventArgs e)
        {
            selector_DataBound(sender, e);
        }

    }
}