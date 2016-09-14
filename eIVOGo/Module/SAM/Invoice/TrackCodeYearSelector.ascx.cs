using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.Module.DataModel;
using Utility;

namespace eIVOGo.Module.SAM.Invoice
{
    public partial class TrackCodeYearSelector : ItemSelector
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            initializeData();
            selector.Load += new EventHandler(selector_Load);
        }

        void selector_Load(object sender, EventArgs e)
        {
            selector_DataBound(sender, e);
        }

        private void initializeData()
        {
            int currentYear = DateTime.Now.Year;
            selector.Items.AddRange((-5).GenerateArray(11).Select(i => new ListItem(String.Format("{0}年", currentYear - 1911 + i), (currentYear + i).ToString())).ToArray());
        }

        public static int MaxValue
        {
            get
            {
                return DateTime.Now.Year + 5;
            }
        }

        public static int MinValue
        {
            get
            {
                return DateTime.Now.Year - 5;
            }
        }

    }
}