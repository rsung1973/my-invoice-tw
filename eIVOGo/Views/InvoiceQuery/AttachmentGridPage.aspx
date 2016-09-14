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
            DocID = d.InvoiceID,
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
            Attachment = d.CDS_Document.Attachment.Select(a => new { a.KeyName }).ToArray()
        });
    }    

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>