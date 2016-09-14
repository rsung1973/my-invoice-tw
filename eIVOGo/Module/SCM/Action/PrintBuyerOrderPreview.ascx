<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintBuyerOrderPreview.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Action.PrintBuyerOrderPreview" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Utility" %>
<%@ Register src="../../Common/PrintingButton3.ascx" tagname="PrintingButton3" tagprefix="uc1" %>
<%@ Register src="../View/BuyerOrderPreview.ascx" tagname="BuyerOrderPreview" tagprefix="uc2" %>
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
<%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
--%>                <!--路徑名稱-->
                <!--交易畫面標題-->
                <uc2:BuyerOrderPreview ID="orderPreview" runat="server" />
                <!--按鈕-->
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="Bargain_btn" align="center">
                            <span class="table-title">
                                 <uc1:PrintingButton3 ID="btnPrint" runat="server" />
                            &nbsp;&nbsp;&nbsp;&nbsp;
                               <asp:Button ID="btnCancel" runat="server" Text="關閉" OnClick="btnCancel_Click" />
                            </span>
                        </td>
                    </tr>
                </table>
<%--            </ContentTemplate>
        </asp:UpdatePanel>
--%>    </asp:Panel>
</asp:Panel>
<asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="btnPopup" CancelControlID="btnCancel"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="true" PopupDragHandleControlID="Panel3" />
