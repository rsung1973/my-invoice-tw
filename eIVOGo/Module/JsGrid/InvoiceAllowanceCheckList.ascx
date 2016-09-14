<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.InvoiceItemList" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/JsGrid/JsGridInitialization.ascx" TagPrefix="uc1" TagName="JsGridInitialization" %>
<%@ Register Src="~/Module/JsGrid/DataField/JsGridField.ascx" TagPrefix="uc1" TagName="JsGridField" %>
<%@ Register Src="~/Module/JsGrid/DataField/CheckBox.ascx" TagPrefix="uc1" TagName="CheckBox" %>
<%@ Register Src="~/Module/JsGrid/DataField/AllowanceNo.ascx" TagPrefix="uc1" TagName="AllowanceNo" %>



<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<div id="fieldsContainer"></div>
<div id="jsGrid"></div>
<uc1:JsGridInitialization runat="server" id="gridInit" JsGridSelector="#jsGrid" FieldsContainerSelector="#fieldsContainer" />
<asp:PlaceHolder ID="phFields" runat="server">
    <script>
        var fields = [];
    </script>
    <uc1:CheckBox runat="server" ID="CheckBox" FieldVariable="fields" name="DocID" />
    <uc1:JsGridField runat="server" ID="Date" FieldVariable="fields" name="日期" title="日期" width="80" align="center" />
    <uc1:JsGridField runat="server" ID="CustomerID" FieldVariable="fields" name="客戶ID" title="GoogleID/客戶ID" width="160" align="left" />
    <uc1:JsGridField runat="server" ID="SellerName" FieldVariable="fields" name="開立發票營業人" title="開立發票營業人" width="240" align="left" />
    <uc1:JsGridField runat="server" ID="SellerReceiptNo" FieldVariable="fields" name="統編" title="統編" width="80" align="center" />
    <uc1:AllowanceNo runat="server" ID="AllowanceNo" FieldVariable="fields" />
    <script>
        fields[fields.length] =
        {
            "name": "未稅金額",
            "type": "text",
            "title": "未稅金額",
            "width": "120",
            "align": "right"
        };
        fields[fields.length] =
        {
            "name": "稅額",
            "type": "text",
            "title": "稅額",
            "width": "120",
            "align": "right",
            footerTemplate: function () { return "總計金額："; }
        };

        fields[fields.length] =
        {
            "name": "含稅金額",
            "type": "text",
            "title": "含稅金額",
            "width": "120",
            "align": "right",
            footerTemplate: function () { return "<%= String.Format("{0:##,###,###,###.00}", Select().Sum(i => i.InvoiceAllowance.TotalAmount)) %>"; }
        };
    </script>
    <uc1:JsGridField runat="server" ID="BuyerReceiptNo" FieldVariable="fields" name="買受人統編" title="買受人統編" width="80" align="center" />
    <uc1:JsGridField runat="server" ID="Remark" FieldVariable="fields" name="備註" title="備註" width="80" align="left" />
</asp:PlaceHolder>

<cc1:DocumentDataSource ID="dsEntity" runat="server">
</cc1:DocumentDataSource>
<script runat="server">

    public override void PrepareJsGridField(object item)
    {
        //if (item != null)
        //{
        //    gridInit.FieldObject = item;
        //}
        //else
        //{
        //    gridInit.FieldName = "fields";
        //}
        gridInit.FieldName = "fields";
    }

    public override IEnumerable<object> GetCsvResult(IEnumerable<CDS_Document> items)
    {
        return items.Select(d => new
        {
            日期 = String.Format("{0:yyyy/MM/dd}", d.InvoiceAllowance.AllowanceDate),
            客戶ID = d.InvoiceAllowance.InvoiceAllowanceBuyer.CustomerID,
            開立發票營業人 = d.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName,
            統編 = d.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo,
            折讓號碼 = d.InvoiceAllowance.AllowanceNumber,
            未稅金額 = String.Format("{0:0,0.00}", d.InvoiceAllowance.TotalAmount - d.InvoiceAllowance.TaxAmount),
            稅額 = String.Format("{0:0,0.00}", d.InvoiceAllowance.TaxAmount),
            含稅金額 = String.Format("{0:0,0.00}", d.InvoiceAllowance.TotalAmount),
            買受人統編 = d.InvoiceAllowance.InvoiceAllowanceBuyer.IsB2C() ? "" : d.InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo,
            備註 = ""
        });
    }

    public override IEnumerable<object> GetQueryResult(IEnumerable<CDS_Document> items)
    {
        return items.Select(d => new
                        {
                            d.DocID,
                            日期 = String.Format("{0:yyyy/MM/dd}", d.InvoiceAllowance.AllowanceDate),
                            客戶ID = d.InvoiceAllowance.InvoiceAllowanceBuyer.CustomerID,
                            開立發票營業人 = d.InvoiceAllowance.InvoiceAllowanceSeller.CustomerName,
                            統編 = d.InvoiceAllowance.InvoiceAllowanceSeller.ReceiptNo,
                            折讓號碼 = d.InvoiceAllowance.AllowanceNumber,
                            未稅金額 = String.Format("{0:0,0.00}",d.InvoiceAllowance.TotalAmount - d.InvoiceAllowance.TaxAmount),
                            稅額 = String.Format("{0:0,0.00}",d.InvoiceAllowance.TaxAmount),
                            含稅金額 = String.Format("{0:0,0.00}",d.InvoiceAllowance.TotalAmount),
                            買受人統編 = d.InvoiceAllowance.InvoiceAllowanceBuyer.IsB2C() ? "" : d.InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo,
                            備註 = ""
                        });
    }

    protected override string doJsonSerialize(System.Web.Script.Serialization.JavaScriptSerializer serializer, int pageIndex, int pageSize)
    {
        var items = this.Select().OrderByDescending(d => d.DocID);

        return serializer.Serialize(
                new
                {
                    data = GetQueryResult(items.Skip(pageIndex * pageSize).Take(pageSize)
                        .ToArray()),
                    itemsCount = items.Count()
                });
    }
    
</script>