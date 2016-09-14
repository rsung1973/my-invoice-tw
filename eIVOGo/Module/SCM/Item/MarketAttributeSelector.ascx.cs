using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uxnet.Web.Module.DataModel;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class MarketAttributeSelector : ItemSelector
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int? MARKET_RESOURCE_SN
        {
            get;
            set;
        }

        public MARKET_ATTRIBUTE Item
        {
            get;
            protected set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ((MarketAttributeDataSource)dsEntity).Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<MARKET_ATTRIBUTE>>(MarketAttributeSelector_Select);
        }

        void MarketAttributeSelector_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<MARKET_ATTRIBUTE> e)
        {
            e.QueryExpr = m => m.MARKET_RESOURCE_SN == MARKET_RESOURCE_SN;
        }

        protected override void selector_DataBound(object sender, EventArgs e)
        {
            base.selector_DataBound(sender, e);
            if (!String.IsNullOrEmpty(selector.SelectedValue))
            {
                Item = ((MarketAttributeDataSource)dsEntity).CreateDataManager().EntityList.Where(a => a.MARKET_ATTR_SN == int.Parse(selector.SelectedValue)).First();
            }
        }
    }
}