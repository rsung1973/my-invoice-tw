<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModifySocialWelfareAgencies.ascx.cs" Inherits="eIVOGo.Module.SAM.ModifySocialWelfareAgencies" %>


<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 社福機構資料維護-<asp:Label ID="lblTitle1" runat="server"></asp:Label></td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />社福機構資料維護-<asp:Label ID="lblTitle2" runat="server"></asp:Label></h1>
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th width="20%" nowrap="nowrap"><font color="red">*</font>社福機構統編&nbsp;</th>
    <td class="tdleft"><asp:TextBox ID="txtOrgCode" CssClass="textfield" size="20" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap"><font color="red">*</font>社福機構名稱&nbsp;</th>
    <td class="tdleft"><asp:TextBox ID="txtOrgName" CssClass="textfield" size="50" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap"><font color="red">*</font>住址&nbsp;</th>
    <td class="tdleft"><asp:TextBox ID="txtOrgAddr" CssClass="textfield" size="50" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap"><font color="red">*</font>電話&nbsp;</th>
    <td class="tdleft"><asp:TextBox ID="txtOrgPhone" CssClass="textfield" size="20" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap">電子郵件&nbsp;</th>
    <td class="tdleft"><asp:TextBox ID="txtOrgEMail" CssClass="textfield" size="50" runat="server"></asp:TextBox></td>
  </tr>
  <tr>
    <td colspan="2" align="center"><asp:Label ID="lblError" Visible="false" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label></td>
  </tr>
  </table>

<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><asp:Button ID="btnConfirm" Text="確定" CssClass="btn" 
            runat="server" onclick="btnConfirm_Click" />
      &nbsp;
    <asp:Button ID="btnReset" Text="重填" CssClass="btn" runat="server" 
            onclick="btnReset_Click" /></td>
  </tr>
</table>
