<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPrint.aspx.cs" Inherits="eIVOGo.Published.TestPrint" StylesheetTheme="Print" %>


<%@ Register src="../Module/Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
        <uc1:PrintingButton2 ID="btnPrint" runat="server" />
    <asp:TextBox ID="Test" runat="server"></asp:TextBox>
    <asp:TextBox ID="length" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server"
        Text="Button" onclick="Button1_Click" />
    
    </div>
    </form>
</body>
</html>
