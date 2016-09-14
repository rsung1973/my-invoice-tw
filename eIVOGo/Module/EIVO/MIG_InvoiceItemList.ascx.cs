using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Module.Base;
using Model.Security.MembershipManagement;
using Model.Locale;
using Business.Helper;
using Utility;
using Model.DataEntity;

namespace eIVOGo.Module.EIVO
{
    public partial class MIG_InvoiceItemList : EntityItemList<EIVOEntityDataContext, InvoiceItem>
    {

    }
}