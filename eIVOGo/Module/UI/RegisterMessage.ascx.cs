using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class RegisterMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Bindable(true)]
        public String ActionName
        {
            get
            {
                return actionItem.ItemName;
            }
            set
            {
                actionItem.ItemName = value;
            }
        }

        [Bindable(true)]
        public String Message
        {
            get
            {
                return litMsg.Text;
            }
            set
            {
                litMsg.Text = value;
            }
        }

        [Bindable(true)]
        public String GoBackText
        {
            get
            {
                return btnBack.Text;
            }
            set
            {
                btnBack.Text = value;
            }
        }

        [Bindable(true)]
        public String GoBackUrl
        {
            get
            {
                return btnBack.CommandArgument;
            }
            set
            {
                btnBack.CommandArgument = value;
                btnBack.Visible = !String.IsNullOrEmpty(btnBack.CommandArgument);
            }
        }

        protected void btnBackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Home/MainPage");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(btnBack.CommandArgument))
            {
                Response.Redirect(btnBack.CommandArgument);
            }
        }
    }
}