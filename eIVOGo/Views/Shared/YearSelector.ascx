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
<select name="year">
    <%  for (int y = DateTime.Today.Year; y >= _startYear; y--)
        { %>
            <option value="<%= y %>"><%= y %></option>
    <%  } %>
</select>


<script runat="server">

    ModelStateDictionary _modelState;
    int _startYear = 2012;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
    }

</script>
