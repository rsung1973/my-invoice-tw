<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Register Src="~/Module/Common/PagingControl.ascx" TagName="PagingControl" TagPrefix="uc2" %>
<%@ Register Assembly="Model" Namespace="Model.DataEntity" TagPrefix="cc1" %>
<%@ Register Src="~/Module/Common/ActionHandler.ascx" TagName="ActionHandler" TagPrefix="uc1" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Utility" %>
<%@ Import Namespace="Newtonsoft.Json" %>

<%--
    <%  Html.RenderPartial("~/Views/Handling/MailTracking/ItemList.ascx", _model); %>--%>
<script>
    $(function() {
        if(!uiHandling.items) {
            uiHandling.items = [];
        }
    <%  if(_model.Count()>0)
        { %>
        var newItems = 
                <%= JsonConvert.SerializeObject(_model.Select(i =>
                  new
                  {
                      PackageID = i.InvoiceID,
                      i.InvoiceID,
                      CustomerID = i.InvoiceBuyer.CustomerID,
                      InvoiceNo = i.TrackCode+i.No,
                      ContactName = i.InvoiceBuyer.ContactName,
                      i.InvoiceBuyer.Address,
                      Remark = ""
                  })) %>; 
        uiHandling.items = uiHandling.items.concat(newItems);
        uiHandling.showDetails();
    <%  } %>
    });
</script>
<%--</div>--%>


<script runat="server">

    IEnumerable<InvoiceItem> _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceItem>)this.Model;
    }

</script>

