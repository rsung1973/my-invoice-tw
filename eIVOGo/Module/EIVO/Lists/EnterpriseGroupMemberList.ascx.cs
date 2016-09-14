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

namespace eIVOGo.Module.EIVO.Lists
{
    public partial class EnterpriseGroupMemberList : EntityItemList<EIVOEntityDataContext, Organization>
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}