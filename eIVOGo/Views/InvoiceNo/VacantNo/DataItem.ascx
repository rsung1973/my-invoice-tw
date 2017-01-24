<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Linq.Expressions" %>
<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="Model.DataEntity" %>
<%@ Import Namespace="Model.Locale" %>
<%@ Import Namespace="Model.DataEntity" %>

<tr>
    <td><%= _model.Year %></td>
    <td><%= String.Format("{0:00}-{1:00}月",_model.PeriodNo*2-1,_model.PeriodNo*2) %></td>
    <td><%= _model.TrackCode %></td>
    <td><%= String.Format("{0:00000000}",_model.StartNo) %></td>
    <td><%= String.Format("{0:00000000}",_model.EndNo) %></td>
    <td><%= _model.CheckNext.HasValue ? checkSummary() : 1 %></td>
</tr>


<script runat="server">

    InquireVacantNoResult _model;
    List<InquireVacantNoResult> _items;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _model = (InquireVacantNoResult)this.Model;
        _items = (List<InquireVacantNoResult>)ViewBag.DataItems;
    }

    long checkSummary()
    {
        var index = _items.IndexOf(_model);
        var tailItem = _items[index + 1];
        return tailItem.SeqNo.Value - _model.SeqNo.Value + 1;
    }

</script>
