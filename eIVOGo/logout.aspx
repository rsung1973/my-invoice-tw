<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage"%>
<%@ Import Namespace="Business.Helper" %>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Context.Logout();
        Response.Redirect(FormsAuthentication.LoginUrl);
    }
</script>