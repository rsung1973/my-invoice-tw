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
        IEnumerable<InvoiceItem> items = (IEnumerable<InvoiceItem>)Model;
        models = TempData.GetModelSource<InvoiceItem>();

        Response.CsvDownload(GetCsvResult(items), null, "text/csv");
    }

    public IEnumerable<object> GetCsvResult(IEnumerable<InvoiceItem> items)
    {
        return items.Select(d => new
        {
            愛心碼 = d.InvoiceDonation != null ? d.InvoiceDonation.AgencyCode : "",
            開立發票營業人 = d.InvoiceSeller.CustomerName,
            發票號碼 = d.TrackCode + d.No,
            是否中獎 = d.InvoiceWinningNumber != null ? "是" : "否"

        });
    }      

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>