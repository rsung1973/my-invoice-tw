<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="eIVOGo.Models.ViewModel" %>
<%@ Register Src="~/Module/jQuery/EnumSelector.ascx" TagPrefix="uc1" TagName="EnumSelector" %>

<tr>
    <td><%= _model.Year-1911 %><input name="Year" type="hidden" value="<%= _model.Year %>" /></td>
    <td><%= String.Format("{0:00}",_model.PeriodNo*2-1)%>~<%= String.Format("{0:00}",_model.PeriodNo*2)%>月</td>
    <td><input type="text" name="TrackCode" class="form-control" placeholder="請輸入字軌" value="<%= _model.TrackCode %>" data-role="edit" /></td>
    <td>
        <div class="btn-group dropdown" data-toggle="dropdown">
            <button class="btn bg-color-blueLight" data-toggle="dropdown" aria-expanded="false">請選擇功能</button>
            <button class="btn bg-color-blueLight dropdown-toggle" data-toggle="dropdown" aria-expanded="true"><span class="caret"></span></button>
            <ul class="dropdown-menu">
                <li><a class="btn" onclick="uiTrackCodeQuery.commitItem(<%= _model.TrackID %>);">確定</a></li>
                <li><a class="btn" onclick="uiTrackCodeQuery.showItem(<%= _model.TrackID %>);">取消</a></li>
            </ul>
        </div>
    </td>
</tr>


<script runat="server">

    TrackCodeQueryViewModel _viewModel;
    InvoiceTrackCode _model;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (TrackCodeQueryViewModel)ViewBag.ViewModel;
        _model = (InvoiceTrackCode)this.Model;
    }

</script>
