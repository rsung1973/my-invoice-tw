<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        IEnumerable<InvoiceItem> items = (IEnumerable<InvoiceItem>)Model;
        models = TempData.GetModelSource<InvoiceItem>();

        Response.CsvDownload(GetCsvResult(items), null, "text/csv");
    }

    public IEnumerable<object> GetCsvResult(IEnumerable<InvoiceItem> items)
    {
        return items.Select(d => new
        {
            發票號碼 = d.TrackCode + d.No,
            發票日期 = String.Format("{0:yyyy/MM/dd}", d.InvoiceDate),
            檔名 = d.CDS_Document.Attachment.Count > 0 ? d.CDS_Document.Attachment.First().KeyName : null,
            //客戶ID = d.InvoiceBuyer.CustomerID,
            //序號 = d.InvoicePurchaseOrder != null ? d.InvoicePurchaseOrder.OrderNo : null,
            //發票開立人 = d.InvoiceSeller.CustomerName,
            //開立人統編 = d.InvoiceSeller.ReceiptNo,
            //未稅金額 = d.InvoiceAmountType.SalesAmount,
            //稅額 = d.InvoiceAmountType.TaxAmount,
            //含稅金額 = d.InvoiceAmountType.TotalAmount,
            //是否中獎 = d.InvoiceWinningNumber != null ? d.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A",
            //買受人統編 = d.InvoiceBuyer.IsB2C() ? "" : d.InvoiceBuyer.ReceiptNo,
            //愛心碼 = d.InvoiceDonation != null ? d.InvoiceDonation.AgencyCode : "",
            //簡訊通知 = d.CDS_Document.SMSNotificationLogs.Any() ? "是" : "否",
            //備註 = String.Join("", d.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)),
            //載具號碼 = d.InvoiceCarrier != null ? d.InvoiceCarrier.CarrierNo : ""
        });
    }      

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>