using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Model.DataEntity;
using eIVOGo.Module.Base;
using System.Linq.Expressions;

namespace eIVOGo.Module.Inquiry
{
    public partial class BuyerDataList : EntityItemListModal<EIVOEntityDataContext, OrganizationCategory>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request["rbItem"] != null)
            {
                this.ViewState["rbItem"] = Request["rbItem"];
            }
        }

        public String SelectedValue
        {
            set
            {
                this.ViewState["rbItem"] = value;
            }
            get
            {
                return (String)this.ViewState["rbItem"];
            }
        }
    }
}