using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.DataEntity;
using eIVOGo.Module.UI;
using Uxnet.Web.WebUI;

namespace eIVOGo.Module.SAM.Business
{
    public partial class ProxySettingOrganizationList : EditEntityItemModal<EIVOEntityDataContext, Organization>
    {

        public int? IssureID
        {
            get
            {
                return (int?)modelItem.DataItem;
            }
            set
            {
                modelItem.DataItem = value;
            }
        }

        protected override bool saveEntity()
        {
            var mgr = dsEntity.CreateDataManager();

            loadEntity();

            if (_entity == null)
            {
                Page.AjaxAlert("發票開立人不存在!!");
                return false;
            }

            //_entity.AsInvoiceInsurer.Clear();

            mgr.DeleteAllOnSubmit<InvoiceIssuerAgent>(i => i.IssuerID == IssureID);
            _entity.AsInvoiceInsurer.AddRange(agentSelector.SelectedValues
                .Select(a => new InvoiceIssuerAgent { AgentID = int.Parse(a) }));
            

            mgr.SubmitChanges();
            return true;

        }
    }
}