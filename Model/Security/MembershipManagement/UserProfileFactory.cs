using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Xml;
using System.Security.Cryptography.X509Certificates;

using DataAccessLayer.basis;

using Model.BaseManagement;
using Model.Resource;
using Model.Properties;

namespace Model.Security.MembershipManagement
{
    using UserProfileImpl = UserProfileMember;
    using Model.DataEntity;
	/// <summary>
	/// Summary description for UserProfile.
	/// </summary>
	public class UserProfileFactory
	{

		private UserProfileFactory()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static UserProfileMember CreateInstance(int uid)
		{
			UserProfileMember profile = null;
			
			using(UserManager mgr = new UserManager())
			{
                UserProfile entity = mgr.GetUserProfile(uid);
                if (entity != null)
                {
                    profile = new UserProfileImpl();
                    profile._profileEntity = entity;
                    profile.DetermineUserRole();
                }
			}

			return profile;
		}

        public static UserProfileMember CreateInstance(string pid, string password)
        {
            UserProfileImpl profile = CreateInstance(pid);
            if (profile != null) 
            {
                //先檢查原密碼,如果不符再檢查IFSMM代進的密碼
                com.uxb2b.util.CipherDecipherSrv cipher = new com.uxb2b.util.CipherDecipherSrv(10);
                if (!String.IsNullOrEmpty(profile._profileEntity.Password) && password.Equals(cipher.decipher(profile._profileEntity.Password)))
                {
                    profile._profileEntity.Password = password;
                    return profile;
                }
                //2010/7/28 改用第二組md5的密碼 Allen
                else if (String.Compare(Utility.ValueValidity.MakePassword(password), profile._profileEntity.Password2, true) == 0)
                {
                    profile._profileEntity.Password = password;
                    return profile;
                }
            }

            return null;
        }

        public static UserProfileMember CreateInstance(string pid)
        {
            UserProfileMember profile = null;

            using (UserManager mgr = new UserManager())
            {
                UserProfile entity = mgr.GetUserProfileByPID(pid);

                if (entity != null)
                {
                    //檢查密碼
                    profile = new UserProfileImpl();
                    profile._profileEntity = entity;
                    profile.DetermineUserRole();
                }
            }
            return profile;
        }

        
		public static void LoadUserProfile(Schema.dsUserProfile dsProfile, DataSet ds)
		{
			XmlNodeReader xnr = new XmlNodeReader(new XmlDataDocument(ds));
			dsProfile.ReadXml(xnr,XmlReadMode.IgnoreSchema);
			xnr.Close();
		}

        public static UserProfileMember CreateInstance(X509Certificate2 cert)
        {
            using (UserProfileManager mgr = new UserProfileManager())
            {
                UserProfile item = mgr.GetUserProfile(cert);
                if (item != null)
                {
                    return CreateInstance(item.UID);
                }
            }
            return null;
        }

        public static UserProfileMember CreateInstance(Guid token)
        {
            using (UserProfileManager mgr = new UserProfileManager())
            {
                UserProfile item = mgr.GetUserProfile(token, Settings.Default.SessionTimeout);
                if (item != null)
                {
                    return CreateInstance(item.UID);
                }
            }
            return null;
        }
    }
}
