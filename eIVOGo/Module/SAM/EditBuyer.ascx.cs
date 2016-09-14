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
using eIVOGo.Module.Base;
using Uxnet.Web.WebUI;
using System.Text.RegularExpressions;
using Utility;

namespace eIVOGo.Module.SAM
{
    public partial class EditBuyer : EditEntityItemBase<EIVOEntityDataContext, InvoiceBuyer>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(EditBuyer_PreRender);
            this.Done+=new EventHandler(EditBuyer_Done);
            this.QueryExpr = m => m.InvoiceID == (int?)modelItem.DataItem;
        }

        void EditBuyer_PreRender(object sender, EventArgs e)
        {
            this.BindData();
            if (_entity != null)
            {
                modelItem.DataItem = _entity.InvoiceID;
            }
        }

        void EditBuyer_Done(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("register_msg.aspx?backUrl=BuyerManager.aspx&action={0}&back={1}", Server.UrlEncode(actionItem.ItemName), Server.UrlEncode("回買受人資料維護")));
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();
            loadEntity();

            if (_entity != null)
            {
                _entity.ContactName = this.ContactName.Text.Trim();
                _entity.Phone = this.Phone.Text.Trim();
                _entity.Address = this.Addr.Text.Trim();
                mgr.SubmitChanges();
            }
            return true;
        }
    }
}