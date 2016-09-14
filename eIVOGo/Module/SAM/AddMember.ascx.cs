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

namespace eIVOGo.Module.SAM
{
    public partial class AddMember : System.Web.UI.UserControl
    {
        private UserProfileMember _userProfile;
        public String Title = "會員管理-新增帳號";
        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
            if (!this.IsPostBack)
            {
                
                this.trservicerule.Visible = false;
                this.RegisterMessage1.Visible = false;
                initdlldata();
                if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GUEST)
                {
                    Title = "加入會員";
                    this.trRole.Visible = false;
                    this.trservicerule.Visible = true;
                }
                if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_BUYER )
                {
                    //if (Request["PID"] != null)
                    //{
                    //    if (Request["PID"].ToString() == _userProfile.CurrentUserRole.UserProfile.PID)
                    //    {
                            Title = "修改帳號";
                            this.trRole.Visible = false;        
                            getUserdata();
                    //    }
                    //    else
                    //    {
                    //        Response.Redirect("/MainPage.aspx");
                    //    }

                    //}
                    //else
                    //{
                    //    Response.Redirect("/MainPage.aspx");
                    //}

                }
                if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS )
                {
                    Title = "會員管理-新增帳號";
                    if (Request["PID"] != null)
                    {
                        Title = "會員管理-修改帳號";
                        getUserdata();

                    }
                    this.btnOK.Text = "確認";
                }
                
            }
        }
        protected void initdlldata()
        {
            ddlCompany.Visible = false;
            this.ddlCompany.Items.Clear();
            this.ddlRole.Items.Clear();
            ddlRole.Items.Add(new ListItem("進階會員", ((int)Naming.RoleID.ROLE_BUYER).ToString()));
            ddlRole.Items.Add(new ListItem("店家", ((int)Naming.RoleID.ROLE_SELLER).ToString()));
            using (UserManager mgr = new UserManager())
            {
                
                var seller = from c in mgr.GetTable<OrganizationCategory>().Where(w => w.CategoryID == (int)Naming.CategoryID.COMP_E_INVOICE_B2C_SELLER).ToList() 
                             select new 
                             {
                                 companyname = c.Organization.CompanyName ,
                                 cateid = c.OrgaCateID 
                             };

                ddlCompany.DataSource = seller;
                ddlCompany.DataTextField = "companyname";
                ddlCompany.DataValueField = "cateid";
                ddlCompany.DataBind(); 


            }
        }
        protected void getUserdata()
        {
            lblupdpw.Visible = true;
            this.ddlRole.Enabled = false;
            this.ddlCompany.Enabled = false;
            this.txtID.Visible = false;
            this.lblID.Visible = true;
            this.btnchkid.Visible = false;
            this.trCaptchaImg.Visible = false;
            this.trservicerule.Visible = false;
            using (UserManager mgr = new UserManager())
            {
                string pid = "";
                if (Request["PID"] != null && _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS)
                {
                    pid=Request["PID"].ToString().Trim();
                 }
                if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_BUYER)
                {
                    pid = _userProfile.PID;
                    this.btnOK.Text = "確認";
                }
                var userdata = mgr.GetAllUsers().Where(w => w.PID ==pid ).FirstOrDefault();
                this.txtID.Text = userdata.PID;
                this.lblID.Text = userdata.PID;
                this.txtAddr.Text = userdata.Address;
                this.txtPhone1.Text = userdata.Phone;
                this.txtMobilePhone.Text = userdata.MobilePhone;
                this.txtName.Text = userdata.UserName;
                this.txtEmail.Text = userdata.EMail;


            }
        }

        protected void btnchkid_Click(object sender, EventArgs e)
        {
            this.lblIDmsg.Text = "";
            using (UserManager mgr = new UserManager())
            {
                if (mgr.GetAllUsers().Where(w => w.PID == this.txtID.Text.Trim()).Count() > 0)
                {
                    this.lblIDmsg.ForeColor = System.Drawing.Color.Red;
                    this.lblIDmsg.Text = "這個帳號已被使用，請更換申請帳號!!";
                }
                else
                {
                    this.lblIDmsg.ForeColor = System.Drawing.Color.Green;
                    this.lblIDmsg.Text = "這個帳號尚未被申請，可以使用";
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkdata())
                {
                    using (UserManager mgr = new UserManager())
                    {
                        int RoleID ;
                        int OrgCateID;
                        if (lblID.Visible == false)
                        {

                            RoleID = int.Parse (ddlRole.SelectedValue);
                            if (ddlRole.SelectedValue == Naming.RoleID.ROLE_BUYER.ToString())
                            {
                                OrgCateID = mgr.GetTable<OrganizationCategory>().Where(w => w.Organization.ReceiptNo == "CONSUMER" && w.CategoryDefinition.Category == "買方").FirstOrDefault().OrgaCateID;
                            }
                            else
                            {
                                OrgCateID = int.Parse (ddlCompany.SelectedValue);
                            }
                            
                            Model.DataEntity.UserRole role = new Model.DataEntity.UserRole();
                            Model.DataEntity.UserProfile up = new Model.DataEntity.UserProfile();
                            up.PID = this.txtID.Text;
                            
                                up.Password2 = Utility.ValueValidity.MakePassword(this.txtPassword.Text.Trim());
                          
                            up.Phone = this.txtPhone1.Text.Trim();
                            //up.Phone2 = this.txtPhone2.Text.Trim();
                            up.UserName = this.txtName.Text.Trim();
                            up.MobilePhone = this.txtMobilePhone.Text.Trim();
                            up.Address = this.txtAddr.Text;
                            up.EMail = this.txtEmail.Text;
                            role.OrgaCateID = OrgCateID;
                            role.RoleID = RoleID;
                            role.UserProfile = up;
                            mgr.GetTable<UserProfile>().InsertOnSubmit(up);
                            mgr.GetTable<UserRole>().InsertOnSubmit(role);
                            mgr.SubmitChanges();
                            this.mainpage.Visible = false;
                            this.RegisterMessage1.Visible = true;
                        }
                        else
                        {
                            string pid = "";
                            if (Request["PID"] != null && _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS)
                            {
                                pid = Request["PID"].ToString().Trim();
                            }
                            if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_BUYER)
                            {
                                pid = _userProfile.PID;
                              
                            }
                            var up = mgr.GetAllUsers().Where(w => w.PID == pid).FirstOrDefault();
                            if (this.txtPassword.Text.Trim().Length > 0)
                            {
                                up.Password2 = Utility.ValueValidity.MakePassword(this.txtPassword.Text.Trim());
                            }
                                 up.Phone = this.txtPhone1.Text.Trim();
                                //up.Phone2 = this.txtPhone2.Text.Trim();
                                up.UserName = this.txtName.Text.Trim();
                                up.MobilePhone = this.txtMobilePhone.Text.Trim();
                                up.Address = this.txtAddr.Text;
                                up.EMail = this.txtEmail.Text;
                                mgr.SubmitChanges();
                                this.mainpage.Visible = false;
                                this.RegisterMessage1.Visible = true;


                            
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                this.lblmsg.Text = "系統發生錯誤,錯誤原因:" + ex.Message;
            }
            finally
            {

            }
        }

        private bool checkdata()
        {
            //檢查帳號
            if (lblID.Visible == false)
            {
                if (!checkID(this.txtID.Text))
                {
                    this.lblIDmsg.ForeColor = System.Drawing.Color.Red;
                    this.lblIDmsg.Text = "這個帳號已被使用，請更換申請帳號!!";
                    return false;
                }
                if (!this.CaptchaImg1.Verify())
                {
                    //檢查驗證碼
                    this.lblmsg.Text = "驗證碼輸入錯誤!!";
                    return false;
                }
            }
            if (this.txtName.Text.Trim() == "")
            {
                //檢查名稱
                this.lblmsg.Text = "名字還沒輸入!!";
                return false;
            }
            if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_GUEST || (Request["PID"] == null && _userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS))
            {
                if (this.txtPassword.Text.Trim() == "")
                {
                    //檢查密碼
                    this.lblmsg.Text = "密碼不可為空白!!";
                    return false;
                }
                if (this.txtPassword.Text.Trim().Length < 6)
                {
                    //檢查密碼
                    this.lblmsg.Text = "密碼不可少於６個字碼!!";
                    return false;
                }
            }
            else
            {
                if (this.txtPassword.Text.Trim() != "" && this.txtPassword.Text.Trim().Length < 6)
                {
                    this.lblmsg.Text = "密碼不可少於６個字碼!!";
                    return false;
                }
            }
            if (!this.CompareValidator1.IsValid  )
            {
                //檢查密碼
                this.lblmsg.Text = "二組密碼輸入不同!!";
                return false;
            }
            if (this.txtEmail.Text.Trim() == "" || !txtEmail.Text.Contains('@') || !txtEmail.Text.Contains('.'))
            {
                //檢查email
                this.lblmsg.Text = "電子信箱尚未輸入或輸入錯誤!!";
                return false;
            }
            if (this.txtAddr.Text.Trim() == "")
            {
                //檢查住址
                this.lblmsg.Text = "住址尚未輸入!!";
                return false;
            }
            if (this.txtMobilePhone.Text.Trim() == "" || this.txtMobilePhone.Text.Trim().Length < 10 || this.txtMobilePhone.Text.Substring(0,2) != "09" )
            {
                //檢查行動電話
                this.lblmsg.Text = "手機號碼尚未輸入或輸入錯誤!!";
                return false;
            }
          

            return true;
        }

        private bool checkID(string id)
        {

            using (UserManager mgr = new UserManager())
            {
                if (mgr.GetAllUsers().Where(w => w.PID == id.Trim()).Count() == 0)
                    return true;
            }
            return false;
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRole.SelectedValue == ((int)Naming.RoleID.ROLE_BUYER).ToString())
            {
                ddlCompany.Visible = false;
            }
            else
            {
                ddlCompany.Visible = true;
            }
        }
    }
}