<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.Year-1911 %></td>
    <td><%= String.Format("{0:00}",_model.PeriodNo*2-1)%>~<%= String.Format("{0:00}",_model.PeriodNo*2)%>月</td>
    <td><%= _model.TrackCode %></td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiTrackCodeQuery.edit(<%= _model.TrackID %>);">修改</a></li>
                <li><a class="btn" onclick="uiTrackCodeQuery.delete(<%= _model.TrackID %>);">刪除</a></li>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    InvoiceTrackCode _model;
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (InvoiceTrackCode)this.Model;
    }

</script>
