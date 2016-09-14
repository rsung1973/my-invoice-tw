using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using eIVOGo.Helper;
using System.Linq.Expressions;
using Model.SCMDataEntity;

namespace eIVOGo.Module.SCM
{
    public partial class ProductsDataList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public Expression<Func<PRODUCTS_DATA, bool>> QueryExpr;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.PRODUCTS_DATA>>(dsEntity_Select);
            doDelete.DoAction = arg =>
            {
                delete(int.Parse(arg));
            };
            doEdit.DoAction = arg =>
                {
                    edit(arg);
                };
            doCreate.DoAction = arg =>
            {

            };
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.SCMDataEntity.PRODUCTS_DATA> e)
        {
            if (QueryExpr != null)
            {
                e.QueryExpr = QueryExpr;
            }
            //else
            //{
            //    e.QueryExpr = p => false;
            //}
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
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    Server.Transfer("Goods_Maintain_Add.aspx");
                    break;
            }
        }

        private void edit(String keyValue)
        {
            Page.Items["id"] = int.Parse(keyValue);
            Server.Transfer("Goods_Maintain_Add.aspx");
        }

        private void delete(int keyValue)
        {
            dsEntity.CreateDataManager().DeleteAny(r => r.PRODUCTS_SN == keyValue);
            gvEntity.DataBind();
        }

        public void BindData()
        {
            gvEntity.DataBind();
        }

    }
}