using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;
using Model.DataEntity;
using Model.Locale;
using Model.Security.MembershipManagement;
using Utility;
using System.Linq.Expressions;
using Business.Helper;
using Uxnet.Web.Module.Common;
using eIVOGo.Module.Base;
using Model.Helper;
using System.Xml;

namespace eIVOGo.Module.SYS
{
    public partial class UnusedInvoiceItemList : EntityItemList<EIVOEntityDataContext, UnassignedInvoiceNo>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}