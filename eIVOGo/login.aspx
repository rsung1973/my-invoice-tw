<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        Response.Redirect("~/Account/Login");
    }

</script>
