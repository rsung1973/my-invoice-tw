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
        var $postFrame = $('<iframe name="dataFrame" width="0" height="0">').appendTo($('body'));
        if (!window.uiInvoiceQuery) {
            window.uiInvoiceQuery = {};
        }
        window.uiInvoiceQuery.C0401 = function () {
            if ($postForm) {
                $postForm.remove();
            }

            $postForm = $('<form method="post" target="dataFrame" />').prop('action', '<%= Url.Action("DownloadC0401","InvoiceProcess") %>')
                .css('display', 'none').appendTo($('body'));

            $('#theForm').serializeArray().forEach(function (item, index) {
                $('<input type="hidden">')
                    .prop('name', item.name).prop('value',item.value)
                    .appendTo($postForm);
            });
            $postForm.submit();
            //showLoading();
        };

        window.uiInvoiceQuery.C0701 = function () {
            if ($postForm) {
                $postForm.remove();
            }

            $postForm = $('<form method="post" target="dataFrame" />').prop('action', '<%= Url.Action("DownloadC0701","InvoiceProcess") %>')
                .css('display', 'none').appendTo($('body'));

            $('#theForm').serializeArray().forEach(function (item, index) {
                $('<input type="hidden">')
                    .prop('name', item.name).prop('value',item.value)
                    .appendTo($postForm);
            });
            $postForm.submit();
            //showLoading();
        };
        window.uiInvoiceQuery.C0501 = function () {
            if ($postForm) {
                $postForm.remove();
            }

            $postForm = $('<form method="post" target="dataFrame" />').prop('action', '<%= Url.Action("DownloadC0501","InvoiceProcess") %>')
                .css('display', 'none').appendTo($('body'));

            $('#theForm').serializeArray().forEach(function (item, index) {
                $('<input type="hidden">')
                    .prop('name', item.name).prop('value',item.value)
                    .appendTo($postForm);
            });
            $postForm.submit();
            //showLoading();
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

