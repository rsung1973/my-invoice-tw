using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;

namespace eIVOGo
{
    public partial class main_page : System.Web.UI.MasterPage
    {
        private UserProfileMember _userProfile;
        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.IsPostBack)
            {
               
                //系統管理員
                this.lblRole.Text = _userProfile.CurrentUserRole.UserRoleDefinition.Role+":"+_userProfile.UserName ;
                //if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS )
                //{
                //    this.regist.Visible = false;
                //    //this.cardBelong.Visible = false;
                    
                //}
                ////店家
                //if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SELLER || _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GOOGLETW)
                //{
                //    this.regist.Visible = false;
                //    //this.cardBelong.Visible = false;
                //}
                ////會員
                //if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_BUYER)
                //{
                //    this.regist.Visible = false;
                //    //this.cardBelong.Visible = true;
                //}
                ////訪客
                //if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GUEST)
                //{
                //     this.regist.Visible = false;
                //    //this.cardBelong.Visible = false;
                //}
            }
        }
    }
}