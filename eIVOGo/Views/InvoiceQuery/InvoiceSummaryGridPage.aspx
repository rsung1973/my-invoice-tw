﻿<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        models = TempData.GetModelSource<InvoiceItem>();
        int recordCount;
        
        Response.Clear();
        Response.ContentType = "application/json";
        Response.Write(serializer.Serialize(
                new
                {
                    data = GetQueryResult(out recordCount),
                    itemsCount = recordCount
                }));
        Response.End();
    }

    public IEnumerable<object> GetQueryResult(out int recordCount)
    {
        var items = models.Items.GroupBy(i => i.SellerID);
        recordCount = items.Count();

        return items.Select(d => new
                {
                    SellerID = d.Key,
                    RecordCount = d.Count()
                })
            .Join(models.GetTable<Organization>(), i => i.SellerID, o => o.CompanyID,
                (i, o) => new
                    {
                        SellerName = o.CompanyName,
                        SellerReceiptNo = o.ReceiptNo,
                        i.RecordCount
                    })
            .OrderBy(r => r.SellerReceiptNo)
            .Skip(models.InquiryPageSize * models.InquiryPageIndex).Take(models.InquiryPageSize);

    }    

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>