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
            AgencyCode = d.InvoiceDonation != null ? d.InvoiceDonation.AgencyCode : "",
            SellerName = d.InvoiceSeller.CustomerName,
            InvoiceNo = d.TrackCode + d.No,
            IsWinning = d.InvoiceWinningNumber != null ? "是" : "否"
        });
    }    

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>
