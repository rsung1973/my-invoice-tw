<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
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
<%@ Import Namespace="eIVOGo.Helper" %>

<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <%  Html.RenderPartial(_inquiryView); %>
</asp:Content>
<script runat="server">

    String _inquiryView;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ViewBag.ActionName = "首頁 > 發票作業";
        _inquiryView = (String)ViewBag.InquiryView ?? "~/Views/InvoiceProcess/InvoiceQuery.ascx";
    }

    //public override void Dispose()
    //{
    //    var models = TempData.GetGenericModelSource();
    //    if (models != null)
    //        models.Dispose();

    //    base.Dispose();
    //}

</script>