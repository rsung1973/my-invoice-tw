using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.ComponentModel;

namespace eIVOGo.Module.SYS
{
    public partial class FunctionItem : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [BindableAttribute(true)]
        public string ItemName
        {
            get
            {
                return litItemName.Text;
            }
            set
            {
                litItemName.Text = value;
            }
        }

        [BindableAttribute(true)]
        public string ActionUrl
        {
            get
            {
                return this.ViewState["au"] == null ? "" : (string)this.ViewState["au"];
            }
            set
            {
                this.ViewState["au"] = value;
            }
        }

        [BindableAttribute(true)]
        public bool Enabled
        {
            get
            {
                return cbFunction.Checked;
            }
            set
            {
                cbFunction.Checked = value;
            }
        }

        [BindableAttribute(true)]
        public bool ViewOnly
        {
            get
            {
                return cbFunction.Disabled;
            }
            set
            {
                cbFunction.Disabled = value;
            }
        }

        [BindableAttribute(true)]
        public bool IsDefault
        {
            get
            {
                return this.ViewState["default"] == null ? false : (bool)this.ViewState["default"];
            }
            set
            {
                this.ViewState["default"] = value;
            }
        }


        [BindableAttribute(true)]
        public bool ReadOnly
        { 
            get
            {
                return !cbFunction.Visible;
            }
            set
            {
                cbFunction.Visible = !value;
            }
        }


        public XElement Save(XElement menuItem)
        {
            if (cbFunction.Checked)
            {
                XElement element = new XElement("menuItem",
                    new XAttribute("value", litItemName.Text),
                    new XAttribute("url", ActionUrl));

                if (IsDefault)
                {
                    element.Add(new XAttribute("default", true));
                }

                if (menuItem != null)
                {
                    menuItem.Add(element);
                }

                return element;
            }
            return null;
        }

        internal void RestoreCheckItem(IEnumerable<XElement> elements)
        {
            cbFunction.Checked = elements.Where(e => e.Attribute("value").Value == litItemName.Text).FirstOrDefault() != null;
        }
    }
}