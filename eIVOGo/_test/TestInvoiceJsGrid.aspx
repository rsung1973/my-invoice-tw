<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Src="~/Module/Common/CommonScriptManager.ascx" TagPrefix="uc1" TagName="CommonScriptManager" %>
<%@ Register Src="~/Module/Common/JsGrid.ascx" TagPrefix="uc1" TagName="JsGrid" %>


<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/InvoiceItemList.ascx" TagPrefix="uc1" TagName="InvoiceItemList" %>

<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <uc1:CommonScriptManager runat="server" ID="CommonScriptManager" />
        <uc1:JsGrid runat="server" ID="JsGrid" />
        <uc1:InvoiceItemList runat="server" ID="itemList" />
    </form>
</body>
</html>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        this.Load += _test_testjsgrid_aspx_Load;
        itemList.BuildQuery = table =>
        {
            return table.Join(table.Context.GetTable<InvoiceItem>(), d => d.DocID, i => i.InvoiceID, (d, i) => d);
        };
    }

    void _test_testjsgrid_aspx_Load(object sender, EventArgs e)
    {
        if(Request["q"]!=null)
        {
            int pageIndex = 0, pageSize = 15;
            if (Request.GetRequestValue("index", out pageIndex) && pageIndex > 0)
            {
                pageIndex--;
            }
            Request.GetRequestValue("size", out pageSize);

            itemList.RenderJsGridDataResult(pageIndex, pageSize);

        }
    }
</script>