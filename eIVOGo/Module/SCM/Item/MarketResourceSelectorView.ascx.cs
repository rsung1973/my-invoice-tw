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
    public partial class MarketResourceSelectorView : ItemSelector
    {
        protected IList<ORDERS_MARKET_ATTRIBUTE_MAPPING> _mappingItems;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void CheckMappingData()
        {
            if (_mappingItems!=null && (String)this.ViewState["presentValue"] != SelectedValue)
            {
                _mappingItems.Clear();
            }
        }

        protected MARKET_ATTRIBUTE loadItem(ORDERS_MARKET_ATTRIBUTE_MAPPING item)
        {
            item.MARKET_ATTRIBUTE = ((MarketResourceDataSource)dsEntity).CreateDataManager().GetTable<MARKET_ATTRIBUTE>().Where(a => a.MARKET_ATTR_SN == item.MARKET_ATTR_NAME_SN).First();
            return item.MARKET_ATTRIBUTE;
        }

        public MARKET_RESOURCE Item
        {
            get;
            protected set;
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(MarketResourceSelectorView_PreRender);
            this.Load += new EventHandler(MarketResourceSelectorView_Load);
        }

        void MarketResourceSelectorView_Load(object sender, EventArgs e)
        {
            CheckMappingData();
        }

        protected override void selector_DataBound(object sender, EventArgs e)
        {
            base.selector_DataBound(sender, e);
            if (!String.IsNullOrEmpty(selector.SelectedValue))
            {
                Item = ((MarketResourceDataSource)dsEntity).CreateDataManager().EntityList.Where(m => m.MARKET_RESOURCE_SN == int.Parse(selector.SelectedValue)).First();
                MarketAttribute.MARKET_RESOURCE_SN = Item.MARKET_RESOURCE_SN;
                MarketAttribute.BindData();
            }
        }

        protected virtual void MarketResourceSelectorView_PreRender(object sender, EventArgs e)
        {
            if (Item != null && _mappingItems != null)
            {
                attrResult.Visible = true;
                rpAttrName.DataSource = _mappingItems;
                rpAttrName.DataBind();
                rpAttrValue.DataSource = _mappingItems;
                rpAttrValue.DataBind();
                attrResult.DataBind();
            }

            this.ViewState["presentValue"] = SelectedValue;

        }

        protected void btnAddAttr_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(Request[MARKET_ATTR_VALUE.UniqueID]))
            {
                this.AjaxAlert("請輸入平台屬性值!!");
                return;
            }

            if (_mappingItems == null)
                _mappingItems = new List<ORDERS_MARKET_ATTRIBUTE_MAPPING>();

            var item = _mappingItems.Where(a => a.MARKET_ATTR_NAME_SN == MarketAttribute.Item.MARKET_ATTR_SN).FirstOrDefault();
            if (item == null)
            {
                item = new ORDERS_MARKET_ATTRIBUTE_MAPPING
                {
                    MARKET_ATTR_NAME_SN = MarketAttribute.Item.MARKET_ATTR_SN,
                    MARKET_ATTRIBUTE = MarketAttribute.Item
                };
                _mappingItems.Add(item);
            }
            item.MARKET_ATTR_VALUE = Request[MARKET_ATTR_VALUE.UniqueID];
            MARKET_ATTR_VALUE.Text = "";
        }

    }
}