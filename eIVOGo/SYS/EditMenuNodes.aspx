﻿<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SYS/MaintainMenuNodes.ascx" tagname="MaintainMenuNodes" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:MaintainMenuNodes ID="menuNodes" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(published_testeditingmenudoc_aspx_Load);
    }

    void published_testeditingmenudoc_aspx_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack && Page.PreviousPage != null && Page.PreviousPage.Items["menuPath"] != null)
        {
            menuNodes.MenuPath = Page.PreviousPage.Items["menuPath"] as String;
        }
    }
</script>