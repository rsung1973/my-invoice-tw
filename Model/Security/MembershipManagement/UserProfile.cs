using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections.Generic;

using DataAccessLayer.basis;


using Model.BaseManagement;
using Model.DataEntity;

namespace Model.Security.MembershipManagement
{
	/// <summary>
	/// Summary description for UserProfile.
	/// </summary>
	public interface IUserProfile
	{
		void Reload();

        //bool IsInsurer
        //{
        //    get;
        //}


        //string CurrentSiteMenu
        //{
        //    get;
        //}

        string MenuDataPath
        {
            get;
            set;
        }

		ToDoManager @ToDoManager
		{
			get;
		}

        int UID
        {
            get;
        }

        string PID
		{
			get;
		}

		string UserName
		{
			get;
		}

		string CompanyName
		{
			get;
		}

		

        IEnumerable<UserRole> DetermineUserRole();

        //DataSet GetProfileDataSet();

        //DataTable GetProfileData();

		UserRole CurrentUserRole
		{
			get;
		}

		UserProfile Profile
		{
			get;
		}

        IEnumerable<UserRole> UserRoleTable
        {
            get;
        }


		int RoleIndex
		{
			get;
			set;
		}


		void PutInfoObject(object key,object @value);

		object GetInfoObject(object key);

        object this[object index]
        {
            get;
            set;
        }

        void Remove(object index);
	}

	public enum USER_ROLE
	{
		SysAdmin = 1,	//	系統管理員
		Manager_A,		//	甲級主管
		Manager_B,		//	乙級主管
		Handler,		//	經辦
		Audit,			//	稽核
		ManagerAgent	//	代理主管
	}

}
