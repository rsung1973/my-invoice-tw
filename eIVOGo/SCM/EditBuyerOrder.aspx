<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/SCM/CreateBuyerOrder.ascx" tagname="CreateBuyerOrder" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:CreateBuyerOrder ID="CreateBuyerOrder1" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        //        Page.Error += new EventHandler(Page_Error);
        if (Request["id"] != null)
        {
            CreateBuyerOrder1.PrepareDataFromDB(int.Parse(Request["id"]));
        }
    }
    
</script>