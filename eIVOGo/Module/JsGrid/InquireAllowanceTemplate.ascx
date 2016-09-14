<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireAllowanceTemplate.ascx.cs" Inherits="eIVOGo.Module.JsGrid.InquireAllowanceTemplate" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagPrefix="uc1" TagName="FunctionTitleBar" %>

<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
        </asp:PlaceHolder>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<uc1:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
