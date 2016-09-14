<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>GoogleID/客戶ID</th>
    <td class="tdleft">
        <input name="customerID" type="text" value="<%= Request["customerID"] %>" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["customerID"]))
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceBuyer.CustomerID == Request["customerID"].Trim());
        }

        return qExpr;
    }
</script>
