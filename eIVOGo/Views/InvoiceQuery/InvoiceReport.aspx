<%@ Page Title="" Language="C#" MasterPageFile="~/template/MvcMainPage.Master" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Src="~/Views/InvoiceQuery/Module/InvoiceReport.ascx" TagPrefix="uc1" TagName="InvoiceReport" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<asp:Content ID="header" ContentPlaceHolderID="headContent" runat="server">
</asp:Content>
<asp:Content ID="mainContent" ContentPlaceHolderID="formContent" runat="server">
    <uc1:InvoiceReport runat="server" ID="InvoiceReport" />
</asp:Content>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ViewBag.ActionName = "首頁 > 資料管理";
    }

    //public override void Dispose()
    //{
    //    var models = TempData.GetGenericModelSource();
    //    if (models != null)
    //        models.Dispose();

    //    base.Dispose();
    //}

</script>