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
       Html.RenderPartial("~/Views/Module/WinningInvoiceReport.ascx", models); 
    %>
</asp:Content>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ViewBag.ActionName = "首頁 > 發票作業";
    }

</script>
