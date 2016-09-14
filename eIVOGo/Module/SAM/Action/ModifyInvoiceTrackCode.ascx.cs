using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Security.MembershipManagement;
using Model.DataEntity;

using Business.Helper;
using Model.Locale;
using Uxnet.Web.WebUI;
using System.Text.RegularExpressions;
using Utility;

namespace eIVOGo.Module.SAM.Action
{
    public partial class ModifyInvoiceTrackCode : CreateInvoiceTrackCode
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(ModifyInvoiceTrackCode_Load);
            EditItem.Done += new EventHandler(EditItem_Done);
        }

        void ModifyInvoiceTrackCode_Load(object sender, EventArgs e)
        {
            initializeData();
        }

        private void initializeData()
        {
            if (Page.PreviousPage != null && Page.PreviousPage.Items["trackID"] != null)
            {
                EditItem.QueryExpr = u => u.TrackID == (int)Page.PreviousPage.Items["trackID"];
            }
        }

        protected override void EditItem_Done(object sender, EventArgs e)
        {
            _userProfile["msg"] = "字軌修改完成!!";
            Response.Redirect("~/EIVO/track_number_list.aspx");
        }
      
    }
}