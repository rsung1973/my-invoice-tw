<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.Item.BODetailsEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        料品資訊</h2>
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
        DataKeyNames="BO_DETAILS_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
        OnDataBinding="gvEntity_DataBinding" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" SortExpression="PO_DETAILS_SN">
                <ItemTemplate>
                    <%# loadItem((BUYER_ORDERS_DETAILS)Container.DataItem).PRODUCTS_BARCODE %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_NAME%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="促銷專案代號" SortExpression="UID">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS_DETAILS)Container.DataItem).SALES_PROMOTION_PRODUCTS != null ? ((BUYER_ORDERS_DETAILS)Container.DataItem).SALES_PROMOTION_PRODUCTS.SALES_PROMOTION.SALES_PROMOTION_SYMBOL : "N/A"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="促銷專案名稱" SortExpression="UID">
                <ItemTemplate>
                    <%# ((BUYER_ORDERS_DETAILS)Container.DataItem).SALES_PROMOTION_PRODUCTS != null ? ((BUYER_ORDERS_DETAILS)Container.DataItem).SALES_PROMOTION_PRODUCTS.SALES_PROMOTION.SALES_PROMOTION_NAME : "N/A"%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="單價" SortExpression="PO_UNIT_PRICE">
                <ItemTemplate>
                    <%# Eval("BO_UNIT_PRICE") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出貨數量" SortExpression="PO_QUANTITY">
                <ItemTemplate>
                    <%# Eval("BO_QUANTITY") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />
