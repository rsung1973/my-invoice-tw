<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>
<%@ Register src="../Module/Entity/OrganizationEntrustingTokenItem.ascx" tagname="OrganizationEntrustingTokenItem" tagprefix="uc1" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:OrganizationEntrustingTokenItem ID="orgToken" runat="server" />
</asp:Content>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        orgToken.Done += new EventHandler(orgToken_Done);
    }

    void orgToken_Done(object sender, EventArgs e)
    {
        this.AjaxAlertAndRedirect("憑證設定完成!!",VirtualPathUtility.ToAbsolute("~/UserProfile/EditMySelf"));
    }
</script>