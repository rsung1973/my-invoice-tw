<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.SYS.UserProfileList" %>
<%@ Register Src="../Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc1" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="Item/UserProfileItem.ascx" TagName="UserProfileItem" TagPrefix="uc2" %>
<%@ Register Src="Item/MemberUserRoleItem.ascx" TagName="MemberUserRoleItem" TagPrefix="uc3" %>
<div class="border_gray">
    <asp:GridView ID="gvEntity" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="UID" DataSourceID="dsEntity" EnableViewState="False" OnRowCommand="gvEntity_RowCommand"
        ShowFooter="True">
        <Columns>
            <asp:BoundField DataField="PID" HeaderText="PID" SortExpression="PID" />
            <asp:BoundField DataField="UserName" HeaderText="姓名" SortExpression="UserName" />
            <asp:BoundField DataField="EMail" HeaderText="EMail" SortExpression="EMail" />
            <asp:TemplateField HeaderText="使用角色" >
                <ItemTemplate>
                    <%# String.Join("", ((UserProfile)Container.DataItem).UserRole.Where(r=>r.OrgaCateID==_userProfile.CurrentUserRole.OrgaCateID).Select(r=>r.UserRoleDefinition.Role).ToArray()) %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <FooterTemplate>
                    <asp:Button ID="btnCreate" runat="server" CausesValidation="False" CommandName="Create"
                        Text="新增" />
                </FooterTemplate>
                <ItemTemplate>
                    <asp:Button ID="btnRole" runat="server" CausesValidation="False" CommandName="EditMemberRole"
                        Text="角色設定" CommandArgument='<%# Eval(gvEntity.DataKeyNames[0]) %>' />
                    &nbsp;
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
<cc1:UserProfileDataSource ID="dsEntity" runat="server">
</cc1:UserProfileDataSource>
<uc2:UserProfileItem ID="editItem" runat="server" />
<uc3:MemberUserRoleItem ID="roleItem" runat="server" />
<script runat="server">

    private Model.Security.MembershipManagement.UserProfileMember _userProfile;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
    }
    
    protected override void gvEntity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditMemberRole")
        {
            roleItem.UID = int.Parse((String)e.CommandArgument);
            roleItem.Show();
        }
        else
        {
            base.gvEntity_RowCommand(sender, e);
        }
    }
</script>