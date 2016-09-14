using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Properties;

namespace eIVOGo.Module.SAM
{
    public partial class UXB2BCardGen : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(typeof(UXB2BCardGen), "redirect",
            //    String.Format("window.location.href='{0}';", VirtualPathUtility.ToAbsolute(String.Format("{0}?companyID={1}&count={2}", Settings.Default.GenerateMemberCodeUrl, SellerID.Selector.SelectedValue, !String.IsNullOrEmpty(rbTotal.SelectedValue) ? rbTotal.SelectedValue : TotalCount.Text))), true);
            int count = 100;
            if(!String.IsNullOrEmpty(rbTotal.SelectedValue))
                count = int.Parse(rbTotal.SelectedValue);
            else if (!String.IsNullOrEmpty(TotalCount.Text))
            {
                if (int.TryParse(TotalCount.Text, out count) && !(count > 0 && count <= 1000))
                    count = 100;
            }
            Response.Redirect(String.Format("{0}?companyID={1}&count={2}", Settings.Default.GenerateMemberCodeUrl, SellerID.Selector.SelectedValue, count));

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(UXB2BCardGen_PreRender);
        }

        void UXB2BCardGen_PreRender(object sender, EventArgs e)
        {
            btnCreate.OnClientClick = Page.ClientScript.GetPostBackEventReference(btnCreate, "") + String.Format(";document.all(\"{0}\").disabled=true;return false;", btnCreate.UniqueID);
        }
    }
}