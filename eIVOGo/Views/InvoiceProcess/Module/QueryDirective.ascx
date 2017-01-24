<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>

<tr>
    <th>查詢項目
    </th>
    <td class="tdleft">
        <input type="radio" name="urlTo" onchange="window.location.href='<%= Url.Action("Index","InvoiceProcess") %>';" />發票
        &nbsp;&nbsp;&nbsp;&nbsp;
        <input type="radio" name="urlTo" onchange="window.location.href='<%= Url.Action("Index","AllowanceProcess") %>';" />折讓單
    </td>
    <script>
        $(function () {
            $('input[name="urlTo"]').eq(<%= _model %>).prop('checked', true);
        });
    </script>
</tr>
<script runat="server">

    public String[] NamingDirection
    { get; set; }
    int? _model;    

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (int?)this.Model ?? 0;
    }
   

</script>
