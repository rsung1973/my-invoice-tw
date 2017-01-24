<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireInvoice.ascx.cs" Inherits="eIVOGo.Module.JsGrid.InquireInvoice" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagName="UrlRadioDirective" TagPrefix="uc8" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceSeller.ascx" TagPrefix="uc4" TagName="InquireInvoiceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceDate.ascx" TagPrefix="uc4" TagName="InquireInvoiceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceNo.ascx" TagPrefix="uc4" TagName="InquireInvoiceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc4" TagName="DataModelCache" %>
<%@ Register Src="~/Module/Inquiry/QueryPrintableInfo.ascx" TagPrefix="uc4" TagName="QueryPrintableInfo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc4" TagName="SMSNotification" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceConsumption.ascx" TagPrefix="uc4" TagName="InquireInvoiceConsumption" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceB2CKind.ascx" TagPrefix="uc4" TagName="InquireInvoiceB2CKind" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PurchaseOrderNo.ascx" TagPrefix="uc4" TagName="PurchaseOrderNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceAttachment.ascx" TagPrefix="uc4" TagName="InvoiceAttachment" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceAttachment.ascx" TagPrefix="uc4" TagName="InquireInvoiceAttachment" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireQuerySizePerPage.ascx" TagPrefix="uc4" TagName="InquireQuerySizePerPage" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PrintBackFooter.ascx" TagPrefix="uc4" TagName="PrintBackFooter" %>
<%@ Register Src="~/Module/Handler/DoPrintInvoice.ascx" TagPrefix="uc4" TagName="DoPrintInvoice" %>
<%@ Register Src="~/Module/JsGrid/InvoiceItemPrintCheckList.ascx" TagPrefix="uc4" TagName="InvoiceItemPrintCheckList" %>
<%@ Register Src="~/Module/Common/DoPrintHandler.ascx" TagPrefix="uc4" TagName="DoPrintHandler" %>
<%@ Register Src="~/Module/Common/DoPrintPOSHandler.ascx" TagPrefix="uc4" TagName="DoPrintPOSHandler" %>
<%@ Register Src="~/Module/Common/DoDownloadHandler.ascx" TagPrefix="uc4" TagName="DoDownloadHandler" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceType.ascx" TagPrefix="uc4" TagName="InquireInvoiceType" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceBuyer.ascx" TagPrefix="uc4" TagName="InquireInvoiceBuyer" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceBuyerName.ascx" TagPrefix="uc4" TagName="InquireInvoiceBuyerName" %>



<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 發票列印" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="發票列印" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <uc8:UrlRadioDirective ID="urlGo" runat="server" DefaultName="電子發票" />
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
            <uc4:InquireInvoiceConsumption runat="server" ID="inquireConsumption" />
            <uc4:InquireInvoiceSeller runat="server" ID="inquireSeller" QueryRequired="true" AlertMessage="請選擇公司名稱!!" />
            <uc4:InquireCustomerID runat="server" ID="inquireCustomerID" />
            <uc4:InquireInvoiceBuyer runat="server" ID="inquireBuyer" />
            <uc4:InquireInvoiceBuyerName runat="server" ID="inquireBuyerName" />
            <uc4:InquireInvoiceDate runat="server" ID="inquireDate" />
            <uc4:InquireInvoiceNo runat="server" ID="inquireNo" />
            <uc4:InquireInvoiceAttachment runat="server" ID="inquireAttachment" />
            <uc4:InquireQuerySizePerPage runat="server" ID="inquireSize" />
        </asp:PlaceHolder>
    </table>
    <!--表格 結束-->
</div>
<!--按鈕-->
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <asp:Button ID="btnQuery" CssClass="btn" runat="server" Text="查詢" OnClick="btnQuery_Click" />
        </td>
    </tr>
</table>
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢結果" Visible="false" />
<!--表格 開始-->
<% if(btnQuery.CommandArgument == "Query") { %>
<div class="border_gray">
    <uc4:QueryPrintableInfo runat="server" ID="queryInfo" />
    <uc4:InvoiceItemPrintCheckList runat="server" ID="itemList" />
    <!--按鈕-->
</div>
    <% if(itemList.Select().Count()<=10000) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <uc4:DoPrintHandler runat="server" ID="doPrint" />
             &nbsp;&nbsp;
            <uc4:DoPrintPOSHandler runat="server" ID="doPrintPOS" />
            &nbsp;&nbsp;
            <uc4:DoDownloadHandler runat="server" ID="doDownload" />
        </td>
    </tr>
</table>
<%     } 
   }%>
<uc4:DataModelCache runat="server" ID="modelItem" KeyName="defaultEntity" />
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<%--<script>
    $(function () {
        $("input[type='radio'][name='cc1']").change(
            function () {
                if ($("input[type='radio'][name='cc1'][value='B2C']")[0].checked) {
                    $("#b2cKind div").css("display", "block");
                } else {
                    $("#b2cKind div").css("display", "none");
                }
            }).change();
    });
</script>--%>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)urlGo).CreatePrintQuery();

        doDownload.DoAction = arg =>
            {
                this.buildQueryItem();
                Response.CsvDownload(itemList.GetCsvResult(itemList.Select().ToList()), null, Encoding.GetEncoding(950), "text/csv");
            };

        doPrint.DoAction = arg =>
            {
                ((ASP.module_common_doprinthandler_ascx)doPrint).DoPrintInvoice();
            };
        doPrintPOS.DoAction = arg =>
        {
            ((ASP.module_common_doprintposhandler_ascx)doPrintPOS).DoPrintInvoicePOS();
        };

    }

    protected override void buildQueryItem()
    {
        if (((ASP.module_inquiry_inquiryitem_inquireinvoiceconsumption_ascx)inquireConsumption).QueryForB2C)
        {
            _queryExpr = _queryExpr.And(i => i.DonateMark == "0"
                && (i.PrintMark == "Y" || (i.PrintMark == "N" && i.InvoiceWinningNumber != null))
                && !i.CDS_Document.DocumentPrintLog.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
        }        
        base.buildQueryItem();
    }

    
</script>
