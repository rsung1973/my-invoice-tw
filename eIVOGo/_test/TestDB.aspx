<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Schema.EIVO" %>
<%@ Import Namespace="Utility" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
        <cc1:DocumentDataSource ID="dsEntity" runat="server">
        </cc1:DocumentDataSource>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    void testHtmlDom()
    {
        String urlPage = "DemoImage.aspx";
        var content = urlPage.GetPageContent();
        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
        doc.LoadHtml(content);
        
        List<System.Net.Mail.Attachment> attachedItems = new List<System.Net.Mail.Attachment>();

        foreach (var imgNode in doc.DocumentNode.SelectNodes("//img"))
        {
            System.Net.Mail.Attachment item = new System.Net.Mail.Attachment(Server.MapPath(imgNode.Attributes["src"].Value));
            item.NameEncoding = Encoding.UTF8;
            item.ContentId = Guid.NewGuid().ToString();
            item.ContentDisposition.Inline = true;
            imgNode.SetAttributeValue("src","cid:"+item.ContentId);
            attachedItems.Add(item);
        }
        
    }
    
    void testInvoiceAgent()
    {
        int companyID = 2359;   //Google
        var mgr = dsEntity.CreateDataManager();
        var items = mgr.GetQueryByAgent(companyID);
        var maps = mgr.GetTable<DocumentMappingQueue>()
                .Join(mgr.GetTable<CDS_Document>()
                    .Where(d => d.DocType == (int)Model.Locale.Naming.DocumentTypeDefinition.E_Invoice)
                    .Join(mgr.GetTable<DocumentOwner>()
                        .Join(items, o => o.OwnerID, a => a.CompanyID, (o, a) => o),
                        d => d.DocID, o => o.DocID, (d, o) => d),
                q => q.DocID, d => d.DocID, (q, d) => q);
        
        var result = new RootResponseForB2CInvoiceMapping
        {
            InvoiceMapRoot = new InvoiceMapRoot
            {
                InvoiceMap = maps.Select(d => new InvoiceMapRootInvoiceMap
                {
                    InvoiceNumber = d.CDS_Document.InvoiceItem.TrackCode + d.CDS_Document.InvoiceItem.No,
                    InvoiceDate = String.Format("{0:yyyy/MM/dd}", d.CDS_Document.InvoiceItem.InvoiceDate),
                    DataNumber = d.CDS_Document.InvoiceItem.InvoicePurchaseOrder.OrderNo,
                    InvoiceTime = String.Format("{0:HH:mm:ss}", d.CDS_Document.InvoiceItem.InvoiceDate)
                }).ToArray()
            }
        };        
    }
</script>