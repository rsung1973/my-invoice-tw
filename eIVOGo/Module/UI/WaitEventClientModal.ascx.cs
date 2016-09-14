using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace eIVOGo.Module.UI
{
    public partial class WaitEventClientModal : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [Bindable(true)]
        public String TargetControlID
        {
            get
            {
                return ModalPopupExtender.TargetControlID;
            }
            set
            {
                ModalPopupExtender.TargetControlID = value;
            }
        }

        [Bindable(true)]
        public String CancelControlID
        {
            get
            {
                return ModalPopupExtender.CancelControlID;
            }
            set
            {
                ModalPopupExtender.CancelControlID = value;
            }
        }

    }
}