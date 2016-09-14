<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupplierProductsList.ascx.cs"
    Inherits="eIVOGo.Module.SCM.SupplierProductsList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/SupplierProductsItem.ascx" TagName="SupplierProductsItem"
    TagPrefix="uc2" %>
<%@ Register Src="../Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc3" %>
<asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    DataKeyNames="SUPPLIER_SN,PRODUCTS_SN" DataSourceID="dsEntity" EnableViewState="False"
    OnRowCommand="gvEntity_RowCommand" ShowFooter="True">
    <Columns>
        <asp:TemplateField HeaderText="供應商" SortExpression="SUPPLIER_SN">
            <ItemTemplate>
                <%# ((SUPPLIER_PRODUCTS_NUMBER)Container.DataItem).SUPPLIER.SUPPLIER_NAME  %>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="SUPPLIER_PRODUCTS_NUMBER1" HeaderText="貨號" SortExpression="SUPPLIER_PRODUCTS_NUMBER" />
        <asp:TemplateField ShowHeader="False">
            <FooterTemplate>
                <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                    Text="新增" OnClientClick='<%# doCreate.GetPostBackEventReference(null) %>' />
            </FooterTemplate>
            <ItemTemplate>
                <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                    Text="修改" CommandArgument='<%# String.Format("{0},{1}",Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1])) %>'
                    OnClientClick='<%# doEdit.GetPostBackEventReference(String.Format("{0},{1}",Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1]))) %>' />
                &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                    Text="刪除" CommandArgument='<%# String.Format("{0},{1}",Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1])) %>'
                    OnClientClick='<%# doDelete.GetConfirmedPostBackEventReference("確認刪除此筆資料?", String.Format("{0},{1}",Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1]))) %>' />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <font color="red">供貨商資料未建立!!</font>
        <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
            Text="新增" />
    </EmptyDataTemplate>
    <PagerTemplate>
        <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
    </PagerTemplate>
</asp:GridView>
<cc1:SupplierProductsNumberDataSource ID="dsEntity" runat="server">
</cc1:SupplierProductsNumberDataSource>
<uc2:SupplierProductsItem ID="editItem" runat="server" />
<uc3:ActionHandler ID="doDelete" runat="server" />
<uc3:ActionHandler ID="doEdit" runat="server" />
<uc3:ActionHandler ID="doCreate" runat="server" />
