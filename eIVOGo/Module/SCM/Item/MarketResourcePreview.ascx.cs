using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;

using Utility;
using Uxnet.Web.Module.DataModel;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM.Item
{
    public partial class MarketResourcePreview : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(MarketResourcePreview_PreRender);
        }

        protected IList<ORDERS_MARKET_ATTRIBUTE_MAPPING> _mappingItems;

        void MarketResourcePreview_PreRender(object sender, EventArgs e)
        {
            rpAttr.DataSource = _mappingItems;
            rpAttr.DataBind();
        }

        protected MARKET_ATTRIBUTE loadItem(ORDERS_MARKET_ATTRIBUTE_MAPPING item)
        {
            item.MARKET_ATTRIBUTE = ((MarketResourceDataSource)dsEntity).CreateDataManager().GetTable<MARKET_ATTRIBUTE>().Where(a => a.MARKET_ATTR_SN == item.MARKET_ATTR_NAME_SN).First();
            return item.MARKET_ATTRIBUTE;
        }

        public IList<ORDERS_MARKET_ATTRIBUTE_MAPPING> MappingItems
        {
            get
            {
                return _mappingItems;
            }
            set
            {
                _mappingItems = value;
            }
        }


    }
}