<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SCM/ExchangeGoodsQuery.ascx" tagname="ExchangeGoodsQuery" tagprefix="uc1" %>
<%@ Register src="../Module/SCM/ReturnedGoodsQuery.ascx" tagname="ReturnedGoodsQuery" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc2:ReturnedGoodsQuery ID="ReturnedGoodsQuery1" runat="server" />
</asp:Content>
