<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>

<% Html.RenderPartial("~/Views/SiteAction/FunctionTitleBar.ascx", "店家POS機維護");  %>
<div class="table-responsive" data-pattern="priority-columns" data-add-focus-btn="">
    <table class="table table-small-font table-bordered table-striped table01">
        <thead>
            <tr>
                <th>POS機編號</th>
                <th>管理</th>
            </tr>
        </thead>
        <tbody>
            <%  int idx = 0;
                foreach (var item in _items)
                {
                    idx++;
                    Html.RenderPartial("~/Views/InvoiceBusiness/POSDevice/DataItem.ascx", item);
                }
                Html.RenderPartial("~/Views/InvoiceBusiness/POSDevice/AddItem.ascx", _model);
            %>
        </tbody>
        <tfoot>
            <tr>
                <td colspan="2"> &nbsp;</td>
            </tr>
        </tfoot>
    </table>
</div>



<script runat="server">

    IEnumerable<POSDevice> _items;
    Organization _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (Organization)this.Model;
        _items = _model.POSDevice;
    }

</script>
