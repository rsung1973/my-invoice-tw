<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopupModalMessage.ascx.cs"
    Inherits="eIVOGo.Module.UI.PopupModalMessage" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc1" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc1:FunctionTitleBar ID="titleBar" runat="server" ItemName="請填入交易事項" />
                <!--按鈕-->
                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="display:none;">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                <asp:Button ID="btnOK" runat="server" Text="確定" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" Text="取消" />
                            </span>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="btnCancel"
    DropShadow="true" PopupDragHandleControlID="Panel3" />