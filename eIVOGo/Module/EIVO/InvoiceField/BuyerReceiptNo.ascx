<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceBuyer.ReceiptNo.Equals("0000000000") ? "N/A" : ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceBuyer.ReceiptNo %>
