<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.Inquiry.InquireInvoiceBasic" %>
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
<%@ Register Src="~/Module/Inquiry/QueryResultInfo.ascx" TagPrefix="uc4" TagName="QueryResultInfo" %>
<%@ Register Src="~/Module/Base/InvoiceItemCheckList.ascx" TagPrefix="uc4" TagName="InvoiceItemCheckList" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/SMSNotification.ascx" TagPrefix="uc4" TagName="SMSNotification" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceConsumption.ascx" TagPrefix="uc4" TagName="InquireInvoiceConsumption" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceB2CKind.ascx" TagPrefix="uc4" TagName="InquireInvoiceB2CKind" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PurchaseOrderNo.ascx" TagPrefix="uc4" TagName="PurchaseOrderNo" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/InvoiceAttachment.ascx" TagPrefix="uc4" TagName="InvoiceAttachment" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireInvoiceAttachment.ascx" TagPrefix="uc4" TagName="InquireInvoiceAttachment" %>
<%@ Register Src="~/Module/Inquiry/InquiryItem/InquireQuerySizePerPage.ascx" TagPrefix="uc4" TagName="InquireQuerySizePerPage" %>
<%@ Register Src="~/Module/EIVO/InvoiceField/PrintBackFooter.ascx" TagPrefix="uc4" TagName="PrintBackFooter" %>
<%@ Register Src="~/Module/Handler/DoPrintInvoice.ascx" TagPrefix="uc4" TagName="DoPrintInvoice" %>

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
            <%--<uc4:InquireInvoiceB2CKind runat="server" ID="inquireB2CKind" />--%>
            <uc4:InquireInvoiceSeller runat="server" ID="inquireSeller" QueryRequired="true" AlertMessage="請選擇公司名稱!!" />
            <uc4:InquireCustomerID runat="server" ID="inquireCustomerID" />
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <!--表格 開始-->
        <div class="border_gray" id="resultInfo" visible="false" enableviewstate="false" runat="server">
            <uc4:QueryResultInfo runat="server" ID="queryInfo" />
            <uc4:InvoiceItemCheckList runat="server" ID="itemList" />
            <!--按鈕-->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnQuery" />
    </Triggers>
</asp:UpdatePanel>
<table id="tblAction" runat="server" visible="false" enableviewstate="false" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <uc4:DoPrintInvoice runat="server" ID="doPrint" />
        </td>
    </tr>
</table>
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
        this.PreRender += module_inquiry_inquireinvoicetoprint_ascx_PreRender;

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)urlGo).NamingDirection
            = new String[] { 
                QueryType.電子發票.ToString(), "~/SAM/PrintInvoices.aspx",
                QueryType.電子折讓單.ToString(), "~/SAM/PrintAllowances.aspx",
                QueryType.中獎發票.ToString(), "~/SAM/PrintWinningInvoices.aspx"};

        var field = new TemplateField
        {
            HeaderText = "序號",
            ItemTemplate = new Uxnet.Web.WebUI.ItemTemplateCreator
            {
                BuildControl = () => new ASP.module_eivo_invoicefield_purchaseorderno_ascx()
            }
        };
        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        itemList.Grid.Columns.Insert(3,field);

        
        field = new TemplateField
        {
            HeaderText = "附件檔",
            ItemTemplate = new Uxnet.Web.WebUI.ItemTemplateCreator
            {
                BuildControl = () => new ASP.module_eivo_invoicefield_invoiceattachment_ascx()
            }
        };
        field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        field.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
        itemList.Grid.Columns.Add(field);

        ((TemplateField)itemList.Grid.Columns[0]).FooterTemplate = new Uxnet.Web.WebUI.ItemTemplateCreator
            {
                BuildControl = () => new ASP.module_eivo_invoicefield_printbackfooter_ascx()
            };
        ((DataControlField)itemList.Grid.Columns[1]).FooterText = "列印發票背面";

        itemList.Grid.PageSize = inquireSize.PageSize;

        _queryExpr = i => i.InvoiceCancellation == null;

    }

    void module_inquiry_inquireinvoicetoprint_ascx_PreRender(object sender, EventArgs e)
    {
        tblAction.Visible = resultInfo.Visible;
    }

    protected override void buildQueryItem()
    {
        if (inquireConsumption.QueryForB2C)
        {
            _queryExpr = _queryExpr.And(i => i.DonateMark == "0"
                && (i.PrintMark == "Y" || (i.PrintMark == "N" && i.InvoiceWinningNumber != null))
                && !i.CDS_Document.DocumentPrintLog.Any(l => l.TypeID == (int)Naming.DocumentTypeDefinition.E_Invoice));
        }
        
        base.buildQueryItem();
    }

    
</script>
