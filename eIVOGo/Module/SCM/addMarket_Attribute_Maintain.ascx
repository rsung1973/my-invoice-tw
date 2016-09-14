<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="addMarket_Attribute_Maintain.ascx.cs"
    Inherits="eIVOGo.Module.SCM.addMarket_Attribute_Maintain" %>
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
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 新增網購通路平台屬性" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="titleBar" runat="server" ItemName="新增網購通路平台屬性" />
<div id="border_gray">
    <h2>
        網購通路平台屬性基本資料</h2>
    <table width="100%" border="0" cellpadding="0" cellspacing="0" id="left_title">
        <tr>
            <th width="15%">
                <span style="color: red">*</span>網購通路來源名稱：
            </th>
            <td width="35%" class="tdleft">
                <asp:DropDownList ID="ddlMarket" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th width="15%">
                <span style="color: red">*</span>網購通路平台屬性名稱：
            </th>
            <td class="tdleft">
                <asp:TextBox ID="Name" runat="server"></asp:TextBox>
            </td>
        </tr>
    </table>
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnOk" class="btn" runat="server" Text="確定" OnClick="btnOk_Click" />&nbsp;&nbsp;
            <input id="Reset1" type="reset" value="重填" class="btn" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
        </td>
    </tr>
</table>
<div>
    <center>
        <asp:Label ID="lblError" ForeColor="Red" Font-Size="Larger" runat="server"></asp:Label>
    </center>
</div>
