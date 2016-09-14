<%@ Page Title="" Language="C#" MasterPageFile="~/template/main_page.Master" AutoEventWireup="true" CodeBehind="base_page.aspx.cs" Inherits="eIVOGo.template.base_page" StylesheetTheme="Visitor" %>

<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Register Src="~/Views/DataExchange/Module/DataIndex.ascx" TagPrefix="uc1" TagName="DataIndex" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
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