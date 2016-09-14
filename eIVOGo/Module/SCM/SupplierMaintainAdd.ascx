<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierMaintainAdd.ascx.cs"
    Inherits="eIVOGo.Module.SCM.SupplierMaintainAdd" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="../Common/SignContext.ascx" TagName="SignContext" TagPrefix="uc1" %>
<%@ Register Src="../Common/ROCCalendarInput.ascx" TagName="ROCCalendarInput" TagPrefix="uc3" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="../Common/PrintingButton2.ascx" TagName="PrintingButton2" TagPrefix="uc4" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc4" %>


<!--路徑名稱-->
<uc5:pageaction id="actionItem" runat="server" itemname="首頁 > 新增供應商資料" />
<!--交易畫面標題-->
<uc6:functiontitlebar id="titleBar" runat="server" itemname="新增供應商資料" />
<div id="border_gray">
    <h2>
        供應商基本資料</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%">
                <span style="color: red">*</span>統一編號：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="BAN" runat="server" MaxLength="8"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                <span style="color: red">*</span>名稱：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="NAME" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                地址：
            </th>
            <td colspan="3" class="tdleft">
                <asp:TextBox ID="ADDR" size="80" runat="server" MaxLength="200"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                電話：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="PHONE" runat="server" MaxLength="20"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                傳真：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="FAX" runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <th width="15%">
                聯絡人名稱：
            </th>
            <td width="35%" class="tdleft">
                <asp:TextBox ID="CONTACT_NAME" runat="server" MaxLength="10"></asp:TextBox>
            </td>
            <th width="15%" class="tdleft">
                聯絡人電子郵件：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="CONTACT_EMAIL" runat="server" MaxLength="50"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <center>
                <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" OnClick="btnOk_Click" />
                <input name="Reset" type="reset" class="btn" value="重填" />
                <asp:HiddenField ID="HiddenField1" runat="server" />
            </center>
        </td>
    </tr>
</table>
<center>
    <asp:Label ID="lblError" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
</center>
