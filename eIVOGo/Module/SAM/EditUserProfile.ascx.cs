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
using Uxnet.Web.WebUI;
using System.Text.RegularExpressions;
using eIVOGo.Properties;

namespace eIVOGo.Module.SAM
{
    public partial class EditUserProfile : System.Web.UI.UserControl
    {
        private UserProfile _dataItem;
        UserProfileMember _userProfile = WebPageUtility.UserProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DataItem == null)
                {
                    this.txtID.Attributes.Add("value", "由系統產生");
                    this.txtID.Enabled = false;
                    this.lblIDWarning.Visible = true;
                    string _tempPass = Utility.ExtensionMethods.CreateRandomPassword(6);
                    this.txtPassword.Attributes.Add("value", _tempPass);
                    this.txtPassword2.Attributes.Add("value", _tempPass);
                    this.txtPassword.Enabled = false;
                    this.txtPassword2.Enabled = false;
                    this.lblupdpw.Text = "新增會員時，密碼由系統產生";
                    this.lblupdpw.ForeColor = System.Drawing.Color.Red;
                    this.lblWorning.Visible = false;
                }
            }
        }

        public UserProfile DataItem
        {
            get
            {
                return _dataItem;
            }
            set
            {
                _dataItem = value;
                if (_dataItem != null && _dataItem.UserProfileExtension == null)
                {
                    _dataItem.UserProfileExtension = new UserProfileExtension();
                }
            }
        }

        public string NewPassword
        {
            get
            {
                return this.txtPassword.Text;
            }
        }

        public override void DataBind()
        {
            if (DataItem != null)
            {
                base.DataBind();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(AddMember_PreRender);
            this.dsCarrier.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceUserCarrier>>(dsCarrier_Select);
        }

        void dsCarrier_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<InvoiceUserCarrier> e)
        {
            if (_dataItem != null)
            {
                e.QueryExpr = c => c.UID == _dataItem.UID;
            }
            else
            {
                e.QueryExpr = c => false;
            }
        }

        void AddMember_PreRender(object sender, EventArgs e)
        {
            if (DataItem != null)
            {
                this.DataBind();
                if (_userProfile.CurrentUserRole.RoleID == (int)Naming.RoleID.ROLE_SYS)
                {
                    if (DataItem.UserProfileStatus.CurrentLevel == (int)Naming.MemberStatusDefinition.Wait_For_Check)
                    {
                        this.txtID.Enabled = false;
                        this.txtPassword.Enabled = false;
                        this.txtPassword2.Enabled = false;
                    }
                }
            }
        }

        public UserProfile Update()
        {
            if (DataItem == null)
            {
                ///新增帳號
                ///
                if (String.IsNullOrEmpty(txtPassword.Text))
                {
                    //檢查密碼
                    this.AjaxAlert("密碼不可為空白!!");
                    return null;
                }

                DataItem = new UserProfile();
            }

            //檢查帳號
            var mgr = dsCarrier.CreateDataManager();
            var pid = this.txtID.Text.Trim().Equals("由系統產生") ? this.txtEmail.Text.Trim() : this.txtID.Text.Trim();
            if (_dataItem.PID!=pid && mgr.GetTable<UserProfile>().Where(w => w.PID == pid).Count() > 0)
            {
                this.AjaxAlert("這個帳號已被使用，請更換申請帳號!!");
                DataItem = null;
                return null;
            }

            //若為待確認人員須檢查密碼是否更改,沒有更改則須強迫變更並異動人員狀態
            if (_userProfile.CurrentUserRole.RoleID != (int)Naming.RoleID.ROLE_SYS)
            {
                if (mgr.GetTable<UserProfileStatus>().Where(u => u.UID == DataItem.UID).Count() > 0)
                {
                    if (mgr.GetTable<UserProfileStatus>().Where(u => u.UID == DataItem.UID).FirstOrDefault().CurrentLevel == (int)Naming.MemberStatusDefinition.Wait_For_Check)
                    {
                        if (!string.IsNullOrEmpty(this.txtPassword.Text))
                        {
                            if (Utility.ValueValidity.MakePassword(this.txtPassword.Text).Equals(DataItem.Password2))
                            {
                                this.AjaxAlert("密碼不可與預設值相同!!");
                                DataItem = null;
                                return null;
                            }
                            else
                            {
                                DataItem.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
                            }
                        }
                        else
                        {
                            this.AjaxAlert("首次登入請更改密碼!!");
                            DataItem = null;
                            return null;
                        }
                    }
                }
            }

            Regex reg = new Regex("^(?=.*\\d)(?=.*[a-zA-Z])");

            if (!String.IsNullOrEmpty(txtPassword.Text))
            {
                if (txtPassword.Text.Length < 6)
                {
                    //檢查密碼
                    this.AjaxAlert("密碼不可少於６個字碼!!");
                    DataItem = null;
                    return null;
                }
                else if (!reg.IsMatch(this.txtPassword.Text))
                {                    
                    //檢查密碼
                    this.AjaxAlert("密碼須由英文、數字組成!!");
                    DataItem = null;
                    return null; 
                }
                else if (this.txtPassword.Text != txtPassword2.Text)
                {
                    //檢查密碼
                    this.AjaxAlert("二組密碼輸入不同!!");
                    DataItem = null;
                    return null;
                }

                DataItem.Password2 = Utility.ValueValidity.MakePassword(this.txtPassword.Text);
                DataItem.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
            }

            DataItem.PID = this.txtID.Text.Trim().Equals("由系統產生") ? this.txtEmail.Text.Trim() : this.txtID.Text.Trim();
            DataItem.Phone = this.txtPhone1.Text.Trim();
            DataItem.UserProfileExtension.NightPhone = this.txtPhone2.Text.Trim();
            DataItem.UserName = this.txtName.Text.Trim();
            DataItem.MobilePhone = this.txtMobilePhone.Text.Trim();
            DataItem.Address = this.txtAddr.Text.Trim();
            DataItem.EMail = this.txtEmail.Text.Trim();
            DataItem.UserProfileExtension.Birthday = null;

            return DataItem;
        }

        public bool Validate()
        {
            if (String.IsNullOrEmpty(_dataItem.UserName))
            {
                //檢查名稱
                WebMessageBox.AjaxAlert(this, "名字還沒輸入!!");
                DataItem = null;
                return false;
            }

            Regex reg = new Regex("\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            if (!reg.IsMatch(_dataItem.EMail))
            {
                //檢查email
                this.AjaxAlert("電子信箱尚未輸入或輸入錯誤!!");
                DataItem = null;
                return false;
            }
            if (String.IsNullOrEmpty(_dataItem.Address))
            {
                //檢查住址
                this.AjaxAlert("住址尚未輸入!!");
                DataItem = null;
                return false;
            }

            return true;
        }
    }
}