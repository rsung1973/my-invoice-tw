<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Native2Ascii.aspx.cs" Inherits="eIVOGo.Published.Native2Ascii" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        請選擇檔案:<asp:FileUpload ID="srcFile" runat="server" />
        &nbsp;文件編碼:<asp:DropDownList ID="ddEncoding" runat="server">
            <asp:ListItem>big5</asp:ListItem>
            <asp:ListItem>utf-8</asp:ListItem>
            <asp:ListItem>unicode</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <asp:CheckBox ID="cbToCN" runat="server" Text="繁體轉簡體" />
        &nbsp;
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="確定" />
        <br />
        文字轉換:<asp:TextBox ID="textContent" runat="server" Columns="40" Rows="10" 
            TextMode="MultiLine"></asp:TextBox>
&nbsp;<asp:Button ID="btnConvert" runat="server" OnClick="btnConvert_Click" Text="轉換" />
        <br />
        <pre id="codeText" runat="server" enableviewstate="false"></pre>
    </div>
    </form>
</body>
</html>
