<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<tr>
    <th width="20%">日期區間
    </th>
    <td class="tdleft">自&nbsp;<input id="invoiceDateFrom" name="invoiceDateFrom" type="text" size="10" readonly="readonly" value="<%= Request["invoiceDateFrom"] %>" />
        &nbsp;至&nbsp;
        <input id="invoiceDateTo" name="invoiceDateTo" type="text" size="10" readonly="readonly" value="<%= Request["invoiceDateTo"] %>" />
    </td>
</tr>
<script>
    $(function () {
        $('#invoiceDateFrom').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
        $('#invoiceDateTo').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
    });
</script>
