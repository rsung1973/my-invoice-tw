<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Register Src="~/Module/UI/FunctionTitleBar.ascx" TagPrefix="uc1" TagName="FunctionTitleBar" %>
<%@ Register Src="~/Module/EIVO/CancelInvoiceItemList.ascx" TagPrefix="uc1" TagName="CancelInvoiceItemList" %>
<%@ Register Src="~/Module/Inquiry/InquireInvoiceToCancel.ascx" TagPrefix="uc1" TagName="CancelInvoice" %>
<%@ Register Src="~/Module/EIVO/CancelInvoice.ascx" TagPrefix="uc2" TagName="CancelInvoice" %>




<form id="theForm" runat="server">
    <div id="result">
        <uc1:FunctionTitleBar runat="server" ID="resultTitle" ItemName="查詢結果" />
        <uc1:CancelInvoiceItemList runat="server" id="itemList" />
        <uc2:CancelInvoice runat="server" id="CancelInvoice" />
    </div>
</form>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ASP.module_inquiry_inquireinvoicetocancel_ascx.BuildQuery(itemList);
    }
</script>