<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "新增/修改發票號碼區間"); %>

<div class="border_gray">
    <!--表格 開始-->
    <table class="left_title" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tbody>
            <tr>
                <th class="Head_style_a" colspan="2">
                    查詢條件
                </th>
            </tr>
            <tr>
                <th width="20%" nowrap>
                    發票年度（民國年）
                </th>
                <td class="tdleft">
                    <select name="year">
                    <%  for (int yy = 2012; yy <= DateTime.Now.Year + 1; yy++)
                        { %>
                            <option value="<%= yy %>"><%= yy-1911 %></option>
                    <%  } %>
                    </select>
                    <script>
                        $(function () {
                            $('select[name="year"]').val(<%= DateTime.Now.Year %>);
                        });
                    </script>
                </td>
            </tr>
            <tr>
                <th>
                    發票期別
                </th>
                <td class="tdleft">
                    <select name="periodNo">
                        <option value="">全部</option>
                        <option value="1">01-02月</option>
                        <option value="2">03-04月</option>
                        <option value="3">05-06月</option>
                        <option value="4">07-08月</option>
                        <option value="5">09-10月</option>
                        <option value="6">11-12月</option>
                    </select>
                    <script>
                        $(function () {
                            $('select[name="periodNo"]').val(<%= (DateTime.Now.Month+1)/2 %>);
                        });
                    </script>
                </td>
            </tr>
        </tbody>
    </table>

    <!--表格 結束-->
</div>
<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiTrackCodeNo.inquire();" >查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->


<script>
    var uiTrackCodeNo;
    $(function () {
        uiTrackCodeNo = {
            $result: null,
            commitItem: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                clearErrors();
                $.post('<%= Url.Action("CommitItem","InvoiceNo") %>', 'intervalID=' + (value ? value : '') + '&' + $.param($tr.find('input,select,textarea')), function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            if (value) {
                                alert('資料已更新!!');
                                $tr.remove();
                            }
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            },
            inquire: function (pageNum, onPaging) {
                var event = event || window.event;
                var $form = $(event.target).closest('form');
                $form.ajaxForm({
                    url: "<%= Url.Action("InquireInterval","InvoiceNo") %>",
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            if (uiTrackCodeNo.$result)
                                uiTrackCodeNo.$result.remove();
                            uiTrackCodeNo.$result = $(data);
                            $('.queryAction').after(uiTrackCodeNo.$result);
                        }
                        hideLoading();
                    },
                    error: function () {
                        hideLoading();
                    }
                }).submit();
            },
            editItem: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("EditNoInterval","InvoiceNo") %>', 'id=' + value, function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            },
            deleteItem: function (value) {
                if (confirm('確定刪除此筆資料?')) {
                    var event = event || window.event;
                    var $tr = $(event.target).closest('tr');
                    $.post('<%= Url.Action("DeleteNoInterval","InvoiceNo") %>', { 'id': value }, function (data) {
                        if (data.result) {
                            alert('資料已刪除!!')
                            $tr.remove();
                        } else {
                            alert(data.message);
                        }
                    });
                }
            },
            showItem: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("IntervalItem","InvoiceNo") %>', 'id=' + value, function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
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
