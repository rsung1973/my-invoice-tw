<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CardManager.ascx.cs" Inherits="eIVOGo.Module.SAM.CardManager" %>
<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img1" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 會員管理-載具管理</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img3" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />會員管理-載具管理</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01">
  <tr>
    <th nowrap="nowrap">角色</th>
    <th nowrap="nowrap">帳號</th>
    <th nowrap="nowrap">姓名/店家名稱</th>
  </tr>
  <tr>
    <td align="center">一般會員</td>
    <td align="center">LEE</td>
    <td align="center">李大華</td>
  </tr>
</table>
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th colspan="2" class="Head_style_a">新增載具</th>
    </tr>
  <tr>
    <th width="20%">載具種類</th>
    <td class="tdleft"><input type="radio" value="V1" checked="checked" name="R2" id="ux" />
      UXB2B條碼卡
      <input type="radio" value="V1" name="R2" id="easy" />
      悠遊卡
  </tr>
  <tr id="uxb2b">
    <th>UXB2B條碼卡號</th>
    <td class="tdleft"><input name="T" type="text" class="textfield" size="20" />
      （共20碼）
        <input name="button" type="button" class="btn" id="button" value="確定" onclick="location.href='cardBelong.htm'" /></td>
  </tr>
</table>
<!--表格 結束-->
</div>
<!--按鈕-->
<h1><img runat="server" enableviewstate="false" id="img4" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />載具清單</h1>
<div id="border_gray">
  <!--表格 開始-->
  <table width="100%" border="0" cellpadding="0" cellspacing="0" class="table01">
    <tr>
      <th nowrap="nowrap">載具種類</th>
      <th nowrap="nowrap">載具卡號</th>
      <th nowrap="nowrap">&nbsp;</th>
    </tr>
    <tr>
      <td align="center">UXB2B條碼卡</td>
      <td>1ADFJEUREWKJ8SD7DSJK</td>
      <td align="center"><input name="delCar" type="button" class="btn" value="刪除" onclick="if(confirm('確定刪除此筆料？')){alert('該資料已刪除。');}" /></td>
    </tr>
    <tr>
      <td align="center">悠遊卡</td>
      <td>FDSJFDSGIOSDFGS299DS</td>
      <td align="center"><input name="delCar" type="button" class="btn" value="刪除" onclick="if(confirm('確定刪除此筆料？')){alert('該資料已刪除。');}" /></td>
    </tr>
    <tr>
      <td align="center">UXB2B條碼卡</td>
      <td>FDSFKLDSFSSDL99FDKK</td>
      <td align="center"><input name="delCar" type="button" class="btn" value="刪除" onclick="if(confirm('確定刪除此筆料？')){alert('該資料已刪除。');}" /></td>
    </tr>
    <tr>
      <td align="center">UXB2B條碼卡</td>
      <td>888FDSKJHDFSKEJUDSKS</td>
      <td align="center"><input name="delCar" type="button" class="btn" value="刪除" onclick="if(confirm('確定刪除此筆料？')){alert('該資料已刪除。');}" /></td>
    </tr>
    <tr>
      <td align="center">悠遊卡</td>
      <td>FDSJFSDIWEKR98FJDSGK</td>
      <td align="center"><input name="delCar" type="button" class="btn" value="刪除" onclick="if(confirm('確定刪除此筆料？')){alert('該資料已刪除。');}" /></td>
    </tr>
  </table>
  <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><span class="table-title">
      <input name="closewin" type="button" class="btn" value="關閉視窗" onclick="window.close();" />
    </span></td>
  </tr>
</table>
