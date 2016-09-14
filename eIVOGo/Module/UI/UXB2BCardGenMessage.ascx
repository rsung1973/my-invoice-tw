<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UXB2BCardGenMessage.ascx.cs" Inherits="eIVOGo.Module.UI.UXB2BCardGenMessage" %>
<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > UXB2B條碼卡發行</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />作業訊息</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" id="table_msg">
  <tr>
    <td align="center">作業已完成！</td>
  </tr>
</table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" name="Submit" class="btn" value="回首頁" onClick="window.location='main.htm'" /></td>
  </tr>
</table>
