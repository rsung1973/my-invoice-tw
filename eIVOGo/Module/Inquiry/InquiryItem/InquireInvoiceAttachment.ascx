<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr runat="server">
    <th>附件檔
    </th>
    <td class="tdleft">
        <select id="attachment" name="attachment">
            <option value="">全部</option>
            <option value="1">有</option>
            <option value="0">無</option>
        </select>
    </td>
</tr>
<script>
    $(function () {
        $('select[name="attachment"]').val('<%= Request["attachment"]%>');
    });
</script>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        var qExpr = queryExpr;
        if (Request["attachment"] == "1")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.CDS_Document.Attachment.Count() > 0);
        }
        else if (Request["attachment"] == "0")
        {
            HasSet = true;
            qExpr = qExpr.And(i => i.CDS_Document.Attachment.Count() <= 0);
        }

        return qExpr;
    }
    
</script>