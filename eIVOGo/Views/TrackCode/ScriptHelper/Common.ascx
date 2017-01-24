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
    $(function () {
        var $postForm;
        if (!window.uiTrackCodeQuery) {
            window.uiTrackCodeQuery = {};
        }

        window.uiTrackCodeQuery.inquire = function (pageNum, onPaging) {
            if (pageNum) {
                $('input[name="pageIndex"]').val(pageNum);
            } else {
                $('input[name="sort"]').remove();
            }
            $('#theForm').ajaxForm({
                url: "<%= Url.Action("Inquire","TrackCode",new { resultAction = ViewBag.ResultAction }) %>",
                beforeSubmit: function () {
                    showLoading();
                },
                success: function (data) {
                    if (data) {
                        if (onPaging) {
                            onPaging(data);
                        } else {
                            if (uiTrackCodeQuery.$result)
                                uiTrackCodeQuery.$result.remove();
                            uiTrackCodeQuery.$result = $(data);
                            $('.queryAction').after(uiTrackCodeQuery.$result);
                        }
                    }
                    hideLoading();
                },
                error: function () {
                    hideLoading();
                }
            }).submit();
        };

        window.uiTrackCodeQuery.edit = function (value) {

                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("EditItem","TrackCode") %>', 'id=' + value, function (data) {
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

        };

        window.uiTrackCodeQuery.showItem = function (value) {
            var event = event || window.event;
            var $tr = $(event.target).closest('tr');
            uiTrackCodeQuery.dataItem($tr, value);
        };

        window.uiTrackCodeQuery.dataItem = function ($tr, value) {
            $.post('<%= Url.Action("DataItem","TrackCode") %>', 'id=' + value, function (data) {
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
        };

        window.uiTrackCodeQuery.commitItem = function (value) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                clearErrors();
                $.post('<%= Url.Action("CommitItem","TrackCode") %>', 'trackID=' + (value ? value : '') + '&' + $.param($tr.find('input,select,textarea')), function (data) {
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

        window.uiTrackCodeQuery.delete = function (value) {
            if (confirm('確定刪除此筆資料?')) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("DeleteItem","TrackCode") %>', { 'id': value }, function (data) {
                    if (data.result) {
                        alert('資料已刪除!!')
                        $tr.remove();
                    } else {
                        alert(data.message);
                    }
                });
            }
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

