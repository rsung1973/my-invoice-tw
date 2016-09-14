<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="previewPurchaseOrder.ascx.cs" Inherits="eIVOGo.Module.SCM.previewPurchaseOrder" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="../UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="PODetailsList.ascx" TagName="PODetailsList" TagPrefix="uc4" %>
<%@ Register src="../Common/DataModelContainer.ascx" tagname="DataModelContainer" tagprefix="uc1" %>


<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc5:PageAction ID="PageAction1" runat="server" ItemName="首頁 > 採購單預覽" />
        <!--交易畫面標題-->
        <uc6:FunctionTitleBar ID="FunctionTitleBar1" runat="server" ItemName="採購單預覽" />
        <div id="border_gray">
            <h2>採購資訊</h2>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
              <tr>
                <th width="20%">採購單號</th>
                <td class="tdleft">
    	            <%# string.IsNullOrEmpty(_item.PURCHASE_ORDER_NUMBER)?"尚未開立":_item.PURCHASE_ORDER_NUMBER%>
                </td>
              </tr>
              <tr>
                <th width="20%">採購入庫倉儲</th>
                <td class="tdleft">
    	            <%# _item.WAREHOUSE!=null?_item.WAREHOUSE.WAREHOUSE_NAME : null%>
                </td>
              </tr>
              <tr>
                <th width="20%">結案方式</th>
                <td class="tdleft">
    	            <%# _item.PO_CLOSED_MODE== 0 ? "正常足量入庫結案" : "特許未足量入庫結案"%>
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
            <uc4:PODetailsList ID="PODetais" runat="server" />
        </div>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnReturn" CssClass="btn" Text="回上頁" runat="server" onclick="btnReturn_Click" />
            <asp:Button ID="btnCreatPO" CssClass="btn" Text="開立採購單" runat="server" onclick="btnCreatPO_Click" />
            <asp:Button ID="btnConfirm" CssClass="btn" Text="核准" Visible="false" runat="server" onclick="btnConfirm_Click" />
            <asp:Button ID="btnDeny" CssClass="btn" Text="退回" Visible="false" runat="server" onclick="btnDeny_Click" />
        </td>
    </tr>
</table>
<cc1:PurchaseDataSource ID="dsPurchase" runat="server">
</cc1:PurchaseDataSource>
<cc1:PurchaseDataSource ID="dsUpdate" runat="server" Isolated="true">
</cc1:PurchaseDataSource>
<uc1:DataModelContainer ID="DMContainer" runat="server" />
