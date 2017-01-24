<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (ViewBag.Message != null)
    { %>
<script>
    $(function () {
        alert('<%= HttpUtility.JavaScriptStringEncode(ViewBag.Message) %>');
    });
</script>
<%  } %>
<%--<script>
    function smartAlert(message, hander) {
        $.SmartMessageBox({
            title: "<i class=\"fa fa-fw fa fa-check\" aria-hidden=\"true\"></i> " + message,
            content: "",
            buttons: '[關閉]'
        }, hander);
    }
</script>--%>
