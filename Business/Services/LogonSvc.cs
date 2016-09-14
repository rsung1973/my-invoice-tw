using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Web.SessionState;

using Model;
using Model.Resource;
using Model.Security.MembershipManagement;

namespace Services
{
	/// <summary>
	/// Summary description for LogonSvc.
	/// </summary>
	public class LogonSvc
	{

		private LogonSvc()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		
		public static UserProfileMember CreateUserProfile(int uid)
		{
			//
			// TODO: ���ҨϥΪ̵n�J,�ëإ�UserProfile.
			//
			return UserProfileFactory.CreateInstance(uid);
		}

		public static UserProfileMember CreateUserProfile(String PID,String PWD)
		{
			//
			// TODO: ���ҨϥΪ̵n�J,�ëإ�UserProfile.
			//
			return UserProfileFactory.CreateInstance(PID,PWD);
		}

        public static UserProfileMember CreateUserProfile(String pid)
        {
            return UserProfileFactory.CreateInstance(pid);
        }

        public static UserProfileMember CreateUserProfile(X509Certificate2 cert)
        {
            return UserProfileFactory.CreateInstance(cert);
        }

        //public static UserProfile CreateUserProfileRemote(int uid)
        //{
        //    //
        //    // TODO: ���ҨϥΪ̵n�J,�ëإ�UserProfile.
        //    //
        //    return UserProfileFactory.CreateInstanceRemote(uid);
        //}

        //public static UserProfile CreateUserProfileRemote(String PID,String PWD)
        //{
        //    //
        //    // TODO: ���ҨϥΪ̵n�J,�ëإ�UserProfile.
        //    //
        //    return UserProfileFactory.CreateInstanceRemote(PID,PWD);
        //}


		public static UserProfileMember GetUserProfile(UIType uiType)
		{
			UserProfileMember profile = null;

			if(uiType == UIType.WebUI)
			{
                HttpSessionState session = System.Web.HttpContext.Current.Session;
                if (session != null)
                {
                    profile = (UserProfileMember)session[WebKey.USER_PROFILE];
                }
			}
			return profile;
		}

		public static void UserLogout()
		{
            FormsAuthentication.SignOut();
			System.Web.HttpContext.Current.Session.Abandon();
		}
	}
}
