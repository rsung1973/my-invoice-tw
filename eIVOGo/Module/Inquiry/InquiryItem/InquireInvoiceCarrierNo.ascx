<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>發票載具號碼
    </th>
    <td class="tdleft">
        <input name="carrierNo" type="text" value="<%= Request["carrierNo"] %>" />
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (!String.IsNullOrEmpty(Request["carrierNo"]))
        {
            String no = Request["carrierNo"].Trim();
            qExpr = qExpr.And(i => i.InvoiceCarrier.CarrierNo == no || i.InvoiceCarrier.CarrierNo2 == no);
            HasSet = true;
        }
        
        return qExpr;
    }
</script>