using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace eIVOGo.Module.UI
{
    public partial class DynamicAddTextbox : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (string.IsNullOrEmpty(this.HiddenField1.Value))
                {
                    ViewState["count"] = 1;
                }
                else
                {
                    ViewState["count"] = this.HiddenField1.Value.Split(',').Length;
                }
            }
            RecreateControls();                
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {             
             int num = (int)ViewState["count"];
             creatrControls("txt" + num.ToString(), num, "");
             ViewState["count"] = (int)ViewState["count"] + 1;
        }

        private void creatrControls(string ID, int num, string text)
        {
            Label lb = new Label();
            lb.ID = "lbl" + num.ToString();
            lb.Text = num + 1 + ".";
            this.PlaceHolder1.Controls.Add(lb);

            TextBox tx = new TextBox();
            tx.ID = ID;
            if (text != "")
            {
                tx.Text = text;
            }
            tx.Width = 80;
            this.PlaceHolder1.Controls.Add(tx);
        }

        private void buildTextValue()
        {
            ArrayList allValue = new ArrayList();
            foreach (Control ctl in this.PlaceHolder1.Controls)
            {
                if (ctl is TextBox)
                {
                    TextBox tx = (TextBox)ctl;
                    if (!String.IsNullOrEmpty(tx.Text))
                    {
                        allValue.Add(tx.Text.Trim());
                    }
                }
            }
            this.HiddenField1.Value = string.Join(",", allValue.ToArray());
        }

        private void RecreateControls()
        {
            int cnt = (int)ViewState["count"];
            ArrayList al = null;
            if (!string.IsNullOrEmpty(this.HiddenField1.Value))
            {
                al = new ArrayList(this.HiddenField1.Value.Split(','));
            }
            if (cnt > 0)
            {
                for (int k = 0; k < cnt; k++)
                {
                    if (al == null || k >= al.Count)
                    {
                        creatrControls("txt" + k.ToString(), k, "");
                    }
                    else
                    {
                        creatrControls("txt" + k.ToString(), k, al[k].ToString());
                    }
                }
            }
        }

        public string allTextboxValue
        {
            get
            {
                buildTextValue();
                return this.HiddenField1.Value;
            }
            set { this.HiddenField1.Value = value; }
        }

        public int resetValue
        {
            set { ViewState["count"] = value; }
        }
    }
}