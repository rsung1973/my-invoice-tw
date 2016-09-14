<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.Item.ReturnedGoodsDetailsEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        料品資訊
    </h2>
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
        DataKeyNames="GR_DETAILS_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
        OnDataBinding="gvEntity_DataBinding" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" SortExpression="">
                <ItemTemplate>
                    <%# loadItem((GOODS_RETURNED_DETAILS)Container.DataItem).PRODUCTS_BARCODE%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
                <ItemTemplate>
                    <%# _currentItem.PRODUCTS_NAME%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="單價" SortExpression="BUYER_PRICE">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}",_currentItem.BUY_PRICE ) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="數量" SortExpression="PO_QUANTITY">
                <ItemTemplate>
                    <%# ((GOODS_RETURNED_DETAILS)Container.DataItem).GR_QUANTITY%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="退貨數量" SortExpression="GR_QUANTITY">
                <ItemTemplate>
                    <%# Eval("GR_QUANTITY") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="訂價" SortExpression="SELL_PRICE">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}",_currentItem.SELL_PRICE ) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="售價" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,##0}", ((GOODS_RETURNED_DETAILS)Container.DataItem).BUYER_ORDERS_DETAILS.BO_UNIT_PRICE)%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<cc1:ReturnedGoodsDetailsDataSource ID="dsEntity" runat="server">
</cc1:ReturnedGoodsDetailsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />