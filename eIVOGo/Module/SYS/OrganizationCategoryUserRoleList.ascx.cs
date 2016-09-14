using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.DataEntity;
using eIVOGo.Helper;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SYS
{
    public partial class OrganizationCategoryUserRoleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<OrganizationCategoryUserRole, bool>> QueryExpr
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            editItem.Done += new EventHandler(categoryItem_Done);
            editMenu.Done += new EventHandler(categoryItem_Done);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.DataEntity.OrganizationCategoryUserRole>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.DataEntity.OrganizationCategoryUserRole> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = o => false;
            }
        }

        void categoryItem_Done(object sender, EventArgs e)
        {
            gvEntity.DataBind();
        }

        void gvEntity_DataBound(object sender, EventArgs e)
        {
            gvEntity.SetPageIndex("pagingList", dsEntity.CurrentView.LastSelectArguments.TotalRowCount);
        }


        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int[] key;
            switch (e.CommandName)
            { 
                case "Modify":
                    key = ((String)e.CommandArgument).GetKeyValue();
                    editItem.QueryExpr = r => r.RoleID == key[1];
                    editItem.BindData();
                    break;
                case "Delete":
                    delete((String)e.CommandArgument);
                    break;
                case "Create":
                    editItem.Show();
                    break;
                case "EditMenu":
                    key = ((String)e.CommandArgument).GetKeyValue();
                    //Page.Items["roleID"] = key[1];
                    //Server.Transfer("~/SYS/EditMemberMenu.aspx");
                    editMenu.RoleID = key[1];
                    editMenu.Show();
                    break;
            }
        }

        private void delete(String keyValue)
        {
            var key = keyValue.GetKeyValue();
            var mgr = dsEntity.CreateDataManager();
            if (mgr.GetTable<UserRole>().Any(r => r.OrgaCateID == key[0] && r.RoleID == key[1]))
            {
                this.AjaxAlert("該角色尚有使用者使用中,無法刪除!!");
            }
            else
            {
                mgr.DeleteAny(r => r.OrgaCateID == key[0] && r.RoleID == key[1]);
                gvEntity.DataBind();
            }
        }

    }
}