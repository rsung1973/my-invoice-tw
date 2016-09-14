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

<select name="sellerID">
    <%  if (ViewBag.SelectorIndication != null)
        { %>
            <option value="<%= ViewBag.SelectorIndicationValue ?? "" %>"><%= ViewBag.SelectorIndication %></option>
    <%  }
        foreach (var o in _model)
        { %>
            <option value="<%= o.CompanyID %>"><%= String.Format("{0} {1}", o.ReceiptNo, o.CompanyName) %></option> 
    <%  } %>
</select>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    IEnumerable<Organization> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _model = (IEnumerable<Organization>)this.Model;
    }

</script>
