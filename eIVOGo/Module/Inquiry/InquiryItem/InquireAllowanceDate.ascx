<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireAllowance" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th width="20%">日期區間
    </th>
    <td class="tdleft">自&nbsp;<input id="allowanceDateFrom" name="allowanceDateFrom" type="text" size="10" readonly="readonly" value="<%= Request["allowanceDateFrom"] %>" />
        &nbsp;至&nbsp;
        <input id="allowanceDateTo" name="allowanceDateTo" type="text" size="10" readonly="readonly" value="<%= Request["allowanceDateTo"] %>" />
    </td>
</tr>
<script>
    $(function () {
        $('#allowanceDateFrom').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
        $('#allowanceDateTo').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
    });
</script>
<script runat="server">

    public override Expression<Func<InvoiceAllowance, bool>> BuildQueryExpression(Expression<Func<InvoiceAllowance, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        DateTime? dateFrom;
        if (Request["allowanceDateFrom"].ParseDate(out dateFrom))
        {
            qExpr = qExpr.And(i => i.AllowanceDate >= dateFrom.Value);
            HasSet = true;
        }

        DateTime? dateTo;
        if (Request["allowanceDateTo"].ParseDate(out dateTo))
        {
            qExpr = qExpr.And(i => i.AllowanceDate < dateTo.Value.AddDays(1));
            HasSet = true;
        }

        return qExpr;
    }

</script>