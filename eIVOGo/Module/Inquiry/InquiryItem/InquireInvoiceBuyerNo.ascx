<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>買受人統編</th>
    <td class="tdleft">
        <input name="receiptNo" type="text" value="<%= Request["receiptNo"] %>" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["receiptNo"]))
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceBuyer.ReceiptNo == Request["receiptNo"].Trim());
        }

        return qExpr;
    }
</script>
