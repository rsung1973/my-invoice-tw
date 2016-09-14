<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>
<%  if (_model != null && _model.Count() > 0)
    {
        int idx = 1;
        foreach (var item in _model)
        {
            if (item.InvoiceCancellation == null)
            {
                Writer.Write("35");     //	格式代號	X(002)	1	2
                Writer.Write(String.Format("{0,9}", ViewBag.TaxNo));       //	申報營業人稅籍編號	X(009)	3	11
                Writer.Write(String.Format("{0:d7}", idx++));       //	流水號	X(007)	12	18
                Writer.Write(String.Format("{0:d3}", item.InvoiceDate.Value.Year - 1911));       //	資料所屬年度	9(003)	19	20
                Writer.Write(String.Format("{0:MM}", item.InvoiceDate));       //	資料所屬月份	9(002)	21	22
                Writer.Write(String.Format("{0,8}", item.InvoiceBuyer.IsB2C() ? "" : item.InvoiceBuyer.ReceiptNo));       //	買受人統一編號	X(008)	23	30
                Writer.Write(item.InvoiceSeller.ReceiptNo);       //	銷售人統一編號	X(008)	31	38
                Writer.Write(item.TrackCode);       //	發票字軌	X(002)	39	40
                Writer.Write(item.No);       //	發票(起)號碼	9(008)	41	48
                writeAmount(item);
                Writer.Write(" ");       //	扣抵代號	X(001)	72	72
                Writer.Write("     ");       //	空白	X(005)	73	77
                Writer.Write(" ");       //	特種稅額稅率	X(001)	78	78
                Writer.Write(" ");       //	彙加註記	X(001)	79	79
                Writer.WriteLine(" ");       //	通關方式註記	X(001)	80	80
            }
            else
            {
                Writer.Write("35");     //	格式代號	X(002)	1	2
                Writer.Write(String.Format("{0,9}",ViewBag.TaxNo));       //	申報營業人稅籍編號	X(009)	3	11
                Writer.Write(String.Format("{0:d7}",idx++));       //	流水號	X(007)	12	18
                Writer.Write(String.Format("{0:d3}",item.InvoiceDate.Value.Year-1911));       //	資料所屬年度	9(003)	19	20
                Writer.Write(String.Format("{0:MM}",item.InvoiceDate));       //	資料所屬月份	9(002)	21	22
                Writer.Write(String.Format("{0,8}", ""));       //	買受人統一編號	X(008)	23	30
                Writer.Write(item.InvoiceSeller.ReceiptNo);       //	銷售人統一編號	X(008)	31	38
                Writer.Write(item.TrackCode);       //	發票字軌	X(002)	39	40
                Writer.Write(item.No);       //	發票(起)號碼	9(008)	41	48
                Writer.Write("000000000000");       //	銷售金額	9(012)	49	60
                Writer.Write("F");       //	課稅別	X(001)	61	61
                Writer.Write("0000000000");       //	營業稅額	9(010)	62	71
                Writer.Write(" ");       //	扣抵代號	X(001)	72	72
                Writer.Write("     ");       //	空白	X(005)	73	77
                Writer.Write(" ");       //	特種稅額稅率	X(001)	78	78
                Writer.Write(" ");       //	彙加註記	X(001)	79	79
                Writer.WriteLine(" ");       //	通關方式註記	X(001)	80	80
            }
        }
    }  %>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    IEnumerable<InvoiceItem> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _model = (IEnumerable<InvoiceItem>)this.Model;

        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AddHeader("Cache-control", "max-age=1");
        Response.ContentType = "message/rfc822";
        Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", HttpUtility.UrlEncode(ViewBag.FileName)));

        //Response.Flush();
        //Response.End();
    }

    void writeAmount(InvoiceItem item)
    {
        if (item.InvoiceBuyer.IsB2C())
        {
            Writer.Write(String.Format("{0:000000000000}", item.InvoiceAmountType.TotalAmount));       //	銷售金額	9(012)	49	60
            Writer.Write("1");       //	課稅別	X(001)	61	61
            Writer.Write("0000000000");       //	營業稅額	9(010)	62	71
        }
        else
        {
            Writer.Write(String.Format("{0:000000000000}", item.InvoiceAmountType.SalesAmount));       //	銷售金額	9(012)	49	60
            Writer.Write("1");       //	課稅別	X(001)	61	61
            Writer.Write(String.Format("{0:0000000000}", item.InvoiceAmountType.TaxAmount));       //	營業稅額	9(010)	62	71
        }
    }

</script>
