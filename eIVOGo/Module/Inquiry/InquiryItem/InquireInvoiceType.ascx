<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票類別</th>
    <td class="tdleft">
        <div>
            <input type="radio" name="cc2" value="1" />全部
            <input type="radio" name="cc2" value="2" />中獎發票
        </div>
    </td>
</tr>
<script>
    $(function () {
        $('input[type="radio"][name="cc2"][value="<%=Request["cc2"]%>"]').prop('checked', true);
    });
</script>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["cc2"] == "2")
        {
            qExpr = qExpr.And(i => i.InvoiceBuyer.ReceiptNo == "0000000000" && i.InvoiceWinningNumber != null);
            HasSet = true;
        }
 
        return qExpr;
    }

</script>
