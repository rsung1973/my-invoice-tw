<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor"  %>
<%@ Register src="../Module/SCM/WarehouseMaintainAdd.ascx" tagname="WarehouseMaintainAdd" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:WarehouseMaintainAdd ID="WarehouseMaintainAdd1" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["id"] != null)
        {
            WarehouseMaintainAdd1.WAREHOUSE_SN = (int)Page.PreviousPage.Items["id"];
        }
    }
</script>