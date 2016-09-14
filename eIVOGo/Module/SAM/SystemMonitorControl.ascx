<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemMonitorControl.ascx.cs" Inherits="eIVOGo.Module.SAM.SystemMonitorControl" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>
<%@ Register assembly="Uxnet.Web" namespace="Uxnet.Web.Module.Common" tagprefix="cc2" %>
<%@ Register src="../UI/GovPlatformNotification.ascx" tagname="GovPlatformNotification" tagprefix="uc3" %>
<%@ Register src="InvoiceClientStatusList.ascx" tagname="InvoiceClientStatusList" tagprefix="uc4" %>
<!--路徑名稱-->


<uc1:PageAction ID="pageAction" runat="server" ItemName="首頁 > 用戶端連線狀態" />
<uc4:InvoiceClientStatusList ID="clientStatus" runat="server" />
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><asp:Button ID="btnGov" runat="server" class="btn" onclick="btnGov_Click" Text="重新取得大平台錯誤訊息" />
    &nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnRefresh" runat="server" class="btn" Text="重新整理" />  
        &nbsp;&nbsp;&nbsp;狀態:<%= __BackgroundService.ThreadState.ToString()%>
      </td>
  </tr>
</table>














































