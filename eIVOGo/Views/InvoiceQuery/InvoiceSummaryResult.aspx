﻿<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <% Html.RenderPartial("Module/InvoiceSummaryResult", Model); %>
</asp:Content>
<script runat="server">

    public override void Dispose()
    {
        var models = TempData.GetGenericModelSource();
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>