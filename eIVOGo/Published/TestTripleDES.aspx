<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestTripleDES.aspx.cs" Inherits="eIVOGo.Published.TestTripleDES" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        PIN Code:<asp:TextBox ID="AuthCode" runat="server" TextMode="Password"></asp:TextBox>
        <br />
        Content File:<asp:FileUpload ID="fData" runat="server" />
        <br />
        <asp:Button ID="btnEncrypt" runat="server" onclick="btnEncrypt_Click" 
            Text="加密" />
&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnDecrypt" runat="server" onclick="btnDecrypt_Click" 
            Text="解密" />
    
    </div>
    </form>
</body>
</html>
