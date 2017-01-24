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
            <th aria-sort="other"><input id="chkAll" name="chkAll" type="checkbox" /></th>
            <th style="min-width: 80px;">日期</th>
            <th style="min-width: 120px;">GoogleID/客戶ID</th>
            <th style="min-width: 240px;">序號</th>
            <th style="min-width: 160px;">開立發票營業人</th>
            <th style="min-width: 80px;">統編</th>
            <th style="min-width: 120px;">發票號碼</th>
            <th style="min-width: 80px;">未稅金額</th>
            <th style="min-width: 80px;">稅額</th>
            <th style="min-width: 80px;">含稅金額</th>
            <th style="min-width: 80px;">是否中獎</th>
            <th style="min-width: 90px;">買受人統編</th>
            <th style="min-width: 120px;" aria-sort="other">名稱</th>
            <th style="min-width: 120px;" aria-sort="other">連絡人名稱</th>
            <th style="min-width: 240px;" aria-sort="other">地址</th>
            <th style="min-width: 240px;" aria-sort="other">email</th>
            <th style="min-width: 120px;" aria-sort="other">備註</th>
            <th style="min-width: 80px;" aria-sort="other">簡訊通知</th>
            <th style="min-width: 80px;" aria-sort="other">載具資訊</th>
            <th style="min-width: 150px" aria-sort="other">管理</th>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _items)
            {
                idx++;
                Html.RenderPartial(_dataItemView, item);
            }
        %>
    </tbody>
    <tfoot>
        <tr>
            <td colspan="10">&nbsp;
