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
    public partial class UserRoleDefinitionItem : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.dsRole.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRoleDefinition>>(dsRole_Select);
        }

        void dsRole_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRoleDefinition> e)
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

        public Expression<Func<UserRoleDefinition, bool>> QueryExpr
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

        protected void dvEntity_DataBound(object sender, EventArgs e)
        {
            if (dvEntity.Rows.Count > 0)
            {

            }
        }

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
                var mgr = dsRole.CreateDataManager();
                UserRoleDefinition item = new UserRoleDefinition
                {
                    RoleID = int.Parse((String)e.Values["RoleID"]),
                    SiteMenu = (String)e.Values["SiteMenu"],
                    Role = (String)e.Values["Role"]
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

        protected virtual void dvEntity_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            try
            {
                var mgr = dsRole.CreateDataManager();
                var item = mgr.EntityList.Where(r => r.RoleID == (int)e.Keys[0]).First();
                item.Role = (String)e.NewValues[0];
                item.SiteMenu = (String)e.NewValues[1];
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