<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<table class="table01 itemList">
    <thead>
        <tr>
            <th style="min-width: 120px;" aria-sort="other">發票年度</th>
            <th style="min-width: 120px;">發票期別</th>
            <th style="min-width: 120px;">字軌</th>
            <th style="min-width: 150px" aria-sort="other">管理</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _items)
            {
                idx++;
                Html.RenderPartial("~/Views/TrackCode/Module/DataItem.ascx", item);
            }
        %>
    </tbody>
    <tfoot>
        <%  Html.RenderPartial("~/Views/TrackCode/Module/AddItem.ascx"); %>
        <tr>
            <td colspan="4">
<%  if (_sort != null && _sort.Length > 0)
    { %>
<script>
    $(function () {
        initSort(<%= JsonConvert.SerializeObject(_sort) %>,1);
    });
</script>
<%  } %>
<script>
    $(function () {
        buildSort(uiTrackCodeQuery.inquire,<%= (int)ViewBag.PageIndex + 1 %>,1);
    });
</script>
            </td>
        </tr>
    </tfoot>
</table>

<script runat="server">

    IEnumerable<InvoiceTrackCode> _items;
    IEnumerable<InvoiceTrackCode> _model;
    IOrderedEnumerable<InvoiceTrackCode> _order;
    int[] _sort;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceTrackCode>)this.Model;
        _sort = (int[])ViewBag.Sort;
        _pageSize = (int)ViewBag.PageSize;

        if (_sort != null && _sort.Length>0)
        {
            sorting();

            if (_order == null)
            {
                _items = _model.Skip((int)ViewBag.PageIndex * _pageSize)
                    .Take(_pageSize);
            }
            else
            {
                _items = _order.Skip((int)ViewBag.PageIndex * _pageSize)
                    .Take(_pageSize);
            }
        }
        else
        {
            _items = _model.Skip((int)ViewBag.PageIndex * _pageSize)
                .Take(_pageSize);
        }
    }

    void sorting()
    {
        foreach (var idx in _sort)
        {
            switch (idx)
            {
                case 2:
                    _order = _order == null ? _model.OrderBy(i => i.PeriodNo) : _order.ThenBy(i => i.PeriodNo);
                    break;
                case -2:
                    _order = _order == null ? _model.OrderByDescending(i => i.PeriodNo) : _order.ThenByDescending(i => i.PeriodNo);
                    break;
                case 3:
                    _order = _order == null ? _model.OrderBy(i => i.TrackCode ) : _order.ThenBy(i => i.TrackCode);
                    break;
                case -3:
                    _order = _order == null ? _model.OrderByDescending(i => i.TrackCode) : _order.ThenByDescending(i => i.TrackCode);
                    break;
            }
        }
    }

</script>
