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
    public partial class UserRoleItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRole>>(dsEntity_Select);
        }

        public int? UID
        {
            get;
            set;
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRole> e)
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

        public Expression<Func<UserRole, bool>> QueryExpr
        { get; set; }

        public void Show()
        {
            dvEntity.ChangeMode(DetailsViewMode.Insert);
            dvEntity.DataBind();
            this.ModalPopupExtender.Show();
        }

        //public void BindData()
        //{
        //    dvEntity.ChangeMode(DetailsViewMode.Edit);
        //    dvEntity.DataBind();
        //    this.ModalPopupExtender.Show();

        //}

        protected void dvEntity_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            if (e.CommandName == "Cancel")
            {
                this.ModalPopupExtender.Hide();
            }
        }

        protected virtual void dvEntity_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            try
            {
                var mgr = dsEntity.CreateDataManager();
                UserRole item = new UserRole
                {
                    UID = int.Parse(((DropDownList)dvEntity.Rows[1].FindControl("UID")).SelectedValue),
                    RoleID = int.Parse(((DropDownList)dvEntity.Rows[1].FindControl("RoleID")).SelectedValue),
                    OrgaCateID = int.Parse(((OrgaCateSelector)dvEntity.Rows[1].FindControl("OrgaCateID")).OrgaCateID)
                };

                if (mgr.EntityList.Any(r => r.UID == item.UID && r.RoleID == item.RoleID && r.OrgaCateID == item.OrgaCateID))
                {
                    this.AjaxAlert("該使用者角色、所屬公司類別已存在!!請勿重複設定!!");
                }
                else
                {
                    mgr.EntityList.InsertOnSubmit(item);
                    mgr.SubmitChanges();
                }
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

        protected virtual void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert(String.Format("修改資料失敗,原因:{0}", ex.Message));
            }
            
        }

        protected void UID_PreRender(object sender, EventArgs e)
        {
            DropDownList userSelect = sender as DropDownList;
            if (userSelect != null)
            {
                userSelect.Items.Clear();
                userSelect.Items.AddRange(dsUser.CreateDataManager().EntityList.Where(u => u.UID == UID)
                    .Select(u => new ListItem(String.Format("{0}({1})", u.PID, u.UserName), u.UID.ToString())).ToArray());
            }

        }
    }
}