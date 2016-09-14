<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BonusReport.ascx.cs" Inherits="eIVOGo.Module.Inquiry.BonusReport" %>
<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img id="img1" runat="server" enableviewstate="false" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 中獎統計表</td>
    <td width="18"><img id="img2" runat="server" enableviewstate="false" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img id="img6" runat="server" enableviewstate="false" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />中獎統計表</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
  <tr>
    <th width="20%">日期區間</th>
    <td class="tdleft">
      自&nbsp;<input readonly name="appstartday" type="text" class="textfield" size="10" />
      &nbsp;&nbsp;<img id="img3" runat="server" enableviewstate="false" src="~/images/date.gif" width="16" height="15" border="0" align="absmiddle" style="cursor:hand;" onClick=calender(addForm.appstartday) />
      至&nbsp;<input readonly name="appendday" type="text" class="textfield" size="10" />
      &nbsp;<img id="img4" runat="server" enableviewstate="false" src="~/images/date.gif" width="16" height="15" border="0" align="absmiddle" style="cursor:hand;" onClick=calender(addForm.appendday) /></td>
  </tr>
  <tr>
    <th width="20%">營 業 人</th>
    <td class="tdleft"><select name="seller" size="1" class="textfield">
      <option>全部</option>
      <option>70762419&nbsp;網際優勢</option>
      <option>12345678&nbsp;來來小吃</option>
      <option>87654321&nbsp;來客精品</option>
      <option>87654322&nbsp;南海圖書</option>
      <option>87654323&nbsp;地虎書局</option>
    </select></td>
  </tr>
</table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" name="Submit" class="btn" value="查詢" onClick="window.location='BonusReport_list.htm'" /></td>
  </tr>
</table>
