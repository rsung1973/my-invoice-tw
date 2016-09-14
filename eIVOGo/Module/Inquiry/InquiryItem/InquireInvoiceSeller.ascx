<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<tr>
    <th>統編
    </th>
    <td class="tdleft">
        <uc4:InvoiceSellerSelector ID="SellerID" runat="server" SelectorIndication="全部" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(SellerID.SelectedValue))
        {
            HasSet = true;
            qExpr = qExpr.And(d => d.InvoiceSeller.SellerID == int.Parse(SellerID.SelectedValue));
        }
        
        return qExpr;
    }

</script>