using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ProductAttributeEditList : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IList<PRODUCTS_ATTRIBUTE_MAPPING> Items
        {
            get
            {
                return productAttrList.Items;
            }
            set
            {
                productAttrList.Items = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void btnAddAttri_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(attrValue.Text))
            {
                this.AjaxAlert("請輸入屬性內容!!");
                return;
            }

            if (!productAttr.PRODUCTS_ATTR_NAME_SN.HasValue)
            {
                this.AjaxAlert("請選擇屬性名稱!!");
                return;
            }

            if (Items == null)
                Items = new List<PRODUCTS_ATTRIBUTE_MAPPING>();

            if (Items.Count(p => p.PRODUCTS_ATTR_NAME_SN == productAttr.PRODUCTS_ATTR_NAME_SN) > 0)
            {
                this.AjaxAlert("該屬性已存在,請先刪除!!");
                return;
            }

            Items.Add(new PRODUCTS_ATTRIBUTE_MAPPING
            {
                PRODUCTS_ATTR_NAME_SN = productAttr.PRODUCTS_ATTR_NAME_SN.Value,
                PRODUCTS_ATTRIBUTE_NAME = productAttr.Item,
                PRODUCTS_ATTR_VALUE = attrValue.Text
            });
        }
    }
}