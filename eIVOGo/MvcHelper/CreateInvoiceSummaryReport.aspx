<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="ClosedXML.Excel" %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    Model.Security.MembershipManagement.UserProfileMember _userProfile;
    
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _userProfile = Business.Helper.WebPageUtility.UserProfile;
        
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        models = (ModelSource<InvoiceItem>)_userProfile["modelSource"];
        _userProfile.Remove("modelSource");

        createReport(models.Items);
    }

    void createReport(IEnumerable<InvoiceItem> dataSource)
    {
        var items = models.Items.GroupBy(i => i.SellerID)
            .Select(d => new
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
            .OrderBy(r => r.SellerReceiptNo);
        
        using(XLWorkbook wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("發票資料統計");
            int colIdx = 1;
            ws.Cell(1, colIdx++).Value = "開立發票營業人";
            ws.Cell(1, colIdx++).Value = "統編";
            ws.Cell(1, colIdx++).Value = "發票筆數";

            var row = ws.Row(2);
            foreach(var item in items)
            {
                colIdx = 1;
                row.Cell(colIdx++).Value =  item.SellerName;
                row.Cell(colIdx++).Value =  item.SellerReceiptNo;
                row.Cell(colIdx++).Value =  item.RecordCount;

                row = row.RowBelow();
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("發票資料統計.xlsx")));

            wb.SaveAs(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
    }      

    public override void Dispose()
    {
        if (models != null)
            models.Dispose();
        
        base.Dispose();
    }

</script>