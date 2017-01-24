<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
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
<script>
    $(function () {
        var $postForm;
        if (!window.uiAccountQuery) {
            window.uiAccountQuery = {};
        }

        window.uiAccountQuery.inquire = function (pageNum, onPaging) {
            var event = event || window.event;
            var $form;
            var $queryAction;
            if (event.target) {
                $form = $(event.target).closest('form');
                $queryAction = $(event.target).closest('.queryAction');
            }
            if (!$form) {
                alert('查詢表單資料錯誤!!');
                return;
            }
            if (pageNum) {
                $('input[name="pageIndex"]').val(pageNum);
            } else {
                $('input[name="sort"]').remove();
            }
            $form.ajaxForm({
                url: "<%= Url.Action("Inquire","Account",new { resultAction = ViewBag.ResultAction }) %>",
                beforeSubmit: function () {
                    showLoading();
                },
                success: function (data) {
                    if (data) {
                        if (onPaging) {
                            onPaging(data);
                        } else {
                            if (uiAccountQuery.$result)
                                uiAccountQuery.$result.remove();
                            uiAccountQuery.$result = $(data);
                            $queryAction.after(uiAccountQuery.$result);
                        }
                    }
                    hideLoading();
                },
                error: function () {
                    hideLoading();
                }
            }).submit();
        };

        window.uiAccountQuery.edit = function (uid) {

            var event = event || window.event;
            var $tr = $(event.target).closest('tr');

            if ($postForm) {
                $postForm.remove();
            }
            $('<form method="post">').load('<%= Url.Action("EditItem","UserProfile",new { SellerID = _viewModel.SellerID, DefaultRoleID = _viewModel.RoleID }) %>', { 'uid': uid }, function (evt) {
                $postForm = $(this);
                $postForm.dialog({
                    width: 640,
                    <%--                    height: height,
                    buttons: [
                        {
                            text: "關閉",
                            icons: {
                                primary: "ui-icon-close"
                            },
                            click: function () {
                                $(this).dialog("close").remove();
                            }
                        }
                    ],--%>
                    close: function (evt, ui) {
                        $postForm.remove();
                        $postForm = null;
                        if (uid) {
                            uiAccountQuery.dataItem($tr, uid);
                        } else {
                            uiAccountQuery.inquire();
                        }
                    }
                });
            });

        };

        window.uiAccountQuery.showItem = function (value) {
            var event = event || window.event;
            var $tr = $(event.target).closest('tr');
            uiAccountQuery.dataItem($tr, value);
        };

        window.uiAccountQuery.dataItem = function ($tr, value) {
            $.post('<%= Url.Action("DataItem","Account") %>', 'id=' + value, function (data) {
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

        window.uiAccountQuery.sendConfirmation = function (value) {
            $.post('<%= Url.Action("SendConfirmation","Account") %>', 'id=' + value, function (data) {
                if (data) {
                    var $data = $(data);
                    $('body').append($data);
                    $data.remove();
                }
            });
        };

        window.uiAccountQuery.activate = function (value) {
            if (confirm('確認啟用此帳號?')) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("Activate","Account") %>', { id: value }, function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            alert('資料已更新!!');
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            }
        };

        window.uiAccountQuery.deactivate = function (value) {
            if (confirm('確認停用此帳號?')) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("Deactivate","Account") %>', { id: value }, function (data) {
                    if (data) {
                        var $data = $(data);
                        if ($data.is('tr')) {
                            $tr.before($data);
                            alert('資料已更新!!');
                            $tr.remove();
                        } else {
                            $('body').append($data);
                            $data.remove();
                        }
                    }
                });
            }
        };

        window.uiAccountQuery.delete = function (value) {
            if (confirm('確認刪除此帳號?')) {
                var event = event || window.event;
                var $tr = $(event.target).closest('tr');
                $.post('<%= Url.Action("DeleteItem","Account") %>', { id: value }, function (data) {
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
    UserAccountQueryViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (UserAccountQueryViewModel)ViewBag.ViewModel;
    }
</script>

