using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class TwiceMonthlyPeriod : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(TwiceMonthlyPeriod_PreRender);
        }

        void TwiceMonthlyPeriod_PreRender(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (!DateFrom.HasValue)
                    DateFrom = DateTime.Now;
                if (!DateTo.HasValue)
                    DateTo = DateFrom.Value.AddYears(-2);

                DateTime start = new DateTime(DateFrom.Value.Year, (DateFrom.Value.Month + 1) / 2 * 2 - 1, 1);
                DateTime end = new DateTime(DateTo.Value.Year, (DateTo.Value.Month + 1) / 2 * 2 - 1, 1);
                var items = buildData(start,end);
                period.Items.AddRange(items.Select(d => new ListItem(String.Format("{0:000}年 {1:00}月-{2:00}月", d.Year - 1911, d.Month, d.Month + 1))).ToArray());
                DateFrom = items.First();
                DateTo = items.Last();
            }
        }

        public DropDownList DatePeriod
        {
            get
            {
                return period;
            }
        }

        public DateTime? SelectedDate
        {
            get
            {
                if (period.SelectedIndex >= 0 && DateFrom.HasValue)
                {
                    return DateFrom.Value.AddMonths(period.SelectedIndex * (-2));
                }
                return (DateTime?)null;
            }
        }



        private IEnumerable<DateTime> buildData(DateTime start, DateTime end)
        {
            for (DateTime d = start; d > end; d = d.AddMonths(-2))
            {
                yield return d;
            }
        }

        [Bindable(true)]
        public DateTime? DateFrom
        {
            get
            {
                return (DateTime?)ViewState["from"];
            }
            set
            {
                ViewState["from"] = value;
            }
        }

        [Bindable(true)]
        public DateTime? DateTo
        {
            get
            {
                return (DateTime?)ViewState["to"];
            }
            set
            {
                ViewState["to"] = value;
            }
        }


    }
}