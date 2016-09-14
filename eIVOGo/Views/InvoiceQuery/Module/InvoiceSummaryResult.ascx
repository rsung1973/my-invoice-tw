<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/InquireInvoiceTemplate.ascx" TagPrefix="uc4" TagName="InquireInvoiceTemplate" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>

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
<uc6:functiontitlebar id="resultTitle" runat="server" itemname="查詢結果" />
<div class="border_gray">
    <% 
       Html.RenderPartial("~/Views/Module/InvoiceSummaryList.ascx", models); 
    %>
    <!--按鈕-->
</div>
<%   if(models.Items.Count() > 0) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <% Html.RenderPartial("~/Views/Module/PrintData.ascx"); %>
            <input type="button" value="Excel下載" name="btnExcel" class="btn" onclick="$('form').prop('action', '<%= Url.Action("CreateInvoiceSummaryXlsx") %>    ').submit();" />
        </td>
    </tr>
</table>
<%  } %>
<% } %>

<script runat="server">
    ModelSource<InvoiceItem> models;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        models.DataSourcePath = VirtualPathUtility.ToAbsolute("~/InvoiceQuery/InvoiceSummaryGridPage");
        
    }

</script>