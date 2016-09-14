<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RefuseDocument.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.RefuseDocument" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Utility" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
--%>                <!--路徑名稱-->
                <!--交易畫面標題-->
                <!--按鈕-->
                <div class="border_gray">
                    <!--表格 開始-->
                    <h2>
                        刪除文件</h2>
                    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
                        <tbody>
                            <tr>
                                <th width="20%">
                                    刪除原因
                                </th>
                                <td class="tdleft">
                                    <asp:TextBox ID="Reason" runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
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
                                 <asp:Button ID="btnConfirm" runat="server" Text="確定" OnClick="btnConfirm_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
                            </span>
                        </td>
                    </tr>
                </table>
<%--            </ContentTemplate>
        </asp:UpdatePanel>
--%>    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="Panel3" />
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<cc1:DocumentDataSource ID="dsUpdate" runat="server"></cc1:DocumentDataSource>
