<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<table class="table01 itemList">
    <thead>
        <tr>
            <th style="min-width: 100px" aria-sort="other">發票年度</th>
            <th style="min-width: 100px" aria-sort="other">發票期別</th>
            <th style="min-width: 120px">獎別</th>
            <th style="min-width: 160px">中獎金額</th>
            <th style="min-width: 160px">中獎號碼</th>
            <th style="min-width: 150px" aria-sort="other">管理</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
                foreach (var item in _items)
                {
                    idx++;
                    Html.RenderPartial("~/Views/WinningNumber/Module/DataItem.ascx", item);
                }
                Html.RenderPartial("~/Views/WinningNumber/Module/AddItem.ascx");
        %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="10">&nbsp;</td>
        </tr>
    </tfoot>
</table>

<script runat="server">

    IEnumerable<UniformInvoiceWinningNumber> _items;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _items = ((IEnumerable<UniformInvoiceWinningNumber>)this.Model).OrderBy(w => w.Rank);
    }

</script>
