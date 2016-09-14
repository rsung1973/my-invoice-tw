using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.Common;

namespace eIVOGo.Module.SYS
{
    public partial class SystemMessageList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            gvEntity.DataBound += new EventHandler(gvEntity_DataBound);
            MessagesItem.Done += new EventHandler(userRoleItem_Done);
        }

        void userRoleItem_Done(object sender, EventArgs e)
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
                    modelItem.DataItem = int.Parse((String)e.CommandArgument);
                    MessagesItem.BindData();
                    MessagesItem.Show();
                    break;
                case "Delete":
                    delete(int.Parse((String)e.CommandArgument));
                    break;
                case "Create":
                    MessagesItem.Clean();
                    MessagesItem.Show();
                    break;
            }
        }

        private void delete(int msgid)
        {
            dsEntity.CreateDataManager().DeleteAny(m => m.MsgID.Equals(msgid));
            gvEntity.DataBind();
            Response.Redirect(Request.RawUrl);
        }
    }
}