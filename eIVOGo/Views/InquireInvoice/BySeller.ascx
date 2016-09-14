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
    <th>統編
    </th>
    <td class="tdleft">
        <%= this.Model != null ? Html.DropDownList("sellerID", (IEnumerable<SelectListItem>)this.Model, "全部") : null %>
        <script>
            $(function () {
                $('select[name="sellerID"]').val('<%= Request["sellerID"] %>');
            });
        </script>
    </td>
</tr>


