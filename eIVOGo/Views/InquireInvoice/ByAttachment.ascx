<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>附件檔 
    </th>
    <td class="tdleft">
        <select name="attachment">
            <option value="">全部</option>
            <option value="1">有</option>
            <option value="0">無</option>
        </select>
    </td>
</tr>
<script>
    $(function () {
        $('select[name="attachment"]').val('<%= Request["attachment"] %>');
    });
</script>
