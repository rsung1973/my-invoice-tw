<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/EIVO/ImportInvoiceCancellationFile.ascx" tagname="ImportInvoiceCancellationFile" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:ImportInvoiceCancellationFile ID="ImportInvoiceCancellationFile1" runat="server" EncodingName="GB2312" Prefix="InvoiceCancellation" />
</asp:Content>
