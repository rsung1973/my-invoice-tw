<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestEMail.aspx.cs" Inherits="eIVOGo.Published.TestEMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        郵件主旨:<asp:TextBox ID="Subject" runat="server" Columns="64"></asp:TextBox>
        <br />
        內容頁網址:<asp:TextBox ID="Url" runat="server" Columns="64"></asp:TextBox>
        <br />
        收件人:<asp:TextBox ID="MailTo" runat="server" Columns="64"></asp:TextBox>
        <br />
        <asp:Button ID="btnSend" runat="server" onclick="btnSend_Click" Text="傳送" />
    
    </div>
    </form>
</body>
</html>
