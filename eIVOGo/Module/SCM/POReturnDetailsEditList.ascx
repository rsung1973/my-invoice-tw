<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POReturnDetailsEditList.ascx.cs" Inherits="eIVOGo.Module.SCM.POReturnDetailsEditList" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<%@ Import Namespace="Model.Locale" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="POR_DETAILS_SN" EnableViewState="false"
    ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <input id="chkItem" name="chkItem" type="checkbox" value='<%# Container.DataItemIndex %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER1 %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品Barcode" SortExpression="UID">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_BARCODE %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# loadItem1((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_DATA.PRODUCTS_NAME %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="ItemDate">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("POR_UNIT_PRICE{0}",Container.DataItemIndex) %>' value='<%# Eval("POR_UNIT_PRICE") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="庫存數量" SortExpression="ItemDate">
            <ItemTemplate>
                <%#  loadItem2((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_TOTAL_AMOUNT %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="退貨數量" SortExpression="POR_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("POR_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("POR_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="瑕疵數量" SortExpression="POR_DEFECTIVE_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("POR_DEFECTIVE_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("POR_DEFECTIVE_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>

