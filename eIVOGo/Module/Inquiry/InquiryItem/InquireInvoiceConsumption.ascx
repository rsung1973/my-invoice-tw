<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票類別
    </th>
    <td class="tdleft">
        <input type="radio" name="cc1" value="B2B" />B2B
        <input type="radio" name="cc1" value="B2C" />B2C
    </td>
</tr>
<script>
    $(function () {
        $('input[type="radio"][name="cc1"][value="<%= Request["cc1"]%>"]').prop('checked', true);
    });
</script>

<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["cc1"]=="B2B")
        {
            qExpr = qExpr.And(i => i.InvoiceBuyer.ReceiptNo != "0000000000");
            HasSet = true;
        }
        else if (Request["cc1"] == "B2C")
        {
            qExpr = qExpr.And(i => i.InvoiceBuyer.ReceiptNo == "0000000000");
            HasSet = true;
        }

        return qExpr;
    }

    public bool QueryForB2C
    {
        get
        {
            return Request["cc1"] == "B2C";
        }
    }
    
</script>