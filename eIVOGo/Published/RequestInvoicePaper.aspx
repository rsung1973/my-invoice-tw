<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RequestInvoicePaper.aspx.cs" Inherits="eIVOGo.Published.RequestInvoicePaper" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
</body>
</html>
