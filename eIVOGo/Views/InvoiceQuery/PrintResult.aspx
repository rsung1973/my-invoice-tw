<%@ Page Title="" Language="C#" MasterPageFile="~/template/ContentPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" StylesheetTheme="Visitor" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    <% 
       Html.RenderPartial("~/Views/Module/HidePostData.ascx");
       Html.RenderPartial("~/Views/Module/InvoiceItemList.ascx", models); 
    %>
</asp:Content>
<script runat="server">

    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/InvoiceQuery/GridPage");
    }

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();

        base.Dispose();
    }

</script>
