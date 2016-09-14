<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsDataList.ascx.cs" Inherits="eIVOGo.Module.SCM.ProductsDataList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem" TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="PRODUCTS_SN" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand">
        <Columns>
            <asp:BoundField DataField="PRODUCTS_NAME" HeaderText="料品名稱" 
                SortExpression="PRODUCTS_NAME" />
            <asp:BoundField DataField="PRODUCTS_BARCODE" HeaderText="料品Barcode" 
                SortExpression="PRODUCTS_BARCODE" />
            <asp:BoundField DataField="SELL_PRICE" HeaderText="進價" 
                SortExpression="SELL_PRICE" />
            <asp:BoundField DataField="BUY_PRICE" HeaderText="售價" 
                SortExpression="BUY_PRICE" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# String.Format("{0}",Eval(gvEntity.DataKeyNames[0])) %>'
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
<cc1:ProductsDataSource ID="dsEntity" runat="server">
</cc1:ProductsDataSource>
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
