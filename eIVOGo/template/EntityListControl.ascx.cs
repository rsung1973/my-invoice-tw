using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Module.Base;
using Model.DataEntity;
using Model.Security.MembershipManagement;
using Business.Helper;
using Model.Locale;

namespace eIVOGo.template
{
    public partial class EntityListControl : EntityItemList<EIVOEntityDataContext, EnterpriseGroupMember>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            doActivate.DoAction = arg => 
            {
                var mgr = dsEntity.CreateDataManager();
                var item = mgr.EntityList.Where(m => m.CompanyID == int.Parse(arg)).First();
                item.Organization.OrganizationStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Checked;
                mgr.SubmitChanges();
            };
            doCreate.DoAction = arg =>
            {
                Server.Transfer(ToEdit.TransferTo);
            };
            doEdit.DoAction = arg =>
            {
                modelItem.DataItem = int.Parse(arg);
                Server.Transfer(ToEdit.TransferTo);
            };
            doDelete.DoAction = arg =>
            {
                delete(arg);
            };
        }


        protected void delete(string keyValue)
        {
            var mgr = dsEntity.CreateDataManager();
            var item = mgr.EntityList.Where(m => m.CompanyID == int.Parse(keyValue)).First();
            item.Organization.OrganizationStatus.CurrentLevel = (int)Naming.MemberStatusDefinition.Mark_To_Delete;
            mgr.SubmitChanges();
        }

    }
}