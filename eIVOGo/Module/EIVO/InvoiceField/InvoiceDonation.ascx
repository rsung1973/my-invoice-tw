<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceDonation!=null?((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceDonation.AgencyCode:""%>
