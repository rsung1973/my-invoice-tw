using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;

namespace eIVOGo.Published
{
    public partial class TestPagingControl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            this.gvEntity.Load += new EventHandler(gvEntity_Load);
            //this.dsEntity.Select += new EventHandler<DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.DataEntity.InvoiceItem>>(dsEntity_Select);
        }

        void gvEntity_Load(object sender, EventArgs e)
        {
            
        }

        void gvEntity_DataBound(object sender, EventArgs e)
        {
            gvEntity.SetPageIndex("pagingList", dsEntity.CurrentView.LastSelectArguments.TotalRowCount);
        }

        void dsEntity_Select(object sender, DataAccessLayer.basis.LinqToSqlDataSourceEventArgs<Model.DataEntity.InvoiceItem> e)
        {
            e.QueryExpr = i => true;
        }

        protected void PageIndexChanged(object source, PageChangedEventArgs e)
        {
            gvEntity.PageIndex = e.NewPageIndex;
            gvEntity.DataBind();
        }


        protected void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }


    }
}