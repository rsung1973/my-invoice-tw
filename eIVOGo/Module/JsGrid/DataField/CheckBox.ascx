<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    "name": "<%= name %>",
    "type": "checkbox",
    "title": "<%= title %>",
    "width": "20",
    "align": "center",
    <% if(!String.IsNullOrEmpty(footerTemplate)) { %>
    "footerTemplate": <%= footerTemplate%>,
    <% } %>
    headerTemplate: function() {
        return $('<input id="chkAll" name="chkAll" type="checkbox" />')
            .on('click',function(evt) {
                $('input[name="chkItem"]').prop('checked',$(this).is(':checked'));
            });
    },
    itemTemplate: function (value, item) {
        return $('<input type="checkbox" name="chkItem"/>')
            .val(item.<%= name %>)
            .on('click',function(evt) {
                if(!$(this).is(':checked')) {
                    $('input[name="chkAll"]').prop('checked',false);
                }
            });
    }

};
</script>
<% } %>