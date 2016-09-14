using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Mail;
using Model.Resource;
using eIVOGo.Properties;

namespace OpenSite
{
	/// <summary>
	/// Summary description for errorPage.
	/// </summary>
    public partial class CAUsageNote : System.Web.UI.Page
	{
        protected void Page_Load(object sender, System.EventArgs e)
        {
            // Put user code to initialize the page here
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

        protected void lbCommand_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentEncoding = System.Text.Encoding.ASCII;
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", "AddTrustedSite.bat"));
            Response.Output.WriteLine(lbCommand.Text);
            Response.Flush();
            Response.End();

        }
	}
}
