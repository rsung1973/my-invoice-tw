using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;



using Model.Resource;
using DataAccessLayer.basis;
using Model.ModelTemplate;
using Model.DataEntity;
using Model.Locale;
using System.Xml.Linq;

namespace Model.Security.MembershipManagement
{
	/// <summary>
	/// UserManager ªººK­n´y­z¡C
	/// </summary>
	public class UserManager: EIVOGenericManager<UserProfile>
    {
        public UserManager() : base() { }
        public UserManager(GenericManager<EIVOEntityDataContext > mgr) : base(mgr) { }

        internal UserProfile GetUserProfile(int uid)
        {
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<Model.DataEntity.UserProfile>(u => u.UserRole);
            ops.LoadWith<Model.DataEntity.UserRole>(r => r.UserRoleDefinition);
            ops.LoadWith<Model.DataEntity.UserRole>(r => r.OrganizationCategory);
            ops.LoadWith<Model.DataEntity.OrganizationCategory>(c => c.CategoryDefinition);
            ops.LoadWith<Model.DataEntity.OrganizationCategory>(c => c.Organization);
            
            _db.LoadOptions = ops;

            return _db.UserProfile.Where(u => u.UID == uid).FirstOrDefault();
        }


        internal UserProfile GetUserProfileByPID(string pid)
        {
            DataLoadOptions ops = new DataLoadOptions();
            ops.LoadWith<Model.DataEntity.UserProfile>(u => u.UserRole);
            ops.LoadWith<Model.DataEntity.UserRole>(r => r.UserRoleDefinition);
            ops.LoadWith<Model.DataEntity.UserRole>(r => r.OrganizationCategory);
            ops.LoadWith<Model.DataEntity.OrganizationCategory>(c => c.CategoryDefinition);
            ops.LoadWith<Model.DataEntity.OrganizationCategory>(c => c.Organization);
            ops.LoadWith<Model.DataEntity.UserProfile>(u => u.UserProfileStatus);
            
            _db.LoadOptions = ops;

            return _db.UserProfile.Where(u => u.PID == pid & u.UserProfileStatus.CurrentLevel != (int)Naming.MemberStatusDefinition.Mark_To_Delete).FirstOrDefault();
        }

       

        public IEnumerable<UserProfile> GetAllUsers()
        {
            return _db.UserProfile;
        }

       
    }

    public static partial class ExtensionMethods
    { 
        public static string GetCurentSiteMenu(this Model.DataEntity.UserProfile profile,int roleID,int categoryID)
        {
            using (UserManager mgr = new UserManager())
            {
                Model.DataEntity.MenuControl menu = mgr.GetTable<Model.DataEntity.UserMenu>().Where(m => m.RoleID == roleID && m.CategoryID == categoryID).Select(m => m.MenuControl).FirstOrDefault();
                if (menu != null)
                {
                    return menu.SiteMenu;
                }
            }
            return null;
        }

        public static XElement GetOrganizationCategoryUserRoleMenuContent(this Model.DataEntity.UserProfile profile, int roleID, int orgaCateID)
        {
            using (UserManager mgr = new UserManager())
            {
                var menu = mgr.GetTable<Model.DataEntity.OrganizationCategoryUserRole>().Where(m => m.RoleID == roleID && m.OrgaCateID == orgaCateID).FirstOrDefault();
                if (menu != null)
                {
                    return menu.MainMenu;
                }
            }
            return null;
        }

    }

	public enum LoginStatus
	{
		FirstLogin,
		ExpiredPassword,
		ExpiredSystemPassword,
		NeedAuthorized,
		NormalLogin
	}
}
