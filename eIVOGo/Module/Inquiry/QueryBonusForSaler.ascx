<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBonusForSaler.ascx.cs" Inherits="eIVOGo.Module.Inquiry.QueryBonusForSaler" %>

<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 查詢/列印中獎清冊</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢/列印中獎清冊</h1>
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
  <tr>
    <th width="20%">日期區間</th>
    <td class="tdleft"><select name="startday" size="1" class="textfield">
      <option>99年11-12月</option>
      <option>99年09-10月</option>
      <option>99年07-08月</option>
      <option>99年05-06月</option>
      <option>99年03-04月</option>
      <option>99年01-02月</option>
      <option>98年11-12月</option>
      <option>98年09-10月</option>
      <option>98年07-08月</option>
      <option>98年05-06月</option>
      <option>98年03-04月</option>
      <option>98年01-02月</option>
      </select>
      ～
  <select name="startday" size="1" class="textfield">
    <option>99年11-12月</option>
    <option>99年09-10月</option>
    <option>99年07-08月</option>
    <option>99年05-06月</option>
    <option>99年03-04月</option>
    <option>99年01-02月</option>
    <option>98年11-12月</option>
    <option>98年09-10月</option>
    <option>98年07-08月</option>
    <option>98年05-06月</option>
    <option>98年03-04月</option>
    <option>98年01-02月</option>
  </select></td>
  </tr>
</table>

<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" name="Submit" class="btn" value="查詢" onClick="window.location='queryBonus_list.htm'" /></td>
  </tr>
</table>
