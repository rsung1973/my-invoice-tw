<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    "name": "折讓號碼",
    "type": "text",
    "title": "折讓號碼",
    "width": "120",
    "align": "left",
    itemTemplate: function (value, item) {
        return $('<a>')
            .attr('href', '#')
            .html(value)
            .on('click', function (evt) {
                showAllowanceModal(item.DocID);
            });
    }
};
</script>
<% } %>