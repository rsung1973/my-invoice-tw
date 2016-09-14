﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.Export.GoogleInvoiceCSVList" %>
<%@ Import Namespace="Model.InvoiceManagement" %><%@ Import Namespace="Model.DataEntity" %>序號,GoogleID,折讓號碼,作廢原因,買受人名稱,買受人統編,未稅金額,稅額,含稅金額,匯入狀態
<asp:Repeater ID="rpExcel" runat="server">
<ItemTemplate><%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoicePurchaseOrder.OrderNo.Substring(0,11) %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoiceBuyer.CustomerID %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.AllowanceNumber %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceAllowanceCancellation.Remark %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoiceBuyer.CustomerName %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoiceBuyer.IsB2C() ? "N/A" : ((GoogleInvoiceAllowance)Container.DataItem).Allowance.InvoiceItem.InvoiceBuyer.ReceiptNo %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TotalAmount-((GoogleInvoiceAllowance)Container.DataItem).Allowance.TaxAmount %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TaxAmount %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).Allowance.TotalAmount %>,<%# ((GoogleInvoiceAllowance)Container.DataItem).UploadStatus.ToString() %>
</ItemTemplate>
</asp:Repeater>