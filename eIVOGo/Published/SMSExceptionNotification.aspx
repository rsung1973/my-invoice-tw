<%@ Page Language="C#" AutoEventWireup="true"  %>
<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>
<%@ Import Namespace="Model.DataEntity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="theForm" runat="server">
    簡訊傳送失敗通知:<br />
    <asp:PlaceHolder ID="plSMS" runat="server" Visible="false" EnableViewState="false">簡訊內容:<%# _message %>
    <br />
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="plInvoice" runat="server" Visible="false" EnableViewState="false">
        發票號碼:<%# _invoice.TrackCode + _invoice.No %><br />
        發票金額:<%# String.Format("{0:.##}",_invoice.InvoiceAmountType.TotalAmount) %><br />
        開立人:<%# _invoice.InvoiceSeller.CustomerName %><br />
        買受人:<%# _invoice.InvoiceBuyer.CustomerName %><br />
    </asp:PlaceHolder>
    失敗原因:<asp:Literal ID="reason" runat="server" EnableViewState="false"></asp:Literal>
    <cc1:InvoiceDataSource ID="dsEntity" runat="server">
    </cc1:InvoiceDataSource>
    </form>
</body>
</html>
<script runat="server">
    
    String _message;
    InvoiceItem _invoice;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.PreRender += new EventHandler(published_smsexceptionnotification_aspx_PreRender);
    }

    void published_smsexceptionnotification_aspx_PreRender(object sender, EventArgs e)
    {
        reason.Text = Request["reason"];
        
        if (!String.IsNullOrEmpty(Request["sms"]))
        {
            _message = Request["sms"];
            plSMS.Visible = true;
            plSMS.DataBind();
        }

        int id;
        if (Request["id"] != null && int.TryParse(Request["id"], out id))
        {
            _invoice = dsEntity.CreateDataManager().EntityList.Where(i => i.InvoiceID == id).FirstOrDefault();
            if (_invoice != null)
            {
                plInvoice.Visible = true;
                plInvoice.DataBind();
            }
        }
    }
    
</script>