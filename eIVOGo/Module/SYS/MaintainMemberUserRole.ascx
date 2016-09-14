<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc1" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc2" %>
<%@ Register Src="OrganizationCategoryUserRoleList.ascx" TagName="OrganizationCategoryUserRoleList"
    TagPrefix="uc3" %>


<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 角色功能維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="角色功能維護" />
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
--%>
<uc3:OrganizationCategoryUserRoleList ID="OrganizationCategoryUserRoleList1" runat="server" />
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
<script runat="server">
    private Model.Security.MembershipManagement.UserProfileMember _userProfile;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        OrganizationCategoryUserRoleList1.QueryExpr = u => u.OrgaCateID == _userProfile.CurrentUserRole.OrgaCateID;
    }
</script>
