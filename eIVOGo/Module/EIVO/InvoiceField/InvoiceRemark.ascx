<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%# String.Join("",((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceDetails.Select(d=> d.InvoiceProduct.InvoiceProductItem.FirstOrDefault().Remark))%>
