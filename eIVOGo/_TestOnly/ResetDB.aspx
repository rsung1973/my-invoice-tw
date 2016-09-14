<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetDB.aspx.cs" Inherits="eIVOGo._TestOnly.ResetDB" %>

<%@ Register assembly="Model" namespace="Model.DataEntity" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>OK!<cc1:InvoiceDataSource ID="dsEntity" runat="server">
        </cc1:InvoiceDataSource>
    </div>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += new EventHandler(_testonly_resetdb_aspx_Load);
    }

    void _testonly_resetdb_aspx_Load(object sender, EventArgs e)
    {
        dsEntity.CreateDataManager().ExecuteCommand(@"
                delete InvoiceCancellationUploadList
                delete InvoiceWinningNumber
                delete InvoiceNoAssignment
                delete InvoicePaperRequest
                delete InvoiceAllowanceDetails
                delete InvoiceAllowanceCancellation
                delete InvoiceAllowance
                delete InvoiceSeller
                delete InvoicePrintQueue
                delete InvoiceDeliveryTracking
                delete InvoiceDonation
                delete InvoiceCarrier
                delete InvoicePurchaseOrder
                delete InvoiceItem
                delete DerivedDocument
                delete Attachment
                delete DocumentSubscriptionQueue
                delete CDS_Document");
    }
</script>