<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterMessage.ascx.cs" Inherits="eIVOGo.Module.UI.RegisterMessage" %>

<%@ Register src="PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>

<!--路徑名稱-->
<uc1:PageAction 
        ID="actionItem" runat="server" ItemName="首頁 > 加入會員" />
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />作業訊息
</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" id="table_msg">
  <tr>
    <td align="center">
        <asp:Literal ID="litMsg" runat="server" EnableViewState="false" Text="作業已完成！"></asp:Literal></td>
  </tr>
</table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn" align="center">
    <asp:Button ID="btnBackHome" runat="server"   class="btn" 
            Text="回首頁" onclick="btnBackHome_Click"  />
    &nbsp;
    <asp:Button ID="btnBack" runat="server"   class="btn" Text="回會員管理" 
            onclick="btnBack_Click" Visible="false"  />
    </td>
  </tr>
</table>
