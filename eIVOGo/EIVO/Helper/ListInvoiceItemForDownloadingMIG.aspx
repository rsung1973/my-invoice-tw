<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagPrefix="uc1" TagName="FunctionTitleBar" %>
<%@ Register Src="~/Module/EIVO/WebDownloadMIG.ascx" TagPrefix="uc1" TagName="WebDownloadMIG" %>
<%@ Register Src="~/Module/EIVO/MIG_InvoiceItemList.ascx" TagPrefix="uc1" TagName="MIG_InvoiceItemList" %>

<form id="theForm" runat="server">
    <div id="result">
        <uc1:FunctionTitleBar runat="server" ID="resultTitle" ItemName="查詢結果" />
        <uc1:MIG_InvoiceItemList runat="server" id="itemList" />
    </div>
</form>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ASP.module_eivo_webdownloadmig_ascx.BuildQuery(itemList);
    }
</script>