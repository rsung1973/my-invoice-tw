<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true"
    Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Register Src="~/Module/EIVO/WebUploadMailMessage.ascx" TagName="WebUploadMailMessage"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:WebUploadMailMessage ID="WebUploadMailMessage1" runat="server" DeliveryStatus="申請退回" />
</asp:Content>
