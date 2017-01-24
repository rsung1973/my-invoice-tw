<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<script>
    uiWinningNo.inquire();
</script>

<script runat="server">

    UniformInvoiceWinningNumber _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (UniformInvoiceWinningNumber)this.Model;
    }

</script>
