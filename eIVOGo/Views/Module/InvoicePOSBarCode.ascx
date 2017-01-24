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

<%  if (_item != null)
    { %>
<img id="barcode" alt="" height="22" src="<%= eIVOGo.Properties.Settings.Default.mailLinkAddress + VirtualPathUtility.ToAbsolute("~/Published/GetBarCode39.ashx")+"?"+String.Format("{0:000}{1:00}{2}{3}{4}", _item.InvoiceDate.Value.Year - 1911, _item.InvoiceDate.Value.Month, _item.TrackCode, _item.No, _item.RandomNo) %>" width="160" />
<%  } %>

<script runat="server">

    ModelSource<InvoiceItem> models;
    InvoiceViewModel _item;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        models = ((SampleController<InvoiceItem>)ViewContext.Controller).DataSource;
        _item = (InvoiceViewModel)ViewBag.ViewModel;
    }

</script>
