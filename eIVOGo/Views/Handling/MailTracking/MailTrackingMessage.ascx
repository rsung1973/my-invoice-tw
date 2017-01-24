<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<!--交易畫面標題-->
<%  Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "登錄掛號郵件"); %>
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <tr>
            <th>發票號碼(起號)
            </th>
            <td class="tdleft">
                <input name="StartNo" type="text" />
            </td>
            <th>發票號碼(迄號)
            </th>
            <td class="tdleft">
                <input name="EndNo" type="text" />
            </td>
        </tr>
        <tr>
            <th>寄件過程
            </th>
            <td class="tdleft">
                <input name="DeliveryStatus" value="1303" type="radio" />初次寄送
                <input name="DeliveryStatus" value="1309" type="radio" />退回重寄
            </td>
            <th>郵件張數
            </th>
            <td class="tdleft">
                <input name="MailingCount" type="text" />
            </td>
        </tr>
        <tr>
            <th>郵件號碼1(流水號碼段)
            </th>
            <td class="tdleft">
                <input name="MailNo1" type="text" />
            </td>
            <th>郵件號碼2(固定號碼段)
            </th>
            <td class="tdleft">
                <input name="MailNo2" type="text" />
            </td>
        </tr>
        <tr>
            <th>寄件日期
            </th>
            <td class="tdleft" colspan="3">
                <input name="DeliveryDate" type="text" class="form_date" />
            </td>
        </tr>
    </table>
    <!--表格 結束-->
</div>

<!--按鈕-->
<table border="0" cellspacing="0" cellpadding="0" width="100%" class="queryAction">
    <tbody>
        <tr>
            <td class="Bargain_btn">
                <button type="button" onclick="uiHandling.inquireTracking();">查詢</button>
            </td>
        </tr>
    </tbody>
</table>
<!--表格 開始-->
<%  Html.RenderPartial("~/Views/Handling/ScriptHelper/Common.ascx"); %>
<script runat="server">

    ModelSource<InvoiceItem> models;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;

        //if (ViewBag.HasQuery)
        //{
        //    ModelSource<InvoiceItem> models = TempData.GetModelSource<InvoiceItem>();

        //    if (((ASP.module_inquiry_inquiryitem_inquireinvoiceconsumption_ascx)inquireConsumption).QueryForB2C)
        //    {
        //        models.Items = models.Items.Where(i => i.DonateMark == "0"
        //            && (i.PrintMark == "Y" || (i.PrintMark == "N" && i.InvoiceWinningNumber != null))
        //            && !i.CDS_Document.DocumentPrintLog.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
        //    }

        //    result.Visible = true;
        //}
    }
</script>

