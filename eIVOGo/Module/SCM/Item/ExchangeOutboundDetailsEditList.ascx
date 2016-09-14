<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeOutboundDetailsEditList.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.ExchangeOutboundDetailsEditList" %>
<%@ Register src="../../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="EGO_DETAILS_SN" EnableViewState="false"
    onrowcommand="gvEntity_RowCommand" 
    ondatabinding="gvEntity_DataBinding" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" 
            SortExpression="PO_DETAILS_SN">
            <ItemTemplate>
                   <%# loadItem((EXCHANGE_GOODS_OUTBOND_DETAILS)Container.DataItem).PRODUCTS_BARCODE %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# _currentItem.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="BO_UNIT_PRICE">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("BO_UNIT_PRICE{0}",Container.DataItemIndex) %>' value='<%# Eval("BO_UNIT_PRICE") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="出貨數量" SortExpression="GR_BS_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("GR_BS_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("GR_BS_QUANTITY") %>' />
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





