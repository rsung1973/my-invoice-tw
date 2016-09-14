<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addDelivery_Company_Maintain.ascx.cs" Inherits="eIVOGo.Module.SCM.addDelivery_Company_Maintain" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register src="../UI/PageAction.ascx" tagname="PageAction" tagprefix="uc5" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc6" %>
<%@ Register src="../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc4" %>
<%@ Register src="../UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc4" %>


<!--路徑名稱-->
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增宅配公司" />
<!--交易畫面標題--><uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增宅配公司" />

<div id="border_gray">
    <h2>宅配公司基本資料</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%" class="tdleft"><span style="color: red">*</span>名稱：</th>
            <td class="tdleft">
                <asp:TextBox ID="NAME" runat="server" MaxLength="100"></asp:TextBox>
            </td>
            <th width="15%">統一編號：</th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="BAN" runat="server" MaxLength="8"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">地址：</th>
            <td colspan="3" class="tdleft">
                <asp:TextBox ID="ADDR"  size="80" runat="server" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">電話：</th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="PHONE" runat="server" MaxLength="20"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">傳真：</th>
            <td class="tdleft">
                <asp:TextBox ID="FAX" runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">聯絡人名稱：</th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="CONTACT_NAME" runat="server" MaxLength="10"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">聯絡人電子郵件：</th>
            <td class="tdleft">
                <asp:TextBox ID="CONTACT_EMAIL" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr style="display :none">
            <th width="15%"><span style="color: red">*</span>列印收件人單據選項：</th>
            <td colspan="3" class="tdleft">
                &nbsp;
                <asp:RadioButton ID="RadioButton1" runat="server" Checked="True" Text="列印" GroupName="r1" />
                <asp:RadioButton ID="RadioButton2" runat="server" Text="不列印" GroupName="r1" />
            </td>
        </tr>
    </table>
<!--按鈕-->
</div>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td class="Bargain_btn">    
        <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" onclick="btnOk_Click" />&nbsp;&nbsp;
        <input id="Reset1" type="reset" value="重填" class="btn" />    
        <asp:HiddenField ID="HiddenField1" runat="server" />    
    </td>
  </tr>
</table>
<div>
    <center>
        <asp:Label ID="lblError"  ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
    </center> 
</div> 