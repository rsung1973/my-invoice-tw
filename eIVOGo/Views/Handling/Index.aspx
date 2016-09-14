<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="eIVOGo.Helper" %>

<script>
    $(function () {
        alert('<%= HttpUtility.JavaScriptStringEncode((String)ViewBag.Message) %>');
    });
</script>
<script runat="server">

    public override void Dispose()
    {
        var models = TempData.GetGenericModelSource();
        if (models != null)
            models.Dispose();

        base.Dispose();
    }
    
</script>
