<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票類別
    </th>
    <td class="tdleft">
        <select name="cc1">
            <option value="">全部</option>
            <option value="B2B">B2B</option>
            <option value="B2C">B2C</option>
        </select>
        <script>
            $(function () {
                $('select[name="cc1"]').val('<%= Request["cc1"] %>');
            });
        </script>
    </td>
</tr>
