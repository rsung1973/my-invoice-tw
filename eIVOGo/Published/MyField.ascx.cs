using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.DataEntity;

namespace eIVOGo.Published
{
    public partial class MyField : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected object showObject(object container)
        {
            return ((Organization)((GridViewRow)container).DataItem).CompanyName;
        }

        public String Message { get; set; }

    }

    public class MyFieldTemplate : ITemplate
    {

        public TemplateControl ControlLoader
        { get; set; }

        #region ITemplate Members

        public void InstantiateIn(Control container)
        {
            MyField template = ControlLoader.LoadControl("MyField.ascx") as MyField;
            template.Message = "DataField:";
            container.Controls.Add(template);
        }

        #endregion
    }
}