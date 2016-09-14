<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Views/Module/InvoiceItemListTemplate.ascx" TagPrefix="uc1" TagName="InvoiceItemListTemplate" %>


<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<uc1:InvoiceItemListTemplate runat="server" ID="listTemplate">
    <JsGridFields>
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
                    "width": "120",
                    "align": "left"
                },
                {
                    "name": "OrderNo",
                    "type": "text",
                    "title": "序號",
                    "width": "240",
                    "align": "left"
                },
                {
                    "name": "SellerName",
                    "type": "text",
                    "title": "開立發票營業人",
                    "width": "160",
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
                    "width": "120",
                    "align": "center",
                    itemTemplate: function (value, item) {
                        return $('<a>')
                            .attr('href', '#')
                            .html(value)
                            .on('click', function (evt) {
                                showInvoiceModal(item.InvoiceID);
                            });
                    }
                },
                {
                    "name": "SalesAmount",
                    "type": "text",
                    "title": "未稅金額",
                    "width": "120",
                    "align": "right"
                },
                {
                    "name": "TaxAmount",
                    "type": "text",
                    "title": "稅額",
                    "width": "120",
                    "align": "right",
                    footerTemplate: function () { return "總計金額："; }
                },
                {
                    "name": "TotalAmount",
                    "type": "text",
                    "title": "含稅金額",
                    "width": "120",
                    "align": "right",
                    footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", listTemplate.ModelSource.Items.Sum(i => i.InvoiceAmountType.TotalAmount)) %>"; }

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
            }
    ];

        </script>

    </JsGridFields>
</uc1:InvoiceItemListTemplate>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        listTemplate.JsonSerialize = doJsonSerialize;
    }

    public IEnumerable<object> GetQueryResult(IEnumerable<InvoiceItem> items)
    {
        return items.Select(d => new
                        {
                            d.InvoiceID,
                            InvoiceDate = String.Format("{0:yyyy/MM/dd}", d.InvoiceDate),
                            d.InvoiceBuyer.CustomerID,
                            OrderNo = d.InvoicePurchaseOrder != null ? d.InvoicePurchaseOrder.OrderNo : null,
                            SellerName = d.InvoiceSeller.CustomerName,
                            SellerReceiptNo = d.InvoiceSeller.ReceiptNo,
                            InvoiceNo = d.TrackCode + d.No,
                            SalesAmount = d.InvoiceAmountType.SalesAmount,
                            TaxAmount = d.InvoiceAmountType.TaxAmount,
                            TotalAmount = d.InvoiceAmountType.TotalAmount,
                            Winning = d.InvoiceWinningNumber != null ? d.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A",
                            BuyerReceiptNo = d.InvoiceBuyer.IsB2C() ? "" : d.InvoiceBuyer.ReceiptNo,
                            Donation = d.InvoiceDonation != null ? d.InvoiceDonation.AgencyCode : "",
                            SMS = d.CDS_Document.SMSNotificationLogs.Any() ? "是" : "否",
                            Remark = String.Join("", d.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)),
                            CarrierNo = d.InvoiceCarrier != null ? d.InvoiceCarrier.CarrierNo : ""
                        });
    }

    protected string doJsonSerialize(System.Web.Script.Serialization.JavaScriptSerializer serializer, int pageIndex, int pageSize)
    {
        var items = listTemplate.ModelSource.Items.OrderByDescending(d => d.InvoiceID);

        return serializer.Serialize(
                new
                {
                    data = GetQueryResult(items.Skip(pageIndex * pageSize).Take(pageSize)
                        .ToArray()),
                    itemsCount = items.Count()
                });
    }
    
</script>
