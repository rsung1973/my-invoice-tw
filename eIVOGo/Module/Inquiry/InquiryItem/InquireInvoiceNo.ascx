<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票／折讓單號碼
    </th>
    <td class="tdleft">
        <input name="invoiceNo" type="text" value="<%= Request["invoiceNo"] %>" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["invoiceNo"]))
        {
            String invoiceNo = Request["invoiceNo"].Trim();
            if (invoiceNo.Length == 10)
            {
                String trackCode = invoiceNo.Substring(0, 2);
                String no = invoiceNo.Substring(2);
                qExpr = qExpr.And(i => i.No == no && i.TrackCode == trackCode);

            }
            else
            {
                qExpr = qExpr.And(i => i.No == invoiceNo);
            }
            HasSet = true;
        }

        return qExpr;
    }
</script>