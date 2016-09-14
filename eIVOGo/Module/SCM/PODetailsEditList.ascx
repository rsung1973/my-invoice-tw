<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PODetailsEditList.ascx.cs" Inherits="eIVOGo.Module.SCM.PODetailsEditList" %>
<%@ Register src="../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="PO_DETAILS_SN" EnableViewState="false"
    onrowcommand="gvEntity_RowCommand" 
    ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                <%# loadItem((PURCHASE_ORDER_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER1%>
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
                <input type="text" name='<%# String.Format("PO_UNIT_PRICE{0}",Container.DataItemIndex) %>' value='<%# Eval("PO_UNIT_PRICE") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量" SortExpression="PO_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("PO_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("PO_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="刪除" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?",Eval(gvEntity.DataKeyNames[0]).ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />





