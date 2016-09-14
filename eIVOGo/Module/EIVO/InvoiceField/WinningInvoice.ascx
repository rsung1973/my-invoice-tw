<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceWinningNumber != null ? ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceWinningNumber.UniformInvoiceWinningNumber.PrizeType : "N/A"%>
