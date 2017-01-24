<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "電子發票開立"); %>
<form role="form" method="post">
    <div class="border_gray">
        <!--表格 開始-->
        <h2>營業人</h2>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th nowrap="nowrap" width="120">發票開立人
                </th>
                <td class="tdleft">
                    <%  Html.RenderAction("SellerSelector", "DataFlow"); %>
                </td>
                <th nowrap="nowrap" width="120">買受人統編
                </th>
                <td class="tdleft">
                    <input class="form-control" name="BuyerReceiptNo" type="text" />
                </td>
            </tr>
            <tr>
                <th nowrap="nowrap" width="120">隨機碼
                </th>
                <td class="tdleft" colspan="3">
                    <input class="form-control" name="RandomNo" type="text" required value="<%= _viewModel.RandomNo %>" maxlength="4" />
                </td>
            </tr>
        </table>
        <!--表格 結束-->
    </div>
    <div class="border_gray">
        <!--表格 開始-->
        <h2>發票</h2>
        <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
            <tr>
                <th colspan="4" class="Head_style_a">新增發票主檔
                </th>
            </tr>
            <tr>
                <th nowrap="nowrap" width="150">課稅別
                </th>
                <td class="tdleft">
                    <select class="form-control" name="TaxType">
                        <option value="1">應稅</option>
                        <option value="2">零稅率</option>
                        <option value="3">免稅</option>
                    </select>
                </td>
                <th nowrap="nowrap" width="150">買受人簽署適用零稅率註記
                </th>
                <td class="tdleft">
                    <select class="form-control" name="TaxType">
                        <option value="0">無</option>
                        <option value="1">買受人為園區事業</option>
                        <option value="2">買受人為遠洋漁業</option>
                        <option value="3">買受人為保稅區(自由貿易港區)</option>
                    </select>
                </td>
            </tr>
            <tr>
                <th nowrap="nowrap">稅率
                </th>
                <td class="tdleft">
                    <input class="form-control" name="TaxRate" type="text" value="0.05" />
                </td>
                <th nowrap="nowrap">銷售額合計(新台幣)
                </th>
                <td class="tdleft">
                    <input class="form-control" name="SalesAmount" type="text" value="" />
                </td>
            </tr>
            <tr>
                <th nowrap="nowrap" width="150">營業稅額
                </th>
                <td class="tdleft">
                    <input class="form-control" name="TaxAmount" type="text" value="" />
                </td>
                <th nowrap="nowrap" width="150">總計(含稅)
                </th>
                <td class="tdleft">
                    <input class="form-control" name="TotalAmount" type="text" value="" />
                </td>
            </tr>
        </table>
    </div>
    <div class="border_gray">
        <!--表格 開始-->
        <h2>發票明細</h2>
        <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline left_title">
            <thead>
                <tr>
                    <th>產品編號</th>
                    <th>品名</th>
                    <th>單價</th>
                    <th>數量</th>
                    <th>小計</th>
                    <th>備註</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <%  Html.RenderPartial("~/Views/InvoiceBusiness/InvoiceProduct/AddItem.ascx"); %>
            </tbody>
        </table>
    </div>
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <td class="Bargain_btn">
                    <a class="btn" onclick="uiInvoice.sum();">小計</a>
                    <a class="btn" onclick="uiInvoice.preview();">發票預覽</a>
                    <a class="btn" onclick="uiInvoice.commit();">發票開立</a>
                    <button type="reset" class="btn">清除</button>
                </td>
            </tr>
        </tbody>
    </table>
</form>
<iframe name="commit" width="0" height="0"></iframe>
<script>
    var uiInvoice;
    $(function () {
        uiInvoice = {
            preview: function () {
                //var event = event || window.event;
                //validateForm($(event.target).parents('form')[0]);
                if (this.sum()) {
                    var event = event || window.event;
                    var $form = $(event.target).parents('form');
                    var prnWin = window.open('', '_preview', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=320,height=600');
                    prnWin.name = '_preview';
                    $form[0].target = '_preview';
                    $form.prop('action', '<%= Url.Action("PreviewInvoice","InvoiceBusiness") %>').submit();
                }
            },
            commit: function () {
                if (this.sum()) {
                    var event = event || window.event;
                    var $form = $(event.target).parents('form');
                    //window.open('', 'commit', 'toolbar=no,location=no,status=no,menubar=no,scrollbars=auto,resizable=yes,alwaysRaised,dependent,titlebar=no,width=320,height=600');
                    $form.prop('target', 'commit').prop('action', '<%= Url.Action("CommitInvoice","InvoiceBusiness") %>').submit();
                }
            },
            sum: function () {
                var event = event || window.event;
                var $form = $(event.target).parents('form');
                if (validateForm($(event.target).parents('form')[0])) {
                    var taxAmt = 0;
                    var totalAmt = 0;
                    var $piece = $form.find('input[name="Piece"]');
                    var $unitCost = $form.find('input[name="UnitCost"]');
                    var $costAmt = $form.find('input[name="CostAmount"]');

                    $piece.each(function (idx) {
                        var cost = math.multiply(math.number($piece.eq(idx).val()), math.number($unitCost.eq(idx).val()));
                        $costAmt.eq(idx).val(cost);
                        totalAmt += cost;
                    });
                    taxAmt = math.round(math.multiply(totalAmt, $form.find('input[name="TaxRate"]').val()));

                    var $buyer = $form.find('input[name="BuyerReceiptNo"]');
                    $buyer.val($.trim($buyer.val()));
                    if ($buyer.val() == '') {
                        $form.find('input[name="TotalAmount"]').val(totalAmt);
                        $form.find('input[name="TaxAmount"]').val(taxAmt);
                        $form.find('input[name="SalesAmount"]').val(totalAmt);
                    } else {
                        $form.find('input[name="TotalAmount"]').val(totalAmt/* + taxAmt*/);
                        $form.find('input[name="TaxAmount"]').val(taxAmt);
                        $form.find('input[name="SalesAmount"]').val(totalAmt);
                    }
                    return true;
                }
                return false;
            },
            addRow: function () {
                var event = event || window.event;
                var $tr = $(event.target).parents('tr');
                var $row = $tr.clone(true);
                $row.find('input').val('');
                $tr.before($row);
                $row.find('td').last().empty().append($('<a class="btn" onclick="uiInvoice.deleteRow();">刪除</a>'));
            },
            deleteRow: function () {
                if (confirm('確定刪除此項資料?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).parents('tr');
                    $tr.remove();
                }
            }
        };
    });
</script>

<script runat="server">

    InvoiceViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ViewBag.ActionName = "首頁 > 電子發票開立";
        _viewModel = (InvoiceViewModel)ViewBag.ViewModel;
    }
</script>
