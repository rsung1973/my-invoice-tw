<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<input id="chkItem" name="chkItem" type="checkbox" value='<%# ((CDS_Document)((GridViewRow)Container).DataItem).InvoiceItem.InvoiceID  %>' />
