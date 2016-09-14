<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<% if(this.Model!=null) { %>
<tr>
    <th>代理業者統編
    </th>
    <td class="tdleft">
        <%= ((IEnumerable<SelectListItem>)this.Model).Count()>1 ? Html.DropDownList("agentID", (IEnumerable<SelectListItem>)this.Model, "全部") :  Html.DropDownList("agentID", (IEnumerable<SelectListItem>)this.Model) %>
        <% if(Request["agentID"]!=null) { %>
        <script>
            $(function () {
                $('select[name="agentID"]').val('<%= Request["agentID"] %>');
            });
        </script>
        <% } %>
    </td>
</tr>
<% } %>


