<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserRoleList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UserRoleList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/UserRoleItem.ascx" TagName="UserRoleItem" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="UID,RoleID,OrgaCateID" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="使用者ID" SortExpression="UID">
                <ItemTemplate>
                    <%# String.Format("{0}({1})", ((UserRole)Container.DataItem).UserProfile.PID,((UserRole)Container.DataItem).UserProfile.UserName) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用者角色" SortExpression="RoleID">
                <ItemTemplate>
                    <%# ((UserRole)Container.DataItem).UserRoleDefinition.Role  %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="所屬公司類別" SortExpression="OrgaCateID">
                <ItemTemplate>
                    <%# String.Format("{0}({1})",((UserRole)Container.DataItem).OrganizationCategory.Organization.CompanyName,((UserRole)Container.DataItem).OrganizationCategory.CategoryDefinition.Category) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0},{1},{2}", Eval("UID"),Eval("RoleID"),Eval("OrgaCateID")) %>'
                        OnClientClick='return confirm("確認刪除此筆資料?");' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">角色尚未設定!!</font>
            <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                Text="新增" />
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:UserRoleDataSource ID="dsEntity" runat="server">
</cc1:UserRoleDataSource>
<uc2:UserRoleItem ID="editItem" runat="server" />
