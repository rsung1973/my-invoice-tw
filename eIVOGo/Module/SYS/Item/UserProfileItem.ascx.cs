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
    public partial class UserProfileItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;
        private UserProfileMember _userProfile;

        protected void Page_Load(object sender, EventArgs e)
        {
            _userProfile = WebPageUtility.UserProfile;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = r => false;
            }
        }

        public Expression<Func<UserProfile, bool>> QueryExpr
        { get; set; }

        public void Show()
        {
            dvEntity.ChangeMode(DetailsViewMode.Insert);
            this.ModalPopupExtender.Show();
        }

        public void BindData()
        {
            dvEntity.ChangeMode(DetailsViewMode.Edit);
            dvEntity.DataBind();
            this.ModalPopupExtender.Show();

        }

        protected void dvEntity_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                this.ModalPopupExtender.Hide();
            }
        }

        protected void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                UserProfile item = new UserProfile
                {
                    UserName = (String)e.Values["UserName"],
                    PID = (String)e.Values["PID"],
                    Password = (new com.uxb2b.util.CipherDecipherSrv()).cipher((String)e.Values["Password"]),
                    ContactTitle = (String)e.Values["ContactTitle"],
                    Address = (String)e.Values["Address"],
                    MobilePhone = (String)e.Values["MobilePhone"],
                    Phone = (String)e.Values["Phone"],
                    EMail = (String)e.Values["EMail"],
                    Password2 = ValueValidity.MakePassword((String)e.Values["Password"]),
                    Creator = _userProfile.UID,
                    UserProfileStatus = new UserProfileStatus
                    {
                        CurrentLevel = (int)Naming.MemberStatusDefinition.Checked
                    }
                };
                mgr.EntityList.InsertOnSubmit(item);
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("新增資料失敗,原因:{0}", ex.Message));
            }
        }

        protected void dvEntity_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        protected void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                var item = mgr.EntityList.Where(r => r.UID == (int)e.Keys[0]).First();
                item.UserName = (String)e.NewValues["UserName"];
                item.PID = (String)e.NewValues["PID"];
                item.Password = (new com.uxb2b.util.CipherDecipherSrv()).cipher((String)e.NewValues["Password"]);
                item.Password2 = ValueValidity.MakePassword((String)e.NewValues["Password"]);
                item.ContactTitle = (String)e.NewValues["ContactTitle"];
                item.Address = (String)e.NewValues["Address"];
                item.MobilePhone = (String)e.NewValues["MobilePhone"];
                item.Phone = (String)e.NewValues["Phone"];
                item.EMail = (String)e.NewValues["EMail"];
                mgr.SubmitChanges();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
            
        }
    }
}