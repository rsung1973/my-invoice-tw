<%@ Control Language="C#" AutoEventWireup="true" Inherits="eIVOGo.Module.JsGrid.DataField.JsGridField" %>
<% if (!String.IsNullOrEmpty(FieldVariable)) { %>
<script>
<%= FieldVariable%>[<%= FieldVariable%>.length] = {
    "name": "Attachment",
    "type": "text",
    "title": "附件檔",
    "width": "220",
    "align": "left",
    itemTemplate: function (value, item) {
        var $attach = $(item.Attachment);
        if($attach.length>0){
            var $div = $("<div>");
            $attach.each(function(idx,elmt) {
                $div.append($('<a>')
                        .attr('href', '#')
                        .html(elmt.KeyName)
                        .on('click', function (evt) {
                            window.location.href = '<%= VirtualPathUtility.ToAbsolute("~/Helper/DownloadAttachment.ashx") %>' + "?keyName=" + elmt.KeyName;
                        })).append($('<br/>'));
            });
            return $div.get(0);
        } else {
            return "";
        }
    }
};
</script>
<% } %>