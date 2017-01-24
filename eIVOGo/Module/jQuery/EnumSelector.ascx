<%@ Control Language="C#" AutoEventWireup="true" Inherits="Uxnet.Web.Module.jQuery.EnumSelector" %>
<select name="<%= FieldName %>">
    <% if(!String.IsNullOrEmpty(SelectorIndication)) { %>
    <option value="<%: SelectorIndicationValue %>"><%: SelectorIndication %></option>
    <% } %>
    <% foreach(var item in _items) { %>
    <option value="<%: item.Key %>"><%: item.Value %></option>
    <% } %>
</select>
<%  if(FieldName!=null && Request[FieldName]!=null)
    {   %>
<script>
    $(function () {
        $('select[name="<%= FieldName %>"]').val('<%= Request[FieldName] %>');
    });
</script>
<%  }
    else if(!String.IsNullOrEmpty(DefaultValue))
    { %>
<script>
    $(function () {
        $('select[name="<%= FieldName %>"]').val('<%= DefaultValue %>');
    });
</script>
<%  } %>
