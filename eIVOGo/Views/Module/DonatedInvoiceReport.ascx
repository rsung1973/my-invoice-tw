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
                    "name": "AgencyCode",
                    "type": "text",
                    "title": "愛心碼",
                    "width": "80",
                    "align": "left"
                },
                {
                    "name": "SellerName",
                    "type": "text",
                    "title": "開立發票營業人",
                    "width": "320",
                    "align": "left"
                },
        {
            "name": "InvoiceNo",
            "type": "text",
            "title": "發票號碼",
            "width": "120",
            "align": "center",
            itemTemplate: function (value, item) {
                return $('<a>')
                    .attr('href', '#')
                    .html(value)
                    .on('click', function (evt) {
                        showInvoiceModal(item.InvoiceID);
                    });
            }
        },
        {
                "name": "IsWinning",
                "type": "text",
                "title": "是否中獎",
                "width": "80",
                "align": "center"
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
                    .Count();
            };
        gridInit.AllowPaging = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
