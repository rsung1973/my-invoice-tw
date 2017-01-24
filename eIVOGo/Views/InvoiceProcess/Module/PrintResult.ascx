<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<div><%= _message %>
    <%  if (!String.IsNullOrEmpty(_model))
        { %>
    <a href="<%= VirtualPathUtility.ToAbsolute(_model) %>" target="_blank">列印資料下載</a>
    <%  } %>
</div>


<script runat="server">

    String _model;
    String _message;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (String)this.Model;
        _message = (String)ViewBag.Message;
    }

</script>
