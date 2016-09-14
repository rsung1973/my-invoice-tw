<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Register Src="~/Module/JsGrid/InquireWinningInvoiceToPrint.ascx" TagPrefix="uc1" TagName="InquireWinningInvoiceToPrint" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:InquireWinningInvoiceToPrint runat="server" ID="InquireWinningInvoiceToPrint" />
</asp:Content>
