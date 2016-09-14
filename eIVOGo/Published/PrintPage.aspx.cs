using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace eIVOGo.Published
{
    public partial class PrintPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        protected virtual void initializeData()
        {
            if (this.PreviousPage != null && this.PreviousPage.Items["PrintDoc"] != null && this.PreviousPage.Items["PrintDoc"] is IEnumerable)
            {
                IEnumerable controls = (IEnumerable)this.PreviousPage.Items["PrintDoc"];
                foreach (Control ctrl in controls)
                {
                    ctrl.Visible = true;
                    theForm.Controls.Add(ctrl);
                }
            }

        }

    }
}