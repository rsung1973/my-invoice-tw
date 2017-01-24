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
using eIVOGo.Module.Base;
using Utility;
using com.uxb2b.util;

namespace eIVOGo.Module.Entity
{
    public partial class UserProfileItem : EditEntityItemBase<EIVOEntityDataContext,UserProfile>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.QueryExpr = u => u.UID == (int?)modelItem.DataItem;
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();

            loadEntity();

            String pid = PID.Text.Trim();
            if (_entity == null || _entity.PID != pid)
            {
                if (mgr.EntityList.Any(o => o.PID == pid))
                {
                    Updatepanel1.AjaxAlert("這個帳號已被使用，請更換申請帳號!!");
                    return false;
                }
            }

            if (_entity == null)
            {
                _entity = new UserProfile
                {
                    UserProfileStatus = new UserProfileStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                    }
                };
                mgr.EntityList.InsertOnSubmit(_entity);
            }

            _entity.PID = pid;

            if (checkInput())
            {
                mgr.SubmitChanges();
                return true;
            }

            return false;
        }


        protected void btnchkid_Click(object sender, EventArgs e)
        {
            String pid = PID.Text.Trim();
            var mgr = dsEntity.CreateDataManager();

            if (mgr.EntityList.Any(o => o.PID == pid))
            {
                Updatepanel1.AjaxAlert("這個帳號已被使用，請更換申請帳號!!");
            }
            else
            {
                Updatepanel1.AjaxAlert("這個帳號尚未被申請，可以使用");
            }
        }

        private bool checkInput()
        {
            //檢查帳號
            if (!this.CaptchaImg1.Verify())
            {
                //檢查驗證碼
                Updatepanel1.AjaxAlert("驗證碼輸入錯誤!!");
                return false;
            }
            _entity.UserName = UserName.Text.Trim();
            if (String.IsNullOrEmpty(_entity.UserName))
            {
                //檢查名稱
                Updatepanel1.AjaxAlert("名字還沒輸入!!");
                return false;
            }

            if (_entity.UserRole.Count > 0)
            {
                dsEntity.CreateDataManager().GetTable<UserRole>().DeleteAllOnSubmit(_entity.UserRole);
            }

            new UserRole
           {
               UserProfile = _entity,
               RoleID = int.Parse(RoleID.SelectedValue),
               OrgaCateID = int.Parse(OrgaCateID.SelectedValue)
           };

            String pwd = Password.Text.Trim();
            String pwd1 = Password1.Text.Trim();

            if (String.IsNullOrEmpty(pwd))
            {
                //檢查密碼
                Updatepanel1.AjaxAlert("密碼不可為空白!!");
                return false;
            }
            else if (pwd.Length < 6)
            {
                //檢查密碼
                Updatepanel1.AjaxAlert("密碼不可少於６個字碼!!");
                return false;
            }
            else if (pwd != pwd1)
            {
                //檢查密碼
                Updatepanel1.AjaxAlert("二組密碼輸入不同!!");
                return false;
            }
            _entity.Password2 = ValueValidity.MakePassword(pwd);
            _entity.Password = (new CipherDecipherSrv()).cipher(pwd);
            if (_entity.UserProfileStatus == null)
            {
                _entity.UserProfileStatus = new UserProfileStatus { };
            }
            _entity.UserProfileStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;

            _entity.EMail = EMail.Text.Trim();

            if (String.IsNullOrEmpty(_entity.EMail) || !ValueValidity.ValidateString(_entity.EMail, 16))
            {
                //檢查email
                Updatepanel1.AjaxAlert("電子信箱尚未輸入或輸入錯誤!!");
                return false;
            }
            _entity.Address = Address.Text.Trim();
            if (String.IsNullOrEmpty(_entity.Address))
            {
                //檢查住址
                Updatepanel1.AjaxAlert("住址尚未輸入!!");
                return false;
            }
            _entity.MobilePhone = MobilePhone.Text.Trim();
            if (String.IsNullOrEmpty(_entity.MobilePhone) || _entity.MobilePhone.Length < 10 || !_entity.MobilePhone.StartsWith("09"))
            {
                //檢查行動電話
                Updatepanel1.AjaxAlert("手機號碼尚未輸入或輸入錯誤!!");
                return false;
            }

            _entity.Phone = Phone.Text.Trim();
            _entity.Phone2 = Phone2.Text.Trim();

            return true;
        }

    }
}