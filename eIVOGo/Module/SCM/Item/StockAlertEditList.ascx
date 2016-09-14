<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockAlertEditList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.Item.StockAlertEditList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc2" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<asp:GridView ID="gvEntity" runat="server" CssClass="table01" AutoGenerateColumns="False"
    DataKeyNames="WAREHOUSE_SN" EnableViewState="false" OnRowCommand="gvEntity_RowCommand"
    OnDataBinding="gvEntity_DataBinding" Width="100%">
    <Columns>
        <asp:TemplateField HeaderText="料品Barcode" InsertVisible="False" SortExpression="WAREHOUSE_SN">
            <ItemTemplate>
                <%# loadItem((PRODUCTS_WAREHOUSE_MAPPING)Container.DataItem).PRODUCTS_BARCODE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="料品名稱" SortExpression="ItemDate">
            <ItemTemplate>
                <%# _currentItem.PRODUCTS_NAME%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="單價" SortExpression="ItemDate">
            <ItemTemplate>
                <%# _currentItem.SELL_PRICE%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="實際庫存總量" SortExpression="ItemDate">
            <ItemTemplate>
                <%# ((PRODUCTS_WAREHOUSE_MAPPING)Container.DataItem).PRODUCTS_TOTAL_AMOUNT %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="計畫庫存總量" SortExpression="PO_UNIT_PRICE">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("PRODUCTS_PLAN_AMOUNT{0}",Container.DataItemIndex) %>'
                    value='<%# Eval("PRODUCTS_PLAN_AMOUNT") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="安全庫存總量百分比" SortExpression="PO_QUANTITY">
            <ItemTemplate>
                <input type="text" name='<%# String.Format("PRODUCTS_SAFE_AMOUNT_PERCENTAGE{0}",Container.DataItemIndex) %>'
                    value='<%# Eval("PRODUCTS_SAFE_AMOUNT_PERCENTAGE") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False" Visible="false">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                    Text="刪除" CommandArgument='<%# Container.DataItemIndex %>' OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?",Container.DataItemIndex.ToString()) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<cc1:PW_MappingDataSource ID="dsEntity" runat="server">
</cc1:PW_MappingDataSource>
<uc2:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
