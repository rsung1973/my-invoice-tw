<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMemberMenuPopupModal.ascx.cs"
    Inherits="eIVOGo.Module.SYS.EditMemberMenuPopupModal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="MenuFactory.ascx" TagName="MenuFactory" TagPrefix="uc3" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
        <!--按鈕-->
        <div class="border_gray">
            <uc3:MenuFactory ID="menuFactory" runat="server" />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td class="Bargain_btn" align="center">
                        <asp:Button ID="btnConfirm" runat="server" CausesValidation="false" CssClass="btn"
                            Text="確定" OnClick="btnConfirm_Click" />
                        &nbsp;
                        <asp:Button ID="btnReset" runat="server" CausesValidation="false" CssClass="btn"
                            Text="取消" OnClick="btnReset_Click" />
                        &nbsp; &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true"
    PopupDragHandleControlID="Panel3" />
<cc1:OrganizationCategoryUserRoleDataSource ID="dsRole" runat="server">
</cc1:OrganizationCategoryUserRoleDataSource>
