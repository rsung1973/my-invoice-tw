<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/Inquiry/BonusReport.ascx" tagname="BonusReport" tagprefix="uc1" %>
<%@ Register src="../Module/Inquiry/QueryBonusList.ascx" tagname="QueryBonusList" tagprefix="uc2" %>
<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <uc2:QueryBonusList ID="QueryBonusList1" runat="server" />
</asp:Content>
