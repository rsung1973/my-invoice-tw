<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JsGridField.ascx.cs" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    <% if(!String.IsNullOrEmpty(itemTemplate)) { %>
    "itemTemplate": <%= itemTemplate%>,
    <% } %>
    <% if(!String.IsNullOrEmpty(headerTemplate)) { %>
    "headerTemplate": <%= headerTemplate%>,
    <% } %>
    <% if(!String.IsNullOrEmpty(footerTemplate)) { %>
    "footerTemplate": <%= footerTemplate%>,
    <% } %>
    "type": "<%= type%>",
    "name": "<%= name%>",
    "title": "<%= title%>",
    "align": "<%= align%>",
    "width": <%= width%>,
    "css": "<%= css%>",
    "headercss": "<%= headercss%>",
    "filtercss": "<%= filtercss%>",
    "insertcss": "<%= insertcss%>",
    "editcss": "<%= editcss%>"
};
</script>
<% } %>