<%  if (_sort != null && _sort.Length > 0)
    { %>
<script>
    $(function () {
        var sort = <%= JsonConvert.SerializeObject(_sort) %>;
        $('.itemList th').each(function (idx, elmt) {
            var $this = $(this);
            if(sort.indexOf(idx)>=0) {
                $this.attr('aria-sort', 'ascending');
                $this.append('<i class="fa fa-sort-asc" aria-hidden="true"></i>')
                    .append($('<input type="hidden" name="sort"/>').val(idx));
            } else if(sort.indexOf(-idx)>=0) {
                $this.attr('aria-sort', 'desending');
                $this.append('<i class="fa fa-sort-desc" aria-hidden="true"></i>')
                    .append($('<input type="hidden" name="sort"/>').val(-idx));
            }
        });
    });
</script>
<%  } %>
<script>
    $(function () {

        var chkBox = $(".itemList input[name='chkAll']");
        var chkItem = $(".itemList input[name='chkItem']");
        chkBox.click(function () {
            chkItem.prop('checked', chkBox.is(':checked'));
        });

        chkItem.click(function (e) {
            if (!$(this).is(':checked')) {
                chkBox.prop('checked', false);
            }
        });

        $('.itemList th').each(function (idx, elmt) {
            var $this = $(this);
            if (!$this.is('[aria-sort="other"]')) {
                if(!$this.is('[aria-sort]')) {
                    $this.append('<i class="fa fa-sort" aria-hidden="true"></i>')
                        .append('<input type="hidden" name="sort"/>');
                    $this.attr('aria-sort', 'none');
                }
                $this.on('click', function (evt) {
                    var $target = $(this);
                    $target.find('i').remove();
                    if ($target.is('[aria-sort="none"]')) {
                        $target.append('<i class="fa fa-sort-asc" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'ascending');
                        $target.find('input[name="sort"]').val(idx);
                    } else if ($target.is('[aria-sort="ascending"]')) {
                        $target.append('<i class="fa fa-sort-desc" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'descending');
                        $target.find('input[name="sort"]').val(-idx);
                    } else {
                        $target.append('<i class="fa fa-sort" aria-hidden="true"></i>');
                        $target.attr('aria-sort', 'none');
                        $target.find('input[name="sort"]').val('');
                    }
                    uiInvoiceQuery.inquire(<%= (int)ViewBag.PageIndex + 1 %>,function(data){
                        var $node = $('.itemList').next();
                        $('.itemList').remove();
                        $node.before(data);
                    });
                });
            }
        });
    });
</script>
            </td>
        </tr>
    </tfoot>
</table>

<script runat="server">

    IEnumerable<InvoiceItem> _items;
    IEnumerable<InvoiceItem> _model;
    IOrderedEnumerable<InvoiceItem> _order;
    int[] _sort;
    int _pageSize;
    String _dataItemView;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceItem>)this.Model;
        _sort = (int[])ViewBag.Sort;
        _pageSize = (int)ViewBag.PageSize;
        _dataItemView = ViewBag.DataItemView ?? "~/Views/InvoiceProcess/Module/DataItem.ascx";

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
                case 1:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceDate) : _order.ThenBy(i => i.InvoiceDate);
                    break;
                case -1:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceDate) : _order.ThenByDescending(i => i.InvoiceDate);
                    break;
                case 2:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceBuyer.CustomerID) : _order.ThenBy(i => i.InvoiceBuyer.CustomerID);
                    break;
                case -2:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceBuyer.CustomerID) : _order.ThenByDescending(i => i.InvoiceBuyer.CustomerID);
                    break;
                case 3:
                    _order = _order == null ? _model.OrderBy(i => i.InvoicePurchaseOrder.OrderNo) : _order.ThenBy(i => i.InvoicePurchaseOrder.OrderNo);
                    break;
                case -3:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoicePurchaseOrder.OrderNo) : _order.ThenByDescending(i => i.InvoicePurchaseOrder.OrderNo);
                    break;
                case 4:
                    _order = _order == null ? _model.OrderBy(i => i.SellerID) : _order.ThenBy(i => i.SellerID);
                    break;
                case -4:
                    _order = _order == null ? _model.OrderByDescending(i => i.SellerID) : _order.ThenByDescending(i => i.SellerID);
                    break;
                case 5:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceSeller.ReceiptNo) : _order.ThenBy(i => i.InvoiceSeller.ReceiptNo);
                    break;
                case -5:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceSeller.ReceiptNo) : _order.ThenByDescending(i => i.InvoiceSeller.ReceiptNo);
                    break;
                case 7:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAmountType.SalesAmount) : _order.ThenBy(i => i.InvoiceAmountType.SalesAmount);
                    break;
                case -7:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAmountType.SalesAmount) : _order.ThenByDescending(i => i.InvoiceAmountType.SalesAmount);
                    break;
                case 8:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAmountType.TaxAmount) : _order.ThenBy(i => i.InvoiceAmountType.TaxAmount);
                    break;
                case -8:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAmountType.TaxAmount) : _order.ThenByDescending(i => i.InvoiceAmountType.TaxAmount);
                    break;
                case 9:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAmountType.TotalAmount) : _order.ThenBy(i => i.InvoiceAmountType.TotalAmount);
                    break;
                case -9:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAmountType.TotalAmount) : _order.ThenByDescending(i => i.InvoiceAmountType.TotalAmount);
                    break;
                case 10:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceWinningNumber!=null ? i.InvoiceWinningNumber.UniformInvoiceWinningNumber.Rank : 0) : _order.ThenBy(i => i.InvoiceWinningNumber!=null ? i.InvoiceWinningNumber.UniformInvoiceWinningNumber.Rank : 0);
                    break;
                case -10:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceWinningNumber!=null ? i.InvoiceWinningNumber.UniformInvoiceWinningNumber.Rank : 0) : _order.ThenByDescending(i => i.InvoiceWinningNumber!=null ? i.InvoiceWinningNumber.UniformInvoiceWinningNumber.Rank : 0);
                    break;
                case 11:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceBuyer.ReceiptNo) : _order.ThenBy(i => i.InvoiceBuyer.ReceiptNo);
                    break;
                case -11:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceBuyer.ReceiptNo) : _order.ThenByDescending(i => i.InvoiceBuyer.ReceiptNo);
                    break;
                case 6:
                    _order = _order == null ? _model.OrderBy(i => i.TrackCode).ThenBy(i => i.No) : _order.ThenBy(i => i.TrackCode).ThenBy(i => i.No);
                    break;
                case -6:
                    _order = _order == null ? _model.OrderByDescending(i => i.TrackCode).ThenByDescending(i => i.No) : _order.ThenByDescending(i => i.TrackCode).ThenByDescending(i => i.No);
                    break;
            }
        }
    }

</script>
