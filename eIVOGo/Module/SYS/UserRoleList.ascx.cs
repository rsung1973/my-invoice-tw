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

namespace eIVOGo.Module.SYS
{
    public partial class UserRoleList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int? UID
        {
            get
            {
                return (int?)ViewState["uid"];
            }
            set
            {
                ViewState["uid"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            editItem.Done += new EventHandler(editItem_Done);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRole>>(dsEntity_Select);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<UserRole> e)
        {
            if (UID.HasValue)
            {
                e.QueryExpr = r => r.UID == UID;
            }
            else
            {
                e.QueryExpr = r => false;
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

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            { 
                case "Modify":
                    break;
                case "Delete":
                    delete((String)e.CommandArgument);
                    break;
                case "Create":
                    editItem.UID = UID;
                    editItem.Show();
                    break;
            }
        }

        private void delete(String keyValue)
        {
            var key = keyValue.GetKeyValue();
            dsEntity.CreateDataManager().DeleteAny(r => r.UID == key[0] && r.RoleID == key[1] && r.OrgaCateID == key[2]);
            gvEntity.DataBind();
        }

    }
}