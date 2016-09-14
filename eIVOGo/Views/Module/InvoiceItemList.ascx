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
            "name": "InvoiceDate",
            "type": "text",
            "title": "日期",
            "width": "80",
            "align": "left"
        },
        {
            "name": "CustomerID",
            "type": "text",
            "title": "GoogleID/客戶ID",
            "width": "120",
            "align": "left"
        },
        {
            "name": "OrderNo",
            "type": "text",
            "title": "序號",
            "width": "240",
            "align": "left"
        },
        {
            "name": "SellerName",
            "type": "text",
            "title": "開立發票營業人",
            "width": "160",
            "align": "left"
        },
        {
            "name": "SellerReceiptNo",
            "type": "text",
            "title": "統編",
            "width": "80",
            "align": "center"
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
            },
            footerTemplate: function () { return "總計金額："; }
        },
        {
            "name": "SalesAmount",
            "type": "text",
            "title": "未稅金額<br/>稅額<br/>含稅金額",
            "width": "120",
            "align": "right",
            itemTemplate: function (value, item) {
                return $('<pre>')
                    .append(value).append('<br/>')
                    .append(item.TaxAmount).append('<br/>')
                    .append(item.TotalAmount);
            },
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", ((ModelSource<InvoiceItem>)Model).Items.Sum(i => i.InvoiceAmountType.TotalAmount)) %>"; }
        },
            {
                "name": "Winning",
                "type": "text",
                "title": "是否中獎",
                "width": "80",
                "align": "left"
            },
            {
                "name": "BuyerReceiptNo",
                "type": "text",
                "title": "買受人統編<br/>名稱<br/>連絡人名稱<br/>地址<br/>email",
                "width": "240",
                "align": "left",
                itemTemplate: function (value, item) {
                    return $('<pre>')
                        .append(value).append('<br/>')
                        .append(item.CustomerName).append('<br/>')
                        .append(item.ContactName).append('<br/>')
                        .append(item.Address).append('<br/>')
                        .append(item.EMail);
                }

            },
            {
                "name": "Remark",
                "type": "text",
                "title": "備註",
                "width": "120",
                "align": "left"
            },
            {
                "name": "SMS",
                "type": "text",
                "title": "簡訊通知",
                "width": "60",
                "align": "center"
            },
            {
                "name": "CarrierNo",
                "type": "text",
                "title": "載具資訊",
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
                return ((ModelSource<InvoiceItem>)Model).Items.Count();
            };
        gridInit.AllowPaging = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Display;
        gridInit.PrintMode = ((ModelSource<InvoiceItem>)Model).ResultModel == Naming.DataResultMode.Print;
    }
</script>
