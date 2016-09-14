using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Uxnet.Web.Module.DataModel;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM_SYS.Item
{
    public partial class ProductsAttributeNameSelector : ItemSelector
    {
        public PRODUCTS_ATTRIBUTE_NAME Item
        {
            get;
            protected set;
        }

        protected override void selector_DataBound(object sender, EventArgs e)
        {
            base.selector_DataBound(sender, e);
            if (!String.IsNullOrEmpty(selector.SelectedValue))
            {
                Item = ((ProductsAttributeNameDataSource)dsEntity).CreateDataManager().EntityList.Where(a => a.PRODUCTS_ATTR_NAME_SN == int.Parse(selector.SelectedValue)).First();
            }
        }
    }
}