<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.InvoiceItemList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>
<%@ Register Src="~/Module/JsGrid/DataField/InvoiceNo.ascx" TagPrefix="uc1" TagName="InvoiceNo" %>
<%@ Register Src="~/Module/JsGrid/DataField/Attachment.ascx" TagPrefix="uc1" TagName="Attachment" %>
<%@ Register Src="~/Module/JsGrid/DataField/CheckBox.ascx" TagPrefix="uc1" TagName="CheckBox" %>
<%@ Register Src="~/Module/JsGrid/DataField/JsGridField.ascx" TagPrefix="uc1" TagName="JsGridField" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" id="gridInit" FieldName="fields" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
<asp:PlaceHolder ID="phFields" runat="server">
    <script>
        var $cbxFooter = function () {
            return $('<input id="printBack" name="printBack" type="checkbox" value="True"/>')
                .prop('checked',<%= Request["printBack"]==null?"false":"true"  %>);
        };

        var fields = [];
    </script>
    <uc1:CheckBox runat="server" ID="CheckBox" FieldVariable="fields" name="DocID" footerTemplate="$cbxFooter" />
    <uc1:JsGridField runat="server" ID="InvoiceDate" FieldVariable="fields" name="InvoiceDate" title="日期" width="80" align="center" footerTemplate="function() { return '兌獎訊息';}" />
    <uc1:JsGridField runat="server" ID="SellerName" FieldVariable="fields" name="SellerName" title="開立發票營業人" width="260" align="left" />
    <uc1:JsGridField runat="server" ID="OrderNo" FieldVariable="fields" name="OrderNo" title="序號" width="140" align="left" />
    <uc1:JsGridField runat="server" ID="SellerReceiptNo" FieldVariable="fields" name="SellerReceiptNo" title="統編" width="80" align="center" />
    <uc1:InvoiceNo runat="server" ID="InvoiceNo" FieldVariable="fields" width="80" />
    <script>
        fields[fields.length] =
        {
            "name": "TaxAmount",
            "type": "text",
            "title": "稅額",
            "width": "80",
            "align": "right",
            footerTemplate: function () { return "總計金額："; }
        };
        fields[fields.length] =
        {
            "name": "TotalAmount",
            "type": "text",
            "title": "含稅金額",
            "width": "80",
            "align": "right",
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", Select().Sum(i => i.InvoiceItem.InvoiceAmountType.TotalAmount)) %>"; }
        };
    </script>
    <uc1:JsGridField runat="server" ID="Winning" FieldVariable="fields" name="Winning" title="是否中獎" width="80" align="center" />
    <uc1:Attachment runat="server" ID="Attachment" FieldVariable="fields" width="100" />
</asp:PlaceHolder>
<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
<script runat="server">

    public override IEnumerable<object> GetCsvResult(IEnumerable<CDS_Document> items)
    {
        return items.Select(d => new
        {
            發票日期 = String.Format("{0:yyyy/MM/dd}", d.InvoiceItem.InvoiceDate),
            客戶ID = d.InvoiceItem.InvoiceBuyer.CustomerID,
            序號 = d.InvoiceItem.InvoicePurchaseOrder != null ? d.InvoiceItem.InvoicePurchaseOrder.OrderNo : null,
            發票開立人 = d.InvoiceItem.InvoiceSeller.CustomerName,
            開立人統編 = d.InvoiceItem.InvoiceSeller.ReceiptNo,
            發票號碼 = d.InvoiceItem.TrackCode + d.InvoiceItem.No,
            未稅金額 = d.InvoiceItem.InvoiceAmountType.SalesAmount,
            稅額 = d.InvoiceItem.InvoiceAmountType.TaxAmount,
            含稅金額 = d.InvoiceItem.InvoiceAmountType.TotalAmount,
            是否中獎 = d.InvoiceItem.InvoiceWinningNumber != null ? d.InvoiceItem.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A",
            買受人統編 = d.InvoiceItem.InvoiceBuyer.IsB2C() ? "" : d.InvoiceItem.InvoiceBuyer.ReceiptNo,
            愛心碼 = d.InvoiceItem.InvoiceDonation != null ? d.InvoiceItem.InvoiceDonation.AgencyCode : "",
            簡訊通知 = d.SMSNotificationLogs.Any() ? "是" : "否",
            備註 = String.Join("", d.InvoiceItem.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)),
            載具號碼 = d.InvoiceItem.InvoiceCarrier != null ? d.InvoiceItem.InvoiceCarrier.CarrierNo : ""
        });
    }

    public override IEnumerable<object> GetQueryResult(IEnumerable<CDS_Document> items)
    {
        return items.Select(d => new
                        {
                            d.DocID,
                            InvoiceDate = String.Format("{0:yyyy/MM/dd}", d.InvoiceItem.InvoiceDate),
                            d.InvoiceItem.InvoiceBuyer.CustomerID,
                            OrderNo = d.InvoiceItem.InvoicePurchaseOrder != null ? d.InvoiceItem.InvoicePurchaseOrder.OrderNo : null,
                            SellerName = d.InvoiceItem.InvoiceSeller.CustomerName,
                            SellerReceiptNo = d.InvoiceItem.InvoiceSeller.ReceiptNo,
                            InvoiceNo = d.InvoiceItem.TrackCode + d.InvoiceItem.No,
                            SalesAmount = d.InvoiceItem.InvoiceAmountType.SalesAmount,
                            TaxAmount = d.InvoiceItem.InvoiceAmountType.TaxAmount,
                            TotalAmount = d.InvoiceItem.InvoiceAmountType.TotalAmount,
                            Winning = d.InvoiceItem.InvoiceWinningNumber != null ? d.InvoiceItem.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A",
                            BuyerReceiptNo = d.InvoiceItem.InvoiceBuyer.IsB2C() ? "" : d.InvoiceItem.InvoiceBuyer.ReceiptNo,
                            Donation = d.InvoiceItem.InvoiceDonation != null ? d.InvoiceItem.InvoiceDonation.AgencyCode : "",
                            SMS = d.SMSNotificationLogs.Any() ? "是" : "否",
                            Remark = String.Join("", d.InvoiceItem.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)),
                            CarrierNo = d.InvoiceItem.InvoiceCarrier != null ? d.InvoiceItem.InvoiceCarrier.CarrierNo : "",
                            Attachment = d.Attachment.Select(a => new { a.KeyName }).ToArray()
                        });
    }

    protected override string doJsonSerialize(System.Web.Script.Serialization.JavaScriptSerializer serializer, int pageIndex, int pageSize)
    {
        var items = this.Select().OrderBy(i => i.InvoiceItem.InvoiceID); //OrderByDescending(d => d.DocID);

        return serializer.Serialize(
                new
                {
                    data = GetQueryResult(items.Skip(pageIndex * pageSize).Take(pageSize)
                        .ToArray()),
                    itemsCount = items.Count()
                });
    }
    
</script>