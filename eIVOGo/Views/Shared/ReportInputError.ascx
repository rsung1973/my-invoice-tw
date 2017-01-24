<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="eIVOGo.Helper" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="eIVOGo.Controllers" %>

<%  if (_modelState != null && !_modelState.IsValid)
    {  %>
        <script>
            $(function () {
                <%  foreach(var key in _modelState.Keys.Where(k => _modelState[k].Errors.Count > 0))
                    {  
                        if(String.IsNullOrEmpty(_dataRole))
                        { %>
                            $('[name="<%= key %>"]').addClass('error').after($('<label id="<%= key%>-error" class="error" for="<%= key%>"><%= HttpUtility.JavaScriptStringEncode(String.Join("、", _modelState[key].Errors.Select(r => r.ErrorMessage))) %></label>'));
                <%      }
                        else
                        {   %>
                            $('[name="<%= key %>"][data-role="<%= _dataRole %>"]').addClass('error').after($('<label id="<%= key%>-error" class="error" for="<%= key%>"><%= HttpUtility.JavaScriptStringEncode(String.Join("、", _modelState[key].Errors.Select(r => r.ErrorMessage))) %></label>'));
                    <%  }
                    }  %>
            });
        </script>
<%  } %>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    String _dataRole;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _modelState = (ModelStateDictionary)ViewBag.ModelState;
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _dataRole = (String)ViewBag.DataRole;
    }

</script>
