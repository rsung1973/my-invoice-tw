<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Ver2016" %>

<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/EIVO/Item/AllowanceView2016.ascx" TagPrefix="uc1" TagName="AllowanceView2016" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>電子發票系統</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
</head>
<body style="font-family:KaiTi">
    <form id="theForm" runat="server">
        <uc1:AllowanceView2016 runat="server" id="itemView" />
        <cc1:AllowanceDataSource ID="dsEntity" runat="server"></cc1:AllowanceDataSource>
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        initializeData();
    }

    protected virtual void initializeData()
    {
        int allowanceID;
        if (Request.GetRequestValue("id", out allowanceID))
        {
            var mgr = dsEntity.CreateDataManager();
            itemView.Item = mgr.GetTable<InvoiceAllowance>().Where(i => i.AllowanceID == allowanceID).FirstOrDefault();
        }
    } 
</script>