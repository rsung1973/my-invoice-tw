<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo.Equals("0000000000") ? "N/A" : ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceAllowance.InvoiceAllowanceBuyer.ReceiptNo%>
