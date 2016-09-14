<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InquireAllowance.ascx.cs" Inherits="eIVOGo.Module.JsGrid.InquireAllowance" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/UI/PageAction.ascx" TagName="PageAction" TagPrefix="uc5" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagName="FunctionTitleBar" TagPrefix="uc6" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/UrlRadioDirective.ascx" TagName="UrlRadioDirective" TagPrefix="uc8" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceSeller.ascx" TagPrefix="uc4" TagName="InquireAllowanceSeller" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceDate.ascx" TagPrefix="uc4" TagName="InquireAllowanceDate" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireAllowanceNo.ascx" TagPrefix="uc4" TagName="InquireAllowanceNo" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireCustomerID.ascx" TagPrefix="uc4" TagName="InquireCustomerID" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireSMSNotification.ascx" TagPrefix="uc4" TagName="InquireSMSNotification" %>
<%@ Register Src="~/Module/Common/DataModelCache.ascx" TagPrefix="uc4" TagName="DataModelCache" %>
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc4" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc4" TagName="SMSNotification" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PurchaseOrderNo.ascx" TagPrefix="uc4" TagName="PurchaseOrderNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceAttachment.ascx" TagPrefix="uc4" TagName="InvoiceAttachment" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireQuerySizePerPage.ascx" TagPrefix="uc4" TagName="InquireQuerySizePerPage" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PrintBackFooter.ascx" TagPrefix="uc4" TagName="PrintBackFooter" %>
<%@ Register Src="~/Module/JsGrid/InvoiceAllowanceList.ascx" TagPrefix="uc4" TagName="InvoiceAllowanceList" %>
<%@ Register Src="~/Module/Common/DoPrintHandler.ascx" TagPrefix="uc4" TagName="DoPrintHandler" %>
<%@ Register Src="~/Module/Common/DoDownloadHandler.ascx" TagPrefix="uc4" TagName="DoDownloadHandler" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 查詢發票/折讓" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="resultTitle" runat="server" ItemName="查詢發票折讓" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <uc8:UrlRadioDirective ID="urlGo" runat="server" DefaultName="電子折讓單" />
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
            <uc4:InquireAllowanceSeller runat="server" ID="inquireSeller" />
            <uc4:InquireAllowanceNo runat="server" ID="inquireNo" />
            <uc4:InquireAllowanceDate runat="server" ID="inquireDate" />
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
<% if(btnQuery.CommandArgument == "Query") { %>
<div class="border_gray">
    <uc4:InvoiceAllowanceList runat="server" id="itemList" />
    <!--按鈕-->
</div>
<% if(itemList.Select().Count()<=10000) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <uc4:DoPrintHandler runat="server" ID="doPrint" />
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

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)urlGo).CreateDefaultQuery();


        doDownload.DoAction = arg =>
        {
            this.buildQueryItem();

            Response.CsvDownload(itemList.GetCsvResult(itemList.Select().ToList()), null, Encoding.GetEncoding(950), "text/csv");
        };

        doPrint.DoAction = arg =>
        {
            this.buildQueryItem();
            itemList.AllowPaging = false;
            itemList.PrintMode = true;
            ((ASP.module_common_doprinthandler_ascx)doPrint).DoPrint(itemList.GetContent(), true);

        };

    }
    
</script>
