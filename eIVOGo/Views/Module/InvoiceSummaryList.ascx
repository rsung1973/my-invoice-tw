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
    <uc1:JsGridField runat="server" ID="SellerName" FieldVariable="fields" name="SellerName" title="開立發票營業人" width="240" align="left" />
    <uc1:JsGridField runat="server" ID="SellerReceiptNo" FieldVariable="fields" name="SellerReceiptNo" title="統編" width="240" align="center" footerTemplate="function () { return '總筆數：'; }" />
    <script>
        fields[fields.length] = {
            "name": "RecordCount",
            "type": "text",
            "title": "資料筆數",
            "width": "200",
            "align": "right",
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###}", ((ModelSource<InvoiceItem>)Model).Items.Count()) %>"; }
        };
    </script>
</asp:PlaceHolder>

<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gridInit.DataSourceUrl = ((ModelSource<InvoiceItem>)Model).DataSourcePath;
        gridInit.GetRecordCount = () =>
            {
                return ((ModelSource<InvoiceItem>)Model).Items
                    .GroupBy(i=>i.SellerID)
                    .Count();
            };
        gridInit.AllowPaging = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
