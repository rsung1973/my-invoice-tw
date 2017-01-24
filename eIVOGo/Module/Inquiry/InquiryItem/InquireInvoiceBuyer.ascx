<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<tr>
    <th>買受人統編
    </th>
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
            qExpr = qExpr.And(d => d.InvoiceBuyer.ReceiptNo == Request["receiptNo"].GetEfficientString());
        }
        
        return qExpr;
    }

</script>