<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<!DOCTYPE html>
<html>
<head>
    <%= Styles.Render("~/App_Themes/NewPrint") %>
    <style type="text/css">
        div.fspace
        {
            height: 8.8cm;
        }
        div.bspace
        {
            height: 8.9cm;
        }
        
        body, td, th
        {
            font-family: Verdana, Arial, Helvetica, sans-serif, "細明體" , "新細明體";
        }

        body {
            margin: 0px;
            margin-left: 0cm;
            margin-right: 0cm;
        }      

    </style>
    <title>電子發票系統</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />    
</head>
<body>
    <script src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/css3-multi-column.js") %>" type="text/javascript"></script>
    <%  Html.RenderPartial("~/Views/Module/InvoicePOSPrintView.ascx"); %>
</body>
</html>
<script runat="server">

    InvoiceViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (InvoiceViewModel)ViewBag.ViewModel;
    }
</script>