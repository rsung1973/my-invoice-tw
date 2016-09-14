<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>

<%@ Register src="UserProfileListByMemberAdmin.ascx" tagname="UserProfileList" tagprefix="uc3" %>



<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 員工帳號管理" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="員工帳號管理" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc3:UserProfileList ID="UserProfileList1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<script runat="server">
    private Model.Security.MembershipManagement.UserProfileMember _userProfile;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        UserProfileList1.QueryExpr = u => u.Creator == _userProfile.UID;
    }
</script>