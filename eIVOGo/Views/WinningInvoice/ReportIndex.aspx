<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="~/Views/WinningInvoice/Module/InquireWinning.ascx" TagPrefix="uc1" TagName="InquireWinning" %>


<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <uc1:InquireWinning runat="server" ID="InquireWinning" />
</asp:Content>
