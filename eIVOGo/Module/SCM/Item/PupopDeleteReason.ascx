<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PupopDeleteReason.ascx.cs" Inherits="eIVOGo.Module.EIVO.PupopDeleteReason" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>

<asp:Button ID="btnHidden" runat="Server" Style="display: none" />
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd; border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <!--路徑名稱-->
        <div id="border_gray">
            <h2>刪除資訊</h2>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
              <tr>
                <th width="20%">刪除時間</th>
                <td class="tdleft">
    	            <asp:Label ID="lblDelTime" runat="server" Text='<%# _item.TimeToRefuse %>'></asp:Label>
                </td>
              </tr>
              <tr>
                <th width="20%">刪除者帳號</th>
                <td class="tdleft">
    	            <asp:Label ID="lblDelUser" runat="server" Text='<%# _item.DocumentProcessLog.UserProfile.UserName %>'></asp:Label>
                </td>
              </tr>
              <tr>
                <th width="20%">刪除原因</th>
                <td class="tdleft">
                    <asp:Label ID="Label1" runat="server" Text='<%# _item.Reason %>'></asp:Label>
                </td>
              </tr>
            </table>
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn">
                    <span class="table-title">
                        <asp:Button ID="CancelButton" CssClass="btn" runat="server" Text="關閉視窗" />
                    </span>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnHidden"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" CancelControlID="CancelButton"
    DropShadow="true" PopupDragHandleControlID="Panel3" />
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>