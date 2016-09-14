using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SYS
{
    public partial class UserMenuList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            editItem.Done += new EventHandler(editItem_Done);
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
                    break;
                case "Delete":
                    delete((String)e.CommandArgument);
                    break;
                case "Create":
                    editItem.Show();
                    break;
            }
        }

        private void delete(String keyValue)
        {
            var key = keyValue.Split(',').Select(s => int.Parse(s)).ToArray();
            dsEntity.CreateDataManager().DeleteAny(r => r.RoleID == key[0] && r.CategoryID == key[1] && r.MenuID == key[2]);
            gvEntity.DataBind();
        }

    }
}