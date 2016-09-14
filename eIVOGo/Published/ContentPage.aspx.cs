using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace eIVOGo.Published
{
    public partial class ContentPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected virtual void initializeData()
        {
            if (this.PreviousPage != null && this.PreviousPage.Items.Count > 0)
            {
                if (this.PreviousPage.Items["contentList"] is IEnumerable<Control>)
                {
                    foreach (Control ctrl in (IEnumerable<Control>)this.PreviousPage.Items["contentList"])
                    {
                        theForm.Controls.Add(ctrl);
                    }
                }
            }
        }

    }
}