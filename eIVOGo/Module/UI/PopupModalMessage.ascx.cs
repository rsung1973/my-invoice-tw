using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.UI
{
    public partial class PopupModalMessage : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Show()
        {
            this.ModalPopupExtender.Show();
        }

        public String Message
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

        public String GetClientTriggerScript()
        {
            return String.Format("$('#{0}').trigger('click');", btnPopup.ClientID);
        }
    }
}