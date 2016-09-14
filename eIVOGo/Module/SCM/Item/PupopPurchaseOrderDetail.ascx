<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PupopPurchaseOrderDetail.ascx.cs" Inherits="eIVOGo.Module.EIVO.PupopPurchaseOrderDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../PODetailsPreview.ascx" TagName="PODetailsPreview" TagPrefix="uc3" %>
<%@ Register src="../../Common/PrintingButton2.ascx" tagname="PrintingButton2" tagprefix="uc4" %>

<asp:Button ID="btnHidden" runat="Server" Style="display: none" />
<asp:HiddenField ID="HiddenField1" runat="server" />
<asp:Panel ID="Panel1" runat="server" Style="display: none; width: 650px; background-color: #ffffdd; border-width: 3px; border-style: solid; border-color: Gray; padding: 3px;">
    <asp:Panel ID="Panel3" runat="server" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <!--路徑名稱-->
        <div id="printArea" runat="server">
        <div id="border_gray">
            <h2>採購資訊</h2>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
                <tr>
                <th width="20%">採購單號</th>
                <td class="tdleft">
    	            <asp:Label ID="lblPONO" runat="server"></asp:Label>
                </td>
                </tr>
                <tr>
                <th width="20%">採購入庫倉儲</th>
                <td class="tdleft">
    	            <asp:Label ID="lblWardhouse" runat="server"></asp:Label>
                </td>
                </tr>
                <tr>
                <th width="20%">結案方式</th>
                <td class="tdleft">
    	            <asp:Label ID="lblCloseType" runat="server"></asp:Label>
                </td>
                </tr>
            </table>
        </div>
        <div class="border_gray">
            <!--表格 開始-->
            <h2>供應商</h2>
            <asp:GridView ID="gvSupplier" runat="server" Width="100%" CellPadding="0"
            EnableViewState="False" GridLines="None" AutoGenerateColumns="False" 
            CssClass="table01">
            <Columns>
                <asp:TemplateField HeaderText="供應商名稱">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_NAME %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="統編">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_BAN%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="地址">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_ADDRESS%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="電話">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_PHONE%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="傳真">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_FAX%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="聯絡人">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).SUPPLIER_CONTACT_NAME%></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="聯絡人Email">
                    <ItemTemplate>
                        <%# ((SUPPLIER)Container.DataItem).CONTACT_EMAIL%></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="OldLace" />
        </asp:GridView>
        </div>
        <div class="border_gray">
            <h2>料品資訊</h2>
            <uc3:PODetailsPreview ID="PODetailsPreview" runat="server" />
        </div>
        </div>
        <!--按鈕-->
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="Bargain_btn">
                    <span class="table-title">
                        <uc4:PrintingButton2 ID="PrintingButton21" runat="server" />
                        <asp:Button ID="btnExport" CssClass="btn" runat="server" Text="匯出供應商採購單" 
                        onclick="btnExport_Click" />
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

<cc1:PurchaseDataSource ID="dsPurchase" runat="server">
</cc1:PurchaseDataSource>
<cc1:SupplierDataSource ID="dsSupplier" runat="server">
</cc1:SupplierDataSource>