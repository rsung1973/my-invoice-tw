using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;

using DataAccessLayer.basis;
using Uxnet.Web.Module.Common;
using Uxnet.Web.Module.DataModel;
using System.ComponentModel;

namespace eIVOGo.Module.Base
{
    public abstract partial class EntityItemList<T,TEntity>: EntityDataSource<T,TEntity>
        where T : DataContext, new()
        where TEntity : class, new()
    {

        protected global::System.Web.UI.WebControls.GridView gvEntity;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            gvEntity.Load += new EventHandler(gvEntity_Load);
            this.PreRender += new EventHandler(EntityItemList_PreRender);
        }


        void EntityItemList_PreRender(object sender, EventArgs e)
        {
            if (_deferredQuery)
            {
                gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
                gvEntity.DataBind();
            }
        }

        void gvEntity_Load(object sender, EventArgs e)
        {
            gvEntity.PageIndex = PagingControl.GetCurrentPageIndex(gvEntity, 0);
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

        [Bindable(true)]
        public bool AllowPaging
        {
            get
            {
                return gvEntity.AllowPaging;
            }
            set
            {
                gvEntity.AllowPaging = value;
            }
        }

        public GridView Grid
        {
            get
            {
                return gvEntity;
            }
        }
    }
}