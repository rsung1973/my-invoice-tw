<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>

<%@ Register src="UserProfileList.ascx" tagname="UserProfileList" tagprefix="uc3" %>



<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 系統維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
--%>        <uc3:UserProfileList ID="UserProfileList1" runat="server" />
<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        UserProfileList1.QueryExpr = u => true;
    }
</script>