<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models" %>
<%@ Import Namespace="Model.DataEntity" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        IEnumerable<WinningInvoiceReportItem> items = (IEnumerable<WinningInvoiceReportItem>)Model;
        models = TempData.GetModelSource<InvoiceItem>();
        
        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(serializer.Serialize(
                new
                {
                    data = items,
                    itemsCount = items.Count()
                }));
        Response.End();
    }
   

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>