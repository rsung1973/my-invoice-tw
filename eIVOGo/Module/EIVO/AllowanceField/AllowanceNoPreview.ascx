<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.EIVO.CDS_DocumentField" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Uxnet.Web.Module.Common" %>
<a href='#' onclick='<%# "javascript:showAllowanceModal(" + DataItem.DocID.ToString() + ");" %>'><%# DataItem.InvoiceAllowance.AllowanceNumber %></a>
