<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrganizationCategoryUserRoleList.ascx.cs"
    Inherits="eIVOGo.Module.SYS.OrganizationCategoryUserRoleList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/OrganizationCategoryUserRoleItem.ascx" TagName="OrganizationCategoryUserRoleItem"
    TagPrefix="uc2" %>
<%@ Register src="EditMemberMenuPopupModal.ascx" tagname="EditMemberMenuPopupModal" tagprefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="OrgaCateID,RoleID" DataSourceID="dsEntity" EnableViewState="False"
        OnRowCommand="gvEntity_RowCommand" ShowFooter="True">
        <Columns>
            <asp:TemplateField HeaderText="角色名稱" SortExpression="RoleID">
                <ItemTemplate>
                    <%# ((OrganizationCategoryUserRole)Container.DataItem).UserRoleDefinition.Role  %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnAuth" runat="server" CausesValidation="False" CommandName="EditMenu"
                        Text="設定選單" CommandArgument='<%# String.Format("{0},{1}", Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1])) %>' />
                    &nbsp;
                    <asp:Button ID="btnModify" runat="server" CausesValidation="False" CommandName="Modify"
                        Text="修改" CommandArgument='<%# String.Format("{0},{1}", Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1])) %>' />
                    &nbsp;<asp:Button ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                        Text="刪除" CommandArgument='<%# String.Format("{0},{1}", Eval(gvEntity.DataKeyNames[0]),Eval(gvEntity.DataKeyNames[1])) %>'
                        OnClientClick='return confirm("確認刪除此筆資料?");' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <font color="red">查無資料!!</font><asp:Button ID="btnCreate" runat="server" CausesValidation="False"
                CommandName="Create" Text="新增" />
        </EmptyDataTemplate>
        <PagerTemplate>
            <uc1:PagingControl ID="pagingList" runat="server" OnPageIndexChanged="PageIndexChanged" />
        </PagerTemplate>
    </asp:GridView>
</div>
<cc1:OrganizationCategoryUserRoleDataSource ID="dsEntity" runat="server">
</cc1:OrganizationCategoryUserRoleDataSource>
<uc2:OrganizationCategoryUserRoleItem ID="editItem" runat="server" />
<uc3:EditMemberMenuPopupModal ID="editMenu" runat="server" />

