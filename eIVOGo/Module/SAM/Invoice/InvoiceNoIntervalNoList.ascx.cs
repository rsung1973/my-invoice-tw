using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Business.Helper;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class InvoiceNoIntervalNoList : EntityItemList<EIVOEntityDataContext, InvoiceNoInterval>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            doEdit.DoAction = arg =>
            {
                modelItem.DataItem = int.Parse(arg);
//                Server.Transfer(ToEdit.TransferTo);
                editItemModal.Show();
            };
            doDelete.DoAction = arg =>
            {
                delete(arg);
            };
        }


        protected void delete(string keyValue)
        {
            try
            {
                dsEntity.CreateDataManager().DeleteAny(m => m.IntervalID == int.Parse(keyValue));
                this.AjaxAlert("刪除完成!!");
            }
            catch (Exception ex)
            {
                this.AjaxAlert("無法刪除資料!!原因:" + ex.Message);
            }
        }

    }
}