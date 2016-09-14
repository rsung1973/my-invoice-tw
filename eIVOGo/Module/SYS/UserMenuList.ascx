<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMenuList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.UserMenuList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/UserMenuItem.ascx" TagName="UserMenuItem" TagPrefix="uc2" %>
<%@ Import Namespace="Model.DataEntity" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="RoleID,CategoryID,MenuID" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="組織單位類別" SortExpression="CategoryID">
                <ItemTemplate>
                    <%# ((UserMenu)Container.DataItem).CategoryDefinition.Category %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="使用者角色" SortExpression="RoleID">
                <ItemTemplate>
                    <%# ((UserMenu)Container.DataItem).UserRoleDefinition.Role
                    %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="工作選單" SortExpression="MenuID">
                <ItemTemplate>
                    <%# ((UserMenu)Container.DataItem).MenuControl.SiteMenu %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0},{1},{2}", Eval("RoleID"),Eval("CategoryID"),Eval("MenuID")) %>'
                        OnClientClick='return confirm("確認刪除此筆資料?");' />
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
<cc1:UserMenuDataSource ID="dsEntity" runat="server">
</cc1:UserMenuDataSource>
<uc2:UserMenuItem ID="editItem" runat="server" />
