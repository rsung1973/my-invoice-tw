<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>
<%@ Register Src="~/Module/JsGrid/DataField/InvoiceNo.ascx" TagPrefix="uc1" TagName="InvoiceNo" %>
<%@ Register Src="~/Module/JsGrid/DataField/Attachment.ascx" TagPrefix="uc1" TagName="Attachment" %>
<%@ Register Src="~/Module/JsGrid/DataField/CheckBox.ascx" TagPrefix="uc1" TagName="CheckBox" %>
<%@ Register Src="~/Module/JsGrid/DataField/JsGridField.ascx" TagPrefix="uc1" TagName="JsGridField" %>


<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Module.Base" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Uxnet.Web.WebUI" %>

<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" ID="gridInit" FieldName="fields" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
<asp:PlaceHolder ID="phFields" runat="server">
    <script>
        var fields = [];
    </script>
    <uc1:CheckBox runat="server" ID="CheckBox" FieldVariable="fields" name="DocID" />
    <uc1:JsGridField runat="server" ID="InvoiceDate" FieldVariable="fields" name="InvoiceDate" title="日期" width="80" align="center" />
    <uc1:JsGridField runat="server" ID="CustomerID" FieldVariable="fields" name="CustomerID" title="Google ID/客戶ID" width="120" align="left" />
    <uc1:JsGridField runat="server" ID="OrderNo" FieldVariable="fields" name="OrderNo" title="序號" width="240" align="left" />
    <uc1:JsGridField runat="server" ID="SellerName" FieldVariable="fields" name="SellerName" title="開立發票營業人" width="160" align="left" />
    <uc1:JsGridField runat="server" ID="SellerReceiptNo" FieldVariable="fields" name="SellerReceiptNo" title="統編" width="80" align="center" />
    <uc1:InvoiceNo runat="server" ID="InvoiceNo" FieldVariable="fields" />
    <script>
        fields[fields.length] = {
            "name": "SalesAmount",
            "type": "text",
            "title": "未稅金額",
            "width": "120",
            "align": "right"
        };
        fields[fields.length] =
        {
            "name": "TaxAmount",
            "type": "text",
            "title": "稅額",
            "width": "120",
            "align": "right",
            footerTemplate: function () { return "總計金額："; }
        };
        fields[fields.length] =
        {
            "name": "TotalAmount",
            "type": "text",
            "title": "含稅金額",
            "width": "120",
            "align": "right",
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", ((ModelSource<InvoiceItem>)Model).Items.Sum(i => i.InvoiceAmountType.TotalAmount)) %>"; }
        };
    </script>
    <uc1:Attachment runat="server" ID="Attachment" FieldVariable="fields" />
</asp:PlaceHolder>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gridInit.DataSourceUrl = ((ModelSource<InvoiceItem>)Model).DataSourcePath;
        gridInit.GetRecordCount = () =>
            {
                return ((ModelSource<InvoiceItem>)Model).Items.Count();
            };
        gridInit.AllowPaging = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
