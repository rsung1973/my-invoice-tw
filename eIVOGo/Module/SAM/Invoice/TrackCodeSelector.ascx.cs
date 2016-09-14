using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.DataModel;
using Utility;
using Model.DataEntity;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class TrackCodeSelector : ItemSelector<EIVOEntityDataContext, InvoiceTrackCode>
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(TrackCodeSelector_PreRender);
        }

        void TrackCodeSelector_PreRender(object sender, EventArgs e)
        {
            if (!_isBound)
            {
                selector.DataSource = Select();
                selector.DataBind();
            }
        }
    }
}