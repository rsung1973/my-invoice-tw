using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using eIVOGo.Helper;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM.Item
{
    public partial class ProductsAttributeMappingList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IList<PRODUCTS_ATTRIBUTE_MAPPING> Items
        {
            get
            {
                return gvEntity.DataSource as IList<PRODUCTS_ATTRIBUTE_MAPPING>;
            }
            set
            {
                gvEntity.DataSource = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            this.PreRender += new EventHandler(ProductsAttributeMappingList_PreRender);
        }

        void ProductsAttributeMappingList_PreRender(object sender, EventArgs e)
        {
            if (Items != null)
            {
                BindData();
            }
        }

        public void BindData()
        {
            gvEntity.DataBind();
        }

        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }

        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            { 
                case "Modify":  //
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    break;
            }
        }

        private void delete(int key)
        {
            Items.RemoveAt(key);
        }

    }
}