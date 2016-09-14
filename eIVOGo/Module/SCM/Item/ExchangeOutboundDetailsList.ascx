<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.Item.ExchangeOutboundDetailsEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        換貨出貨明細</h2>
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
        DataKeyNames="EGO_DETAILS_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
        OnDataBinding="gvEntity_DataBinding" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" SortExpression="PO_DETAILS_SN">
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
                    <%# String.Format("{0:##,###,###,###,###}", Eval("BO_UNIT_PRICE")) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="出貨數量" SortExpression="GR_BS_QUANTITY">
                <ItemTemplate>
                    <%# Eval("GR_BS_QUANTITY") %>
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
