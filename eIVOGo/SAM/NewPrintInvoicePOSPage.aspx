<%@ Page Language="C#" AutoEventWireup="true" Inherits="eIVOGo.SAM.NewPrintInvoicePage" Theme="NewPrint" %>

<!DOCTYPE html>
<html>
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
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _defaultPrintView = "~/Module/EIVO/NewInvoicePOSPrintView.ascx";
    }
</script>