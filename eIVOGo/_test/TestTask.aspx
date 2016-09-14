<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/_test/MyPopupModal.ascx" TagPrefix="uc1" TagName="PopupModal" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Net" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>OK!!</div>
        <cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </form>
</body>
</html>
<script runat="server">

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        doTask();
    }
    
    void doTask()
    {
        var mgr = dsEntity.CreateDataManager();
        var items = mgr.EntityList.Where(i => i.SellerID == 2359 && i.CDS_Document.DocDate >= new DateTime(2015, 6, 30)
            && i.CDS_Document.DocDate < new DateTime(2015, 7, 2) && i.InvoiceBuyer.ReceiptNo != "0000000000");

        Response.Write(items.Count());
        
        foreach (var item in items)
        {
            try
            {
                //將Log下的B2B發票PDF，Copy至暫存資料夾
                String pdfFile = Path.Combine(Logger.LogPath.GetDateStylePath(item.InvoiceDate.Value), String.Format("{0}{1}.pdf", item.TrackCode, item.No));

                if (!File.Exists(pdfFile))
                {
                    String tmpFile;
                    using (WebClient client = new WebClient())
                    {
                        client.Encoding = System.Text.Encoding.UTF8;
                        tmpFile = client.DownloadString(String.Format("{0}{1}?nameOnly=true&id={2}", eIVOGo.Properties.Settings.Default.mailLinkAddress, VirtualPathUtility.ToAbsolute("~/Published/PrintSingleInvoiceAsPDF.aspx"), item.InvoiceID));

                    }
                    File.Move(tmpFile, pdfFile);
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }        
    }


</script>
