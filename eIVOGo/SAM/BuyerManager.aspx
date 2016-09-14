<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Register Src="~/Module/SAM/InquireInvoiceToUpdateBuyer.ascx" TagPrefix="uc1" TagName="InquireInvoiceToUpdateBuyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:InquireInvoiceToUpdateBuyer runat="server" ID="InquireInvoiceToUpdateBuyer" />
</asp:Content>
