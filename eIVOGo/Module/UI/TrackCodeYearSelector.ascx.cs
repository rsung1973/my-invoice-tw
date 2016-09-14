using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class TrackCodeYearSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                initializeData();
            }
        }

        private void initializeData()
        {
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear-5; i <= currentYear + 5; i++)
            {
                _Selector.Items.Add(new ListItem((i-1911).ToString(), i.ToString()));
            }

            //_Selector.SelectedIndex = 5;
            _Selector.SelectedValue = currentYear.ToString();
        }

        public DropDownList Selector
        {
            get
            {
                return _Selector;
            }
        }

        [Bindable(true)]
        public short Year
        {
            get
            {
                return short.Parse(_Selector.SelectedValue);
            }
            set
            {
                _Selector.SelectedValue = value.ToString();
            }
        }

        [Bindable(true)]
        public String SelectedValue
        {
            get
            {
                return _Selector.SelectedValue;
            }
            set
            {
                _Selector.SelectedValue = value;
            }
        }


    }
}