<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoicePaperRequest!=null?"索取紙本":null%>