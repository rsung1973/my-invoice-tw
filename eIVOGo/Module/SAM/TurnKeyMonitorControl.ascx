<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TurnKeyMonitorControl.ascx.cs" Inherits="eIVOGo.Module.SAM.TurnKeyMonitorControl" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register assembly="Uxnet.Web" namespace="Uxnet.Web.Module.Common" tagprefix="cc2" %>
<%@ Register src="../UI/GovPlatformNotification.ascx" tagname="GovPlatformNotification" tagprefix="uc3" %>
<%@ Register src="TurnKeyStatusList.ascx" tagname="TurnKeyStatusList" tagprefix="uc4" %>
<!--路徑名稱-->
<uc1:PageAction ID="pageAction" runat="server" ItemName="首頁 > TurnKey傳送狀態" />
<uc4:TurnKeyStatusList ID="TurnKeyStatus" runat="server" />















































