<%@ Page Language="C#" AutoEventWireup="true" Inherits="eIVOGo.SAM.PrintInvoiceAsPDF" StylesheetTheme="Paper" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
<script runat="server">
    protected override void createPDF()
    {
        String pdfFile = Server.CreateContentAsPDF("~/SAM/PrintAllowancePage.aspx", Session.Timeout);
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