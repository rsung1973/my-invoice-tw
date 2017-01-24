<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>Invoice No:
        <textarea cols="80" rows="10" name="invoiceNo"><%= Request["invoiceNo"] %></textarea><br />
            Track Code:<input type="text" name="trackCode" value="<%= Request["trackCode"] %>" />
            No: <input type="text" name="startNo" value="<%= Request["startNo"] %>" /> ~ 
            <input type="text" name="endNo" value="<%= Request["endNo"] %>" />  
            Cancel Date: <input type="text" name="cancelDate" value="<%= cancelDate.ToString("yyyy/MM/dd") %>" />
            <asp:Button ID="btnCreate" runat="server" Text="OK!!" />
        </div>
        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
    <br />
    <%  if (_items != null && _items.Count > 0)
        { %>
            下列發票已作廢：<br />
            <%  foreach (var invNo in _items)
                {   %>
                    <%= invNo %><br />
            <%  } %>
    <%  } %>
</body>
</html>
<script runat="server">

    DateTime cancelDate = DateTime.Now;
    List<String> _items;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        _items = new List<string>();
        if(!String.IsNullOrEmpty(Request["cancelDate"]))
        {
            DateTime.TryParse(Request["cancelDate"], out cancelDate);
        }
        doTask();
    }

    void doTask()
    {
        var mgr = dsEntity.CreateDataManager();

        String invoiceNo = Request["invoiceNo"].GetEfficientString();
        if(invoiceNo!=null)
        {
            if (invoiceNo.Length == 10)
            {
                var item = mgr.EntityList.Where(i => i.TrackCode == invoiceNo.Substring(0, 2)
                    && i.No == invoiceNo.Substring(2)).FirstOrDefault();
                if (item != null)
                {
                    cancelInvoice(item);
                }
            }
            else
            {

                String[] items = invoiceNo.Split(new String[] { "\r\n", ",", ";", "、" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var invNo in items)
                {
                    if (invNo.Length != 10)
                        break;
                    var item = mgr.EntityList.Where(i => i.TrackCode == invNo.Substring(0, 2)
                        && i.No == invNo.Substring(2)).FirstOrDefault();
                    if (item != null)
                    {
                        cancelInvoice(item);
                    }
                }
            }
        }
        else if(Request["trackCode"]!=null)
        {
            System.Linq.Expressions.Expression<Func<InvoiceItem, bool>> queryExpr = i => i.TrackCode == Request["trackCode"];
            bool hasNo = false;
            if(Request["startNo"]!=null)
            {
                queryExpr = queryExpr.And(i => String.Compare(i.No, Request["startNo"]) >= 0);
                hasNo = true;
            }
            if (Request["endNo"] != null)
            {
                queryExpr = queryExpr.And(i => String.Compare(i.No, Request["endNo"]) <= 0);
                hasNo = true;
            }
            if(hasNo)
            {
                foreach(var item in mgr.EntityList.Where(queryExpr))
                {
                    cancelInvoice(item);
                }
            }
        }

    }

    void cancelInvoice(InvoiceItem item)
    {
        if (item.InvoiceCancellation == null)
        {
            var mgr = dsEntity.CreateDataManager();
            InvoiceCancellation cancelItem = new InvoiceCancellation
            {
                InvoiceItem = item,
                CancellationNo = item.TrackCode + item.No,
                Remark = "作廢發票",
                ReturnTaxDocumentNo = "",
                CancelDate = cancelDate,
                CancelReason = "作廢發票"
            };

            var doc = new DerivedDocument
            {
                CDS_Document = new CDS_Document
                {
                    DocType = (int)Naming.DocumentTypeDefinition.E_InvoiceCancellation,
                    DocDate = DateTime.Now,
                    DocumentOwner = new DocumentOwner
                    {
                        OwnerID = item.CDS_Document.DocumentOwner.OwnerID
                    }
                },
                SourceID = item.InvoiceID
            };

            mgr.GetTable<DerivedDocument>().InsertOnSubmit(doc);
            mgr.SubmitChanges();
            _items.Add(item.TrackCode + item.No);
        }

    }


</script>
