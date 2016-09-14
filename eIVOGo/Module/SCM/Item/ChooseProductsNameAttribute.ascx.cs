using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ChooseProductsNameAttribute : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            addItem.Done += new EventHandler(addItem_Done);
        }

        void addItem_Done(object sender, EventArgs e)
        {
            attrSN.BindData();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            addItem.Show();
        }

        public int? PRODUCTS_ATTR_NAME_SN
        {
            get
            {
                int sn;
                return int.TryParse(attrSN.SelectedValue, out sn) ? sn : (int?)null;
            }
        }

        public PRODUCTS_ATTRIBUTE_NAME Item
        {
            get
            {
                return attrSN.Item;
            }
        }
    }
}