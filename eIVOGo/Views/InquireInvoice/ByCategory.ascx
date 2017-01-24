<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>單據類型
    </th>
    <td class="tdleft">
        <select name="Category">
            <option value="10">發票</option>
            <option value="11">折讓證明</option>
        </select>
        <script>
            $(function () {
                $('select[name="Category"]').val('<%= Request["Category"] ?? "0" %>');
            });
        </script>
    </td>
</tr>
