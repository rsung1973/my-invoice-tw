using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class PageAction : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Bindable(true)]
        public String ItemName
        {
            get
            {
                return litItemName.Text;
            }
            set
            {
                litItemName.Text = value;
            }
        }
    }
}