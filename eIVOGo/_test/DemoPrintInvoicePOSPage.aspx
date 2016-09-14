<%@ Page Language="C#" AutoEventWireup="true" Theme="NewPrint" %>

<%@ Register Src="~/Module/EIVO/NewInvoicePOSPrintView.ascx" TagPrefix="uc1" TagName="NewInvoicePrintView" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        div.fspace
        {
            height: 8.8cm;
        }
        div.bspace
        {
            height: 8.9cm;
        }
        
        body, td, th
        {
            font-family: Verdana, Arial, Helvetica, sans-serif, "細明體" , "新細明體";
        }      

    </style>
    <title>電子發票系統</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />    
</head>
<body>
    <script type="text/javascript" src="../Scripts/css3-multi-column.js"></script>
    <form id="theForm" runat="server">
        <uc1:NewInvoicePrintView runat="server" ID="finalView" IsFinal="true" />
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        int invoiceID;
        if (!String.IsNullOrEmpty(Request["invoiceID"]) && int.TryParse(Request["invoiceID"], out invoiceID))
        {
            finalView.InvoiceID = invoiceID;
        }
        else if (Request["invoiceNo"] != null)
        {
            finalView.InvoiceNo = Request["invoiceNo"];
        }
    }
</script>
