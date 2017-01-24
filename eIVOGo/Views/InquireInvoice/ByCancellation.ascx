<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>單據狀態
    </th>
    <td class="tdleft">
        <select name="Cancelled">
            <option value="False">未作廢</option>
            <option value="True">己作廢</option>
        </select>
        <script>
            $(function () {
                $('select[name="Cancelled"]').val('<%= Request["Cancelled"] ?? false.ToString() %>');
            });
        </script>
    </td>
</tr>
