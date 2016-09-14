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

namespace eIVOGo.Module.SAM
{
    public partial class EditMember : CreateMember
    {
        UserProfileMember user = WebPageUtility.UserProfile;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(EditMember_Load);
        }

        void EditMember_Load(object sender, EventArgs e)
        {
            initializeData();
            
        }

        protected virtual void initializeData()
        {
            if (!this.IsPostBack && Page.PreviousPage != null)
            {
                btnOK.CommandArgument = Page.PreviousPage.Items["uid"] as String;
            }

            if (!String.IsNullOrEmpty(btnOK.CommandArgument))
            {
                var item = dsUserProfile.CreateDataManager().EntityList.Where(u => u.UID == int.Parse(btnOK.CommandArgument)).FirstOrDefault();
                if (item == null)
                {
                    WebMessageBox.AjaxAlert(this, "使用者不存在!!");
                }
                EditItem.DataItem = item;
            }
        }
        protected override void btnOK_Click(object sender, EventArgs e)
        {
            UserProfile profile = EditItem.Update();
            if (profile == null)
                return;

            try
            {
                if (EditItem.Validate())
                {
                    var mgr  = dsUserProfile.CreateDataManager();
                    mgr.SubmitChanges();

                    if (profile.UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Wait_For_Check)
                    {
                        user.CurrentSiteMenu = null;
                    }

                    if (String.IsNullOrEmpty(btnOK.CommandArgument))
                    {
                        Response.Redirect(String.Format("register_msg.aspx?action={0}", Server.UrlEncode(actionItem.ItemName)));
                    }
                    else
                    {
                        Response.Redirect(String.Format("register_msg.aspx?action={0}&backUrl=memberManager.aspx", Server.UrlEncode(actionItem.ItemName)));
                    }
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