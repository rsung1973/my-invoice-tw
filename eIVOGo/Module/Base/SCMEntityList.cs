using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using DataAccessLayer.basis;
using Model.SCMDataEntity;
using Uxnet.Web.Module.Common;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.Base
{
    public abstract partial class SCMEntityList<TEntity> : System.Web.UI.UserControl 
        where TEntity : class, new()
    {

        public Expression<Func<TEntity, bool>> QueryExpr;

        protected LinqToSqlDataSource<SCMEntityDataContext, TEntity> dsEntity;
        protected global::System.Web.UI.WebControls.GridView gvEntity;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<TEntity>>(dsEntity_Select);
        }

        protected virtual void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<TEntity> e)
        {

            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else
            {
                e.QueryExpr = p => false;
            }
        }


        protected virtual void gvEntity_DataBound(object sender, EventArgs e)
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
                    break;
                case "Delete":
                    break;
                case "Create":
                    break;
            }
        }

        public virtual void BindData()
        {
            gvEntity.DataBind();
        }
    }
}