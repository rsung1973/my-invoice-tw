<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<script runat="server">
    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        return queryExpr.And(i => i.InvoiceWinningNumber == null);
    }
</script>