<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.PODetailsEditList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem" TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>

<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="PO_DETAILS_SN" EnableViewState="false"
    onrowcommand="gvEntity_RowCommand" 
    ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                <%# loadItem((PURCHASE_ORDER_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品Barcode" SortExpression="UID">
            <ItemTemplate>
                <%# loadItem((PURCHASE_ORDER_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_BARCODE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# loadItem((PURCHASE_ORDER_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="PO_UNIT_PRICE">
            <ItemTemplate>
                <%# Eval("PO_UNIT_PRICE") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量" SortExpression="PO_QUANTITY">
            <ItemTemplate>
                <%# Eval("PO_QUANTITY") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
