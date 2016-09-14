<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsAttributeNameList.ascx.cs"
    Inherits="eIVOGo.Module.SCM_SYS.ProductsAttributeNameList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.SCMDataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/ProductsAttributeNameItem.ascx" TagName="ProductsAttributeNameItem"
    TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="PRODUCTS_ATTR_NAME_SN" DataSourceID="dsEntity" 
        EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="PRODUCTS_ATTR_NAME" HeaderText="屬性名稱" 
                SortExpression="PRODUCTS_ATTR_NAME" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">查無資料!!</font>
            <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                Text="新增" />
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:ProductsAttributeNameDataSource ID="dsEntity" runat="server">
</cc1:ProductsAttributeNameDataSource>
<uc2:ProductsAttributeNameItem ID="editItem" runat="server" />
