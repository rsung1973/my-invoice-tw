<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SCM.Item.ExchagneInboundDetailsEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        入庫料品資訊
    </h2>
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
        DataKeyNames="EGI_DETAILS_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
        OnDataBinding="gvEntity_DataBinding" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" SortExpression="SALES_PROMOTION_SN">
                <ItemTemplate>
                    <%# loadItem((EXCHANGE_GOODS_INBOUND_DETAILS)Container.DataItem).PRODUCTS_BARCODE%>
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
            <asp:TemplateField HeaderText="入庫數量" SortExpression="PO_QUANTITY">
                <ItemTemplate>
                    <%# ((EXCHANGE_GOODS_INBOUND_DETAILS)Container.DataItem).GR_WW_QUANTITY %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<cc1:ExchangeInboundDetailsDataSource ID="dsEntity" runat="server">
</cc1:ExchangeInboundDetailsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />
