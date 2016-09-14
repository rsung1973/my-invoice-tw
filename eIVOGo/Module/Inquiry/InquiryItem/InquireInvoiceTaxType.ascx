<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>課稅別</th>
    <td class="tdleft">
        <select id="taxTypes" name="taxTypes">
            <option value="">全部</option>
            <option value="1">應稅</option>
            <option value="2">零稅率</option>
            <option value="3">免稅</option>
        </select>
    </td>
</tr>
<input type="hidden" id="hidTaxTypes" name="hidTaxTypes" value="<%= Request["hidTaxTypes"] %>">
<script>
    $(function () {
        $('select[name="taxTypes"]').val('<%= Request["taxTypes"]%>');
    });
</script>

<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["taxTypes"] == "1")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceAmountType.TaxType == (byte)1);
        }
        else if (Request["taxTypes"] == "2")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceAmountType.TaxType == (byte)2);
        }
        else if (Request["taxTypes"] == "3")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.InvoiceAmountType.TaxType == (byte)3);
        }

        return qExpr;
    }

</script>
