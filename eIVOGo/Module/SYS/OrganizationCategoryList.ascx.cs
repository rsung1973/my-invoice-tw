using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;
using Utility;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SYS
{
    public partial class OrganizationCategoryList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            editItem.Done += new EventHandler(categoryItem_Done);
        }

        void categoryItem_Done(object sender, EventArgs e)
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
                    editItem.QueryExpr = r => r.OrgaCateID == int.Parse((String)e.CommandArgument);
                    editItem.BindData();
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    editItem.Show();
                    break;
            }
        }

        private void delete(int key)
        {
            try
            {
                dsEntity.CreateDataManager().DeleteAny(r => r.OrgaCateID == key);
                //            gvEntity.DataBind();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                this.AjaxAlert("作業失敗!!原因:" + ex.Message);
            }
        }

    }
}