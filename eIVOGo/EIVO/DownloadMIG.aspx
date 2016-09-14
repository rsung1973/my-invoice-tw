<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register Src="~/Module/EIVO/DownloadMIG.ascx" TagName="DownloadMIG" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:DownloadMIG ID="DownloadMIG" runat="server" />
</asp:Content>
