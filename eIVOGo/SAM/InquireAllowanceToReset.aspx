<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Register Src="~/Module/EIVO/InquireAllowanceToReset.ascx" TagPrefix="uc1" TagName="InquireAllowanceToReset" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:InquireAllowanceToReset runat="server" id="InquireAllowanceToReset" />
</asp:Content>
