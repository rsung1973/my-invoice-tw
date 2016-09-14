<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.POReturnDetailsEditList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem" TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>

<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="POR_DETAILS_SN" EnableViewState="false"
    ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER1%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品Barcode" SortExpression="UID">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_BARCODE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="ItemDate">
            <ItemTemplate>
                <%# Eval("POR_UNIT_PRICE") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="庫存數量" SortExpression="ItemDate">
            <ItemTemplate>
                <%#  loadItem2((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_TOTAL_AMOUNT %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="退貨數量" SortExpression="POR_QUANTITY">
            <ItemTemplate>
                <%# Eval("POR_QUANTITY") %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="瑕疵數量" SortExpression="POR_DEFECTIVE_QUANTITY">
            <ItemTemplate>
                <%# Eval("POR_DEFECTIVE_QUANTITY") %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
