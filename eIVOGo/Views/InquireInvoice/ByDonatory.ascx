<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>愛心碼 
    </th>
    <td class="tdleft">
        <input type="text" name="donatory" value="<%= Request["donatory"] %>" />
    </td>
</tr>
