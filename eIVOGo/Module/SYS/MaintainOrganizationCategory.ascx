<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>

<%@ Register src="OrganizationCategoryList.ascx" tagname="OrganizationCategoryList" tagprefix="uc3" %>



<uc1:PageAction ID="actionItem" runat="server" ItemName="首頁 &gt; 系統維護" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" ItemName="公司類別套用維護" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <uc3:OrganizationCategoryList ID="OrganizationCategoryList1" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>

