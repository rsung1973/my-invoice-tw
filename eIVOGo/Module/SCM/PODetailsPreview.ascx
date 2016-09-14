<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PODetailsPreview.ascx.cs" Inherits="eIVOGo.Module.SCM.PODetailsPreview" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="PO_DETAILS_SN" EnableViewState="false"
    DataSourceID="dsEntity" ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                <%# ((PURCHASE_ORDER_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER.SUPPLIER_PRODUCTS_NUMBER1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品Barcode" SortExpression="UID">
            <ItemTemplate>
                <%# ((PURCHASE_ORDER_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_BARCODE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# ((PURCHASE_ORDER_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER.PRODUCTS_DATA.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="PO_UNIT_PRICE">
            <ItemTemplate>
                <%# ((PURCHASE_ORDER_DETAILS)Container.DataItem).PO_UNIT_PRICE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量" SortExpression="PO_QUANTITY">
            <ItemTemplate>
                <%# ((PURCHASE_ORDER_DETAILS)Container.DataItem).PO_QUANTITY%>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:PurchaseOrderDetailsDataSource ID="dsEntity" runat="server">
</cc1:PurchaseOrderDetailsDataSource>