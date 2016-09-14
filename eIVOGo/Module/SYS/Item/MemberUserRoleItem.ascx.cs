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
    public partial class MemberUserRoleItem : UserRoleItem
    {
        private UserProfileMember _userProfile;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _userProfile = WebPageUtility.UserProfile;
            dsRole.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRoleDefinition>>(dsRole_Select);
        }

        void dsRole_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRoleDefinition> e)
        {
            e.Query = dsRole.CreateDataManager().GetTable<OrganizationCategoryUserRole>().Where(o => o.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID).Select(o => o.UserRoleDefinition);
        }


        protected override void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                
                DropDownList roleID = (DropDownList)dvEntity.Rows[1].FindControl("RoleID");

                if (!String.IsNullOrEmpty(roleID.SelectedValue))
                {
                    UserRole item = new UserRole
                    {
                        UID = int.Parse(((DropDownList)dvEntity.Rows[1].FindControl("UID")).SelectedValue),
                        RoleID = int.Parse(roleID.SelectedValue),
                        OrgaCateID = _userProfile.CurrentUserRole.OrgaCateID
                    };

                    if (mgr.EntityList.Any(r => r.UID == item.UID && r.RoleID == item.RoleID && r.OrgaCateID == item.OrgaCateID))
                    {
                        this.AjaxAlert("該使用者角色、所屬公司類別已存在!!請勿重複設定!!");
                    }
                    else
                    {
                        mgr.DeleteAnyOnSubmit(r => r.UID == item.UID && r.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID);
                        mgr.EntityList.InsertOnSubmit(item);
                        mgr.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("新增資料失敗,原因:{0}", ex.Message));
            }
        }

        protected void RoleID_DataBound(object sender, EventArgs e)
        {
            var item = dsEntity.CreateDataManager().EntityList.Where(r => r.UID == UID && r.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID).FirstOrDefault();
            if (item != null)
            {
                ((DropDownList)sender).SelectedValue = item.RoleID.ToString();
            }
        }


    }
}