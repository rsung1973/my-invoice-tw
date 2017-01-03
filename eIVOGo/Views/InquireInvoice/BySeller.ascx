<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<tr>
    <th><%= ViewBag.Title %></th>
    <td class="tdleft">
        <%  Html.RenderPartial("~/Views/DataFlow/SellerSelector.ascx", _model); %>
    </td>
</tr>
<script runat="server">

    IEnumerable<Organization> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<Organization>)this.Model;
    }
</script>

