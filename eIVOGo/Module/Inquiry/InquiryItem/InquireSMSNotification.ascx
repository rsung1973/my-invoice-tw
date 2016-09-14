<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>簡訊通知</th>
    <td class="tdleft">
        <select id="smsState" name="smsState">
            <option value="">全部</option>
            <option value="1">是</option>
            <option value="0">否</option>
        </select>
    </td>
</tr>
<script>
    $(function () {
        $('select[name="smsState"]').val('<%= Request["smsState"]%>');
    });
</script>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["smsState"] == "1")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS == true);
        }
        else if (Request["smsState"] == "0")
        {
            HasSet = true;
            qExpr = qExpr.And(i => !i.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS.HasValue
                || i.Organization.OrganizationStatus.SetToNotifyCounterpartBySMS == false);
        }

        return qExpr;
    }
</script>