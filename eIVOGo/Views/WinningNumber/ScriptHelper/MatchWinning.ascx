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
        if (!window.uiWinningNo) {
            window.uiWinningNo = {};
        }
        uiWinningNo.match = function () {
            $('#theForm').ajaxForm({
                url: "<%= Url.Action("MatchWinningInvoiceNo","WinningNumber",new { resultAction = ViewBag.ResultAction }) %>",
                beforeSubmit: function () {
                    showLoading();
                },
                success: function (data) {
                    hideLoading();
                    if (data) {
                        var $data = $(data);
                        $('body').append($data);
                        $data.remove();
                    }
                },
                error: function () {
                    hideLoading();
                }
            }).submit();
        };
        uiWinningNo.resetWinning = function () {
            $('#theForm').ajaxForm({
                url: "<%= Url.Action("ClearWinningInvoiceNo","WinningNumber",new { resultAction = ViewBag.ResultAction }) %>",
                beforeSubmit: function () {
                    showLoading();
                },
                success: function (data) {
                    hideLoading();
                    if (data) {
                        var $data = $(data);
                        $('body').append($data);
                        $data.remove();
                    }
                },
                error: function () {
                    hideLoading();
                }
            }).submit();
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

