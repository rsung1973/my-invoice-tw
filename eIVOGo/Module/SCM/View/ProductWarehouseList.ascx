<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductWarehouseList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.View.ProductWarehouseList" %>
<%@ Register Src="../../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="../../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.SCMDataEntity" %>
<%@ Register Src="../../Common/PageAnchor.ascx" TagName="PageAnchor" TagPrefix="uc4" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="PW_MAPPING_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="倉儲" SortExpression="WAREHOUSE_SN">
                <ItemTemplate>
                    <%# ((PRODUCTS_WAREHOUSE_MAPPING)Container.DataItem).WAREHOUSE.WAREHOUSE_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品Barcode" SortExpression="PRODUCTS_SN">
                <ItemTemplate>
                    <%# ((PRODUCTS_WAREHOUSE_MAPPING)Container.DataItem).PRODUCTS_DATA.PRODUCTS_BARCODE %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="料品名稱" SortExpression="PRODUCTS_NAME">
                <ItemTemplate>
                    <%# ((PRODUCTS_WAREHOUSE_MAPPING)Container.DataItem).PRODUCTS_DATA.PRODUCTS_NAME %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PRODUCTS_TOTAL_AMOUNT" HeaderText="實際庫存總量" SortExpression="PRODUCTS_TOTAL_AMOUNT" />
            <asp:BoundField DataField="PRODUCTS_PLAN_AMOUNT" HeaderText="計畫庫存總量" SortExpression="PRODUCTS_PLAN_AMOUNT" />
            <asp:BoundField DataField="PRODUCTS_SAFE_AMOUNT_PERCENTAGE" HeaderText="安全庫存總量百分比"
                SortExpression="PRODUCTS_SAFE_AMOUNT_PERCENTAGE" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="編輯" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
                        OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?", String.Format("{0}",Eval(gvEntity.DataKeyNames[0]))) %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <center><font color="red">查無資料!!</font></center>
        </EmptyDataTemplate>
        <EmptyDataRowStyle CssClass="noborder" />
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:PW_MappingDataSource ID="dsEntity" runat="server">
</cc1:PW_MappingDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
<uc4:PageAnchor ID="ToProductWarehouseList" runat="server" 
    TransferTo="~/SCM_SYSTEM/addProducts_Plan_Maintain.aspx" />
