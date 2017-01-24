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

<select name="TrackID">
    <%  if (_items.Count() > 0 && !String.IsNullOrEmpty(_viewModel.SelectIndication))
        {   %>
            <option value=""><%= _viewModel.SelectIndication %></option>
    <%  } %>
    <% foreach (var item in _items)
        { %>
    <option value="<%= item.TrackID %>"><%= item.TrackCode %></option>
    <%  } %>
</select>

<script runat="server">

    ModelStateDictionary _modelState;
    ModelSource<InvoiceItem> models;
    InquireNoIntervalViewModel _viewModel;
    IEnumerable<InvoiceTrackCode> _items;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _viewModel = (InquireNoIntervalViewModel)ViewBag.ViewModel;
        if(_viewModel!=null)
        {
            _items = models.GetTable<InvoiceTrackCode>().Where(t => t.Year == _viewModel.Year && t.PeriodNo == _viewModel.PeriodNo);
        }
        else
        {
            _items = models.GetTable<InvoiceTrackCode>().Where(t => false);
        }
    }

</script>

