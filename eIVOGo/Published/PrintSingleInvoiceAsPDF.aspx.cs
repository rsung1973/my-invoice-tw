using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using eIVOGo.Helper;
using System.IO;
using Utility;


namespace eIVOGo.SAM
{
    public partial class PrintSingleInvoiceAsPDF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createB2BInvPDF();
        } 

        private void createB2BInvPDF()
        {
            String pdfFile = Server.CreateContentAsPDF("~/Published/PrintSingleInvoicePage.aspx", Session.Timeout);
            if (pdfFile != null)
            {
                if (Request["nameOnly"] != null)
                {
                    String outputFile = Path.Combine(Logger.LogDailyPath, Path.GetFileName(pdfFile));
                    File.Move(pdfFile, outputFile);
                    Response.Write(outputFile);
                    Response.End();
                }
                else
                {
                    Response.WriteFileAsDownload(pdfFile, String.Format("{0:yyyy-MM-dd}.pdf", DateTime.Today), true);
                }
            }
            else
            {
                Response.Output.WriteLine("系統忙錄中，請稍後再試...");
                Response.End();
            }
        }
    }
}