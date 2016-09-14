using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.SCM
{
    public partial class PrintShipment : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "open",
                String.Format("window.open('{0}','prnWin','toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=64,height=48');", String.Format("{0}?id={1}",VirtualPathUtility.ToAbsolute("~/Published/PrintShipment.aspx"),shipment.Item.DocID))
                , true);
        }


    }
}