using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections;

using DataAccessLayer.basis;


using Model.BaseManagement;

using System.Collections.Generic;
using System.Linq;
using Model.DataEntity;

namespace Model.Security.MembershipManagement
{
    /// <summary>
    /// Summary description for UserProfile.
    /// </summary>
    public class UserProfileMember : IUserProfile, IDisposable
    {
        protected internal UserProfile _profileEntity;
        protected internal ToDoManager _toDo;
        protected internal HybridDictionary _infoStore;
        protected internal int _roleIndex = -1;
        protected internal int _branchIndex = -1;
        protected internal Hashtable _values;
        protected internal string _menuDataPath;

        private bool _bDisposed = false;
        private String _currentSiteMenu;

        public UserProfileMember()
        {
            //
            // TODO: Add constructor logic here
            //
            _toDo = new ToDoManager();
            _values = new Hashtable();
        }

        public virtual void Reload()
        {
            using (UserManager mgr = new UserManager())
            {
                _profileEntity = mgr.GetUserProfile(_profileEntity.UID);
            }

        }


        //public bool IsInsurer
        //{
        //    get
        //    {
        //        return (int)_profileEntity.UserRole[_roleIndex].OrganizationCategory.CategoryID == Naming.INSURER;
        //    }
        //}


        public string CurrentSiteMenu
        {
            get
            {
                if (_currentSiteMenu == null)
                {
                    _currentSiteMenu = _profileEntity.GetCurentSiteMenu(_profileEntity.UserRole[_roleIndex].RoleID, _profileEntity.UserRole[_roleIndex].OrganizationCategory.CategoryID);
                }
                if (_currentSiteMenu == null && _profileEntity.GetOrganizationCategoryUserRoleMenuContent(_profileEntity.UserRole[_roleIndex].RoleID, _profileEntity.UserRole[_roleIndex].OrgaCateID) != null)
                {
                    _currentSiteMenu = this.GetOrganizationCategoryUserRoleMenuPath(_profileEntity.UserRole[_roleIndex].RoleID, _profileEntity.UserRole[_roleIndex].OrgaCateID);
                }
                return _currentSiteMenu;
            }
            set
            {
                _currentSiteMenu = value;
            }
        }

        public ToDoManager @ToDoManager
        {
            get
            {
                return _toDo;
            }
        }

        public string PID
        {
            get
            {
                return _profileEntity.PID;
            }
        }

        public int UID
        {
            get
            {
                return _profileEntity.UID;
            }
        }

        public string UserName
        {
            get
            {
                return _profileEntity.UserName;
            }
        }

        public string CompanyName
        {
            get
            {
                return _profileEntity.UserRole[_roleIndex].OrganizationCategory.Organization.CompanyName;
            }
        }

       


       
        public UserRole CurrentUserRole
        {
            get
            {
                return _profileEntity.UserRole[_roleIndex];
            }
        }

       

       

        public UserProfile Profile
        {
            get
            {
                return _profileEntity;
            }
        }

        public IEnumerable<UserRole> UserRoleTable
        {
            get
            {
                return _profileEntity.UserRole;
            }
        }


        public int RoleIndex
        {
            get
            {
                return _roleIndex;
            }
            set
            {
                _roleIndex = value;
            }
        }

        public int BranchIndex
        {
            get
            {
                return _branchIndex;
            }
            set
            {
                _branchIndex = value;
            }
        }

        //		public DataTable GetOrganizationBelongTo()
        //		{
        //			DataTable tblOrganization = dsProfile.Tables[OrganizationDALC.TABLE_NAME];
        //			if(tblOrganization==null)
        //			{
        //				//TODO:	讀取Organization中的資料
        //
        //				OrganizationDALC dalc = new OrganizationDALC();
        //				dalc.GetOrganization(dsProfile,(int)DetermineUserRole().Rows[0]["CompanyID"]);
        //				tblOrganization = dsProfile.Tables[OrganizationDALC.TABLE_NAME];
        //			}
        //
        //			return tblOrganization;
        //		}

        public void PutInfoObject(object key, object @value)
        {
            if (_infoStore == null)
            {
                _infoStore = new HybridDictionary();
            }

            if (_infoStore.Contains(key))
            {
                _infoStore[key] = @value;
            }
            else
            {
                _infoStore.Add(key, @value);
            }
        }

        public object GetInfoObject(object key)
        {
            return (_infoStore != null) ? _infoStore[key] : null;
        }

        public virtual IEnumerable<UserRole> DetermineUserRole()
        {
            _roleIndex = (_profileEntity.UserRole.Count > 0) ? 0 : -1;
            
            return _profileEntity.UserRole;
        }

        public String GetOrganizationCategoryUserRoleMenuPath(int roleID,int orgaCateID)
        {
            return String.Format("OrgaCate_{0}_{1}.xml", orgaCateID, roleID);
        }


        #region UserProfile Members


        public object this[object index]
        {
            get
            {
                return _values[index];
            }
            set
            {
                _values[index] = value;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (!_bDisposed)
            {
                _bDisposed = true;
                _values.Clear();
            }
            GC.SuppressFinalize(this);
        }

        #endregion

        ~UserProfileMember()
        {
            this.Dispose();
        }

        #region UserProfile Members


        public void Remove(object index)
        {
            _values.Remove(index);
        }

        public string MenuDataPath
        {
            get
            {
                return _menuDataPath;
            }
            set
            {
                _menuDataPath = value;
            }
        }

        #endregion

    }

    public static partial class ExtensionMethods
    {
        public static bool ChooseUserRoleBySpecifiedInfo(this UserProfileMember profile, int companyID, Model.Locale.Naming.CategoryID cateID)
        {
            int index = 0;
            foreach (var item in profile.UserRoleTable)
            {
                if (item.OrganizationCategory.CategoryID == (int)cateID && item.OrganizationCategory.CompanyID == companyID)
                {
                    profile.RoleIndex = index;
                    return true;
                }
                else
                {
                    index++;
                }
            }
            return false;
        }
    }

}
