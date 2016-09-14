<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GovPlatformNotificationPage.aspx.cs" Inherits="eIVOGo.Published.GovPlatformNotificationPage" %>

<%@ Register src="../Module/UI/GovPlatformNotification.ascx" tagname="GovPlatformNotification" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center"> 下列資料傳送至大平台時,發生錯誤,請處理!!</div>
    <br />
    <uc1:GovPlatformNotification ID="platformNotification" runat="server" />
    </form>
</body>
</html>
