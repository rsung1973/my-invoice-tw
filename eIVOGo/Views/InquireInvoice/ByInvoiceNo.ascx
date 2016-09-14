<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票／折讓單號碼
    </th>
    <td class="tdleft">
        <input name="invoiceNo" type="text" value="<%= Request["invoiceNo"] %>" />
    </td>
</tr>
