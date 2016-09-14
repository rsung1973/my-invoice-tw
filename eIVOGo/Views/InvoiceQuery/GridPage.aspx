<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        IEnumerable<InvoiceItem> items = (IEnumerable<InvoiceItem>)Model;
        models = TempData.GetModelSource<InvoiceItem>();
        
        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(serializer.Serialize(
                new
                {
                    data = GetQueryResult(items),
                    itemsCount = models.Items.Count()
                }));
        Response.End();
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
            d.InvoiceBuyer.CustomerName,
            d.InvoiceBuyer.ContactName,
            d.InvoiceBuyer.Address,
            d.InvoiceBuyer.EMail,
            Donation = d.InvoiceDonation != null ? d.InvoiceDonation.AgencyCode : "",
            SMS = d.CDS_Document.SMSNotificationLogs.Any() ? "是" : "否",
            Remark = String.Join("", d.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark)),
            CarrierNo = d.InvoiceCarrier != null ? d.InvoiceCarrier.CarrierNo : ""
        });
    }    

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>