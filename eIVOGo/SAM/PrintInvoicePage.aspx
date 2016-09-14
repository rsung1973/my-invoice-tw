<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintInvoicePage.aspx.cs" Inherits="eIVOGo.SAM.PrintInvoicePage" StylesheetTheme="Paper" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css" media="print">
        div.fspace
        {
            height: 8.8cm;
        }
        div.bspace
        {
            height: 9.5cm;
        }
    </style>
    <title>電子發票系統</title>
    <%--<meta http-equiv="content-type" content="text/html; charset=UTF-8" />--%>
</head>
<body onload='javascript:self.print();self.close();'>
    <form id="theForm" runat="server">
    </form>
</body>
</html>
