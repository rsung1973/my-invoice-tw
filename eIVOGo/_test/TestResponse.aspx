<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Schema.TXN" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>

    </form>
</body>
</html>
<script runat="server">
    

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_testresponse_aspx_Load;

    }

    void _test_testresponse_aspx_Load(object sender, EventArgs e)
    {
        var items = dsEntity.CreateDataManager().EntityList
            .Where(i => i.SellerID == 2359 && i.InvoiceDate >= new DateTime(2015, 9, 7)
            && i.InvoiceDate < new DateTime(2015, 9, 8));
        
        if(items.Count()>0)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}({1:yyyy-MM-dd}).xml", Server.UrlEncode("測試文件"), DateTime.Today));

            Automation auto = new Automation
            {
                Item = items.Select(i => new AutomationItem
                {
                    Description = "",
                    Status = 1,
                    Invoice = new AutomationItemInvoice
                    {
                        InvoiceNumber = i.TrackCode + i.No,
                        DataNumber = i.InvoicePurchaseOrder.OrderNo,
                        InvoiceDate = String.Format("{0:yyyy/MM/dd}", i.InvoiceDate),
                        InvoiceTime = String.Format("{0:HH:mm:ss}", i.InvoiceDate),
                    }
                }).ToArray()
            };

            auto.ConvertToXml().Save(Response.OutputStream);

            Response.Flush();
            Response.End();
            
        }
    }


</script>
