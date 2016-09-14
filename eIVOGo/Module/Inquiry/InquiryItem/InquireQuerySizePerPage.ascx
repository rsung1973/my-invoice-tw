<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquiryItem.InquireInvoice" %>
<%@ Register Src="~/Module/Common/CalendarInputDatePicker.ascx" TagName="CalendarInputDatePicker"
    TagPrefix="uc1" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<tr>
    <th>列印顯示筆數
    </th>
    <td class="tdleft">
        <input type="text" size="8" name="queryPageSize" value="<%= Request["queryPageSize"] %>" />
        <font color="red">*預設10筆</font>
    </td>
</tr>
<script runat="server">

    public override Expression<Func<InvoiceItem, bool>> BuildQueryExpression(Expression<Func<InvoiceItem, bool>> queryExpr)
    {
        return queryExpr;
    }

    public int PageSize
    {
        get
        {
            int size;
            return Request.GetRequestValue("queryPageSize",out size) ? size : 10;
        }
    }
</script>