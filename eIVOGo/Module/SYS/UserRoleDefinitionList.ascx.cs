using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SYS
{
    public partial class UserRoleDefinitionList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            userRoleItem.Done += new EventHandler(userRoleItem_Done);
        }

        void userRoleItem_Done(object sender, EventArgs e)
        {
            gvEntity.DataBind();
        }

        void gvEntity_DataBound(object sender, EventArgs e)
        {
            gvEntity.SetPageIndex("pagingList", dsRole.CurrentView.LastSelectArguments.TotalRowCount);
        }


        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            { 
                case "Modify":
                    userRoleItem.QueryExpr = r => r.RoleID == int.Parse((String)e.CommandArgument);
                    userRoleItem.BindData();
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    userRoleItem.Show();
                    break;
            }
        }

        private void delete(int roleID)
        {
            dsRole.CreateDataManager().DeleteAny(r => r.RoleID == roleID);
            gvEntity.DataBind();
        }

    }
}