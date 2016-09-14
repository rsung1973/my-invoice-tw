<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductQuery.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.ProductQuery" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="ProductsDataSelector.ascx" TagName="ProductsDataSelector" TagPrefix="uc1" %>
<asp:Button ID="btnQuery" runat="server" Text="查詢選擇" OnClick="btnQuery_Click" />
<asp:Button ID="btnPopup" runat="server" Text="Button" Style="display: none" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; background-color: #ffffdd;
    border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD;
        border: solid 1px Gray; color: Black">
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <!--路徑名稱-->
        <!--交易畫面標題-->
        <!--按鈕-->
        <div class="border_gray">
            <table class="table01">
                <tr>
                    <td>
                        <uc1:ProductsDataSelector ID="productItem" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnConfirm" runat="server" onclick="btnConfirm_Click" 
                            Text="確定" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="取消" />
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
<cc1:ProductsDataSource ID="dsProd" runat="server">
</cc1:ProductsDataSource>
