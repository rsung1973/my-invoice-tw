using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;

using Business.Helper;
using Model.Locale;
using Uxnet.Web.WebUI;
using System.Text.RegularExpressions;
using Utility;

namespace eIVOGo.Module.SAM.Action
{
    public partial class CreateInvoiceTrackCode : System.Web.UI.UserControl
    {
        protected UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EditItem.Done += new EventHandler(EditItem_Done);
        }

        protected virtual void EditItem_Done(object sender, EventArgs e)
        {
            _userProfile["msg"] = "字軌新增完成!!";
            Response.Redirect("~/EIVO/track_number_list.aspx");
        }
      
    }
}