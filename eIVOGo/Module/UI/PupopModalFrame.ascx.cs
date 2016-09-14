using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eIVOGo.Module.UI
{
    public partial class PupopModalFrame : PopupModalMessage
    {
        public String Url { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.PreRender += new EventHandler(PupopModalFrame_PreRender);
        }

        void PupopModalFrame_PreRender(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Url))
            {
                urlTarget.Attributes["src"] = Url;
            }
        }

    }
}