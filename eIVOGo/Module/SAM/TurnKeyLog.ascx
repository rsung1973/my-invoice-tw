<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnKeyLog.ascx.cs" Inherits="eIVOGo.Module.SAM.TurnKeyLog" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register assembly="Uxnet.Web" namespace="Uxnet.Web.Module.Common" tagprefix="cc2" %>
<%@ Register src="../UI/GovPlatformNotification.ascx" tagname="GovPlatformNotification" tagprefix="uc3" %>
<%@ Register src="TurnKeyLogList.ascx" tagname="TurnKeyLogList" tagprefix="uc4" %>
<!--路徑名稱-->
<uc1:PageAction ID="pageAction" runat="server" ItemName="首頁 > TurnKey傳送紀錄" />
<uc4:TurnKeyLogList ID="TurnKeyLogList" runat="server" />















































