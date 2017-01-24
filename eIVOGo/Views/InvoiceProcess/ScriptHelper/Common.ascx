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
    var uiInvoiceQuery;
    $(function () {
        var $postForm;
        uiInvoiceQuery = {
            inquire: function (pageNum, onPaging) {
                if (pageNum) {
                    $('input[name="pageIndex"]').val(pageNum);
                } else {
                    $('input[name="sort"]').remove();
                }
                $('#theForm').ajaxForm({
                    url: "<%= Url.Action("Inquire","InvoiceProcess",new { resultAction = ViewBag.ResultAction }) %>",
                    beforeSubmit: function () {
                        showLoading();
                    },
                    success: function (data) {
                        if (data) {
                            if (onPaging) {
                                onPaging(data);
                            } else {
                                if (uiInvoiceQuery.$result)
                                    uiInvoiceQuery.$result.remove();
                                uiInvoiceQuery.$result = $(data);
                                $('.queryAction').after(uiInvoiceQuery.$result);
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
                    url: "<%= Url.Action("Print","InvoiceProcess") %>" + '?paperStyle=' + style,
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

                $postForm = $('<form method="post" />').prop('action', '<%= Url.Action("CreateXlsx","InvoiceProcess") %>')
                    .css('display', 'none').appendTo($('body'));

                $('#theForm').serializeArray().forEach(function (item, index) {
                    $('<input type="hidden">')
                        .prop('name', item.name).prop('value',item.value)
                        .appendTo($postForm);
                });
                $postForm.submit();
                //showLoading();
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

