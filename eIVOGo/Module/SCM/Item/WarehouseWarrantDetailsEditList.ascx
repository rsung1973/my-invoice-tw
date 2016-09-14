<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WarehouseWarrantDetailsEditList.ascx.cs" Inherits="eIVOGo.Module.SCM.Item.WarehouseWarrantDetailsEditList" %>
<%@ Register src="../../Common/PagingControl.ascx" tagname="PagingControl" tagprefix="uc1" %>
<%@ Register assembly="Model" namespace="Model.SCMDataEntity" tagprefix="cc1" %>
<%@ Register src="../../Common/ActionHandler.ascx" tagname="ActionHandler" tagprefix="uc2" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" 
    AutoGenerateColumns="False" DataKeyNames="WW_DETAILS_SN" EnableViewState="false"
    onrowcommand="gvEntity_RowCommand" 
    ondatabinding="gvEntity_DataBinding">
    <Columns>
        <asp:TemplateField HeaderText="轉入單據號碼" SortExpression="">
            <ItemTemplate>
                <%# ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PO_DETAILS_SN.HasValue ? ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PURCHASE_ORDER_DETAILS.PURCHASE_ORDER.PURCHASE_ORDER_NUMBER
                        : ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).GR_DETAILS_SN.HasValue ? ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).GOODS_RETURNED_DETAILS.GOODS_RETURNED.GOODS_RETURNED_NUMBER
                                            : ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).EGI_DETAILS_SN.HasValue ? ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).EXCHANGE_GOODS_INBOUND_DETAILS.EXCHANGE_GOODS.EXCHANGE_GOODS_NUMBER
                                            : "N/A"
                    %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="供應商貨號" SortExpression="">
            <ItemTemplate>
                <%# loadItem((WAREHOUSE_WARRANT_DETAILS)Container.DataItem)!=null?_supplierItem.SUPPLIER_PRODUCTS_NUMBER1:""%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品Barcode" SortExpression="UID">
            <ItemTemplate>
                <%# _currentItem!=null ? _currentItem.PRODUCTS_BARCODE:""%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# _currentItem!=null ? _currentItem.PRODUCTS_BARCODE:_currentItem.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="ItemDate">
            <ItemTemplate>
                <%# ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PO_DETAILS_SN.HasValue ? String.Format("{0:##,###,###,###,###}",((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PURCHASE_ORDER_DETAILS.PO_UNIT_PRICE) 
                        : ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).GR_DETAILS_SN.HasValue ? String.Format("{0:##,###,###,###}",((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).GOODS_RETURNED_DETAILS.BUYER_ORDERS_DETAILS.BO_UNIT_PRICE)
                                            : ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).EGI_DETAILS_SN.HasValue ? String.Format("{0:##,###,###,###}",((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).EXCHANGE_GOODS_INBOUND_DETAILS.BO_UNIT_PRICE)
                                            : "N/A"
                    %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="採購數量" SortExpression="ItemDate">
            <ItemTemplate>
                <%# ((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PO_DETAILS_SN.HasValue ? String.Format("{0:##,###,###,###,###}",((WAREHOUSE_WARRANT_DETAILS)Container.DataItem).PURCHASE_ORDER_DETAILS.PO_QUANTITY) 
                                            : "N/A"
                    %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="入庫數量" SortExpression="RECEIPT_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("RECEIPT_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("RECEIPT_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="瑕疵數量" SortExpression="WW_DEFECTIVE_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("WW_DEFECTIVE_QUANTITY{0}",Container.DataItemIndex) %>' value='<%# Eval("WW_DEFECTIVE_QUANTITY") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" 
                    CommandName="Delete" Text="刪除" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?",Eval(gvEntity.DataKeyNames[0]).ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:WarehouseWarrantDetailsDataSource ID="dsEntity" runat="server">
</cc1:WarehouseWarrantDetailsDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />





