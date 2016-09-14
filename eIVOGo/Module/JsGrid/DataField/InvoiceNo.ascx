<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    "name": "InvoiceNo",
    "type": "text",
    "title": "發票號碼",
    "width": "120",
    "align": "center",
    itemTemplate: function (value, item) {
        return $('<a>')
            .attr('href', '#')
            .html(value)
            .on('click', function (evt) {
                showInvoiceModal(item.DocID);
            });
    }
};
</script>
<% } %>