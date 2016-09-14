<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BODetailsEditList.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.BODetailsEditList" %>
<%@ Register src="../../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="BO_DETAILS_SN" EnableViewState="false"
    onrowcommand="gvEntity_RowCommand" 
    ondatabinding="gvEntity_DataBinding" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                   <%# loadItem((BUYER_ORDERS_DETAILS)Container.DataItem).PRODUCTS_BARCODE %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# _currentItem.PRODUCTS_NAME%>
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
                <input type="text" name='<%# String.Format("BO_UNIT_PRICE{0}",Container.DataItemIndex) %>' value='<%# Eval("BO_UNIT_PRICE") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="數量" SortExpression="PO_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("BO_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("BO_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="刪除" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?",Container.DataItemIndex.ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />





