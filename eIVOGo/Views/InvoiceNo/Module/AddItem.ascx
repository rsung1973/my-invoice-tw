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
    <td><%= _viewModel.Year %></td>
    <td><%= String.Format("{0:00}-{1:00}月",_viewModel.PeriodNo*2-1,_viewModel.PeriodNo*2) %></td>
    <td><%  Html.RenderPartial("~/Views/InvoiceNo/Module/TrackCodeSelector.ascx");   %></td>
    <td>
        <input type="text" name="StartNo" placeholder="請輸入配號區間起始號碼" value="" data-role="add" />
    </td>
    <td>
        <input type="text" name="EndNo" placeholder="請輸入配號區間結尾號碼" value="" data-role="add" />
    </td>
    <td>
        <a class="btn" onclick="uiTrackCodeNo.commitItem();">新增配號區間</a>
    </td>
</tr>


<script runat="server">

    InquireNoIntervalViewModel _viewModel;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        _viewModel = (InquireNoIntervalViewModel)ViewBag.ViewModel;
    }

</script>
