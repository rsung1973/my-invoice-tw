using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Utility;
using eIVOGo.Helper;

namespace eIVOGo.SAM
{
    public partial class PrintInvoiceAsPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createPDF();
        }

        protected virtual void createPDF()
        {
            String pdfFile = Server.CreateContentAsPDF("~/SAM/PrintInvoicePage.aspx", Session.Timeout);
            if (pdfFile != null)
            {
                Response.WriteFileAsDownload(pdfFile, String.Format("{0:yyyy-MM-dd}.pdf", DateTime.Today), true);
            }
            else
            {
                Response.Output.WriteLine("系統忙錄中，請稍後再試...");
                Response.End();
            }
        }
    }
}