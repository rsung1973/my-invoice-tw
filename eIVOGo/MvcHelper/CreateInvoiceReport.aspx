<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="System.Web.Script.Serialization" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
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
        
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        models = (ModelSource<InvoiceItem>)_userProfile["modelSource"];
        _userProfile.Remove("modelSource");

        //createReport(models.Items);
        createDataSetReport(models);
    }

    void createDataSetReport(ModelSource<InvoiceItem> models)
    {

        var items = models.Items.OrderBy(i => i.InvoiceID)
            .Select(i => new
            {
                發票號碼 = i.TrackCode + i.No,
                發票日期 = i.InvoiceDate,
                附件檔名 = i.CDS_Document.Attachment.Count > 0 ? i.CDS_Document.Attachment.First().KeyName : null,
                客戶ID = i.InvoiceBuyer.CustomerID,
                序號 = i.InvoicePurchaseOrder != null ? i.InvoicePurchaseOrder.OrderNo : null,
                發票開立人 = i.InvoiceSeller.CustomerName,
                開立人統編 = i.InvoiceSeller.ReceiptNo,
                未稅金額 = i.InvoiceAmountType.SalesAmount,
                稅額 = i.InvoiceAmountType.TaxAmount,
                含稅金額 = i.InvoiceAmountType.TotalAmount,
                買受人名稱 = i.InvoiceBuyer.CustomerName,
                買受人統編 = i.InvoiceBuyer.ReceiptNo,
                連絡人名稱 = i.InvoiceBuyer.ContactName,
                連絡人地址 = i.InvoiceBuyer.Address,
                買受人EMail = i.InvoiceBuyer.EMail,
                愛心碼 =  i.InvoiceDonation.AgencyCode,
                是否中獎 = i.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType,
                載具類別 = i.InvoiceCarrier.CarrierType,
                載具號碼 = i.InvoiceCarrier.CarrierNo,
                //備註 = String.Join("", i.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault())
                //    .Select(p => p.Remark))
            });

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Cache-control", "max-age=1");
        Response.ContentType = "message/rfc822";
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("發票資料明細.xlsx")));


        using (SqlCommand sqlCmd = (SqlCommand)models.GetCommand(items))
        {
            sqlCmd.Connection = (SqlConnection)models.GetDataContext().Connection;
            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd))
            {
                using (DataSet ds = new DataSet())
                {
                    adapter.Fill(ds);
                    ds.Tables[0].TableName = ds.DataSetName = "發票資料明細";
                    using (ClosedXML.Excel.XLWorkbook xls = new ClosedXML.Excel.XLWorkbook())
                    {
                        xls.Worksheets.Add(ds);
                        xls.SaveAs(Response.OutputStream);
                    }
                }
            }
        }

        Response.Flush();
        models.Dispose();
        Response.End();

    }

    void createDataSetReportSqlCommand(ModelSource<InvoiceItem> models)
    {

        var items = models.Items.OrderBy(i => i.InvoiceID)
            .Select(i => new
            {
                發票號碼 = i.TrackCode + i.No,
                發票日期 = i.InvoiceDate,
                附件檔名 = i.CDS_Document.Attachment.Count > 0 ? i.CDS_Document.Attachment.First().KeyName : null,
                客戶ID = i.InvoiceBuyer.CustomerID,
                序號 = i.InvoicePurchaseOrder != null ? i.InvoicePurchaseOrder.OrderNo : null,
                發票開立人 = i.InvoiceSeller.CustomerName,
                開立人統編 = i.InvoiceSeller.ReceiptNo,
                未稅金額 = i.InvoiceAmountType.SalesAmount,
                稅額 = i.InvoiceAmountType.TaxAmount,
                含稅金額 = i.InvoiceAmountType.TotalAmount,
                買受人名稱 = i.InvoiceBuyer.CustomerName,
                買受人統編 = i.InvoiceBuyer.IsB2C() ? "" : i.InvoiceBuyer.ReceiptNo,
                連絡人名稱 = i.InvoiceBuyer.ContactName,
                連絡人地址 = i.InvoiceBuyer.Address,
                買受人EMail = i.InvoiceBuyer.EMail,
                愛心碼 = i.InvoiceDonation != null ? i.InvoiceDonation.AgencyCode : "",
                是否中獎 = i.InvoiceWinningNumber != null ? i.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A",
                載具類別 = i.InvoiceCarrier != null ? i.InvoiceCarrier.CarrierType : "",
                載具號碼 = i.InvoiceCarrier != null ? i.InvoiceCarrier.CarrierNo : "",
                備註 = String.Join("", i.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault())
                    .Select(p => p.Remark))
            });

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Cache-control", "max-age=1");
        Response.ContentType = "message/rfc822";
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("發票資料明細.xlsx")));


        using (SqlCommand sqlCmd = (SqlCommand)models.GetCommand(items))
        {
            sqlCmd.Connection = (SqlConnection)models.GetDataContext().Connection;
            using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd))
            {
                using (DataSet ds = new DataSet())
                {
                    adapter.Fill(ds);
                    ds.Tables[0].TableName = ds.DataSetName = "發票資料明細";
                    using (ClosedXML.Excel.XLWorkbook xls = new ClosedXML.Excel.XLWorkbook())
                    {
                        xls.Worksheets.Add(ds);
                        xls.SaveAs(Response.OutputStream);
                    }
                }
            }
        }

        Response.Flush();
        models.Dispose();
        Response.End();

    }    

    void createReport(IEnumerable<InvoiceItem> items)
    {
        using(XLWorkbook wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("發票資料明細");
            int colIdx = 1;
            ws.Cell(1, colIdx++).Value = "發票號碼";
            ws.Cell(1, colIdx++).Value = "發票日期";
            ws.Cell(1, colIdx++).Value = "附件檔名";
            ws.Cell(1, colIdx++).Value = "客戶ID";
            ws.Cell(1, colIdx++).Value = "序號";
            ws.Cell(1, colIdx++).Value = "發票開立人";
            ws.Cell(1, colIdx++).Value = "開立人統編";
            ws.Cell(1, colIdx++).Value = "未稅金額";
            ws.Cell(1, colIdx++).Value = "稅額";
            ws.Cell(1, colIdx++).Value = "含稅金額";
            ws.Cell(1, colIdx++).Value = "買受人名稱";
            ws.Cell(1, colIdx++).Value = "買受人統編";
            ws.Cell(1, colIdx++).Value = "連絡人名稱";
            ws.Cell(1, colIdx++).Value = "連絡人地址";
            ws.Cell(1, colIdx++).Value = "買受人EMail";
            ws.Cell(1, colIdx++).Value = "愛心碼";
            ws.Cell(1, colIdx++).Value = "是否中獎";
            ws.Cell(1, colIdx++).Value = "載具類別";
            ws.Cell(1, colIdx++).Value = "載具號碼";
            ws.Cell(1, colIdx++).Value = "備註";

            var row = ws.Row(2);
            foreach(var item in items)
            {
                colIdx = 1;
                row.Cell(colIdx++).Value =  item.TrackCode + item.No;
                row.Cell(colIdx++).Value =  item.InvoiceDate;
                row.Cell(colIdx++).Value =  item.CDS_Document.Attachment.Count > 0 ? item.CDS_Document.Attachment.First().KeyName : null;
                row.Cell(colIdx++).Value =  item.InvoiceBuyer.CustomerID;
                row.Cell(colIdx++).Value =  item.InvoicePurchaseOrder != null ? item.InvoicePurchaseOrder.OrderNo : null;
                row.Cell(colIdx++).Value =  item.InvoiceSeller.CustomerName;
                row.Cell(colIdx++).Value =  item.InvoiceSeller.ReceiptNo;
                row.Cell(colIdx++).Value =  item.InvoiceAmountType.SalesAmount;
                row.Cell(colIdx++).Value =  item.InvoiceAmountType.TaxAmount;
                row.Cell(colIdx++).Value =  item.InvoiceAmountType.TotalAmount;
                row.Cell(colIdx++).Value = item.InvoiceBuyer.CustomerName;
                row.Cell(colIdx++).Value = item.InvoiceBuyer.IsB2C() ? "" : item.InvoiceBuyer.ReceiptNo;
                row.Cell(colIdx++).Value = item.InvoiceBuyer.ContactName;
                row.Cell(colIdx++).Value = item.InvoiceBuyer.Address;
                row.Cell(colIdx++).Value = item.InvoiceBuyer.EMail;
                row.Cell(colIdx++).Value = item.InvoiceDonation != null ? item.InvoiceDonation.AgencyCode : "";
                row.Cell(colIdx++).Value = item.InvoiceWinningNumber != null ? item.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A";
                row.Cell(colIdx++).Value = item.InvoiceCarrier != null ? item.InvoiceCarrier.CarrierType : "";
                row.Cell(colIdx++).Value = item.InvoiceCarrier != null ? item.InvoiceCarrier.CarrierNo : "";
                row.Cell(colIdx++).Value =  String.Join("", item.InvoiceDetails.Select(t => t.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark));

                row = row.RowBelow();
            }

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Cache-control", "max-age=1");
            Response.ContentType = "message/rfc822";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode("發票資料明細.xlsx")));

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