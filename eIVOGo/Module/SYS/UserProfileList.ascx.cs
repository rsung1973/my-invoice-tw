using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Model.DataEntity;

namespace eIVOGo.Module.SYS
{
    public partial class UserProfileList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        public Expression<Func<UserProfile, bool>> QueryExpr
        {
            get;
            set;
        }
             

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile>>(dsEntity_Select);
            editItem.Done += new EventHandler(editItem_Done);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserProfile> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = u => false;
            }
        }

        void editItem_Done(object sender, EventArgs e)
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

        protected virtual void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            { 
                case "Modify":
                    editItem.QueryExpr = r => r.UID == int.Parse((String)e.CommandArgument);
                    editItem.BindData();
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    editItem.Show();
                    break;
                case "EditRole":
                    Page.Items["uid"] = e.CommandArgument;
                    Server.Transfer("~/SYS/MaintainUserRole.aspx");
                    break;
            }
        }

        private void delete(int key)
        {
            dsEntity.CreateDataManager().DeleteAny(r => r.UID == key);
            gvEntity.DataBind();
        }

    }
}