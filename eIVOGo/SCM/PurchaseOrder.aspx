<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SCM/PurchaseOrder.ascx" tagname="PurchaseOrder" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:PurchaseOrder ID="PurchaseOrder1" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["PONO"] != null)
        {
            this.PurchaseOrder1.PrepareDataFromDB(int.Parse(Page.PreviousPage.Items["PONO"].ToString()));
        }
    }
</script>