<%@ Page Title="" Language="C#" MasterPageFile="~/template/ContentPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" StylesheetTheme="Visitor" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="mainContent" runat="server">
    查詢條件：
    <% 
        ((CommonInquiry<InvoiceItem>)this.Model).RenderQueryMessage(Html);
        Html.RenderPartial("~/Views/Module/HidePostData.ascx");
       Html.RenderPartial("~/Views/Module/DonatedInvoiceReport.ascx", models); 
    %>
</asp:Content>
<script runat="server">

    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/DonatedInvoice/ReportGridPage");
    }

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();

        base.Dispose();
    }

</script>
