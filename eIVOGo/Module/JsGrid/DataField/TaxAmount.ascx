<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    "name": "TaxAmount",
    "type": "text",
    "title": "稅額",
    "width": "60",
    "align": "right",
    footerTemplate: function () { return "總計金額："; }    
};
</script>
<% } %>