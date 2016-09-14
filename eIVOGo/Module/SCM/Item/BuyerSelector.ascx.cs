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
    public partial class BuyerSelector : ListControlSelector
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<BUYER_DATA, bool>> QueryExpr
        {
            get;
            set;
        }

        public BUYER_DATA Item
        {
            get;
            protected set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((BuyerDataSource)dsEntity).Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<BUYER_DATA>>(BuyerSelector_Select);
        }

        void BuyerSelector_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<BUYER_DATA> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
        }

        protected override void selector_DataBound(object sender, EventArgs e)
        {
            base.selector_DataBound(sender, e);
            if (selector.SelectedItem != null)
            {
                Item = ((BuyerDataSource)dsEntity).CreateDataManager().EntityList.Where(b => b.BUYER_SN == int.Parse(selector.SelectedValue)).First();
            }
        }

    }
}