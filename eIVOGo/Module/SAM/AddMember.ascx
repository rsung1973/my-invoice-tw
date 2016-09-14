<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddMember.ascx.cs" Inherits="eIVOGo.Module.SAM.AddMember" %>
<%@ Register src="../UI/CaptchaImg.ascx" tagname="CaptchaImg" tagprefix="uc1" %>
<%@ Register src="../UI/RegisterMessage.ascx" tagname="RegisterMessage" tagprefix="uc2" %>
<!--路徑名稱-->
 <asp:UpdatePanel id="Updatepanel1" runat="server" >
        <ContentTemplate >
<div id="mainpage" runat="server" >
<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
    <tr>
        <td width="30">
            <img id="img2" runat="server" enableviewstate="false" src="~/images/path_left.gif"
                alt="" width="30" height="29" />
        </td>
        <td bgcolor="#ecedd5">
            首頁 > <% =Title %>
        </td>
        <td width="18">
            <img id="img3" runat="server" enableviewstate="false" src="~/images/path_right.gif"
                alt="" width="18" height="29" />
        </td>
    </tr>
</table>
<!--交易畫面標題-->
<h1>
    <img id="img4" runat="server" enableviewstate="false" src="~/images/icon_search.gif"
        width="29" height="28" border="0" align="absmiddle" /><% =Title %></h1>
     
<div id="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
     <tr id="trRole" runat="server" >
      <th width="20%" nowrap="nowrap"><font color="red">*</font>角色別</th>
      <td class="tdleft">
           <asp:DropDownList ID="ddlRole" runat="server" Height="21px" Width="129px" 
               AutoPostBack="true" onselectedindexchanged="ddlRole_SelectedIndexChanged" >
      </asp:DropDownList><asp:DropDownList ID="ddlCompany" runat="server" Height="20px" 
               Width="129px">
      </asp:DropDownList> </td>
      </tr> 
    <tr>
            <th width="20%">
                <font color="red">*</font> 帳　　號
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtID" runat="server" ></asp:TextBox>
                <asp:Button ID="btnchkid" runat="server" Text="檢查可用的帳號" class="btn"
                    onclick="btnchkid_Click" style="height: 21px" />
            &nbsp;<asp:Label ID="lblIDmsg" runat="server"  ForeColor="Red"></asp:Label>
            <asp:Label ID="lblID" runat="server"  Visible ="false" ></asp:Label>
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 姓　　名
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtName" runat="server" size="20" />
            </td>
        </tr>
        
        <tr>
            <th width="20%">
                <font color="red">*</font> 密　　碼
            </th>
            <td class="tdleft">
               <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" size="10" /> 長度最少需要 6 
                個字元，由英文、數字組成。 <asp:Label ID="lblupdpw" Text="(密碼為空白時，為不修改密碼)" runat="server" Visible="false"   ></asp:Label> </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 重新輸入密碼
            </th>
            <td class="tdleft">
                 <asp:TextBox ID="txtPassword2" TextMode="Password" runat="server" size="10" />
                 <asp:CompareValidator ID="CompareValidator1" runat="server" 
                     ControlToCompare="txtPassword" ControlToValidate="txtPassword2" 
                     ErrorMessage="CompareValidator" ForeColor="Red">密碼輸入內容不同</asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 常用電子郵件
            </th>
            <td class="tdleft">
                 <asp:TextBox ID="txtEmail" runat="server" size="60" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 住　　址
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtAddr" runat="server" size="60" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                電話（日）
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtPhone1" runat="server" size="16" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                電話（夜）
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtPhone2" runat="server" size="16" />
            </td>
        </tr>
        <tr>
            <th width="20%">
                <font color="red">*</font> 行動電話
            </th>
            <td class="tdleft">
                <asp:TextBox ID="txtMobilePhone" runat="server" size="16" />
            </td>
        </tr>
         <tr id="trCaptchaImg" runat="server">
            <th width="20%">
                <font color="red">*</font> 驗 証 碼
            </th>
            <td class="tdleft">
                <uc1:CaptchaImg ID="CaptchaImg1" runat="server" />
            </td>
        </tr>
        <tr id="trservicerule" runat="server" >
      <th width="20%" nowrap="nowrap">服務條款</th>
      <td class="tdleft">
          <asp:TextBox  id="servicerule"  runat="server"  cols="80" rows="5" TextMode="MultiLine" Width="644px" >每天上來對發票
    </asp:TextBox></td>
    </tr>
    </table>
    <!--表格 結束-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td  align="center" >
           <asp:Label ID="lblmsg" runat="server"  ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>

<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn" align="center">
            <asp:Button ID="btnOK" runat="server" class="btn" Text="我接受；建立我的帳戶" 
                onclick="btnOK_Click"  />
            &nbsp;
            <input name="Reset" type="reset" class="btn" value="重填" />
        </td>
    </tr>
</table>
</div>
<uc2:RegisterMessage ID="RegisterMessage1" runat="server" />

  </ContentTemplate>
        </asp:UpdatePanel>