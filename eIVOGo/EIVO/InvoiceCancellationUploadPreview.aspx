﻿<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor"%>
<%@ Register src="../Module/EIVO/ImportInvoiceCancellationPreview.ascx" tagname="ImportInvoiceCancellationPreview" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:ImportInvoiceCancellationPreview ID="ImportInvoiceCancellationPreview1" runat="server" />
</asp:Content>
