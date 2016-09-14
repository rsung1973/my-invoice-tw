<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>是否中獎 
    </th>
    <td class="tdleft">
        <select name="winning">
            <option value="">全部</option>
            <option value="1">是</option>
        </select>
    </td>
</tr>
<script>
    $(function () {
        $('select[name="winning"]').val('<%= Request["winning"] %>');
    });
</script>
