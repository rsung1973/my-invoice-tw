<%@ Control Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.Module.Common" %>

<a href='#' onclick='<%# "javascript:showInvoiceModal(" + Item.InvoiceID.ToString() + ");" %>'><%# Item.GetMaskInvoiceNo() %></a>
<script runat="server">
    [System.ComponentModel.Bindable(true)]
    public InvoiceItem Item
    { get; set; }
</script>
