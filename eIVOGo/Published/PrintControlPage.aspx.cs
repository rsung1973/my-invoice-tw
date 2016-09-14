using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace eIVOGo.Published
{
    public partial class PrintControlPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            prepareUserControl();
        }

        protected virtual void prepareUserControl()
        {
            if (this.PreviousPage != null && this.PreviousPage.Items["PrintDoc"] != null)
            {
                List<String> items = this.PreviousPage.Items["PrintDoc"] as List<String>;
                if (items != null)
                {
                    foreach (String controlPath in items)
                    {
                        UserControl ctrl = (UserControl)this.LoadControl(controlPath);
                        ctrl.InitializeAsUserControl(this.Page);
                        theForm.Controls.Add(ctrl);
                    }
                }

            }

        }

    }
}