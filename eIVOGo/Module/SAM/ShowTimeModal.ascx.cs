using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM
{
    public partial class ShowTimeModal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            now.Text = DateTime.Now.ToString();
        }

        public void Show()
        {
            this.ModalPopupExtender.Show();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            this.AjaxAlert(now.Text);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
        }

        protected void btnModal_Click(object sender, EventArgs e)
        {
            modal2.Show();
        }
    }
}