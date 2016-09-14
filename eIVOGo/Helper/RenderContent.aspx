<%@ Page Title="" Language="C#" MasterPageFile="~/template/ContentScriptManagerPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" StylesheetTheme="Visitor" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <asp:PlaceHolder ID="plContent" runat="server"></asp:PlaceHolder>
</asp:Content>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if(!String.IsNullOrEmpty(Request["control"]))
        {
            UserControl control = (UserControl)this.LoadControl(Request["control"]);
            control.InitializeAsUserControl(Page);
            plContent.Controls.Add(control);
        }
    }

</script>
