<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryBonusList.ascx.cs" Inherits="eIVOGo.Module.Inquiry.QueryBonusList" %>


<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img4" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 查詢/列印中獎清冊</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img2" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢/列印中獎清冊</h1>
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th colspan="2" class="Head_style_a">查詢條件</th>
    </tr>
  <tr>
    <th>查詢類別</th>
    <td class="tdleft">
      依載具
      <select name="D1" size="1" class="textfield" id="device">
        <option>-請選擇-</option>
        <option>UXB2B條碼卡</option>
        <option>悠遊卡</option>
        </select></td>
  </tr>
  <tr id="uxb2b">
    <th>UXB2B條碼卡號</th>
    <td class="tdleft"><input name="T" type="text" class="textfield" size="20" />
      （共20碼）</td>
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
  <tr>
    <th>顯示捐贈</th>
    <td class="tdleft"><input type="radio" value="V1" checked="checked" name="R3" />
      全部（含未捐贈）&nbsp;&nbsp;&nbsp;&nbsp;
      <input type="radio" value="V1" name="R3" />
      只顯示捐贈發票 </td>
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
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />查詢結果</h1>
<div id="border_gray">
  <!--表格 開始-->
  <table width="100%" border="0" cellpadding="0" cellspacing="0" id="table01">
    <tr>
      <th nowrap="nowrap">期別</th>
      <th nowrap="nowrap">發票號碼</th>
      <th nowrap="nowrap">開立發票營業人</th>
      <th nowrap="nowrap">營業地址</th>
      <th nowrap="nowrap">載具種類</th>
      <th width="120" nowrap="nowrap">捐贈單位</th>
    </tr>
    <tr>
      <td align="center">99年11-12月</td>
      <td align="center"><a href="newInvalidInvoicePreview.htm" target="_blank">AY74423555</a></td>
      <td>網際優勢</td>
      <td>台北市南海路20號6樓</td>
      <td align="center">UXB2B條碼卡</td>
      <td width="120" align="center">
      <div class="tdbonus">
        <a class="bonus" href="#">UB0001</a>
        <em>伊甸基金會</em>
      </div>
      </td>
    </tr>
    <tr class="OldLace">
      <td align="center">99年11-12月</td>
      <td align="center"><a href="newInvalidInvoicePreview.htm">AY74423556</a></td>
      <td>網際優勢</td>
      <td>台北市南海路20號6樓</td>
      <td align="center">悠遊卡</td>
      <td width="120" align="center">
      <div class="tdbonus">
        N/A
      </div>
      </td>
    </tr>
    <tr>
      <td align="center">99年11-12月</td>
      <td align="center"><a href="newInvalidInvoicePreview.htm" target="_blank">AY74423557</a></td>
      <td>網際優勢</td>
      <td>台北市南海路20號6樓</td>
      <td align="center">悠遊卡</td>
      <td width="120" align="center">
      <div class="tdbonus">
        N/A
      </div>
      </td>
    </tr>
    <tr class="OldLace">
      <td align="center">99年11-12月</td>
      <td align="center"><a href="newInvalidInvoicePreview.htm" target="_blank">AY74423558</a></td>
      <td>網際優勢</td>
      <td>台北市南海路20號6樓</td>
      <td align="center">UXB2B條碼卡</td>
      <td width="120" align="center">
      <div class="tdbonus">
        N/A
      </div>
      </td>
    </tr>
    <tr>
      <td align="center">99年11-12月</td>
      <td align="center"><a href="newInvalidInvoicePreview.htm" target="_blank">AY74423559</a></td>
      <td>網際優勢</td>
      <td>台北市南海路20號6樓</td>
      <td align="center">UXB2B條碼卡</td>
      <td width="120" align="center">
      <div class="tdbonus">
        <a class="bonus" href="#">UB0002</a>
        <em>兒福聯盟</em>
      </div>
      </td>
    </tr>
  </table>
  <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" id="table-count">
    <tr>
      <td>| 1 | <a href="#">2</a> |&nbsp; <a href="#">後10頁</a> | <a href="#">最後1頁</a>
        <input name="textfield" type="text" class="textfield" size="3" />
        <input name="cancel22" type="reset" class="btn" value="頁數" /></td>
      <td align="right" nowrap="nowrap"><span>總筆數：100&nbsp;&nbsp;&nbsp;總頁數：10</span></td>
    </tr>
  </table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input name="B3" type="button" class="btn" value="資料列印" /></td>
  </tr>
</table>
