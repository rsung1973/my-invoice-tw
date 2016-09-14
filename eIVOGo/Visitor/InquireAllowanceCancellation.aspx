<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Register Src="~/Module/Inquiry/InquireAllowanceCancellation.ascx" TagPrefix="uc1" TagName="InquireAllowanceCancellation" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:InquireAllowanceCancellation runat="server" ID="InquireAllowanceCancellation" />
</asp:Content>
