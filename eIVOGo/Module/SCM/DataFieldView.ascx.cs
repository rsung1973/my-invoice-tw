using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.SCM
{
    public partial class DataFieldView : System.Web.UI.UserControl
    {
        public TemplateField DataTemplateField
        { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }

    public partial class DataFieldViewLoader : ITemplate
    {
        public TemplateField Field
        { get; set; }

        public TemplateControl ControlLoader
        {
            get;
            set;
        }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            DataFieldView fieldView = ControlLoader.LoadControl("~/Module/SCM/DataFieldView.ascx") as DataFieldView;
            fieldView.DataTemplateField = Field;
            container.Controls.Add(fieldView);
        }

        #endregion
    }
}