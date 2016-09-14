<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModifyMember.ascx.cs" Inherits="eIVOGo.Module.SAM.ModifyMember" %>

<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 會員管理-修改帳號</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img2" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />會員管理-修改帳號</h1>
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03">帳號&nbsp;</th>
    <td class="tdleft">LEE</td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04"><font color="red">*</font>密碼&nbsp;</th>
    <td class="tdleft"><input name="T2" type="password" class="textfield" size="20" value="111111" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04"><font color="red">*</font>重新輸入密碼&nbsp;</th>
    <td class="tdleft"><input name="T2" type="password" class="textfield" size="20" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04"><font color="red">*</font>姓名&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="20" value="李大華" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03"><font color="red">*</font>電子郵件&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="50" value="lee@mail.chs.com.tw" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03"><font color="red">*</font>住址&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="50" value="台北市南海路20號6樓" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04">電話（日）&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="16" value="02-12345678" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04">電話（夜）&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="16" value="02-12345679" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04">行動電話&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="16" value="0999-888888" /></td>
  </tr>
  </table>

<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" class="btn" value="確定" onclick="window.location.href='memberManger.htm'" />
      &nbsp;
    <input name="Reset" type="reset" class="btn" value="重填" /></td>
  </tr>
</table>
