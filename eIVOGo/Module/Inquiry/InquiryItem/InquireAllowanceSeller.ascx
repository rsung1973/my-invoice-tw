<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireAllowance" %>
<%@ Register Src="~/Module/UI/InvoiceSellerSelector.ascx" TagName="InvoiceSellerSelector" TagPrefix="uc4" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>統編
    </th>
    <td class="tdleft">
        <uc4:InvoiceSellerSelector ID="SellerID" runat="server" SelectorIndication="全部" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceAllowance, bool>> BuildQueryExpression(Expression<Func<InvoiceAllowance, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(SellerID.SelectedValue))
        {
            HasSet = true;
            qExpr = qExpr.And(d => d.InvoiceAllowanceSeller.SellerID == int.Parse(SellerID.SelectedValue));
        }
        return qExpr;
    }
</script>