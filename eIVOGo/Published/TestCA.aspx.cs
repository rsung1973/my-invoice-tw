using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Published
{
    public partial class TestCA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            signContext.Launcher = btnSign;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            signContext.BeforeSign += new EventHandler(signContext_BeforeSign);
        }

        void signContext_BeforeSign(object sender, EventArgs e)
        {
            signContext.DataToSign = DataContext.Text;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (signContext.Verify())
            {
                litMsg.Text = "Successful";
            }
            else
            {
                litMsg.Text = "Failed";
            }
        }

    }
}