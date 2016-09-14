<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="Model.Helper" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
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
            <asp:Button ID="btnCreate" runat="server" Text="OK!!" />
        </div>
        <br />
        C0401:
        <textarea cols="80" rows="10"><%= _item!=null ? _item.ConvertToXml().OuterXml : null %></textarea>
        <br />
        C0701:
                <textarea cols="80" rows="10"><%= _voidItem!=null ? _voidItem.ConvertToXml().OuterXml : null %></textarea>

        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
</body>
</html>
<script runat="server">

    protected Model.Schema.MIG3_1.C0401.Invoice _item;
    protected Model.Schema.TurnKey.C0701.VoidInvoice _voidItem;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        doTask();
    }

    void doTask()
    {
        var mgr = dsEntity.CreateDataManager();
        
        String invoiceNo = Request["invoiceNo"];
        if(invoiceNo!=null)
        {
            if (invoiceNo.Length == 10)
            {
                var item = mgr.EntityList.Where(i => i.TrackCode == invoiceNo.Substring(0, 2)
                    && i.No == invoiceNo.Substring(2)).FirstOrDefault();
                if (item != null)
                {
                    _item = item.CreateC0401();
                    _voidItem = new Model.Schema.TurnKey.C0701.VoidInvoice
                    {
                        VoidInvoiceNumber = invoiceNo,
                        InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
                        BuyerId = item.InvoiceBuyer.ReceiptNo,
                        SellerId = item.InvoiceSeller.ReceiptNo,
                        VoidDate = DateTime.Now.Date.ToString("yyyyMMdd"),
                        VoidTime = DateTime.Now.ToString("HH:mm:ss"),
                        VoidReason = "註銷重開",
                        Remark = ""
                    };
                }
            }
            else
            {
                String storedPath = Path.Combine(Logger.LogDailyPath, "MIG");
                storedPath.CheckStoredPath();
                
                String[] items = invoiceNo.Split(new String[] { "\r\n", ",", ";", "、" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var invNo in items)
                {
                    if (invNo.Length != 10)
                        break;
                    var item = mgr.EntityList.Where(i => i.TrackCode == invNo.Substring(0, 2)
                        && i.No == invNo.Substring(2)).FirstOrDefault();
                    if (item != null)
                    {
                        storedMIG(item, storedPath);
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
                String storedPath = Path.Combine(Logger.LogDailyPath, "MIG");
                storedPath.CheckStoredPath();
                foreach(var item in mgr.EntityList.Where(queryExpr))
                {
                    storedMIG(item, storedPath);
                }
            }
        }
        
    }
    
    void storedMIG(InvoiceItem item,String storedPath)
    {
        String invoiceNo = item.TrackCode + item.No;
        item.CreateC0401().ConvertToXml().Save(Path.Combine(storedPath, "C0401_" + invoiceNo + ".xml"));
        (new Model.Schema.TurnKey.C0701.VoidInvoice
        {
            VoidInvoiceNumber = invoiceNo,
            InvoiceDate = String.Format("{0:yyyyMMdd}", item.InvoiceDate),
            BuyerId = item.InvoiceBuyer.ReceiptNo,
            SellerId = item.InvoiceSeller.ReceiptNo,
            VoidDate = DateTime.Now.Date.ToString("yyyyMMdd"),
            VoidTime = DateTime.Now.ToString("HH:mm:ss"),
            VoidReason = "註銷重開",
            Remark = ""
        }).ConvertToXml().Save(Path.Combine(storedPath, "C0701_" + item.TrackCode + item.No + ".xml"));

    }


</script>
