<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<tr>
    <th width="20%">期別
    </th>
    <td class="tdleft">
        <%= this.Model != null ? Html.DropDownList("period", (IEnumerable<SelectListItem>)this.Model) : null %>
        <script>
            $(function () {
                $('select[name="period"]').val('<%= Request["period"] %>');
            });
        </script>
    </td>
</tr>
