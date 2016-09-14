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
<%@ Register Src="~/Module/Common/DoDownloadHandler.ascx" TagPrefix="uc4" TagName="DoDownloadHandler" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagPrefix="uc4" TagName="ActionHandler" %>





<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<uc5:PageAction ID="actionItem" runat="server" ItemName="首頁 > 資料管理" />
<!--交易畫面標題-->
<uc6:FunctionTitleBar ID="functionTitle" runat="server" ItemName="註銷作業" />
<div class="border_gray">
    <!--表格 開始-->
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="left_title">
        <uc8:UrlRadioDirective ID="urlGo" runat="server" DefaultName="電子發票" />
        <asp:PlaceHolder ID="inquiryHolder" runat="server">
            <uc4:InquireInvoiceConsumption runat="server" ID="inquireConsumption" />
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
<!--表格 開始-->
<% if(btnQuery.CommandArgument == "Query") { %>
<div class="border_gray">
    <uc4:InvoiceItemPrintCheckList runat="server" ID="itemList" />
    <!--按鈕-->
</div>
    <% if(itemList.Select().Count()<=10000) {   %>
<table id="tblAction" width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="Bargain_btn">
            <input type="button" class="btn" name="btnPrint" value="發票註銷" onclick="<%= doVoid.GetPostBackEventReference(null) %>" />
        </td>
    </tr>
</table>
<%     } 
   }%>
<uc4:DataModelCache runat="server" ID="modelItem" KeyName="defaultEntity" />
<cc1:DocumentDataSource ID="dsEntity" runat="server"></cc1:DocumentDataSource>
<uc4:ActionHandler runat="server" ID="doVoid" />

<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        ((ASP.module_inquiry_inquiryitem_urlradiodirective_ascx)urlGo).CreateVoidQuery();

        doVoid.DoAction = arg =>
            {
                String[] ar = Request.GetItemSelection();
                if (ar != null && ar.Count() > 0)
                {
                    String outbound = Model.Properties.Settings.Default.C0401Outbound.Replace("C0401", "C0701");
                    outbound.CheckStoredPath();
                    var mgr = dsEntity.CreateDataManager();

                    foreach (var id in ar.Select(a => int.Parse(a)))
                    {
                        var item = mgr.GetTable<InvoiceItem>().Where(i => i.InvoiceID == id).FirstOrDefault();
                        if (item != null)
                        {
                            String invoiceNo = item.TrackCode + item.No;
                            var voidItem = new Model.Schema.TurnKey.C0701.VoidInvoice
                            {
                                VoidInvoiceNumber = invoiceNo,
                                InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
                                BuyerId = item.InvoiceBuyer.ReceiptNo,
                                SellerId = item.InvoiceSeller.ReceiptNo,
                                VoidDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                                VoidTime = DateTime.Now.ToString("HH:mm:ss"),
                                VoidReason = "註銷重開",
                                Remark = ""
                            };
                            voidItem.ConvertToXml().Save(Path.Combine(outbound, "C0701_" + invoiceNo + ".xml"));
                            item.CDS_Document.CurrentStep = (int)Naming.B2CInvoiceLevelDefinition.已註銷;
                            mgr.SubmitChanges();
                        }
                    }
                    
                    Page.AjaxAlert("註銷發票資料已送出!!");
                }
                else
                {
                    Page.AjaxAlert("請選擇註銷發票資料!!");
                }
                
                DoDefaultQuery = true;
            };
    }

    protected override void buildQueryItem()
    {
        _queryExpr = _queryExpr.And(i => i.CDS_Document.CurrentStep != (int)Naming.B2CInvoiceLevelDefinition.已註銷);
        base.buildQueryItem();
    }

    
</script>
