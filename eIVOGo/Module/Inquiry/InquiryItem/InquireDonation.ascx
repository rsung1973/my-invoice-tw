<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票捐贈</th>
    <td class="tdleft">
        <select id="donation" name="donation">
            <option value="">全部</option>
            <option value="1">是</option>
            <option value="0">否</option>
        </select>
    </td>
</tr>
<script>
    $(function () {
        $('select[name="donation"]').val('<%= Request["donation"]%>');
    });
</script>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["donation"] == "1")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceDonation != null);
        }
        else if (Request["donation"] == "0")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceDonation == null);
        }

        return qExpr;
    }
</script>