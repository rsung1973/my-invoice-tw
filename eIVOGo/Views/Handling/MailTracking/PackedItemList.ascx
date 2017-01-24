<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td>
        <input name="DeliveryDate" class="form_date" type="text" data-package="<%= _item.InvoiceID %>" />
    </td>
    <td><%= _item.InvoiceBuyer.CustomerID %></td>
    <td><pre></pre><%= String.Join("\r\n",_model.Select(i=>i.TrackCode + i.No)) %></pre></td>
    <td><input name="MailNo1" type="text" data-package="<%= _item.InvoiceID %>" />-<input name="MailNo2" type="text" data-package="<%= _item.InvoiceID %>" /></td>
    <td><%= _item.InvoiceBuyer.ContactName %></td>
    <td><%= _item.InvoiceBuyer.Address %></td>
    <td>
        <input name="PackageID" type="checkbox" value="<%= _item.InvoiceID %>" />
    </td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiHandling.pack();">合併</a></li>
                <li><a class="btn" onclick="uiHandling.unpack();">分開</a></li>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    IEnumerable<InvoiceItem> _model;
    InvoiceItem _item;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (IEnumerable<InvoiceItem>)this.Model;
        _item = _model.First();
    }

</script>
