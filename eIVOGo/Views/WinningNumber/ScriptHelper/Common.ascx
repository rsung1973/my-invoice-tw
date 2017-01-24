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
<script>
    var uiWinningNo;
    $(function () {
        var $postForm;
        uiWinningNo = {
            inquire: function (pageNum, onPaging) {
                if (pageNum) {
                    $('input[name="pageIndex"]').val(pageNum);
                } else {
                    $('input[name="sort"]').remove();
                }
                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("Inquire","WinningNumber",new { resultAction = ViewBag.ResultAction }) %>",
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            if (onPaging) {
                                onPaging(data);
                            } else {
                                if (uiWinningNo.$result)
                                    uiWinningNo.$result.remove();
                                uiWinningNo.$result = $(data);
                                $('.queryAction').after(uiWinningNo.$result);
                            }
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
                $.post('<%= Url.Action("EditItem","WinningNumber") %>', 'id=' + value, function (data) {
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
                    $.post('<%= Url.Action("DeleteItem","WinningNumber") %>', { 'id': value }, function (data) {
                        if (data.result) {
                            alert('資料已刪除!!')
                            $tr.remove();
                            uiWinningNo.inquire();
                        } else {
                            alert(data.message);
                        }
                    });
                }
            },
            commitItem: function (value, year, period) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                clearErrors();
                $.post('<%= Url.Action("CommitItem","WinningNumber") %>', 'WinningID=' + (value ? value : '') + '&Year=' + year + '&Period=' + period + '&' + $.param($tr.find('input,select,textarea')), function (data) {
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
            showItem: function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("DataItem","WinningNumber") %>', 'id=' + value, function (data) {
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

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

    }
</script>

