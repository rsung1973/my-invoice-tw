<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "折讓證明查詢／列印／匯出"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <%  Html.RenderPartial("~/Views/InvoiceProcess/Module/QueryDirective.ascx", 1); %>
        <tr>
            <th>發票類別
            </th>
            <td class="tdleft">
                <select name="Consumption">
                    <option value="">全部</option>
                    <option value="B2B">B2B</option>
                    <option value="B2C">B2C</option>
                </select>
            </td>
        </tr>
        <%  Html.RenderAction("BySeller", "InquireInvoice",new { fieldName = "CompanyID" }); %>
        <tr>
            <th>發票／折讓單號碼
            </th>
            <td class="tdleft">
                <input class="form-control" name="DataNo" type="text" />
            </td>
        </tr>
        <tr>
            <th>買受人統編
            </th>
            <td class="tdleft">
                <input class="form-control" name="BuyerReceiptNo" type="text" />
            </td>
        </tr>
        <tr>
            <th>買受人名稱
            </th>
            <td class="tdleft">
                <input class="form-control" name="BuyerName" type="text" />
            </td>
        </tr>
        <tr>
            <th width="20%">日期區間
            </th>
            <td class="tdleft">自&nbsp;<input id="DateFrom" name="DateFrom" type="text" size="10" readonly="readonly" value="" />
                &nbsp;至&nbsp;
                <input id="DateTo" name="DateTo" type="text" size="10" readonly="readonly" value="" />
                <script>
                    $(function () {
                        $('#DateFrom').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
                        $('#DateTo').datepicker({ showButtonPanel: true, changeYear: true, changeMonth: true, yearRange: '2012:+0' });
                    });
                </script>
            </td>
        </tr>
        <tr>
            <th>已作廢
            </th>
            <td class="tdleft">
                <select name="Cancelled">
                    <option value="False">未作廢</option>
                    <option value="True">己作廢</option>
                </select>
            </td>
        </tr>
        <tr>
            <th>每頁資料筆數
            </th>
            <td class="tdleft">
                <input name="pageSize" type="text" value="<%= Request["pageSize"] ?? Uxnet.Web.Properties.Settings.Default.PageSize.ToString() %>" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiAllowanceQuery.inquire();">查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->
<script>
    var uiAllowanceQuery;
    $(function () {
        var $postForm;
        uiAllowanceQuery = {
            inquire: function (pageNum, onPaging) {
                if (pageNum) {
                    $('input[name="pageIndex"]').val(pageNum);
                } else {
                    $('input[name="sort"]').remove();
                }
                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("Inquire","AllowanceProcess") %>",
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            if (onPaging) {
                                onPaging(data);
                            } else {
                                if (uiAllowanceQuery.$result)
                                    uiAllowanceQuery.$result.remove();
                                uiAllowanceQuery.$result = $(data);
                                $('.queryAction').after(uiAllowanceQuery.$result);
                            }
                        }
                        hideLoading();
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },
            print: function (style) {
                if (!$('input[name="chkItem"]').is(':checked')) {
                    alert('請選擇列印資料!!');
                    return false;
                }

                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("Print","AllowanceProcess") %>" + '?paperStyle=' + style,
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        hideLoading();
                        if (data) {
                            var $data = $(data);
                            $data.dialog();
                            $data.find('a').on('click', function (evt) {
                                $data.dialog('close');
                            });
                        }
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },
            download: function () {
                if ($postForm) {
                    $postForm.remove();
                }

                $postForm = $('<form method="post" />').prop('action', '<%= Url.Action("CreateXlsx","AllowanceProcess") %>')
                    .css('display', 'none').appendTo($('body'));

                $('#theForm').serializeArray().forEach(function (item, index) {
                    $('<input type="hidden">')
                        .prop('name', item.name).prop('value', item.value)
                        .appendTo($postForm);
                });
                $postForm.submit();
                //showLoading();
            },
        };
    });
</script>
<script runat="server">


    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
</script>

