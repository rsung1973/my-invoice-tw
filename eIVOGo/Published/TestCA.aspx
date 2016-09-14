<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestCA.aspx.cs" Inherits="eIVOGo.Published.TestCA" %>

<%@ Register src="../Module/Common/SignContext.ascx" tagname="SignContext" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        簽章明文：<br />
        <asp:TextBox ID="DataContext" runat="server" Rows="10" TextMode="MultiLine" 
            Columns="80"></asp:TextBox>
    
        <br />
    
        <asp:Button ID="btnSign" runat="server" Text="Sign" onclick="Button1_Click" />
    
        <br />
        <asp:Literal ID="litMsg" runat="server" EnableViewState="false"></asp:Literal>
    
    </div>
    <uc1:SignContext ID="signContext" runat="server" Catalog="簽章測試" 
        UsePfxFile="False" />
    </form>
</body>
</html>
