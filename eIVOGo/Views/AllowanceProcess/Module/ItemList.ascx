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
            <th style="min-width: 160px;">開立發票營業人</th>
            <th style="min-width: 80px;">統編</th>
            <th style="min-width: 120px;">折讓號碼</th>
            <th style="min-width: 80px;">未稅金額</th>
            <th style="min-width: 80px;">稅額</th>
            <th style="min-width: 80px;">含稅金額</th>
            <th style="min-width: 90px;">買受人統編</th>
            <th style="min-width: 120px;" aria-sort="other">備註</th>
            <%--<th style="min-width: 150px" aria-sort="other">管理</th>--%>
        </tr>
    </thead>
    <tbody>
        <%  int idx = 0;
            foreach (var item in _items)
            {
                idx++;
                Html.RenderPartial("~/Views/AllowanceProcess/Module/DataItem.ascx", item);
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
                    uiAllowanceQuery.inquire(<%= (int)ViewBag.PageIndex + 1 %>,function(data){
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

    IEnumerable<InvoiceAllowance> _items;
    IEnumerable<InvoiceAllowance> _model;
    IOrderedEnumerable<InvoiceAllowance> _order;
    int[] _sort;
    int _pageSize;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceAllowance>)this.Model;
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
                case 1:
                    _order = _order == null ? _model.OrderBy(i => i.AllowanceDate) : _order.ThenBy(i => i.AllowanceDate);
                    break;
                case -1:
                    _order = _order == null ? _model.OrderByDescending(i => i.AllowanceDate) : _order.ThenByDescending(i => i.AllowanceDate);
                    break;
                case 2:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAllowanceBuyer.CustomerID) : _order.ThenBy(i => i.InvoiceAllowanceBuyer.CustomerID);
                    break;
                case -2:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAllowanceBuyer.CustomerID) : _order.ThenByDescending(i => i.InvoiceAllowanceBuyer.CustomerID);
                    break;
                case 3:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAllowanceSeller.SellerID) : _order.ThenBy(i => i.InvoiceAllowanceSeller.SellerID);
                    break;
                case -3:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAllowanceSeller.SellerID) : _order.ThenByDescending(i => i.InvoiceAllowanceSeller.SellerID);
                    break;
                case 4:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAllowanceSeller.ReceiptNo) : _order.ThenBy(i => i.InvoiceAllowanceSeller.ReceiptNo);
                    break;
                case -4:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAllowanceSeller.ReceiptNo) : _order.ThenByDescending(i => i.InvoiceAllowanceSeller.ReceiptNo);
                    break;
                case 5:
                    _order = _order == null ? _model.OrderBy(i => i.AllowanceNumber) : _order.ThenBy(i => i.AllowanceNumber);
                    break;
                case -5:
                    _order = _order == null ? _model.OrderByDescending(i => i.AllowanceNumber) : _order.ThenByDescending(i => i.AllowanceNumber);
                    break;
                case 6:
                    _order = _order == null ? _model.OrderBy(i => i.TotalAmount - i.TaxAmount) : _order.ThenBy(i => i.TotalAmount - i.TaxAmount);
                    break;
                case -6:
                    _order = _order == null ? _model.OrderByDescending(i => i.TotalAmount - i.TaxAmount) : _order.ThenByDescending(i => i.TotalAmount - i.TaxAmount);
                    break;
                case 7:
                    _order = _order == null ? _model.OrderBy(i => i.TaxAmount) : _order.ThenBy(i => i.TaxAmount);
                    break;
                case -7:
                    _order = _order == null ? _model.OrderByDescending(i => i.TaxAmount) : _order.ThenByDescending(i => i.TaxAmount);
                    break;
                case 8:
                    _order = _order == null ? _model.OrderBy(i => i.TotalAmount) : _order.ThenBy(i => i.TotalAmount);
                    break;
                case -8:
                    _order = _order == null ? _model.OrderByDescending(i => i.TotalAmount) : _order.ThenByDescending(i => i.TotalAmount);
                    break;
                case 9:
                    _order = _order == null ? _model.OrderBy(i => i.InvoiceAllowanceBuyer.ReceiptNo) : _order.ThenBy(i => i.InvoiceAllowanceBuyer.ReceiptNo);
                    break;
                case -9:
                    _order = _order == null ? _model.OrderByDescending(i => i.InvoiceAllowanceBuyer.ReceiptNo) : _order.ThenByDescending(i => i.InvoiceAllowanceBuyer.ReceiptNo);
                    break;
            }
        }
    }

</script>
