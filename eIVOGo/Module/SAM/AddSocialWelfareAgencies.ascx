<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddSocialWelfareAgencies.ascx.cs" Inherits="eIVOGo.Module.SAM.AddSocialWelfareAgencies" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<!--路徑名稱-->
<uc1:PageAction ID="pageAction" runat="server" ItemName="首頁 > 社福機構資料維護-新增資料" />
<!--交易畫面標題-->
<h1><img id="img2" runat="server" enableviewstate="false" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />社福機構資料維護-新增資料</h1>
<div id="border_gray">
<!--表格 開始-->
<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04"><font color="red">*</font>社福機構代碼&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="20" value="" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03"><font color="red">*</font>社福機構名稱&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="20" value="" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03"><font color="red">*</font>住址&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="50" value="" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row04"><font color="red">*</font>電話&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="16" value="" /></td>
  </tr>
  <tr>
    <th width="20%" nowrap="nowrap" class="table-row03">電子郵件&nbsp;</th>
    <td class="tdleft"><input name="T2" type="text" class="textfield" size="20" value="" /></td>
  </tr>
  </table>
<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input type="button" class="btn" value="確定" onclick="window.location.href='SocialWelfareAgenciesManger.htm'" />
      &nbsp;
    <input name="Reset" type="reset" class="btn" value="重填" /></td>
  </tr>
</table>










