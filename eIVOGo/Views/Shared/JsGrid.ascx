<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<%  if (ViewBag.JsGrid == null)
    {   %>
        <link href="<%= VirtualPathUtility.ToAbsolute("~/css/jsgrid.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= VirtualPathUtility.ToAbsolute("~/css/jsgrid-theme.css") %>" rel="stylesheet" type="text/css" />
        <link href="<%= VirtualPathUtility.ToAbsolute("~/css/eivo.css") %>" rel="stylesheet" type="text/css" />
        <script src="<%= VirtualPathUtility.ToAbsolute("~/Scripts/jsgrid.js") %>" type="text/javascript"></script>
<%
        ViewBag.JsGrid = this;
    }
%>
<script>
    function createJsConfig() {

        var config = {
            height: "auto",
            width: "100%",

            autoload: true,
            paging: true,
            pageLoading: true,
            pageSize: 10,
            pageButtonCount: 10,
            pageIndex: 2,
            pagerFormat: "{first} | {prev} | {pages} | {next} | {last} &nbsp;&nbsp; {pageIndex} / {pageCount}",
            pageNextText: "下一頁",
            pagePrevText: "上一頁",
            pageFirstText: "首頁",
            pageLastText: "末頁",
            pageNavigatorNextText: "下10頁",
            pageNavigatorPrevText: "上10頁",
            noDataContent: "查無資料!!",

            controller: {},

            fields: []
        };

        return config;
    }

</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
</script>
