using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.Security.MembershipManagement;
using Uxnet.Web.Module.SiteAction;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SYS
{
    public partial class EditMemberMenuItem : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        public int? RoleID
        {
            get
            {
                return (int?)ViewState["roleID"];
            }
            set
            {
                ViewState["roleID"] = value;
            }
        }

        private void initializeData()
        {
            var item = dsRole.CreateDataManager().EntityList.Where(o => o.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID && o.RoleID == RoleID).FirstOrDefault();
            if (item != null && item.MainMenu != null)
            {
                menuFactory.BindData(item.MainMenu);
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            var mgr = dsRole.CreateDataManager();
            var item = mgr.EntityList.Where(o => o.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID && o.RoleID == RoleID).FirstOrDefault();
            if (item != null)
            {
                item.MainMenu = menuFactory.Save();
                mgr.SubmitChanges();
                item.MainMenu.Save(Path.Combine(SiteMenuBar.MenuManager.StoredPath, _userProfile.GetOrganizationCategoryUserRoleMenuPath(RoleID.Value, _userProfile.CurrentUserRole.OrgaCateID)));
                this.AjaxAlert("工作選單設定完成!!");
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            initializeData();
        }
    }
}