<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
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
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        DateTime? dateFrom;
        if (Request["invoiceDateFrom"].ParseDate(out dateFrom))
        {
            qExpr = qExpr.And(i => i.InvoiceDate >= dateFrom.Value);
            HasSet = true;
        }

        DateTime? dateTo;
        if (Request["invoiceDateTo"].ParseDate(out dateTo))
        {
            qExpr = qExpr.And(i => i.InvoiceDate < dateTo.Value.AddDays(1));
            HasSet = true;
        }

        return qExpr;
    }
</script>