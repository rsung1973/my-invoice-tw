<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemErrorLog.ascx.cs" Inherits="eIVOGo.Module.SAM.SystemErrorLog" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register assembly="Uxnet.Web" namespace="Uxnet.Web.Module.Common" tagprefix="cc2" %>
<%@ Register src="../UI/GovPlatformNotification.ascx" tagname="GovPlatformNotification" tagprefix="uc3" %>
<%@ Register src="InvoiceClientStatusList.ascx" tagname="InvoiceClientStatusList" tagprefix="uc4" %>
<%@ Register src="ExceptionLogList.ascx" tagname="ExceptionLogList" tagprefix="uc5" %>
<!--路徑名稱-->


<uc1:PageAction ID="pageAction" runat="server" ItemName="首頁 > 異常記錄查詢" />
<uc5:ExceptionLogList ID="exceptionLog" runat="server" />














































