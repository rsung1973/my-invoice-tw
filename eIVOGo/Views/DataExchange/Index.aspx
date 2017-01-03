<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Register Src="~/Views/DataExchange/Module/DataIndex.ascx" TagPrefix="uc1" TagName="DataIndex" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <uc1:DataIndex runat="server" ID="DataIndex" />
</asp:Content>
<script runat="server">

    public override void Dispose()
    {
        var models = TempData.GetGenericModelSource();
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>