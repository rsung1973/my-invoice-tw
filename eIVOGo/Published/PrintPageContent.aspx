<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Print" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>
<%@ Register Src="~/Module/Common/JsGrid.ascx" TagPrefix="uc1" TagName="JsGrid" %>

<html>
<head runat="server">
    <title></title>
</head>
<body onload='javascript:self.focus();<%= Page.PreviousPage==null || Page.PreviousPage.Items["hasPrintMode"]==null ? "self.print();" : "" %>'>
    <form id="theForm" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
        <uc1:JsGrid runat="server" ID="JsGrid" />
        <% if(Page.PreviousPage!=null && Page.PreviousPage.Items["pageContent"]!=null) { %>
        <%= Page.PreviousPage.Items["pageContent"] %>
        <% } %>
    </form>
</body>
</html>
