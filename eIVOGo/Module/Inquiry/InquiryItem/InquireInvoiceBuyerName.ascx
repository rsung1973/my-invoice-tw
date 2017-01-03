<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>買受人名稱</th>
    <td class="tdleft">
        <input name="buyerName" type="text" value="<%= Request["buyerName"] %>" size="64" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["buyerName"]))
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceBuyer.CustomerName.Contains(Request["buyerName"].Trim()));
        }

        return qExpr;
    }
</script>
