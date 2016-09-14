<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Register.ascx.cs" Inherits="eIVOGo.Module.SAM.Register" %>


<%@ Register src="../UI/CaptchaImg.ascx" tagname="CaptchaImg" tagprefix="uc1" %>


<!--路徑名稱-->
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
  <tr>
    <td width="30"><img runat="server" enableviewstate="false" id="img4" src="~/images/path_left.gif" alt="" width="30" height="29" /></td>
    <td bgcolor="#ecedd5">首頁 > 加入會員</td>
    <td width="18"><img runat="server" enableviewstate="false" id="img3" src="~/images/path_right.gif" alt="" width="18" height="29" /></td>
  </tr>
</table>
<!--交易畫面標題-->
<h1><img runat="server" enableviewstate="false" id="img1" src="~/images/icon_search.gif" width="29" height="28" border="0" align="absmiddle" />加入會員</h1>
<div id="border_gray">
<!--表格 開始-->

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>姓名</th>
      <td class="tdleft"><input name="textfield" type="text" id="textfield" size="20" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>帳號</th>
      <td class="tdleft"><input name="textfield2" type="text" id="textfield2" size="20" />
        <input name="button2" type="button" class="btn" id="button2" value="檢查可用的帳號" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>密碼</th>
      <td class="tdleft"><input name="textfield3" type="text" id="textfield3" size="20" />
        長度最少需要 8 個字元，由英文、數字組成。</td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>重新輸入密碼</th>
      <td class="tdleft"><input name="textfield3" type="text" id="textfield3" size="20" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>常用電子郵件</th>
      <td class="tdleft"><input name="textfield4" type="text" id="textfield4" size="50" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>住址</th>
      <td class="tdleft"><input name="textfield5" type="text" id="textfield5" size="50" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap">電話（日）</th>
      <td class="tdleft"><input name="textfield6" type="text" id="textfield6" size="20" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap">電話（夜）</th>
      <td class="tdleft"><input name="textfield6" type="text" id="textfield7" size="20" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap">行動電話</th>
      <td class="tdleft"><input name="textfield7" type="text" id="textfield8" size="20" /></td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap"><font color="red">*</font>驗證碼</th>
      <td class="tdleft">
          <uc1:CaptchaImg ID="CaptchaImg1" runat="server" />
        </td>
    </tr>
    <tr>
      <th width="20%" nowrap="nowrap">服務條款</th>
      <td class="tdleft"><textarea name="textarea" id="textarea" cols="80" rows="5">ssssssssssssssssssss
    ssssssssssssssssssssssss
    ssssssssssssss
    ssssssssssssssssssssssssss
    sssssssssssssssssssssss
    ssssssssssssssssssssss
    ssssssssssssssssssssssssssss
    sssssssssssssssssssssssssssssss
    </textarea></td>
    </tr>
    </table>

<!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn"><input name="button" type="button" class="btn" id="button" value="我接受；建立我的帳戶" onclick="location.href='regist_msg.htm'" />
      &nbsp;
    <input name="Reset" type="reset" class="btn" value="重填" /></td>
  </tr>
</table>
