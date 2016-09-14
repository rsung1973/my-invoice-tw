<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor"  %>
<%@ Register src="../Module/SCM/PromotionalProjectMaintainAdd.ascx" tagname="PromotionalProjectMaintainAdd" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:PromotionalProjectMaintainAdd ID="PromotionalProjectMaintainAdd1" 
        runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["id"] != null)
        {
            PromotionalProjectMaintainAdd1.PrepareDataFromDB((int)Page.PreviousPage.Items["id"]);
        }
    }
</script>