<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>作廢發票
    </th>
    <td class="tdleft">
        <select name="cancelled">
            <option value="0">未作廢</option>
            <option value="1">己作廢</option>
        </select>
        <script>
            $(function () {
                $('select[name="cancelled"]').val('<%= Request["cancelled"] ?? "0" %>');
            });
        </script>
    </td>
</tr>
