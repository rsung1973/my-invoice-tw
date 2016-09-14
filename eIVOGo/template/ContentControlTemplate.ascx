<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContentControlTemplate.ascx.cs" Inherits="eIVOGo.template.ContentControlTemplate" %>
<%@ Register src="~/Module/UI/PageAction.ascx" tagname="PageAction" tagprefix="uc1" %>
<%@ Register src="~/Module/UI/FunctionTitleBar.ascx" tagname="FunctionTitleBar" tagprefix="uc2" %>



<uc1:PageAction ID="actionItem" runat="server" />
<uc2:FunctionTitleBar ID="titleBar" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
</asp:UpdatePanel>

