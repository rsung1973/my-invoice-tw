using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using eIVOGo.Helper;

namespace eIVOGo.Module.SCM
{
    public partial class SupplierProductsList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int? PRODUCTS_SN
        {
            get
            {
                return editItem.PRODUCTS_SN;
            }
            set
            {
                editItem.PRODUCTS_SN = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            editItem.Done += new EventHandler(editItem_Done);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.SUPPLIER_PRODUCTS_NUMBER>>(dsEntity_Select);
            doDelete.DoAction = arg =>
            {
                delete(arg);
            };
            doEdit.DoAction = arg =>
                {
                    edit(arg);
                };
            doCreate.DoAction = arg =>
            {
                editItem.Show();
            };
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.SUPPLIER_PRODUCTS_NUMBER> e)
        {
            e.QueryExpr = p => p.PRODUCTS_SN == PRODUCTS_SN;
        }

        void editItem_Done(object sender, EventArgs e)
        {
            gvEntity.DataBind();
        }

        void gvEntity_DataBound(object sender, EventArgs e)
        {
            gvEntity.SetPageIndex("pagingList", dsEntity.CurrentView.LastSelectArguments.TotalRowCount);
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
                case "Modify":
                    edit((String)e.CommandArgument);
                    break;
                case "Delete":
                    delete((String)e.CommandArgument);
                    break;
                case "Create":
                    editItem.Show();
                    break;
            }
        }

        private void edit(String keyValue)
        {
            int[] key = keyValue.GetKeyValue();
            editItem.QueryExpr = r => r.SUPPLIER_SN == key[0] && r.PRODUCTS_SN == key[1];
            editItem.BindData();
        }

        private void delete(String keyValue)
        {
            int[] key = keyValue.GetKeyValue();
            dsEntity.CreateDataManager().DeleteAny(r => r.SUPPLIER_SN == key[0] && r.PRODUCTS_SN == key[1]);
            gvEntity.DataBind();
        }

        public void BindData()
        {
            gvEntity.DataBind();
        }

    }
}