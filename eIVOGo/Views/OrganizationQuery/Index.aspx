<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="~/Views/OrganizationQuery/Module/InquireOrganization.ascx" TagPrefix="uc1" TagName="InquireOrganization" %>


<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <uc1:InquireOrganization runat="server" ID="InquireOrganization" />
</asp:Content>
