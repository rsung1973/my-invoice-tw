using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uxnet.Web.Module.DataModel;
using System.Linq.Expressions;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ProductsDataSelector : ListControlSelector
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<PRODUCTS_DATA, bool>> QueryExpr
        {
            get;
            set;
        }

        public IQueryable<PRODUCTS_DATA> Query
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((ProductsDataSource)dsEntity).Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_DATA>>(ProductsDataSelector_Select);
        }

        void ProductsDataSelector_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<PRODUCTS_DATA> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            else if (Query != null)
            {
                e.Query = Query;
            }
        }

        public override void BindData()
        {
            base.BindData();
            emptyMsg.Visible = ((ProductsDataSource)dsEntity).CurrentView == null || ((ProductsDataSource)dsEntity).CurrentView.LastSelectArguments.TotalRowCount <= 0;
        }

    }
}