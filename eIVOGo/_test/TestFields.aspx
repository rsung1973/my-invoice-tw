<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>
<%@ Register Src="~/Module/Common/JsGrid.ascx" TagPrefix="uc1" TagName="JsGrid" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Module.JsGrid" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
        <textarea id="fields" name="fields" cols="64" rows="20">
            <% if (_item != null)
               {
                   System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();%>
            var fields = <%= serializer.Serialize(JsGridHelper.EnumJsGridFields(_item))  %>;
            <% } %>
        </textarea>
    </form>
</body>
</html>
<script runat="server">
    
    Object _item;
    PropertyInfo[] _items;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["dataItem"] != null)
        {
            _item = Page.PreviousPage.Items["dataItem"];
        }
    }


</script>
