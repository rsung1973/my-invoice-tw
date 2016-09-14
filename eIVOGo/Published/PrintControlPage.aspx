<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintControlPage.aspx.cs" Inherits="eIVOGo.Published.PrintControlPage" StylesheetTheme="Print" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload='javascript:self.print(); self.close();'>
    <form id="theForm" runat="server">
    <asp:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    </form>
</body>
</html>
