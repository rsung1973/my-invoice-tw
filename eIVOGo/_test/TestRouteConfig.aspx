<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Schema.TXN" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="theForm" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
    </form>
</body>
</html>
<script>
    $(function () {
        var msg = 'hello...';
        var a = [0,1,2,3,4,5,6];
        $('form').append($('<input type="checkbox" />')
            .val(0)
            .on('click', function (evt) {
                var $this = $(this);
                var i = parseInt($this.val());
                alert(msg + ':' + a[i]);
                $this.val((++i) % 7);
            }));
    });
</script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        System.Web.Routing.RouteTable.Routes.IgnoreRoute("");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("DefaultPage", "", "~/Login.aspx");        
    }
</script>
