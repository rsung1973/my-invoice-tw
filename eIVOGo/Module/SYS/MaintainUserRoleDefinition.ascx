<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>

<%@ Register src="UserRoleDefinitionList.ascx" tagname="UserRoleDefinitionList" tagprefix="uc3" %>



<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 系統維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="使用者角色維護" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc3:UserRoleDefinitionList ID="UserRoleDefinitionList1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

