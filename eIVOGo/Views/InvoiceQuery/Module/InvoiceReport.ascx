<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Views/InquireInvoice/ByWinningNumber.ascx" TagPrefix="uc5" TagName="ByWinningNumber" %>
<%@ Register Src="~/Views/InquireInvoice/ByInvoiceNo.ascx" TagPrefix="uc5" TagName="ByInvoiceNo" %>
<%@ Register Src="~/Views/InquireInvoice/ByCancellation.ascx" TagPrefix="uc5" TagName="ByCancellation" %>




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
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 資料管理" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="發票報表匯出" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th colspan="2" class="Head_style_a">查詢條件
            </th>
        </tr>
        <% 
            ((CommonInquiry<InvoiceItem>)this.Model).Render(Html);
        %>
        <uc5:ByInvoiceNo runat="server" ID="ByInvoiceNo" />
        <uc5:ByWinningNumber runat="server" ID="ByWinningNumber" />
        <uc5:ByCancellation runat="server" ID="ByCancellation" />
    </table>
    <!--表格 結束-->
</div>
<% 
    ((CommonInquiry<InvoiceItem>)this.Model).RenderAlert(Html);
%>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" value="查詢" name="btnQuery" class="btn" onclick="$('form').prop('action', '<%= Url.Action(ViewBag.QueryAction) %>    ').submit();" />
        </td>
    </tr>
</table>
<!--表格 開始-->
<script runat="server">

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = TempData.GetModelSource<InvoiceItem>();
        //if (ViewBag.HasQuery)
        //{
        //    ModelSource<InvoiceItem> models = TempData.GetModelSource<InvoiceItem>();

        //    if (((ASP.module_inquiry_inquiryitem_inquireinvoiceconsumption_ascx)inquireConsumption).QueryForB2C)
        //    {
        //        models.Items = models.Items.Where(i => i.DonateMark == "0"
        //            && (i.PrintMark == "Y" || (i.PrintMark == "N" && i.InvoiceWinningNumber != null))
        //            && !i.CDS_Document.DocumentPrintLogs.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
        //    }

        //    result.Visible = true;
        //}
    }
</script>

