using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.Locale;
using Model.Security.MembershipManagement;
using Model.InvoiceManagement;
using Business.Helper;
using Model.DataEntity;
using Uxnet.Web.Module.Common;
using System.Linq.Expressions;
using Utility;
using eIVOGo.Module.Base;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM.Action
{
    public partial class InvoiceTrackCodeMaintenanceList : InvoiceTrackCodeList
    {
        public override void RaisePostBackEvent(string eventArgument)
        {
            String[] param = eventArgument.Split(':');
            if(param[0]=="D")
            {
                delete(int.Parse(param[1]));
            }
            else if (param[0]=="U")
            {
                edit(int.Parse(param[1]));
            }
        }

        private void delete(int trackID)
        {
            //dsEntity.CreateDataManager().DeleteAny(i => i.TrackID == trackID);
            var mgr = dsEntity.CreateDataManager();
            var item = mgr.EntityList.Where(t => t.TrackID == trackID).FirstOrDefault();
            if (item != null)
            {
                if (item.InvoiceTrackCodeAssignments.Count > 0)
                {
                    this.AjaxAlert("該字軌已被使用,無法刪除!!");
                }
                else
                {
                    mgr.EntityList.DeleteOnSubmit(item);
                    mgr.SubmitChanges();
                }
            }
        }

        private void edit(int trackID)
        {
            Page.Items["trackID"] = trackID;
            Server.Transfer("~/SAM/EditInvoiceTrackCode.aspx");
        }      

    }    
}