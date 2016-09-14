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
    public partial class SpecifiedTrackCodePeriodSelector : ItemSelector<EIVOEntityDataContext,InvoiceTrackCode>
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.PreRender += new EventHandler(SpecifiedTrackCodePeriodSelector_PreRender);
        }

        void SpecifiedTrackCodePeriodSelector_PreRender(object sender, EventArgs e)
        {
            if (!_isBound)
            {
                selector.DataSource = Select()
                   .Select(o => new
                   {
                       Expression = String.Format("{0:00}-{1:00}月", o.PeriodNo * 2 - 1, o.PeriodNo * 2),
                       o.PeriodNo
                   }
                       ).Distinct();
                selector.DataBind();
            }
        }

    }
}