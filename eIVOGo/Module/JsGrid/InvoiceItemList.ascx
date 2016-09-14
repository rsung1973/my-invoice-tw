<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InvoiceItemList.ascx.cs"
    Inherits="eIVOGo.Module.JsGrid.InvoiceItemList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" id="gridInit" FieldName="fields" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
<script>

    var fields = [
        {
            "name": "InvoiceDate",
            "type": "text",
            "title": "日期",
            "width": "80",
            "align": "left"
        },
        {
            "name": "CustomerID",
            "type": "text",
            "title": "GoogleID/客戶ID",
            "width": "100",
            "align": "left"
        },
        {
            "name": "OrderNo",
            "type": "text",
            "title": "序號",
            "width": "100",
            "align": "left"
        },
        {
            "name": "SellerName",
            "type": "text",
            "title": "開立發票營業人",
            "width": "260",
            "align": "left"
        },
        {
            "name": "SellerReceiptNo",
            "type": "text",
            "title": "統編",
            "width": "80",
            "align": "center"
        },
        {
            "name": "InvoiceNo",
            "type": "text",
            "title": "發票號碼",
            "width": "100",
            "align": "center",
            itemTemplate: function (value, item) {
                return $('<a>')
                    .attr('href', '#')
                    .html(value)
                    .on('click', function (evt) {
                        showInvoiceModal(item.DocID);
                    });
            }
        },
        {
            "name": "SalesAmount",
            "type": "text",
            "title": "未稅金額",
            "width": "100",
            "align": "right"
        },
        {
            "name": "TaxAmount",
            "type": "text",
            "title": "稅額",
            "width": "100",
            "align": "right",
            footerTemplate: function () { return "總計金額："; }
        },
        {
            "name": "TotalAmount",
            "type": "text",
            "title": "含稅金額",
            "width": "100",
            "align": "right",
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", Select().Sum(i => i.InvoiceItem.InvoiceAmountType.TotalAmount)) %>"; }

        },
            {
                "name": "Winning",
                "type": "text",
                "title": "是否中獎",
                "width": "80",
                "align": "left"
            },
            {
                "name": "BuyerReceiptNo",
                "type": "text",
                "title": "買受人統編",
                "width": "80",
                "align": "center"
            },
            {
                "name": "Remark",
                "type": "text",
                "title": "備註",
                "width": "120",
                "align": "left"
            },
            {
                "name": "SMS",
                "type": "text",
                "title": "簡訊通知",
                "width": "60",
                "align": "center"
            },
            {
                "name": "CarrierNo",
                "type": "text",
                "title": "載具資訊",
                "width": "80",
                "align": "left"
            },
            {
                "name": "Donation",
                "type": "text",
                "title": "愛心碼",
                "width": "80",
                "align": "left"
            }

    ];

</script>
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

    public override IEnumerable<object> GetCsv4BuyerAddrResult(IEnumerable<CDS_Document> items)
    {
        return items.Select(d => new
        {
            發票號碼 = d.InvoiceItem.TrackCode + d.InvoiceItem.No,
            發票日期 = String.Format("{0:yyyy/MM/dd}", d.InvoiceItem.InvoiceDate),
            買受人姓名 = d.InvoiceItem.InvoiceBuyer.ContactName,
            買受人地址 = d.InvoiceItem.InvoiceBuyer.Address,
            開立人統編 = d.InvoiceItem.InvoiceSeller.ReceiptNo,
            開立人名稱 = d.InvoiceItem.InvoiceSeller.CustomerName
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
                            CarrierNo = d.InvoiceItem.InvoiceCarrier != null ? d.InvoiceItem.InvoiceCarrier.CarrierNo : ""
                        });
    }

    protected override string doJsonSerialize(System.Web.Script.Serialization.JavaScriptSerializer serializer, int pageIndex, int pageSize)
    {
        var items = this.Select().OrderByDescending(d => d.DocID);

        return serializer.Serialize(
                new
                {
                    data = GetQueryResult(items.Skip(pageIndex * pageSize).Take(pageSize)
                        .ToArray()),
                    itemsCount = items.Count()
                });
    }
    
</script>