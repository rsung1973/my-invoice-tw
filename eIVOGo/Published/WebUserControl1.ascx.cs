using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Published
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TemplateField field = new TemplateField();
            field.HeaderText = "公司";
            //MyField template = this.LoadControl("MyField.ascx") as MyField;
            //template.Message = "DataField:";
            MyFieldTemplate template = new MyFieldTemplate();
            template.ControlLoader = this;
            field.ItemTemplate = template;// this.LoadTemplate("MyField.ascx");
            GridView1.Columns.Add(field);
            GridView1.DataBind();
        }

        protected void GridView1_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_DataBinding(object sender, EventArgs e)
        {

        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {

        }
    }
}