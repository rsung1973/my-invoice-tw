<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<select name="<%= _fieldName %>">
    <%  if (ViewBag.SelectorIndication != null)
        { %>
            <option value="<%= ViewBag.SelectorIndicationValue ?? "" %>"><%= ViewBag.SelectorIndication %></option>
    <%  }
        foreach (var o in _items)
        { %>
            <option value="<%= o.RoleID %>"><%= o.Role %></option> 
    <%  } %>
</select>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    IEnumerable<UserRoleDefinition> _items;
    String _fieldName;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _items = models.GetTable<UserRoleDefinition>();
        _fieldName = (String)ViewBag.FieldName ?? "RoleID";

    }

</script>
