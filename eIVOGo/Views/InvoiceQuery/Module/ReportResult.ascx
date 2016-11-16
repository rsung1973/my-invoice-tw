<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/InquireInvoiceTemplate.ascx" TagPrefix="uc4" TagName="InquireInvoiceTemplate" %>
<%@ Register Src="~/Module/Inquiry/QueryPrintableInfo.ascx" TagPrefix="uc4" TagName="QueryPrintableInfo" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Views/InvoiceQuery/Module/DownloadResult.ascx" TagPrefix="uc4" TagName="DownloadResult" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>

<% Html.RenderPartial("Module/InvoiceReport", Model); %>
<% if(!models.InquiryHasError) {  %>
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "查詢結果"); %>
<div class="border_gray">
    <uc4:QueryPrintableInfo runat="server" ID="queryInfo" />
    <% 
       Html.RenderPartial("~/Views/Module/InvoiceItemList.ascx", models); 
    %>
    <!--按鈕-->
</div>
<uc4:DownloadResult runat="server" ID="DownloadResult" />
<% } %>
<script runat="server">
    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/InvoiceQuery/GridPage");
        
    }

</script>