<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrgaCateSelector.ascx.cs" Inherits="eIVOGo.Module.SYS.Item.OrgaCateSelector" %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<asp:DropDownList ID="OrgaCateSelect" runat="server" EnableViewState="False">
</asp:DropDownList>
<cc1:OrganizationCategoryDataSource ID="dsOrgaCate" runat="server">
</cc1:OrganizationCategoryDataSource>
