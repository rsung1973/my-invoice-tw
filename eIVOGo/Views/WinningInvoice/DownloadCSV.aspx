<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        IEnumerable<WinningInvoiceReportItem> items = (IEnumerable<WinningInvoiceReportItem>)Model;
        models = TempData.GetModelSource<InvoiceItem>();

        Response.CsvDownload(GetCsvResult(items), null, "text/csv");
    }

    public IEnumerable<object> GetCsvResult(IEnumerable<WinningInvoiceReportItem> items)
    {
        return items.Select(d => new
        {
            統編 = d.SellerReceiptNo,
            開立發票營業人 = d.SellerName,
            營業人地址 = d.Addr,
            中獎張數 = d.WinningCount,
            捐贈張數 = d.DonationCount

        });
    }      

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>