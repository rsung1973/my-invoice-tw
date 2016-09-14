<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master"  AutoEventWireup="true"  Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SCM/addMarket_Resource_Maintain.ascx" tagname="addMarket_Resource_Maintain" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:addMarket_Resource_Maintain ID="addMarket_Resource_Maintain1" 
    runat="server" />
</asp:Content>

