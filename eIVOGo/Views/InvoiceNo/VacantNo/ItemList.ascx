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
            <th style="min-width: 100px">發票年度</th>
            <th style="min-width: 100px">發票期別</th>
            <th style="min-width: 80px">字軌</th>
            <th style="min-width: 160px">發票號碼起</th>
            <th style="min-width: 160px">發票號碼迄</th>
            <th style="min-width: 150px">數量</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _items)
            {
                idx++;
                Html.RenderPartial("~/Views/InvoiceNo/VacantNo/DataItem.ascx", item);
            }
        %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="10">&nbsp;</td>
        </tr>
    </tfoot>
</table>

<script runat="server">

    List<InquireVacantNoResult> _model;
    IEnumerable<InquireVacantNoResult> _items;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (List<InquireVacantNoResult>)this.Model;
        _items = _model.Where(r => !r.CheckPrev.HasValue);
        ViewBag.DataItems = _model;
    }

</script>
