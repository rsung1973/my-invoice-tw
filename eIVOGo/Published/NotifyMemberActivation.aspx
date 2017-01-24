<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotifyMemberActivation.aspx.cs"
    Inherits="eIVOGo.Published.NotifyMemberActivation" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    本信件由 電子發票系統 寄出，為本站之會員註冊確認信。<br />
    <br />
    (本信件為系統自動發出，請勿回覆本信件。)<br />
    -------------------------------------------------<br />
    會員帳號：<%# _item.PID %><br />
    會員密碼：<%# _tempPWD %><br />
    -------------------------------------------------<br />
    請立即透過下面帳號啟用連結登入 電子發票系統 變更密碼 。<br />
    <br />
    帳號啟用連結： <a href='<%# String.Format("{0}{1}?active=aEfs45WE",eIVOGo.Properties.Settings.Default.mailLinkAddress,VirtualPathUtility.ToAbsolute("~/UserProfile/EditMySelf")) %>'>
        會員帳號啟用</a>
    <br />
    <br />
    電子發票系統 感謝您的加入
    <cc1:UserProfileDataSource ID="dsEntity" runat="server">
    </cc1:UserProfileDataSource>
    </form>
</body>
</html>
