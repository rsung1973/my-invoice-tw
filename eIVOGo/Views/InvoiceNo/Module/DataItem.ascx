<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.InvoiceTrackCodeAssignment.InvoiceTrackCode.Year %></td>
    <td><%= String.Format("{0:00}-{1:00}月",_model.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2-1,_model.InvoiceTrackCodeAssignment.InvoiceTrackCode.PeriodNo*2) %></td>
    <td><%= _model.InvoiceTrackCodeAssignment.InvoiceTrackCode.TrackCode %></td>
    <td><%= String.Format("{0:00000000}",_model.StartNo) %></td>
    <td><%= String.Format("{0:00000000}",_model.EndNo) %></td>
    <td>
    <%  if (_model.InvoiceNoAssignments.Count == 0)
        { %>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiTrackCodeNo.editItem(<%= _model.IntervalID %>);">修改</a></li>
                <li><a class="btn" onclick="uiTrackCodeNo.deleteItem(<%= _model.IntervalID %>);">刪除</a></li>
            </ul>
        </div>
    <%  } %>
</td>
</tr>


<script runat="server">

    InvoiceNoInterval _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (InvoiceNoInterval)this.Model;
    }

</script>
