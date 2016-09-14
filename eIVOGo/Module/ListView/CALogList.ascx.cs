using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eIVOGo.Module.Base;
using Model.DataEntity;

namespace eIVOGo.Module.ListView
{
    public partial class CALogList : EntityItemList<EIVOEntityDataContext, CALog>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}