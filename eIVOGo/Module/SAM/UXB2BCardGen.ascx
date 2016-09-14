<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UXB2BCardGen.ascx.cs" Inherits="eIVOGo.Module.SAM.UXB2BCardGen" %>


<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>


<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>


<%@ Register src="../UI/InvoiceSellerSelector.ascx" tagname="SellerSelector" tagprefix="uc3" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<!--路徑名稱-->

<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 > UXB2B條碼卡發行" />
<!--交易畫面標題-->
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="UXB2B條碼卡發行" />
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th width="20%" nowrap="nowrap">發行張數</th>
    <td class="tdleft">
        <asp:RadioButtonList ID="rbTotal" runat="server" 
            RepeatDirection="Horizontal" BorderStyle="None" RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="10">10 </asp:ListItem>
            <asp:ListItem Value="50">50 </asp:ListItem>
            <asp:ListItem Value="100">100 </asp:ListItem>
            <asp:ListItem Value="1000">1000 </asp:ListItem>
            <asp:ListItem Value="">其他</asp:ListItem>
        </asp:RadioButtonList>
       </td>
  </tr>
  <tr id="other">
    <th nowrap="nowrap">其他張數</th>
    <td class="tdleft">
        <asp:TextBox ID="TotalCount" runat="server" CssClass="textfield"></asp:TextBox>
&nbsp;張（最多輸入1000張）</td>
  </tr>
   <tr>
    <th width="20%" nowrap="nowrap">店家會員</th>
    <td class="tdleft">
        <uc3:SellerSelector ID="SellerID" runat="server" />
    </td>
    </tr>
  </table>

<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn">
        <asp:Button ID="btnCreate" runat="server" Text="條碼卡發行" 
            onclick="btnCreate_Click" />
      </td>
  </tr>
</table>

