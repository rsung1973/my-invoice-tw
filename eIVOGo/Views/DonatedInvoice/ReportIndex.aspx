<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="~/Views/DonatedInvoice/Module/InquireDonation.ascx" TagPrefix="uc1" TagName="InquireDonation" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <uc1:InquireDonation runat="server" id="InquireDonation" />
</asp:Content>
