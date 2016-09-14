<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>

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
<script>

            var fields = [

                {
                    "name": "SellerReceiptNo",
                    "type": "text",
                    "title": "統編",
                    "width": "80",
                    "align": "center"
                },
                {
                    "name": "SellerName",
                    "type": "text",
                    "title": "開立發票營業人",
                    "width": "160",
                    "align": "left"
                },
                {
                    "name": "Addr",
                    "type": "text",
                    "title": "營業人地址",
                    "width": "360",
                    "align": "left"
                },
            {
                "name": "WinningCount",
                "type": "text",
                "title": "中獎張數",
                "width": "80",
                "align": "left"
            },
            {
                "name": "DonationCount",
                "type": "text",
                "title": "捐贈張數",
                "width": "80",
                "align": "left"
            }
    ];

        </script>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        gridInit.DataSourceUrl = ((ModelSource<InvoiceItem>)Model).DataSourcePath;
        gridInit.GetRecordCount = () =>
            {
                return ((ModelSource<InvoiceItem>)Model).Items
                    .GroupBy(i => i.SellerID)
                    .Count();
            };
        gridInit.AllowPaging = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
