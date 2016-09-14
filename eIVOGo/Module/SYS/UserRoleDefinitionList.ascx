<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleDefinitionList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UserRoleDefinitionList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/UserRoleDefinitionItem.ascx" TagName="UserRoleDefinitionItem"
    TagPrefix="uc2" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="RoleID" DataSourceID="dsRole" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="RoleID" HeaderText="RoleID" ReadOnly="True" SortExpression="RoleID" />
            <asp:BoundField DataField="Role" HeaderText="角色名稱" SortExpression="Role" />
            <asp:BoundField DataField="SiteMenu" HeaderText="工作選單" SortExpression="SiteMenu" />
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# Eval("RoleID") %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# Eval("RoleID") %>' OnClientClick='return confirm("確認刪除此筆資料?");' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">查無資料!!</font>
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:UserRoleDefinitionDataSource ID="dsRole" runat="server">
</cc1:UserRoleDefinitionDataSource>
<uc2:UserRoleDefinitionItem ID="userRoleItem" runat="server" />
