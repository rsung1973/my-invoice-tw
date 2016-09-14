<%@ Page Language="C#" AutoEventWireup="true"%>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Utility" %>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_demoprintinvoicepdf_aspx_Load;
        
    }

    void _test_demoprintinvoicepdf_aspx_Load(object sender, EventArgs e)
    {
        String pdfFile = Server.CreateContentAsPDF("DemoPrintInvoiceCSS.aspx", Session.Timeout);
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
</script>
