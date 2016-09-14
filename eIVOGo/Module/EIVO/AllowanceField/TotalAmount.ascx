<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Utility" %>
<%#String.Format("{0:0,0.00}", ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceAllowance.TotalAmount)%>
