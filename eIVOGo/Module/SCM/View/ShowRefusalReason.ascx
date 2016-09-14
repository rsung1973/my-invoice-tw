<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowRefusalReason.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ShowRefusalReason" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Utility" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!--路徑名稱-->
                <!--交易畫面標題-->
                <!--按鈕-->
                <div class="border_gray">
                    <!--表格 開始-->
                    <h2>
                        刪除資訊</h2>
                    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tbody>
                            <tr>
                                <th width="20%">
                                    刪除時間
                                </th>
                                <td class="tdleft">
                                    <%# ValueValidity.ConvertChineseDateTimeString(_item.StepDate) %>
                                </td>
                            </tr>
                            <tr>
                                <th width="20%">
                                    刪除者帳號
                                </th>
                                <td class="tdleft">
                                    <%# _item.UID.HasValue ? _item.UserProfile.UserName : ""%>
                                </td>
                            </tr>
                            <tr>
                                <th width="20%">
                                    </SPAN>刪除原因
                                </th>
                                <td class="tdleft">
                                    <%# _item.DocumentReasonForRefusal.Reason %>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <!--表格 結束-->
                </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                <asp:Button ID="btnCancel" runat="server" Text="關閉視窗" OnClick="btnCancel_Click" />
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
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>

