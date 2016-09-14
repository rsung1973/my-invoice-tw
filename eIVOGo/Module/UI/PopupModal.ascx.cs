using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Uxnet.Web.WebUI;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class PopupModal : System.Web.UI.UserControl
    {
        public event EventHandler Done;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!String.IsNullOrEmpty(ContentControlSource))
            {
                Control ctrl = this.LoadControl(ContentControlSource);
                holder.Controls.Add(ctrl);
            }
        }

        public virtual void Show()
        {
            this.Visible = true;
            this.ModalPopupExtender.Show();
        }

        public virtual void Close()
        {
            ModalPopupExtender.Hide();
            this.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender.Hide();
            if (Done != null)
            {
                Done(this, new EventArgs());
            }
        }

        [Bindable(true)]
        public String TitleName
        {
            get
            {
                return titleBar.ItemName;
            }
            set
            {
                titleBar.ItemName = value;
            }
        }

        [Bindable(true)]
        public String ContentControlSource
        {
            get;
            set;
        }


    }
}