<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Paper" %>

<%@ Register src="../Module/SCM/View/SingleShipmentPreview.ascx" tagname="SingleShipmentPreview" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css" media="print">
        div.fspace
        {
            height: 8.8cm;
        }
        div.bspace
        {
            height: 9.5cm;
        }
    </style>
    <title>電子發票系統</title>
</head>
<body onload='javascript:self.print();self.close();'>
    <form id="theForm" runat="server">
    <uc1:SingleShipmentPreview ID="shipmentView" runat="server" />
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if (Page.PreviousPage != null && Page.PreviousPage.Items["id"] != null)
        {
            shipmentView.PrepareDataFromDB((int)Page.PreviousPage.Items["id"]);
        }
        else if (Request["id"] != null)
        {
            shipmentView.PrepareDataFromDB(int.Parse(Request["id"]));
        }
    }
</script>