using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Uxnet.Com.WebUI.Control
{
    public class Ctrl_InboxItem : WebControl
    {
        private string _message;
        private string _link;

        public Ctrl_InboxItem()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        public string Link
        {
            get
            {
                return _link;
            }
            set
            {
                _link = value;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {

            output.Write(" <SPAN style=\"FONT-WEIGHT: bold; FONT-SIZE: 13px; COLOR: #0066cc; FONT-FAMILY: \"新細明體\", \"Arial\"\"> ");
            output.Write("<IMG height=\"6\" src=\"../page/image/dot_downyellow.gif\" width=\"6\"> ");
            output.Write(" <A href=\"");
            output.Write(_link);
            output.Write("\">");
            output.Write(_message);
            output.Write("</A>	</SPAN>");

        }

    }
}
