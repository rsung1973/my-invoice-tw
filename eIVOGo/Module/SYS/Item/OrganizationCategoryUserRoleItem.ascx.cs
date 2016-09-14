using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SYS.Item
{
    public partial class OrganizationCategoryUserRoleItem : UserRoleDefinitionItem 
    {

        private UserProfileMember _userProfile;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var mgr = dsRole.CreateDataManager();
                var table = mgr.GetTable<OrganizationCategoryUserRole>();

                UserRoleDefinition role = mgr.EntityList.Where(r => r.Role == (String)e.Values["Role"]).FirstOrDefault();
                if (role == null)
                {
                    var initItem = mgr.EntityList.OrderByDescending(r => r.RoleID).FirstOrDefault();
                    int roleID = initItem != null ? initItem.RoleID + 1 : 1;
                    role = new UserRoleDefinition
                        {
                            RoleID = roleID,
                            Role = (String)e.Values["Role"]
                        };
                }

                OrganizationCategoryUserRole item = new OrganizationCategoryUserRole
                {
                    OrgaCateID = _userProfile.CurrentUserRole.OrgaCateID,
                    UserRoleDefinition = role
                };
                table.InsertOnSubmit(item);
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("新增資料失敗,原因:{0}", ex.Message));
            }
        }

        protected override void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                var mgr = dsRole.CreateDataManager();
                var item = mgr.EntityList.Where(r => r.RoleID == (int)e.Keys[0]).First();
                item.Role = (String)e.NewValues[0];
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
        }

    }
}