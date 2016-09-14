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
using System.Net.Mail;
using System.Net;
using eIVOGo.Properties;
using System.Text;
using eIVOGo.Module.Common;

namespace eIVOGo.Module.SAM
{
    public partial class CreateMember : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected virtual void btnOK_Click(object sender, EventArgs e)
        {
            UserProfile profile = EditItem.Update();
            if (profile == null)
                return;

            try
            {
                if (EditItem.Validate())
                {
                    var mgr = dsUserProfile.CreateDataManager();
                    UserProfileManager userMgr = new UserProfileManager(mgr);
                    userMgr.CreateConsumerProfile(profile);
                    SharedFunction.sendMail(profile.EMail, profile.PID, this.EditItem.NewPassword, "member");
                    Response.Redirect("register_msg.aspx?backUrl=memberManager.aspx");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                btnOK.AjaxAlert("系統發生錯誤,錯誤原因:" + ex.Message);
            }
        }
    }
}