<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>捐贈類別 
    </th>
    <td class="tdleft">
        <input type="radio" name="donation" value="donated" />捐贈統計表
        <input type="radio" name="donation" value="winning" />捐贈中獎統計表
    </td>
</tr>
<script>
    $(function () {
        $('input[type="radio"][name="donation"][value="<%= Request["donation"]%>"]').prop('checked', true);
    });
</script>
