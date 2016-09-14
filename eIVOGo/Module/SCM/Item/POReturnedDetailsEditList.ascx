<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="POReturnedDetailsEditList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.POReturnedDetailsEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<div class="border_gray">
    <h2>
        退貨料品資訊
    </h2>
    <asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
        DataKeyNames="POR_DETAILS_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
        OnDataBinding="gvEntity_DataBinding" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="供應商貨號" InsertVisible="False" SortExpression="">
                <ItemTemplate>
                    <%# getSupplierNO((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).SUPPLIER_PRODUCTS_NUMBER1%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品Barcode" SortExpression="ItemDate">
                <ItemTemplate>
                    <%# _currentItem.PRODUCTS_BARCODE%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
                <ItemTemplate>
                    <%# _currentItem.PRODUCTS_NAME%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="單價" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,###}", ((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).POR_UNIT_PRICE)%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="庫存數量" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,###}",getProductWarehouseMapping((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).PRODUCTS_TOTAL_AMOUNT)%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="退貨數量" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,###}", ((PURCHASE_ORDER_RETURNED_DETAILS)Container.DataItem).POR_QUANTITY)%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="瑕疵數量" SortExpression="">
                <ItemTemplate>
                    <%# String.Format("{0:##,###,###,###,###}", _mappingItem.PRODUCTS_DEFECTIVE_AMOUNT)%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
<cc1:PurchaseOrderReturnDetailDataSource ID="dsEntity" runat="server">
</cc1:PurchaseOrderReturnDetailDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />
