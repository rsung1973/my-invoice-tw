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
    <td>
        <uc1:EnumSelector runat="server" ID="Rank" FieldName="Rank" TypeName="Model.Locale.Naming+EditableWinningPrizeType, Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" />
    </td>
    <td></td>
    <td>
        <input type="text" name="WinningNo" placeholder="請輸入中獎號碼" value="" data-role="add" />
    </td>
    <td>
        <a class="btn" onclick="uiWinningNo.commitItem(null,<%= _viewModel.Year %>,<%= _viewModel.PeriodNo %>);">新增中獎號碼</a>
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
