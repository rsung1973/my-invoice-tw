<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>
<%@ Register Src="~/Module/Common/JsGrid.ascx" TagPrefix="uc1" TagName="JsGrid" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/InvoiceItemList.ascx" TagPrefix="uc1" TagName="InvoiceItemList" %>
<%@ Register Src="~/Module/JsGrid/InquireInvoice.ascx" TagPrefix="uc1" TagName="InquireInvoice" %>


<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
        <uc1:JsGrid runat="server" ID="JsGrid" />
        <uc1:InquireInvoice runat="server" ID="inquiry" />
    </form>
</body>
</html>
