<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireAllowance" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票／折讓單號碼
    </th>
    <td class="tdleft">
        <input name="allowanceNo" type="text" value="<%= Request["allowanceNo"] %>" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceAllowance, bool>> BuildQueryExpression(Expression<Func<InvoiceAllowance, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["allowanceNo"]))
        {
            qExpr = qExpr.And(i => i.AllowanceNumber == Request["allowanceNo"].Trim());
            HasSet = true;
        }

        return qExpr;
    }
</script>