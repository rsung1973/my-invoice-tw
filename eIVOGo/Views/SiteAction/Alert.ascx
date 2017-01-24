<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="Business.Helper" %>

<%  if (_message != null)
    { %>
<script>
    alert('<%= HttpUtility.JavaScriptStringEncode(_message) %>');
</script>
<%  } %>

<script runat="server">

    ModelStateDictionary _modelState;
    String _message;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _message = (String)this.Model;
        _modelState = (ModelStateDictionary)ViewBag.ModelState;

        if(String.IsNullOrEmpty(_message))
        {
            if(_modelState!=null)
            {
                _message = _modelState.ErrorMessage();
            }
        }
    }

</script>
