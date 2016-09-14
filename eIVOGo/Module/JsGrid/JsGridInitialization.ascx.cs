using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.JsGrid
{
    public partial class JsGridInitialization : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataSourceUrl == null)
                DataSourceUrl = Request.Path;
        }

        [System.ComponentModel.Bindable(true)]
        public bool AllowPaging
        {
            get;
            set;
        }
        

        [System.ComponentModel.Bindable(true)]
        public bool PrintMode
        { get; set; }

        [System.ComponentModel.Bindable(true)]
        public String FieldName
        { get; set; }

        [System.ComponentModel.Bindable(true)]
        public Object FieldObject
        { get; set; }

        [System.ComponentModel.Bindable(true)]
        public String JsGridSelector
        { get; set; }

        [System.ComponentModel.Bindable(true)]
        public String FieldsContainerSelector
        { get; set; }

        public Func<int> GetRecordCount
        { get; set; }

        [System.ComponentModel.Bindable(true)]
        public String DataSourceUrl
        { get; set; }


    }
}